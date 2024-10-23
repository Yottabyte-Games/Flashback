using System;
using UnityEngine;

namespace _Scripts.Vehicle.CP_CarPhysics
{
    public class CpColliderData : MonoBehaviour
    {
        CpMain _cpMain;
    
        public static event Action<Collision> OnCollision = collisionPosition => { };

        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();
        }
        void OnCollisionEnter(Collision other)
        {
            OnCollision(other);
        }

        void OnCollisionStay(Collision collision)
        {
            var surfaceNormalSum = Vector3.zero;
            for (var i = 0; i < collision.contactCount; i++)
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
