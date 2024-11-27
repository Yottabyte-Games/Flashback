using Eflatun.SceneReference;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace _Scripts.Rive
{
    [RequireComponent(typeof(RiveScreen))]
    public class GameHudController : MonoBehaviour
    {
        RiveScreen _riveScreen;
        SceneReference _sceneToLoad;
        PlayerPositionController _playerPositionController;

        InputAction _pauseAction;

        [SerializeField] private bool isCursorVisible = true;
        
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
                ChangeCursor(isCursorVisible);
            }
        
            if (SceneManager.GetActiveScene().name == "HubWorld 1")
            {
                _playerPositionController = transform.parent.GetComponent<PlayerPositionController>();
            }
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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Set Pause Scene from Rive
                _riveScreen.SetRiveScene(RiveScreen.RiveScenes.PauseMenu);
            }
            
        }

        public void ChangeCursor(bool value)
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