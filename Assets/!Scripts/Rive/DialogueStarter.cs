using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using Eflatun.SceneReference;
using UnityEngine;
namespace _Scripts.Rive
{
    public class DialogueStarter : MonoBehaviour
    {
        [field: SerializeField] public DSDialogueSO StartingDialogue { get; private set; }
        [Header("Only needed if we are swapping scene")]
        [SerializeField]
        SceneReference sceneToLoad;
        [SerializeField] bool loadSceneAfterDialogue = false;

        bool _hasTalkedAlready = false;
        DialogueManager _dialogueManager;

        void Start()
        {
            _dialogueManager = GameObject.FindWithTag("MainCamera").GetComponent<DialogueManager>();
            if (_dialogueManager is null)
            {
                Debug.LogError("Dialogue Manager is null");
            }
        }

        void OnTriggerEnter(Collider other)
        {
        
            StartDialogue();
        }

        public void StartDialogue()
        {
            if (!_hasTalkedAlready)
            {
                if (loadSceneAfterDialogue)
                {
                    _dialogueManager.SetDialogue(StartingDialogue, sceneToLoad);
                    _hasTalkedAlready = true;
                }
                else
                    _dialogueManager.SetDialogue(StartingDialogue);
                _hasTalkedAlready = true;
            }
        
        }
        public void NextDialogue()
        {
            _dialogueManager.NextDialogue();
        }
    }
}
