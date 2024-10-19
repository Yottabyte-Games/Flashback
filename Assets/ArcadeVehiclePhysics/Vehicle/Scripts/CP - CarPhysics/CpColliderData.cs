using System;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics
{
    public class CpColliderData : MonoBehaviour
    {
        CpMain _cpMain;
    
        public static event Action<Collision> OnCollision = collisionPosition => { };

        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision other)
        {
            OnCollision(other);
        }

        void OnCollisionStay(Collision collision)
        {
            Vector3 surfaceNormalSum = Vector3.zero;
            for (int i = 0; i < collision.contactCount; i++)
            {
                surfaceNormalSum += collision.contacts[i].normal;
            }
            _cpMain.averageColliderSurfaceNormal = surfaceNormalSum.normalized;
        }

        void OnCollisionExit(Collision collision)
        {
            _cpMain.averageColliderSurfaceNormal = Vector3.zero;
        }
    }
}
