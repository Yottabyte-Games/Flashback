using System;
using System.Collections.Generic;
using Editor.DialogueSystem.Data.Error;
using Editor.DialogueSystem.Data.Save;
using Editor.DialogueSystem.Elements;
using Editor.DialogueSystem.Utilities;
using Narration.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.DialogueSystem.Windows
{
    public class NarrationGraphView : GraphView
    {
        NarrationEditorWindow _editorWindow;
        NarrationSearchWindow _searchWindow;

        MiniMap _miniMap;

        SerializableDictionary<string, NarrationNodeErrorData> _ungroupedNodes;
        SerializableDictionary<string, NarrationGroupErrorData> _groups;
        SerializableDictionary<Group, SerializableDictionary<string, NarrationNodeErrorData>> _groupedNodes;

        int _nameErrorsAmount;

        public int NameErrorsAmount
        {
            get
            {
                return _nameErrorsAmount;
            }

            set
            {
                _nameErrorsAmount = value;

                if (_nameErrorsAmount == 0)
                {
                    _editorWindow.EnableSaving();
                }

                if (_nameErrorsAmount == 1)
                {
                    _editorWindow.DisableSaving();
                }
            }
        }

        public NarrationGraphView(NarrationEditorWindow narrationEditorWindow)
        {
            _editorWindow = narrationEditorWindow;

            _ungroupedNodes = new SerializableDictionary<string, NarrationNodeErrorData>();
            _groups = new SerializableDictionary<string, NarrationGroupErrorData>();
            _groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, NarrationNodeErrorData>>();

            AddManipulators();
            AddGridBackground();
            AddSearchWindow();
            AddMiniMap();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

            AddStyles();
            AddMiniMapStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", NarrationDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", NarrationDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextualMenu());
        }

        IManipulator CreateNodeContextualMenu(string actionTitle, NarrationDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName", dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

            return contextualMenuManipulator;
        }

        IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        public NarrationGroup CreateGroup(string title, Vector2 position)
        {
            NarrationGroup group = new NarrationGroup(title, position);

            AddGroup(group);

            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is NarrationNode))
                {
                    continue;
                }

                NarrationNode node = (NarrationNode) selectedElement;

                group.AddElement(node);
            }

            return group;
        }

        public NarrationNode CreateNode(string nodeName, NarrationDialogueType dialogueType, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");

            NarrationNode node = (NarrationNode) Activator.CreateInstance(nodeType);

            node.Initialize(nodeName, this, position);

            if (shouldDraw)
            {
                node.Draw();
            }

            AddUngroupedNode(node);

            return node;
        }

        void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(NarrationGroup);
                Type edgeType = typeof(Edge);

                List<NarrationGroup> groupsToDelete = new List<NarrationGroup>();
                List<NarrationNode> nodesToDelete = new List<NarrationNode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is NarrationNode node)
                    {
                        nodesToDelete.Add(node);

                        continue;
                    }

                    if (selectedElement.GetType() == edgeType)
                    {
                        Edge edge = (Edge) selectedElement;

                        edgesToDelete.Add(edge);

                        continue;
                    }

                    if (selectedElement.GetType() != groupType)
                    {
                        continue;
                    }

                    NarrationGroup group = (NarrationGroup) selectedElement;

                    groupsToDelete.Add(group);
                }

                foreach (NarrationGroup groupToDelete in groupsToDelete)
                {
                    List<NarrationNode> groupNodes = new List<NarrationNode>();

                    foreach (GraphElement groupElement in groupToDelete.containedElements)
                    {
                        if (!(groupElement is NarrationNode))
                        {
                            continue;
                        }

                        NarrationNode groupNode = (NarrationNode) groupElement;

                        groupNodes.Add(groupNode);
                    }

                    groupToDelete.RemoveElements(groupNodes);

                    RemoveGroup(groupToDelete);

                    RemoveElement(groupToDelete);
                }

                DeleteElements(edgesToDelete);

                foreach (NarrationNode nodeToDelete in nodesToDelete)
                {
                    if (nodeToDelete.Group != null)
                    {
                        nodeToDelete.Group.RemoveElement(nodeToDelete);
                    }

                    RemoveUngroupedNode(nodeToDelete);

                    nodeToDelete.DisconnectAllPorts();

                    RemoveElement(nodeToDelete);
                }
            };
        }

        void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is NarrationNode))
                    {
                        continue;
                    }

                    NarrationGroup narrationGroup = (NarrationGroup) group;
                    NarrationNode node = (NarrationNode) element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, narrationGroup);
                }
            };
        }

        void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is NarrationNode))
                    {
                        continue;
                    }

                    NarrationGroup narrationGroup = (NarrationGroup) group;
                    NarrationNode node = (NarrationNode) element;

                    RemoveGroupedNode(node, narrationGroup);
                    AddUngroupedNode(node);
                }
            };
        }

        void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                NarrationGroup narrationGroup = (NarrationGroup) group;

                narrationGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(narrationGroup.title))
                {
                    if (!string.IsNullOrEmpty(narrationGroup.OldTitle))
                    {
                        ++NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(narrationGroup.OldTitle))
                    {
                        --NameErrorsAmount;
                    }
                }

                RemoveGroup(narrationGroup);

                narrationGroup.OldTitle = narrationGroup.title;

                AddGroup(narrationGroup);
            };
        }

        void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        NarrationNode nextNode = (NarrationNode) edge.input.node;

                        NarrationChoiceSaveData choiceData = (NarrationChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;

                        NarrationChoiceSaveData choiceData = (NarrationChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }

                return changes;
            };
        }

        public void AddUngroupedNode(NarrationNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!_ungroupedNodes.ContainsKey(nodeName))
            {
                NarrationNodeErrorData nodeErrorData = new NarrationNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                _ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            List<NarrationNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = _ungroupedNodes[nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++NameErrorsAmount;

                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(NarrationNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<NarrationNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                --NameErrorsAmount;

                ungroupedNodesList[0].ResetStyle();

                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                _ungroupedNodes.Remove(nodeName);
            }
        }

        void AddGroup(NarrationGroup group)
        {
            string groupName = group.title.ToLower();

            if (!_groups.ContainsKey(groupName))
            {
                NarrationGroupErrorData groupErrorData = new NarrationGroupErrorData();

                groupErrorData.Groups.Add(group);

                _groups.Add(groupName, groupErrorData);

                return;
            }

            List<NarrationGroup> groupsList = _groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = _groups[groupName].ErrorData.Color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++NameErrorsAmount;

                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        void RemoveGroup(NarrationGroup group)
        {
            string oldGroupName = group.OldTitle.ToLower();

            List<NarrationGroup> groupsList = _groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --NameErrorsAmount;

                groupsList[0].ResetStyle();

                return;
            }

            if (groupsList.Count == 0)
            {
                _groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(NarrationNode node, NarrationGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = group;

            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new SerializableDictionary<string, NarrationNodeErrorData>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                NarrationNodeErrorData nodeErrorData = new NarrationNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                _groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<NarrationNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = _groupedNodes[group][nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++NameErrorsAmount;

                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(NarrationNode node, NarrationGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

            List<NarrationNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --NameErrorsAmount;

                groupedNodesList[0].ResetStyle();

                return;
            }

            if (groupedNodesList.Count == 0)
            {
                _groupedNodes[group].Remove(nodeName);

                if (_groupedNodes[group].Count == 0)
                {
                    _groupedNodes.Remove(group);
                }
            }
        }

        void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<NarrationSearchWindow>();
            }

            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        void AddMiniMap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true
            };

            _miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(_miniMap);

            _miniMap.visible = false;
        }

        void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
            );
        }

        void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            _miniMap.style.backgroundColor = backgroundColor;
            _miniMap.style.borderTopColor = borderColor;
            _miniMap.style.borderRightColor = borderColor;
            _miniMap.style.borderBottomColor = borderColor;
            _miniMap.style.borderLeftColor = borderColor;
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, mousePosition - _editorWindow.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));

            _groups.Clear();
            _groupedNodes.Clear();
            _ungroupedNodes.Clear();

            NameErrorsAmount = 0;
        }

        public void ToggleMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }
    }
}