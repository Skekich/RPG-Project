using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 4f;
        
        private Mover playerMover;
        private ActionSceduler actionSceduler;
        private Animator anim;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int AttackAnim = Animator.StringToHash("attack");
        private static readonly int StopPlayerAttack = Animator.StringToHash("stopAttack");

        private Health health;
        
        private void Start()
        {
            playerMover = GetComponent<Mover>();
            actionSceduler = GetComponent<ActionSceduler>();
            health = GetComponent<Health>();
            anim = GetComponent<Animator>();
        }    

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if(target == null) return;
            
            if (target.IsDead) return;

            if (!IsInRange())
            {
                playerMover.MoveTo(target.transform.position, 1f);
                return;
            }
            
            playerMover.Cancel();
            AttackBehaviour();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack < timeBetweenAttacks) return;
            TriggerAttack();
            timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            anim.ResetTrigger(StopPlayerAttack);
            anim.SetTrigger(AttackAnim);
        }

        //Animation event
        void Hit()
        {
            if(target == null) return;
            target.TakeDamage(weaponDamage);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            return !health.IsDead && combatTarget != null;
        }
        
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            actionSceduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        
        public void Cancel()
        {
            StopAttack();
            playerMover.Cancel();
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger(AttackAnim);
            anim.SetTrigger(StopPlayerAttack);
        }
    }
}