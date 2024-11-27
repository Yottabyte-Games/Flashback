using Eflatun.SceneReference;
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using YottabyteGames.FinalCharacterController.Scripts;

namespace _Scripts.Rive
{
    [RequireComponent(typeof(RiveScreen))]
    public class GameHudController : MonoBehaviour
    {
        RiveScreen _riveScreen;
        SceneReference _sceneToLoad;
        PlayerPositionController _playerPositionController;
        PlayerController _playerController;
        PlayerLocomotionInput _playerLocomotionInput;

        InputAction _pauseAction;

        [SerializeField] private bool isDotHidden = false;
        
        void Awake()
        {
            if (_riveScreen is null)
            {
                _riveScreen = GetComponent<RiveScreen>();
                if (_riveScreen is null)
                {
                    Debug.LogError("No RiveScreen component found on " + gameObject.name);
                }
            }

            _riveScreen = GetComponent<RiveScreen>();
        
        
            _pauseAction = InputSystem.actions.FindAction("Pause");
        }

        void Start()
        {
            _riveScreen.OnRiveEvent += RiveEventHandler;
            if (_riveScreen.CurrentScene == RiveScreen.RiveScenes.HUD)
            {
                _riveScreen.StateMachine.GetTrigger("UnFlash").Fire();
                SetCursorHidden(isDotHidden);
            }
        
            if (SceneManager.GetActiveScene().name == "HubWorld 1")
            {
                _playerPositionController = transform.parent.GetComponent<PlayerPositionController>();
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _playerController = player.GetComponent<PlayerController>();
            _playerLocomotionInput = player.GetComponent<PlayerLocomotionInput>();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void RiveEventHandler(ReportedEvent reportedEvent)
        {
            if (reportedEvent.Name == "FlashbackEvent" && _sceneToLoad != null)
            {
                if (_playerPositionController)
                {
                    _playerPositionController.SavePosition(_sceneToLoad.Name);
                    print("Saved Position");
                }
                else
                    SceneManager.LoadScene(_sceneToLoad.Name);
            }
        }

        void Update()
        {
            if (_pauseAction.WasPressedThisFrame())
            {
                SetPlayerController(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Set Pause Scene from Rive
                _riveScreen.SetRiveScene(RiveScreen.RiveScenes.PauseMenu);
            }
            
        }

        public void SetPlayerController(bool state)
        {
            if (_playerController != null)
            {
                _playerController.enabled = state;
            }

            if (_playerLocomotionInput != null)
            {          
                _playerLocomotionInput.enabled = state;
            }
        }
        public void SetCursorHidden(bool value)
        {
            if (_riveScreen.CurrentScene == RiveScreen.RiveScenes.HUD)
            {
                _riveScreen.StateMachine.GetBool("HideCursor").Value = value;
            }
        }
        // First Dialogue should call this
        public void StartDialogue(string dialogueString)
        {
            SetDialogue(dialogueString);
            _riveScreen.StateMachine.GetTrigger("AddDialogue").Fire();
        }

        // Every other dialogues calls this
        public void NextDialogue(string dialogueString)
        {
            SetDialogue(dialogueString);
            _riveScreen.StateMachine.GetTrigger("NextDialogue").Fire();
        }

        // When last dialogue finishes, call this
        public void EndDialogue(SceneReference sceneReference = null)
        {
            _riveScreen.StateMachine.GetTrigger("RemoveDialogue").Fire();
            if (sceneReference != null)
            {
                _riveScreen.StateMachine.GetTrigger("FlashBack").Fire();
                _sceneToLoad = sceneReference;
            }
        }

        public void HoverOn(string objectName)
        {
            _riveScreen.SetTextRunAtPath(objectName, RiveScreen.TextPath.HUDItem);
            _riveScreen.StateMachine.GetBool("IsHovering").Value = true;
        }

        public void HoverOff()
        {
            _riveScreen.StateMachine.GetBool("IsHovering").Value = false;
        }


        // Set Dialogue Text for the next dialogue
        void SetDialogue(string dialogue)
        {
            _riveScreen.SetTextRunAtPath(dialogue, RiveScreen.TextPath.Dialogue);
        }
    }
}