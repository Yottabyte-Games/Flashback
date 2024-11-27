using _Scripts.Audio;
using Eflatun.SceneReference;
using Rive;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Rive
{

    public class RiveEventHandler : MonoBehaviour
    {
        [SerializeField] SceneReference hubWorldScene;
    
        RiveScreen _riveScreen;
        AudioManager _audioManager;
        PlayerPositionController _playerPositionController;
        void Start()
        {
            _riveScreen = GetComponent<RiveScreen>();
            _audioManager = AudioManager.Instance;
            if (SceneManager.GetActiveScene().name == hubWorldScene.Name)
            {
                _playerPositionController = transform.parent.GetComponent<PlayerPositionController>();
            }
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
                            SceneManager.LoadScene(hubWorldScene.Name);
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
                            SceneManager.LoadScene("ToyCarMinigame");
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
                            //print("Master Change Event");
                            //Get value between 1 and 100
                            //print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Master Slider"));
                            float masterVolume = (float)(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Master Slider") / 100f);
                            UpdateVolume("Master", masterVolume);
                            break;
                        case "Music Change Event":
                            //print("Music Change Event");
                            //Get value between 1 and 100
                            //print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Music Slider"));
                            float musicVolume = (float)(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Music Slider") / 100f);
                            UpdateVolume("Music", musicVolume);
                            break;
                        case "SFX Change Event":
                            //print("SFX Change Event");
                            //Get value between 1 and 100
                            //print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "SFX Slider"));
                            float sfxVolume = (float)(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "SFX Slider") / 100f);
                            UpdateVolume("SFX", sfxVolume);
                            break;
                        case "Voice Change Event":
                            //print("Voice Change Event");
                            //Get value between 1 and 100
                            //print(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Voice Slider"));
                            float voiceVolume = (float)(_riveScreen.artboard.GetNumberInputStateAtPath("SliderValue", "Voice Slider") / 100f);
                            UpdateVolume("Voice", voiceVolume);
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
                        if (_playerPositionController)
                        {
                            _playerPositionController.SavePosition("MainMenu");
                            print("saving position");
                        }
                        else
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
        void UpdateVolume(string volumeType, float volume)
        {
            if (_audioManager is not null)
            {
                switch (volumeType)
                {
                    case "Master":
                        _audioManager.MasterVolume = volume;
                        break;
                    case "Music":
                        _audioManager.MusicVolume = volume;
                        break;
                    case "SFX":
                        _audioManager.SfxVolume = volume;
                        break;
                    case "Voice":
                        _audioManager.VoiceVolume = volume;
                        break;
                    default:
                        Debug.LogError("Invalid volume type specified.");
                        break;
                }
            }
            else
            {
                Debug.LogError("AudioManager is not assigned.");
            }
        }

        void OnDisable()
        {
            _riveScreen.OnRiveEvent -= RiveEventHappens;
        }
    }

}
