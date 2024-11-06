using UnityEngine;

namespace YottabyteGames.Scripts
{
    public class FlashbackLightning : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        Light directionalLight;

        [Header("Settings")]
        [SerializeField]
        float disableTime = 10f;

        float elapsedTime = 0f;

        void Start()
        {
            if (directionalLight == null)
            {
                directionalLight = GetComponent<Light>();
            }
        }

        void Update()
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
}
