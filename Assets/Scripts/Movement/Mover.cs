using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private float maxSpeed = 6f;
    
        private NavMeshAgent navMeshAgent;
        private Animator animatorAgent;
        private ActionSceduler actionSceduler;
        private Vector3 destination;
        private Health health;

        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animatorAgent = GetComponent<Animator>();
            actionSceduler = GetComponent<ActionSceduler>();
            health = GetComponent<Health>();
            destination = navMeshAgent.destination;
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 pointToMove, float speedFraction)
        {
            actionSceduler.StartAction(this);
            MoveTo(pointToMove, speedFraction);
        }
        
        public void MoveTo(Vector3 pointToMove, float speedFraction)
        {
            destination = pointToMove;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        
        private void UpdateAnimator()
        {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            animatorAgent.SetFloat(ForwardSpeed, speed);
        }
    }
}

