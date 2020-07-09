using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoint = 100f;
        
        private Animator anim;
        private ActionSceduler actionSceduler;
        
        private static readonly int Die = Animator.StringToHash("die");
        private bool isDead = false;

        public bool IsDead => isDead;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionSceduler = GetComponent<ActionSceduler>();
        }

        public void TakeDamage(float damage)
        {
            healthPoint = Mathf.Max(healthPoint -damage, 0);
            print(healthPoint);
            if (!(healthPoint <= 0)) return;
            Died();
        }

        private void Died()
        {
            if(isDead) return;
            isDead = true;
            anim.SetTrigger(Die);
            actionSceduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoint;
        }

        public void RestoreState(object state)
        {
            healthPoint = (float)state;
            if (!(healthPoint <= 0)) return;
            Died();
        }
    }
}