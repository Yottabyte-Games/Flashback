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
        if (reportedEvent.Name == "StartGameButtonEvent")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    private void OnDisable()
    {
        RiveScreen.OnRiveEvent -= RiveEventHandler;
    }
}
