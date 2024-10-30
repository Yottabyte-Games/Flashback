using UnityEngine;


public class Boss : OfficeWorker
{
    protected override void Start()
    {
        GenerateOfficeTask();

        base.Start();
    }
}
