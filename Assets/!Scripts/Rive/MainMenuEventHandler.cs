using System;
using Plugins.Rive.UI;
using UnityEngine;
using Rive;
using UnityEngine.SceneManagement;

public class MainMenuEventHandler : MonoBehaviour
{
    private RiveScreen RiveScreen;
    
    private void Start()
    {
        RiveScreen = GetComponent<RiveScreen>();

        RiveScreen.OnRiveEvent += RiveEventHandler;
    }

    private void RiveEventHandler(ReportedEvent reportedEvent)
    {
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
    }
    
    private void OnDisable()
    {
        RiveScreen.OnRiveEvent -= RiveEventHandler;
    }
}
