using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BF_InteractiveEffects : MonoBehaviour
{
    public Transform transformToFollow;
    public RenderTexture rt;
    public string GlobalTexName = "_GlobalEffectRT";
    public string GlobalOrthoName = "_OrthographicCamSize";
    public bool isPaced;

    float orthoMem;
    Coroutine waitPace;
    bool paceRunning;

    void Awake()
    {
        orthoMem = GetComponent<Camera>().orthographicSize;
        Shader.SetGlobalFloat(GlobalOrthoName, orthoMem);
        Shader.SetGlobalTexture(GlobalTexName, rt);
        Shader.SetGlobalFloat("_HasRT", 1);
    }

    void OnEnable()
    {
        orthoMem = GetComponent<Camera>().orthographicSize;
        Shader.SetGlobalFloat(GlobalOrthoName, orthoMem);
        Shader.SetGlobalTexture(GlobalTexName, rt);
        Shader.SetGlobalFloat("_HasRT", 1);
    }

    void MoveCamera()
    {
        if (transformToFollow != null)
        {
            transform.position = new Vector3(transformToFollow.position.x, transformToFollow.position.y + 20, transformToFollow.position.z);
        }
        Shader.SetGlobalVector("_Position", transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    void Update()
    {
        if(isPaced)
        {
            if(!paceRunning)
            {
                waitPace = StartCoroutine(WaitPace());
            }
        }

        else
        {
            if (paceRunning)
            {
                paceRunning = false;
                StopCoroutine(WaitPace());
            }

            MoveCamera();
        }
    }

    IEnumerator WaitPace()
    {
        for(; ;)
        {
            paceRunning = true;

            MoveCamera();

            yield return new WaitForSeconds(1f);
        }
    }
}