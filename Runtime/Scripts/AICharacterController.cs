using UnityEngine;
using UnityEngine.AI;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class AICharacterController : BaseController
    {
        NavMeshAgent agent;
        public float targetMinDist = 10f;
        public float moveSpeed = 10f;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, agent.destination) < targetMinDist)
            {
                agent.SetDestination(Random.insideUnitSphere * 350);
            }
            movementAxis = Vector3.MoveTowards(transform.position, agent.nextPosition, 1);
            movementAxis.Normalize();
            movementAxis *= moveSpeed;

            UpdateLocomotion();
        }
    }
}