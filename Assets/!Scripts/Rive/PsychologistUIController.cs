using System;
using UnityEngine;

public class PsychologistUIController : MonoBehaviour
{
    RiveScreen riveScreen;
    private void Start()
    {
        riveScreen = GetComponent<RiveScreen>();
    }

    public void SetPsychologistText(string textRun)
    {
        riveScreen.SetTextRunAtPath(textRun, RiveScreen.TextPath.Psychologist);
    }

    public void SetOption1Text(string textRun)
    {
        riveScreen.SetTextRunAtPath(textRun, RiveScreen.TextPath.Option1);
    }

    public void SetOption2Text(string textRun)
    {
        riveScreen.SetTextRunAtPath(textRun, RiveScreen.TextPath.Option2);
    }
    public void SetText(RiveScreen.TextPath path, string textRun)
    {
        riveScreen.SetTextRunAtPath(textRun, path);
    }
}
