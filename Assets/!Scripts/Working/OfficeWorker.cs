using System.Collections;
using UnityEngine;

public enum Activity
{
    Nothing,
    Working,
    Break,
    Meeting
}

public class OfficeWorker : Creature
{
    public Activity activity { get; private set; }
    public Transform officeStation;
    public Transform meetingRoom;
    public Transform breakRoom;

    IEnumerator Start()
    {
        SetDestination(officeStation.position);

        while (true)
        {
            SetActivity(Activity.Working);

            if (Random.Range(0, 5) == 4)
            {
                SetActivity(Activity.Break);
            }
            else if (Random.Range(0, 11) == 10)
            {
                SetActivity(Activity.Meeting);
            }

            yield return new WaitForSeconds(Random.Range(20, 60));
        }
    }

    public void SetActivity(Activity activity)
    {
        this.activity = activity;

        UpdateActivity();
    }

    void UpdateActivity()
    {
        switch (activity)
        {
            case Activity.Nothing:
                break;
            case Activity.Working:
                SetDestination(officeStation.position);
                break;
            case Activity.Break:
                SetDestination(breakRoom.position);
                break;
            case Activity.Meeting:
                SetDestination(meetingRoom.position);
                break;
        }
    }
}
