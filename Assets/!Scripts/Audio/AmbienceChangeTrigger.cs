using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] private AmbienceArea area;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.SetAmbienceArea(area);
        }
    }
}
