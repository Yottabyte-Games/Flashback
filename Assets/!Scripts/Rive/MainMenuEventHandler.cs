using System;
using Plugins.Rive.UI;
using UnityEngine;
using Rive;
using UnityEngine.SceneManagement;

public class MainMenuEventHandler : MonoBehaviour
{
    private RiveScreen riveScreen;
    
    private void Start()
    {
        riveScreen = GetComponent<RiveScreen>();

        riveScreen.OnRiveEvent += RiveEventHandler;
    }

    private void RiveEventHandler(ReportedEvent reportedEvent)
    {
        #region ButtonInput Events

        if (reportedEvent.Name == "StartGameEvent")
        {
            print("StartGameEvent");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (reportedEvent.Name == "MiniGameEvent")
        {
            print("MiniGameEvent");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (reportedEvent.Name == "SettingsEvent")
        {
            print("SettingsEvent");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (reportedEvent.Name == "ExitEvent")
        {
            print("ExitEvent");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        

        #endregion

        #region SliderChange Events

        if (reportedEvent.Name == "Master Change Event")
        {
            print("Master Change Event");
        }
        if (reportedEvent.Name == "Music Change Event")
        {
            print("Music Change Event");
        }
        if (reportedEvent.Name == "SFX Change Event")
        {
            print("SFX Change Event");
        }
        if (reportedEvent.Name == "Voice Change Event")
        {
            print("Voice Change Event");
        }

        #endregion
    }
    
    private void OnDisable()
    {
        riveScreen.OnRiveEvent -= RiveEventHandler;
    }
}
