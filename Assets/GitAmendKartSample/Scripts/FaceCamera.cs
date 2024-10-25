using UnityEngine;

namespace GitAmendKartSample.Scripts {
    public class FaceCamera : MonoBehaviour {
        [SerializeField] Transform kartCamera; 

        void Update() {
            if (kartCamera) {
                transform.rotation = kartCamera.rotation;
            }
        }
    }
}