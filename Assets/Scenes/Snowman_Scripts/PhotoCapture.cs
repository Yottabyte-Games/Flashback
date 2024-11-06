using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [SerializeField] GameObject onScreenPhoto;
    [SerializeField] GameObject cameraOverlay;
    bool uiOn;
    bool canOpenUI = true;
    [SerializeField] Image displayImage;

    [SerializeField] Animator animator;

    [SerializeField] GameObject snowball;

    Texture2D capturedImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capturedImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canOpenUI) { ToggleUI();}
        if(uiOn && Input.GetKeyDown(KeyCode.E)) { StartCoroutine("CaptureImage"); }
    }

    void ToggleUI()
    {
        uiOn = !uiOn;
        cameraOverlay.SetActive(uiOn);

    }

    IEnumerator CaptureImage()
    {
        uiOn = false;
        canOpenUI = false;
        cameraOverlay.SetActive(false);
        yield return new WaitForEndOfFrame();
        //Capture image and convert to Sprite

        //Capture screen as texture
        Rect regionToRead = new Rect(0.0f, 0.0f, capturedImage.width, capturedImage.height);
        capturedImage.ReadPixels(regionToRead, 0, 0, false);
        capturedImage.Apply();
        Sprite textureToSprite = Sprite.Create(capturedImage, new Rect(0.0f, 0.0f, capturedImage.width, capturedImage.height), new Vector2(0.5f,0.5f), 100);
        
        //Enable all objects and animations
        onScreenPhoto.SetActive(true);
        animator.Play("ImageFade");
        displayImage.sprite = textureToSprite;

        Invoke("DisplayImage", 3f);
    }

    void DisplayImage()
    {
        onScreenPhoto.SetActive(false);
        displayImage.GetComponent<CanvasGroup>().alpha = 0f;
        capturedImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        canOpenUI = true;

        //Check if completed and go out of scene. Else remove image
        CheckCompleted();
    }

    void CheckCompleted()
    {
        int objects = 0;
        var colliders = Physics.OverlapSphere(snowball.gameObject.transform.position, 3);
        foreach (var Collider in colliders)
        {
            if (Collider.gameObject.tag == "Interactable");
            objects++;
        }

        if (objects > 7) 
        {
            Debug.Log("Finished");
            // Swap Scene
        }
    }

}
