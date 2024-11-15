using System;
using Plugins.Rive.UI;
using Rive;
using UnityEngine;

public class SettingEventHandler : MonoBehaviour
{
    private RiveScreen _riveScreen;
    void Start()
    {
        
        _riveScreen = GetComponent<RiveScreen>();
        _riveScreen.OnRiveEvent += RiveEventHandler;
    }

    private void Update()
    {
        print(_riveScreen.stateMachine.GetNumber("Master Slider/SliderValue"));
    }

    private void RiveEventHandler(ReportedEvent reportedEvent)
    {
        if (reportedEvent.Name == "Return Event")
        {
        }
        if (reportedEvent.Name == "Master Change Event")
        {
        }
        if (reportedEvent.Name == "Music Change Event")
        {
        }
        if (reportedEvent.Name == "SFX Change Event")
        {
        }
        if (reportedEvent.Name == "Voice Change Event")
        {
        }
    }

    private void OnDisable()
    {
        _riveScreen.OnRiveEvent -= RiveEventHandler;
    }
}
