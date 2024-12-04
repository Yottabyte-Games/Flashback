using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public abstract class Creature : MonoBehaviour
    {
        protected NavMeshAgent agent { get; private set;}
        protected Transform destination { get; private set;}
        public bool reachedDestination
        {
            get
            {
                return !agent.hasPath || agent.velocity.sqrMagnitude == 0;
            }
        }

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            if (destination == null) return;
            agent.destination = destination.position;
        }

        public void SetDestination(Transform pos)
        {
            if (!Application.isPlaying) return;
            destination = pos;
        }
    }
}
