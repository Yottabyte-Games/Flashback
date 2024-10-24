using System;
using UnityEngine;

[Serializable]
public class WorkerPosition 
{
    public Transform position;
#nullable enable
    public OfficeWorker? workerInPosition;
#nullable disable

    public void SetWorker(OfficeWorker worker)
    {
        workerInPosition = worker;
    }
}
