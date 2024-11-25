namespace Dreamteck.Splines.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    public class ClipRangeWindow : EditorWindow
    {
        float from = 0f;
        float to = 0f;
        System.Action<float, float> rcv;
        float length = 0f;
        public void Init(System.Action<float, float> receiver, float fromDistance, float toDistance, float totalLength)
        {
            rcv = receiver;
            length = totalLength;
            from = fromDistance;
            to = toDistance;
            titleContent = new GUIContent("Set Clip Range Distances");
            minSize = maxSize = new Vector2(240, 120);
        }

        void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                rcv(from, to);
                Close();
            }
            from = EditorGUILayout.FloatField("From ", from);
            if (from < 0f) from = 0f;
            else if (from > length) from = length;

            to = EditorGUILayout.FloatField("To ", to);
            if (to < 0f) to = 0f;
            else if (to > length) to = length;

            EditorGUILayout.HelpBox("Enter the distance and press Enter. Current spline length: " + length, MessageType.Info);
        }
    }
}
