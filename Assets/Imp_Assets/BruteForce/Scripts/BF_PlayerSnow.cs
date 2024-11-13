using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_PlayerSnow : MonoBehaviour
{
    public Collider playerCollider;
    public ParticleSystem particleSys;

    public PhysicsMaterial playerMatDefault;
    public PhysicsMaterial playerMatSnow;
    public PhysicsMaterial playerMatIce;

    Rigidbody rB;
    float speedMult = 1;
    float lerpIce;
    MeshCollider oldMC;
    Mesh mesh;
    ParticleSystem.MainModule pSMain;


    // Start is called before the first frame update
    void Start()
    {
        oldMC = null;
        mesh = null;
        rB = this.GetComponent<Rigidbody>();
        pSMain = particleSys.main;
    }

    void CheckIceCols(float snowCol)
    {
        lerpIce = snowCol / 255f;

        if (snowCol == -1)
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
            return;
        }


        if (lerpIce <= 0.925f && playerCollider.material != playerMatIce)
        {
            playerCollider.material = playerMatIce;
            rB.angularDamping = 0.25f;
        }
        else if(lerpIce >= 0.925f && playerCollider.material != playerMatSnow)
        {
            playerCollider.material = playerMatSnow;
            rB.angularDamping = 5f;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (lerpIce >= 0.925f && collision.collider.gameObject.layer == 4)
        {
            AddSnow(1.5f);
        }
        else
        {
            RemoveSnow(0.05f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude>10)
        {
           // RemoveSnow(20);
        }
    }

    void AddSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x < 7f)
        {
            speedMult = Mathf.Clamp(rB.linearVelocity.magnitude * 0.02f,0,1);
            playerCollider.transform.localScale += Vector3.zero + Vector3.one * 0.0035f * 2 * multiplier* speedMult;
            playerCollider.transform.localScale += Vector3.zero + Vector3.one * 0.005f * 2 * multiplier * speedMult;
        }
    }

    void RemoveSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x >= 1.1f)
        {
            if (!playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(true);
            }
            playerCollider.transform.localScale -= Vector3.zero + Vector3.one * 0.0035f * 4 * multiplier;
            playerCollider.transform.localScale -= Vector3.zero + Vector3.one * 0.005f * 4 * multiplier;
        }
        if (playerCollider.transform.localScale.x < 1.1f)
        {
            if (playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(false);
            }
            playerCollider.transform.localScale = Vector3.one * 1.1f;
            playerCollider.transform.localScale = Vector3.one * 1.1f;
        }
    }

    void FixedUpdate()
    {
        ChangePlayerMass();
        CheckSnowUnderneath();
    }

    void CheckSnowUnderneath()
    {
        RaycastHit hit;

        var layerMask = 1 << 4;
        if (Physics.Raycast(transform.position+(Vector3.down*(playerCollider.transform.localScale.x/2)+Vector3.up*0.5f), Vector3.down, out hit, 5, layerMask,QueryTriggerInteraction.Ignore))
        {
            var meshCollider = hit.collider as MeshCollider;
            if (oldMC != meshCollider || mesh == null)
            {
                mesh = meshCollider.GetComponent<MeshFilter>().sharedMesh;
            }
            oldMC = meshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                CheckIceCols(255f);
                return;
            }

            //Mesh mesh = meshCollider.sharedMesh;

            var triangles = mesh.triangles;
            Color32[] colorArray;
            colorArray = mesh.colors32;

            var vertIndex1 = triangles[hit.triangleIndex * 3 + 0];
            CheckIceCols(((float)colorArray[vertIndex1].g) / 1);
        }
        else
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
        }
    }

    void ChangePlayerMass()
    {
        rB.mass = Mathf.Lerp(1.95f, 2.5f, (playerCollider.transform.localScale.x-1.2f) / 7);
        pSMain.startSize = playerCollider.transform.localScale.x+0.5f;
    }
}
