using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Snowman_Scripts
{
    public class PhotoCapture : MonoBehaviour
    {
        [SerializeField] GameObject onScreenPhoto;
        [SerializeField] GameObject cameraOverlay;
        bool _uiOn;
        bool _canOpenUI = true;
        [SerializeField] Image displayImage;

        [SerializeField] Animator animator;

        [SerializeField] GameObject snowball;

        Texture2D _capturedImage;

        public UnityEvent FishedPhotoTaken;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _capturedImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && _canOpenUI) { ToggleUI();}
            if(_uiOn && Input.GetKeyDown(KeyCode.E)) { StartCoroutine("CaptureImage"); }
        }

        void ToggleUI()
        {
            _uiOn = !_uiOn;
            cameraOverlay.SetActive(_uiOn);

        }

        IEnumerator CaptureImage()
        {
            _uiOn = false;
            _canOpenUI = false;
            cameraOverlay.SetActive(false);
            yield return new WaitForEndOfFrame();
            //Capture image and convert to Sprite

            //Capture screen as texture
            Rect regionToRead = new Rect(0.0f, 0.0f, _capturedImage.width, _capturedImage.height);
            _capturedImage.ReadPixels(regionToRead, 0, 0, false);
            _capturedImage.Apply();
            Sprite textureToSprite = Sprite.Create(_capturedImage, new Rect(0.0f, 0.0f, _capturedImage.width, _capturedImage.height), new Vector2(0.5f,0.5f), 100);
        
            //Enable all objects and animations
            onScreenPhoto.SetActive(true);
            animator.Play("ImageFade");
            displayImage.sprite = textureToSprite;

            Invoke(nameof(DisplayImage), 3f);
        }

        void DisplayImage()
        {
            onScreenPhoto.SetActive(false);
            displayImage.GetComponent<CanvasGroup>().alpha = 0f;
            _capturedImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            _canOpenUI = true;

            //Check if completed and go out of scene. Else remove image
            CheckCompleted();
        }
        
        void CheckCompleted()
        {
            int objects = 0;
            var colliders = Physics.OverlapSphere(snowball.gameObject.transform.position, 5);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Interactable"))
                {
                    objects++;
                }
            }

            if (objects > 5) 
            {
                Debug.Log("Finished");
                FishedPhotoTaken.Invoke();
            }
        }

    }
}
