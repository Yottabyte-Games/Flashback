using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Narration.Utilities
{
    using Data;
    using Data.Save;
    using Elements;
    using ScriptableObjects;
    using Windows;

    public static class NarrationIOUtility
    {
        static NarrationGraphView graphView;

        static string graphFileName;
        static string containerFolderPath;

        static List<NarrationNode> nodes;
        static List<NarrationGroup> groups;

        static Dictionary<string, NarrationDialogueGroupSO> createdDialogueGroups;
        static Dictionary<string, NarrationDialogueSO> createdDialogues;

        static Dictionary<string, NarrationGroup> loadedGroups;
        static Dictionary<string, NarrationNode> loadedNodes;

        public static void Initialize(NarrationGraphView narrationGraphView, string graphName)
        {
            graphView = narrationGraphView;

            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphName}";

            nodes = new List<NarrationNode>();
            groups = new List<NarrationGroup>();

            createdDialogueGroups = new Dictionary<string, NarrationDialogueGroupSO>();
            createdDialogues = new Dictionary<string, NarrationDialogueSO>();

            loadedGroups = new Dictionary<string, NarrationGroup>();
            loadedNodes = new Dictionary<string, NarrationNode>();
        }

        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();

            NarrationGraphSaveDataSO graphData = CreateAsset<NarrationGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");

            graphData.Initialize(graphFileName);

            NarrationDialogueContainerSO dialogueContainer = CreateAsset<NarrationDialogueContainerSO>(containerFolderPath, graphFileName);

            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        static void SaveGroups(NarrationGraphSaveDataSO graphData, NarrationDialogueContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (NarrationGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        static void SaveGroupToGraph(NarrationGroup group, NarrationGraphSaveDataSO graphData)
        {
            NarrationGroupSaveData groupData = new NarrationGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        static void SaveGroupToScriptableObject(NarrationGroup group, NarrationDialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            NarrationDialogueGroupSO dialogueGroup = CreateAsset<NarrationDialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            createdDialogueGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<NarrationDialogueSO>());

            SaveAsset(dialogueGroup);
        }

        static void UpdateOldGroups(List<string> currentGroupNames, NarrationGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        static void SaveNodes(NarrationGraphSaveDataSO graphData, NarrationDialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (NarrationNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                    continue;
                }

                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        static void SaveNodeToGraph(NarrationNode node, NarrationGraphSaveDataSO graphData)
        {
            List<NarrationChoiceSaveData> choices = CloneNodeChoices(node.Choices);

            NarrationNodeSaveData nodeData = new NarrationNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                GroupID = node.Group?.ID,
                DialogueType = node.DialogueType,
                Position = node.GetPosition().position
            };

            graphData.Nodes.Add(nodeData);
        }

        static void SaveNodeToScriptableObject(NarrationNode node, NarrationDialogueContainerSO dialogueContainer)
        {
            NarrationDialogueSO dialogue;

            if (node.Group != null)
            {
                dialogue = CreateAsset<NarrationDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);
            }
            else
            {
                dialogue = CreateAsset<NarrationDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }

            dialogue.Initialize(
                node.DialogueName,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.DialogueType,
                node.IsStartingNode()
            );

            createdDialogues.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        static List<NarrationDialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<NarrationChoiceSaveData> nodeChoices)
        {
            List<NarrationDialogueChoiceData> dialogueChoices = new List<NarrationDialogueChoiceData>();

            foreach (NarrationChoiceSaveData nodeChoice in nodeChoices)
            {
                NarrationDialogueChoiceData choiceData = new NarrationDialogueChoiceData()
                {
                    Text = nodeChoice.Text
                };

                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }

        static void UpdateDialoguesChoicesConnections()
        {
            foreach (NarrationNode node in nodes)
            {
                NarrationDialogueSO dialogue = createdDialogues[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
                {
                    NarrationChoiceSaveData nodeChoice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(nodeChoice.NodeID))
                    {
                        continue;
                    }

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, NarrationGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, NarrationGraphSaveDataSO graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        public static void Load()
        {
            NarrationGraphSaveDataSO graphData = LoadAsset<NarrationGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"Assets/Editor/DialogueSystem/Graphs/{graphFileName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            NarrationEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        static void LoadGroups(List<NarrationGroupSaveData> groups)
        {
            foreach (NarrationGroupSaveData groupData in groups)
            {
                NarrationGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        static void LoadNodes(List<NarrationNodeSaveData> nodes)
        {
            foreach (NarrationNodeSaveData nodeData in nodes)
            {
                List<NarrationChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);

                NarrationNode node = graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);

                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;

                node.Draw();

                graphView.AddElement(node);

                loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                NarrationGroup group = loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, NarrationNode> loadedNode in loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    NarrationChoiceSaveData choiceData = (NarrationChoiceSaveData) choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        continue;
                    }

                    NarrationNode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        static void CreateDefaultFolders()
        {
            CreateFolder("Assets/Editor/DialogueSystem", "Graphs");

            CreateFolder("Assets", "DialogueSystem");
            CreateFolder("Assets/DialogueSystem", "Dialogues");

            CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

        static void GetElementsFromGraphView()
        {
            Type groupType = typeof(NarrationGroup);

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is NarrationNode node)
                {
                    nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    NarrationGroup group = (NarrationGroup) graphElement;

                    groups.Add(group);

                    return;
                }
            });
        }

        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        static List<NarrationChoiceSaveData> CloneNodeChoices(List<NarrationChoiceSaveData> nodeChoices)
        {
            List<NarrationChoiceSaveData> choices = new List<NarrationChoiceSaveData>();

            foreach (NarrationChoiceSaveData choice in nodeChoices)
            {
                NarrationChoiceSaveData choiceData = new NarrationChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };

                choices.Add(choiceData);
            }

            return choices;
        }
    }
}