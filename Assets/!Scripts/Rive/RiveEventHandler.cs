using System;
using Plugins.Rive.UI;
using UnityEngine;
using Rive;
using UnityEditor;
using UnityEngine.SceneManagement;

public class RiveEventHandler : MonoBehaviour
{
    private RiveScreen riveScreen;
    
    
    
    private void Start()
    {
        riveScreen = GetComponent<RiveScreen>();

        riveScreen.OnRiveEvent += RiveEventHappens;
    }

    private void RiveEventHappens(ReportedEvent reportedEvent)
    {
        switch (riveScreen.currentScene)
        {
            case RiveScreen.RiveScenes.HUD:
                break;
            case RiveScreen.RiveScenes.MainMenu:
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
                break;
            case RiveScreen.RiveScenes.SettingsMenu:
                if (reportedEvent.Name == "Master Active")
                    print("Clicking the thing");
                if (reportedEvent.Name == "Master Inactive")
                    print("Unclick the thing");

                if (reportedEvent.Name == "Return Event")
                {
                    print("ReturnEvent");
                }
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
                break;
            case RiveScreen.RiveScenes.PauseMenu:
                if (riveScreen.StateMachine.GetBool("IsTryingToQuit").Value != true)
                {
                    if (reportedEvent.Name == "ResumeEvent")
                    {
                        riveScreen.ReturnToOriginalScene();
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
                break;
        }
    }
    
    private void OnDisable()
    {
        riveScreen.OnRiveEvent -= RiveEventHappens;
    }
}
