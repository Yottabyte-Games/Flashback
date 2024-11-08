using System.IO;
using Editor.DialogueSystem.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.DialogueSystem.Windows
{
    public class NarrationEditorWindow : EditorWindow
    {
        NarrationGraphView _graphView;

        readonly string _defaultFileName = "DialoguesFileName";

        static TextField _fileNameTextField;
        Button _saveButton;
        Button _miniMapButton;

        [MenuItem("Narration/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<NarrationEditorWindow>("Dialogue Graph");
        }

        void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        void AddGraphView()
        {
            _graphView = new NarrationGraphView(this);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            _fileNameTextField = NarrationElementUtility.CreateTextField(_defaultFileName, "File Name:", callback =>
            {
                _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            _saveButton = NarrationElementUtility.CreateButton("Save", () => Save());

            Button loadButton = NarrationElementUtility.CreateButton("Load", () => Load());
            Button clearButton = NarrationElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = NarrationElementUtility.CreateButton("Reset", () => ResetGraph());

            _miniMapButton = NarrationElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(_miniMapButton);

            toolbar.AddStyleSheets("DialogueSystem/NarrationToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/NarrationVariables.uss");
        }

        void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            NarrationIOUtility.Initialize(_graphView, _fileNameTextField.value);
            NarrationIOUtility.Save();
        }

        void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            NarrationIOUtility.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            NarrationIOUtility.Load();
        }

        void Clear()
        {
            _graphView.ClearGraph();
        }

        void ResetGraph()
        {
            Clear();

            UpdateFileName(_defaultFileName);
        }

        void ToggleMiniMap()
        {
            _graphView.ToggleMiniMap();

            _miniMapButton.ToggleInClassList("narration-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            _fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }
    }
}