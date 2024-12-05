using System;
using UnityEngine;
namespace _Scripts.Audio
{
    public class AmbienceChangeTrigger : MonoBehaviour
    {
        [Header("Area")]
        [SerializeField] AmbienceArea area;
        AudioManager _audioManager;
        void Start()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _audioManager.SetAmbienceArea(area);
            }
        }
    }
}
