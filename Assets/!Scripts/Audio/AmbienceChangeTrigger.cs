using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] AmbienceArea area;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.SetAmbienceArea(area);
        }
    }
}
