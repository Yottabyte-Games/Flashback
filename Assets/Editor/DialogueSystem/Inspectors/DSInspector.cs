using System.Collections.Generic;
using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEditor;

namespace DS.Inspectors
{
    using Utilities;

    [CustomEditor(typeof(DSDialogue))]
    public class DSInspector : Editor
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

            DSDialogueContainerSo currentDialogueContainer = (DSDialogueContainerSo) dialogueContainerProperty.objectReferenceValue;

            if (currentDialogueContainer == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");

                return;
            }

            DrawFiltersArea();

            bool currentGroupedDialoguesFilter = groupedDialoguesProperty.boolValue;
            bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;
            
            List<string> dialogueNames;

            string dialogueFolderPath = $"Assets/DialogueSystem/Dialogues/{currentDialogueContainer.fileName}";

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

                DSDialogueGroupSo dialogueGroup = (DSDialogueGroupSo) dialogueGroupProperty.objectReferenceValue;

                dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += $"/Groups/{dialogueGroup.groupName}/Dialogues";

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
            DSInspectorUtility.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            DSInspectorUtility.DrawSpace();
        }

        void DrawFiltersArea()
        {
            DSInspectorUtility.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();

            DSInspectorUtility.DrawSpace();
        }

        void DrawDialogueGroupArea(DSDialogueContainerSo dialogueContainer, List<string> dialogueGroupNames)
        {
            DSInspectorUtility.DrawHeader("Dialogue Group");

            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            DSDialogueGroupSo oldDialogueGroup = (DSDialogueGroupSo) dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.groupName;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            DSDialogueGroupSo selectedDialogueGroup = DSIOUtility.LoadAsset<DSDialogueGroupSo>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.fileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            DSInspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

            DSInspectorUtility.DrawSpace();
        }

        void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            DSInspectorUtility.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            DSDialogueSo oldDialogue = (DSDialogueSo) dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.dialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            DSDialogueSo selectedDialogue = DSIOUtility.LoadAsset<DSDialogueSo>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            DSInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
        }

        void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            DSInspectorUtility.DrawHelpBox(reason, messageType);

            DSInspectorUtility.DrawSpace();

            DSInspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);

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