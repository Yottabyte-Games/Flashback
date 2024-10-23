using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class String : MonoBehaviour
    {
        public List<Transform> stringPoints;
        LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, stringPoints[i].position);
            }
        }
        private void OnDrawGizmosSelected()
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                GetComponent<LineRenderer>().SetPosition(i, stringPoints[i].position);
            }
        }
    }
}
