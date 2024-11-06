namespace _Scripts.Working
{
    public class Boss : OfficeWorker
    {
        protected override void Start()
        {
            GenerateOfficeTask();

            base.Start();
        }
    }
}
