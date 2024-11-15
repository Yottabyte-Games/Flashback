using System;
using Plugins.Rive.UI;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseEventHandler : MonoBehaviour
{
    private RiveScreen _riveScreen;
    void Start()
    {
        
        _riveScreen = GetComponent<RiveScreen>();
        _riveScreen.OnRiveEvent += RiveEventHandler;
    }
    private void RiveEventHandler(ReportedEvent reportedEvent)
    {
        if (!_riveScreen.stateMachine.GetBool("IsTryingToQuit").Value)
        {
            if (reportedEvent.Name == "ResumeEvent")
            {
                _riveScreen.ReturnToOriginalScene();
            }
            if (reportedEvent.Name == "SettingsEvent")
            {
                print("SettingsEvent");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (reportedEvent.Name == "QuitEvent")
        {
            print("QuitEvent");
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnDisable()
    {
        _riveScreen.OnRiveEvent -= RiveEventHandler;
    }

}
