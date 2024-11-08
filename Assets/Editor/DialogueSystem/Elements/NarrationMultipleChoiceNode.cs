using Editor.DialogueSystem.Data.Save;
using Editor.DialogueSystem.Utilities;
using Editor.DialogueSystem.Windows;
using Narration.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.DialogueSystem.Elements
{
    public class NarrationMultipleChoiceNode : NarrationNode
    {
        public override void Initialize(string nodeName, NarrationGraphView narrationGraphView, Vector2 position)
        {
            base.Initialize(nodeName, narrationGraphView, position);

            DialogueType = NarrationDialogueType.MultipleChoice;

            NarrationChoiceSaveData choiceData = new NarrationChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            Button addChoiceButton = NarrationElementUtility.CreateButton("Add Choice", () =>
            {
                NarrationChoiceSaveData choiceData = new NarrationChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTAINER */

            foreach (NarrationChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            NarrationChoiceSaveData choiceData = (NarrationChoiceSaveData) userData;

            Button deleteChoiceButton = NarrationElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                if (choicePort.connected)
                {
                    GraphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);

                GraphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = NarrationElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}