using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Narration.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class NarrationSingleChoiceNode : NarrationNode
    {
        public override void Initialize(string nodeName, NarrationGraphView narrationGraphView, Vector2 position)
        {
            base.Initialize(nodeName, narrationGraphView, position);

            DialogueType = NarrationDialogueType.SingleChoice;

            NarrationChoiceSaveData choiceData = new NarrationChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */

            foreach (NarrationChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
