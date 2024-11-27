using UnityEngine;
using UnityEngine.AI;

public class Crosspath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.TryGetComponent(out CarDecal car))
        {
            print(car);
            Transform destination = car.Destination;

            switch (car.agent.agentTypeID)
            {
                case -1372625422: //RightCar
                    car.agent.agentTypeID = -334000983; //become left car
                    break;
                case -334000983: //LeftCar
                    car.agent.agentTypeID = -1372625422; //become right car
                    break;
            }

            car.Destination = destination;
        }
    }
}
