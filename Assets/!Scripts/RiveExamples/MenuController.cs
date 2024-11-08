using System;
using Plugins.Rive.UI;
using Rive;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.RiveExamples
{
    [RequireComponent(typeof(RiveScreen))]
    public class MenuController : MonoBehaviour
    {
        [SerializeField] int _quitValue = 6;

        RiveScreen _riveScreen;

        MenuAudioSystem _menuAudioSystem;

        void Start()
        {
            _riveScreen = GetComponent<RiveScreen>();
            _riveScreen.OnRiveEvent += RiveScreen_OnRiveEvent;

            _menuAudioSystem = FindFirstObjectByType<MenuAudioSystem>();
        }

        void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            if (Int32.TryParse(reportedEvent.Name, out int number))
            {
                _menuAudioSystem?.PlayClickSound();

                if (number == _quitValue)
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif

                LoadScene((int)number);
            }

            if (reportedEvent.Name == "OnHover")
            {
                _menuAudioSystem?.PlayHoverSound();
            }
        }

        void LoadScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
        }

        void OnDisable()
        {
            if (_riveScreen != null)
            {
                _riveScreen.OnRiveEvent -= RiveScreen_OnRiveEvent;
            }
        }
    }
}
