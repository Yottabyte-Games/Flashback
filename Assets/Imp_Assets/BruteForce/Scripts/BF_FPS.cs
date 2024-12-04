using UnityEngine;
using System.Collections;

public class BF_FPS : MonoBehaviour
{
    float deltaTime;

    GUIStyle style;

    bool ShowFps;

    void Start()
    {
        ShowFps = true;
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowFps = !ShowFps;
        }

        if(ShowFps)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (ShowFps)
        {
            int w = Screen.width, h = Screen.height;

            style.fontSize = h * 4 / 100;

            var rect = new Rect(0, 0, w, h * 2 / 100);

            var msec = deltaTime * 1000.0f;
            var fps = 1.0f / deltaTime;
            GUI.Label(rect, string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps), style);
        }
    }
}