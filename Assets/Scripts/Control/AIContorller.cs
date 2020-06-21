using UnityEngine;
using RuntimeSet;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIContorller : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private GameObjectRuntimeSet playerSet;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float waypointDwellTime = 1f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 0.5f;
        [Range(0,1)] [SerializeField] private float patrolSpeedFraction = 0.2f;

        private GameObject currentTarget;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private ActionSceduler actionSceduler;

        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        
        private void Start()
        {
            currentTarget = playerSet.GetItemIndex(0);
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionSceduler = GetComponent<ActionSceduler>();
            guardPosition = transform.position;
            print(playerSet);
        }

        private void Update()
        {
            if(health.IsDead) return;
            
            if (InAttackRangeOfPlayer() && fighter.CanAttack(currentTarget))
            {
                AttackBehaviour();
            } else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehavoiur();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehavoiur()
        {
            var nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        
        private bool AtWaypoint()
        {
            var distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            actionSceduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(currentTarget);
        }

        private bool InAttackRangeOfPlayer()
        {
            var distanceToPlayer = Vector3.Distance(currentTarget.transform.position, transform.position);
            return  distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}