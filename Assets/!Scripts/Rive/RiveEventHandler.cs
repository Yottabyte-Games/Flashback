using UnityEngine;
using Rive;
using UnityEngine.SceneManagement;

public class RiveEventHandler : MonoBehaviour
{
    RiveScreen _riveScreen;


    void Start()
    {
        _riveScreen = GetComponent<RiveScreen>();

        _riveScreen.OnRiveEvent += RiveEventHappens;
    }

    void RiveEventHappens(ReportedEvent reportedEvent)
    {
        switch (_riveScreen.currentScene)
        {
            case RiveScreen.RiveScenes.HUD:
                break;
            case RiveScreen.RiveScenes.MainMenu:
                switch (reportedEvent.Name)
                {
                    case "StartGameEvent":
                        print("StartGameEvent");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        break;
                    case "MiniGameEvent":
                        print("MiniGameEvent");
                        _riveScreen.SetRiveScene(RiveScreen.RiveScenes.MiniGameSelectMenu);
                        break;
                    case "SettingsEvent":
                        print("SettingsEvent");
                        _riveScreen.SetRiveScene(RiveScreen.RiveScenes.SettingsMenu);
                        break;
                    case "ExitEvent":
                        print("ExitEvent");
                        Application.Quit();
                        break;
                }

                break;
            case RiveScreen.RiveScenes.MiniGameSelectMenu:
                switch (reportedEvent.Name)
                {
                    case "Return Event":
                        print("Return Event");
                        _riveScreen.ReturnToOriginalScene();
                        break;
                    case "Fishing Event":
                        print("Fishing Event");
                        SceneManager.LoadScene("Fishing");
                        break;
                    case "Racing Event":
                        print("Racing Event");
                        SceneManager.LoadScene("ToyCarGame - Backup");
                        break;
                    case "Snowman Event":
                        print("Snowman Event");
                        SceneManager.LoadScene("Snowman");
                        break;
                    case "Work Event":
                        print("Work Event");
                        SceneManager.LoadScene("Working");
                        break;
                    case "Shooting Event":
                        print("Shoot Event");
                        SceneManager.LoadScene("Psychologist");
                        break;
                }
                break;
            case RiveScreen.RiveScenes.SettingsMenu:
                switch (reportedEvent.Name)
                {
                    case "Return Event":
                        _riveScreen.ReturnToOriginalScene();
                        break;
                    case "Master Change Event":
                        print("Master Change Event");
                        //Get value between 1 and 100
                        print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Master Slider"));
                        break;
                    case "Music Change Event":
                        print("Music Change Event");
                        //Get value between 1 and 100
                        print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Music Slider"));
                        break;
                    case "SFX Change Event":
                        print("SFX Change Event");
                        //Get value between 1 and 100
                        print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "SFX Slider"));
                        break;
                    case "Voice Change Event":
                        print("Voice Change Event");
                        //Get value between 1 and 100
                        print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Voice Slider"));
                        break;
                }
                break;
            case RiveScreen.RiveScenes.PauseMenu:
                if (_riveScreen.stateMachine.GetBool("IsTryingToQuit").Value != true)
                {
                    if (reportedEvent.Name == "ResumeEvent")
                    {
                        print("ResumeEvent");
                        _riveScreen.ReturnToOriginalScene();
                    }
                    if (reportedEvent.Name == "SettingsEvent")
                    {
                        print("SettingsEvent");
                        _riveScreen.SetRiveScene(RiveScreen.RiveScenes.SettingsMenu);
                    }
                }
                
                if (reportedEvent.Name == "QuitEvent")
                {
                    print("QuitEvent");
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            case RiveScreen.RiveScenes.PsychologyScene:
                if (reportedEvent.Name == "Option 1 Pressed Event")
                {
                    print("Option 1 Pressed Event");
                }

                if (reportedEvent.Name == "Option 2 Pressed Event")
                {
                    print("Option 2 Pressed Event");
                }
                break;
        }
    }

    void OnDisable()
    {
        _riveScreen.OnRiveEvent -= RiveEventHappens;
    }
}
