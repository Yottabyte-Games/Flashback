using System;
using System.Collections.Generic;
using System.Linq;
using Editor.DialogueSystem.Data.Save;
using Editor.DialogueSystem.Utilities;
using Editor.DialogueSystem.Windows;
using Narration.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Port = UnityEditor.Experimental.GraphView.Port;

namespace Editor.DialogueSystem.Elements
{
    public class NarrationNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; private set; }
        public List<NarrationChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public NarrationDialogueType DialogueType { get; protected set; }
        public NarrationGroup Group { get; set; }

        protected NarrationGraphView GraphView;
        Color _defaultBackgroundColor;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public virtual void Initialize(string nodeName, NarrationGraphView narrationGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            DialogueName = nodeName;
            Choices = new List<NarrationChoiceSaveData>();
            Text = "Dialogue text.";

            SetPosition(new Rect(position, Vector2.zero));

            GraphView = narrationGraphView;
            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("narration-node__main-container");
            extensionContainer.AddToClassList("narration-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            TextField dialogueNameTextField = NarrationElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField) callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (!string.IsNullOrEmpty(target.value))
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --GraphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++GraphView.NameErrorsAmount;
                    }
                }

                if (Group is null)
                {
                    GraphView.RemoveUngroupedNode(this);

                    DialogueName = target.value;

                    GraphView.AddUngroupedNode(this);

                    return;
                }

                NarrationGroup currentGroup = Group;

                GraphView.RemoveGroupedNode(this, Group);

                DialogueName = target.value;

                GraphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                "narration-node__text-field",
                "narration-node__text-field__hidden",
                "narration-node__filename-text-field"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("narration-node__custom-data-container");

            Foldout textFoldout = NarrationElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = NarrationElementUtility.CreateTextArea(Text, null, callback => Text = callback.newValue);

            textTextField.AddClasses(
                "narration-node__text-field",
                "narration-node__quote-text-field"
            );

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                GraphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }
    }
}