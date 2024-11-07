using System.Collections.Generic;
using UnityEditor;

namespace Narration.Inspectors
{
    using Utilities;
    using ScriptableObjects;

    [CustomEditor(typeof(NarrationDialogue))]
    public class NarrationInspector : Editor
    {
        /* Dialogue Scriptable Objects */
        SerializedProperty dialogueContainerProperty;
        SerializedProperty dialogueGroupProperty;
        SerializedProperty dialogueProperty;

        /* Filters */
        SerializedProperty groupedDialoguesProperty;
        SerializedProperty startingDialoguesOnlyProperty;

        /* Indexes */
        SerializedProperty selectedDialogueGroupIndexProperty;
        SerializedProperty selectedDialogueIndexProperty;

        void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            NarrationDialogueContainerSO currentDialogueContainer = (NarrationDialogueContainerSO) dialogueContainerProperty.objectReferenceValue;

            if (currentDialogueContainer == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");

                return;
            }

            DrawFiltersArea();

            bool currentGroupedDialoguesFilter = groupedDialoguesProperty.boolValue;
            bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;
            
            List<string> dialogueNames;

            string dialogueFolderPath = $"Assets/DialogueSystem/Dialogues/{currentDialogueContainer.FileName}";

            string dialogueInfoMessage;

            if (currentGroupedDialoguesFilter)
            {
                List<string> dialogueGroupNames = currentDialogueContainer.GetDialogueGroupNames();

                if (dialogueGroupNames.Count == 0)
                {
                    StopDrawing("There are no Dialogue Groups in this Dialogue Container.");

                    return;
                }

                DrawDialogueGroupArea(currentDialogueContainer, dialogueGroupNames);

                NarrationDialogueGroupSO dialogueGroup = (NarrationDialogueGroupSO) dialogueGroupProperty.objectReferenceValue;

                dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Dialogues in this Dialogue Group.";
            }
            else
            {
                dialogueNames = currentDialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += "/Global/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Ungrouped Dialogues in this Dialogue Container.";
            }

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);

                return;
            }

            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            serializedObject.ApplyModifiedProperties();
        }

        void DrawDialogueContainerArea()
        {
            NarrationInspectorUtility.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            NarrationInspectorUtility.DrawSpace();
        }

        void DrawFiltersArea()
        {
            NarrationInspectorUtility.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();

            NarrationInspectorUtility.DrawSpace();
        }

        void DrawDialogueGroupArea(NarrationDialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
        {
            NarrationInspectorUtility.DrawHeader("Dialogue Group");

            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            NarrationDialogueGroupSO oldDialogueGroup = (NarrationDialogueGroupSO) dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = NarrationInspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            NarrationDialogueGroupSO selectedDialogueGroup = NarrationIOUtility.LoadAsset<NarrationDialogueGroupSO>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            NarrationInspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

            NarrationInspectorUtility.DrawSpace();
        }

        void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            NarrationInspectorUtility.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            NarrationDialogueSO oldDialogue = (NarrationDialogueSO) dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = NarrationInspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            NarrationDialogueSO selectedDialogue = NarrationIOUtility.LoadAsset<NarrationDialogueSO>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            NarrationInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
        }

        void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            NarrationInspectorUtility.DrawHelpBox(reason, messageType);

            NarrationInspectorUtility.DrawSpace();

            NarrationInspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }

        void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;

                return;
            }

            bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

            if (oldNameIsDifferentThanSelectedName)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);

                    return;
                }

                indexProperty.intValue = 0;
            }
        }
    }
}