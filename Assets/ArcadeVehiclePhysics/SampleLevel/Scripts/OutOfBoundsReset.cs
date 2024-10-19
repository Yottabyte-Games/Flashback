using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
namespace ArcadeVehiclePhysics.SampleLevel.Scripts
{
    public class OutOfBoundsReset : MonoBehaviour
    {
        CpMain _cpMain;

        // Start is called before the first frame update
        void Start()
        {
            _cpMain = FindFirstObjectByType<CpMain>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetPlayer(_cpMain.rb);
            }

        }

        static void ResetPlayer(Rigidbody rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.transform.position = Vector3.up;
            rb.transform.rotation = Quaternion.identity;
        }

        void OnTriggerExit(Collider other)
        {
            ResetPlayer(_cpMain.rb);
        }
    }
}
