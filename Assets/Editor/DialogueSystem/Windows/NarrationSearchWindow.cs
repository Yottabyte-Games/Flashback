using System.Collections.Generic;
using Editor.DialogueSystem.Elements;
using Narration.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.DialogueSystem.Windows
{
    public class NarrationSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        NarrationGraphView _graphView;
        Texture2D _indentationIcon;

        public void Initialize(NarrationGraphView narrationGraphView)
        {
            _graphView = narrationGraphView;

            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", _indentationIcon))
                {
                    userData = NarrationDialogueType.SingleChoice,
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
                {
                    userData = NarrationDialogueType.MultipleChoice,
                    level = 2
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Groups"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", _indentationIcon))
                {
                    userData = new Group(),
                    level = 2
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (searchTreeEntry.userData)
            {
                case NarrationDialogueType.SingleChoice:
                {
                    NarrationSingleChoiceNode singleChoiceNode = (NarrationSingleChoiceNode) _graphView.CreateNode("DialogueName", NarrationDialogueType.SingleChoice, localMousePosition);

                    _graphView.AddElement(singleChoiceNode);

                    return true;
                }

                case NarrationDialogueType.MultipleChoice:
                {
                    NarrationMultipleChoiceNode multipleChoiceNode = (NarrationMultipleChoiceNode) _graphView.CreateNode("DialogueName", NarrationDialogueType.MultipleChoice, localMousePosition);

                    _graphView.AddElement(multipleChoiceNode);

                    return true;
                }

                case Group _:
                {
                    _graphView.CreateGroup("DialogueGroup", localMousePosition);

                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }
    }
}