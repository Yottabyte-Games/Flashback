using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EZCamera;

public class ExplosionTrigger : MonoBehaviour
{
    public ParticleSystem explosion;
    public CameraShake cameraShake;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            explosion.Play();

            Debug.Log("BLABLABLA");
          
            StartCoroutine(cameraShake.Shake(.15f, .4f));
        }
    }
}
