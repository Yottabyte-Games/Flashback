using System.Collections.Generic;
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
                if (_riveScreen.CurrentScene == RiveScreen.RiveScenes.PauseMenu)
                {
                    _riveScreen.ReturnToOriginalScene();
                    SetPlayerController(true);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    SetPlayerController(false);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    // Set Pause Scene from Rive
                    _riveScreen.LoadSceneMode(RiveScreen.RiveScenes.PauseMenu);
                }
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

        public bool GetIsDotHidden()
        {
            return isDotHidden;
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

        
        List<string> taskNames = new (); // Dynamic list of tasks
        private string[] visibleTasks = new string[7]; // Fixed-size array for the 4 visible tasks

        private const int MaxVisibleTasks = 7; // Total UI slots (4 full + 3 indicators)

        public void AddTaskUI(string newTask)
        {
            taskNames.Add(newTask); // Add the task to the dynamic list

            // Find the first empty slot in the visibleTasks array
            for (int i = 0; i < visibleTasks.Length; i++)
            {
                if (visibleTasks[i] == null)
                {
                    visibleTasks[i] = newTask; // Assign the task to the empty visible slot
                    break;
                }
            }

            SetWorkUI();
        }

        public void RemoveTaskUI(string finishedTask)
        {
            // Remove from visibleTasks if present
            for (int i = 0; i < visibleTasks.Length; i++)
            {
                if (visibleTasks[i] == finishedTask)
                {
                    visibleTasks[i] = null; // Mark the slot as empty
                    break;
                }
            }

            // Remove from taskNames
            taskNames.Remove(finishedTask);

            SetWorkUI();
        }

        private void SetWorkUI()
        {
            for (int i = 0; i < MaxVisibleTasks; i++)
            {
                string path = "WorkTask " + i;

                if (i < visibleTasks.Length) // Handle visible tasks
                {
                    if (visibleTasks[i] != null) // Task exists in this slot
                    {
                        _riveScreen.Artboard.SetTextRunValueAtPath("Task Index Run", path, (i + 1).ToString());
                        _riveScreen.Artboard.SetTextRunValueAtPath("Description Run", path, visibleTasks[i]);
                        _riveScreen.Artboard.FireInputStateAtPath("Show", path);
                    }
                    else
                    {
                        _riveScreen.Artboard.FireInputStateAtPath("Hide", path); // Hide empty visible slots
                    }
                }
            }
        }
    }
}