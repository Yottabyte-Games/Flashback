using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Narration.Windows
{
    using System;
    using Utilities;

    public class NarrationEditorWindow : EditorWindow
    {
        NarrationGraphView graphView;

        readonly string defaultFileName = "DialoguesFileName";

        static TextField fileNameTextField;
        Button saveButton;
        Button miniMapButton;

        [MenuItem("Window/DS/Dialogue Graph")]
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
            graphView = new NarrationGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = NarrationElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = NarrationElementUtility.CreateButton("Save", () => Save());

            Button loadButton = NarrationElementUtility.CreateButton("Load", () => Load());
            Button clearButton = NarrationElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = NarrationElementUtility.CreateButton("Reset", () => ResetGraph());

            miniMapButton = NarrationElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(miniMapButton);

            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            NarrationIOUtility.Initialize(graphView, fileNameTextField.value);
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

            NarrationIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            NarrationIOUtility.Load();
        }

        void Clear()
        {
            graphView.ClearGraph();
        }

        void ResetGraph()
        {
            Clear();

            UpdateFileName(defaultFileName);
        }

        void ToggleMiniMap()
        {
            graphView.ToggleMiniMap();

            miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }
    }
}