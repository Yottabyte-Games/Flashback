using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EZCamera;

public class ExplosionTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject cat;

    private void Update()
    {
        if (cat.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                explosion.Play();

                Debug.Log("BLABLABLA");

                StartCoroutine(cameraShake.Shake(.15f, .4f));
            }
        }
    }
}
