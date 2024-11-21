using UnityEngine;
using Rive;
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
                switch (reportedEvent.Name)
                {
                    case "StartGameEvent":
                        print("StartGameEvent");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        break;
                    case "MiniGameEvent":
                        print("MiniGameEvent");
                        riveScreen.SetRiveScene(RiveScreen.RiveScenes.MiniGameSelectMenu);
                        break;
                    case "SettingsEvent":
                        print("SettingsEvent");
                        riveScreen.SetRiveScene(RiveScreen.RiveScenes.SettingsMenu);
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
                        riveScreen.ReturnToOriginalScene();
                        break;
                    case "Fishing Event":
                        print("Fishing Event");
                        SceneManager.LoadScene("Fishing");
                        break;
                    case "Racing Event":
                        print("Racing Event");
                        SceneManager.LoadScene("ToyCarGame");
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
                        //SceneManager.LoadScene();
                        break;
                }
                break;
            case RiveScreen.RiveScenes.SettingsMenu:
                switch (reportedEvent.Name)
                {
                    case "Return Event":
                        riveScreen.ReturnToOriginalScene();
                        break;
                    case "Master Change Event":
                        print("Master Change Event");
                        //Get value between 1 and 100
                        print(riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Master Slider"));
                        break;
                    case "Music Change Event":
                        print("Music Change Event");
                        //Get value between 1 and 100
                        print(riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Music Slider"));
                        break;
                    case "SFX Change Event":
                        print("SFX Change Event");
                        //Get value between 1 and 100
                        print(riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "SFX Slider"));
                        break;
                    case "Voice Change Event":
                        print("Voice Change Event");
                        //Get value between 1 and 100
                        print(riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Voice Slider"));
                        break;
                }
                break;
            case RiveScreen.RiveScenes.PauseMenu:
                if (riveScreen.stateMachine.GetBool("IsTryingToQuit").Value != true)
                {
                    if (reportedEvent.Name == "ResumeEvent")
                    {
                        print("ResumeEvent");
                        riveScreen.ReturnToOriginalScene();
                    }
                    if (reportedEvent.Name == "SettingsEvent")
                    {
                        print("SettingsEvent");
                        riveScreen.SetRiveScene(RiveScreen.RiveScenes.SettingsMenu);
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
