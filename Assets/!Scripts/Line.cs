using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        public List<Transform> stringPoints = new List<Transform>();
        [SerializeField] LineRenderer lineRenderer;

        void Start()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, stringPoints[i].position);
            }
        }

        void OnDrawGizmos()
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                GetComponent<LineRenderer>().SetPosition(i, stringPoints[i].position);
            }
        }

        void OnEnable()
        {
            lineRenderer.enabled = true;
        }

        void OnDisable()
        {
            lineRenderer.enabled = false;
        }
    }
}
