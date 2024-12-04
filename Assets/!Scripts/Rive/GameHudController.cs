using System.Collections;
using System.Collections.Generic;
using _Scripts.Working.Tasks;
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
                    ResumeGame();
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

        public void ResumeGame()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SetPlayerController(true);
            _riveScreen.ReturnToOriginalScene();
            SetCursorHidden(false);
            SetCursorHidden(GetIsDotHidden());
            
            if (SceneManager.GetActiveScene().name == "Working")
            {
                StartCoroutine(SetWorkUI());
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

        #region WorkTaskUI
        
        private OfficeTask[] visibleTasks = new OfficeTask[7]; // Fixed-size array for 7 visible tasks

        List<OfficeTask> overflowntasks = new (); // Dynamic list of tasks

        private const int FullyVisibleTasks = 4; // Total UI slots (4 full + 3 indicators)
        

        public void AddTaskUI(OfficeTask newTask) // Add task to empty space or add to overflow
        {
            bool notAddedToTaskbar = true;
            // Find the first empty slot in array
            for (int i = 0; i < visibleTasks.Length; i++)
            {
                if (visibleTasks[i] == null)
                {
                    visibleTasks[i] = newTask; // Assign the task to the empty visible slot
                    notAddedToTaskbar = false; // task was added
                    StartCoroutine(SetWorkUI()); // Updates the UI
                    break;
                }
            }
            // If no available slots, place task in overflow
            if (notAddedToTaskbar)
            {
                overflowntasks.Add(newTask); // Add the task to overflow when all slots are taken
            }
        }

        public void RemoveTaskUI(OfficeTask finishedTask) // Remove task and refill array
        {
            bool removedTask = false;
            // Remove from visibleTasks and add new from overflow if possible
            for (int i = 0; i < visibleTasks.Length; i++)
            {
                // Removes Task
                if (visibleTasks[i].taskIndex == finishedTask.taskIndex)
                {
                    visibleTasks[i] = null;
                    removedTask = true;
                    StartCoroutine(SetWorkUI()); // Update UI to visibly remove task
                    
                    // Try to fill empty task
                    // Check if tasks in obscured position can fill
                    for (int j = FullyVisibleTasks; j < visibleTasks.Length; j++)
                    {
                        // Try to move from obscured to visible
                        if (visibleTasks[j] != null)
                        {
                            // Move item from first to second
                            visibleTasks[i] = visibleTasks[j];
                            visibleTasks[j] = null;
                            StartCoroutine(SetWorkUI()); // Update UI to visibly remove task
                            
                            // Move from overflow to obscured
                            if (overflowntasks.Count > 0)
                            {
                                // Add to array and Remove from overflow
                                visibleTasks[j] = overflowntasks[0];
                                overflowntasks.RemoveAt(0);
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            if (!removedTask)
            {
                overflowntasks.Remove(finishedTask);
                print("Tryng to remove the task from Overflow");

            }

            StartCoroutine(SetWorkUI());
        }

        
        private IEnumerator SetWorkUI()
        {
            yield return new WaitForSeconds(0.1f); // Allow previous animation to activate
            // Set all icons to correct task and remove if empty
            for (int i = 0; i < visibleTasks.Length; i++)
            {
                string path = "WorkTask " + i;
                
                if (visibleTasks[i] != null) // Task exists in this slot
                {
                    _riveScreen.Artboard.SetTextRunValueAtPath("Task Index Run", path, visibleTasks[i].taskIndex.ToString());
                    _riveScreen.Artboard.SetTextRunValueAtPath("Description Run", path, visibleTasks[i].taskName);
                    _riveScreen.Artboard.SetBooleanInputStateAtPath("ShowTask", true, path);
                    
                }
                else
                {
                    _riveScreen.Artboard.SetBooleanInputStateAtPath("ShowTask", false, path); // Hide empty visible slots
                }
            }
            yield return new WaitForSeconds(0.1f); // Allow previous animation to activate

        }

        #endregion
    }
}