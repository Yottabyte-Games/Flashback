using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        public List<Transform> stringPoints = new List<Transform>();
        [SerializeField] LineRenderer lineRenderer;

<<<<<<< HEAD:Assets/Scenes/Line.cs
    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        for (var i = 0; i < stringPoints.Count; i++)
=======
        private void Start()
>>>>>>> Build:Assets/!Scripts/Line.cs
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }
<<<<<<< HEAD:Assets/Scenes/Line.cs
    }

    void OnDrawGizmos()
    {
        for (var i = 0; i < stringPoints.Count; i++)
=======
        private void Update()
>>>>>>> Build:Assets/!Scripts/Line.cs
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, stringPoints[i].position);
            }
        }
        private void OnDrawGizmos()
        {
            for (var i = 0; i < stringPoints.Count; i++)
            {
                GetComponent<LineRenderer>().SetPosition(i, stringPoints[i].position);
            }
        }

<<<<<<< HEAD:Assets/Scenes/Line.cs
    void OnEnable()
    {
        lineRenderer.enabled = true;
    }

    void OnDisable()
    {
        lineRenderer.enabled = false;
=======
        private void OnEnable()
        {
            lineRenderer.enabled = true;
        }
        private void OnDisable()
        {
            lineRenderer.enabled = false;
        }
>>>>>>> Build:Assets/!Scripts/Line.cs
    }
}
