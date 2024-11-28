using UnityEngine;
using UnityEngine.AI;

public enum CarType
{
    Swap = 0,
    EnteringCar = -1372625422,
    ExitingCar = -334000983,
}

public class CarSwapper : MonoBehaviour
{
    [SerializeField] CarType setToCarType;

    [SerializeField] bool onExit;
    private void OnTriggerEnter(Collider other)
    {
        if (onExit) return;

        SetCarType(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!onExit) return;

        SetCarType(other);
    }

    void SetCarType(Collider other)
    {
        if (other.TryGetComponent(out CarDecal car))
        {
            Transform destination = car.Destination;

            switch (setToCarType)
            {
                case CarType.Swap:
                    switch (car.agent.agentTypeID)
                    {
                        case -1372625422: //entering car
                            car.agent.agentTypeID = -334000983; //become exiting car
                            break;
                        case -334000983: //exiting car
                            car.agent.agentTypeID = -1372625422; //become entering car
                            break;
                    }
                    break;
                case CarType.ExitingCar:
                    car.agent.agentTypeID = (int)CarType.ExitingCar;
                    break;
                case CarType.EnteringCar:
                    car.agent.agentTypeID = (int)CarType.EnteringCar;
                    break;
            }

            car.Destination = destination;
        }
    }

}
