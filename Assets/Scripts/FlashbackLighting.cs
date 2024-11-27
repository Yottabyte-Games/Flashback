using Unity.VisualScripting;
using UnityEngine;

public class FlashbackLightning : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light directionalLight;

    [Header("Settings")]
    [SerializeField] private float disableTime = 10f;

    private float elapsedTime = 0f;

    private void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= disableTime)
        {
            if (directionalLight != null)
            {
                directionalLight.enabled = false;
                enabled = false;
            }
        }
    }
}
