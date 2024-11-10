using System;
using UnityEngine;

namespace _Scripts.Working
{
    [Serializable]
    public class WorkerPosition 
    {
        public Transform transform;
#nullable enable
        public OfficeWorker? workerInPosition;
#nullable disable

        public void SetWorker(OfficeWorker worker)
        {
            workerInPosition = worker;
        }
    }
}
