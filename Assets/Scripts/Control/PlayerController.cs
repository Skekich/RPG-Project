using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover movePlayer;
        private Fighter playerAttack;
        private Camera myCam;
        private Health health;
        private void Start()
        {
            movePlayer = GetComponent<Mover>();
            playerAttack = GetComponent<Fighter>();
            health = GetComponent<Health>();
            myCam = Camera.main;
        }

        private void Update()
        {
            if (health.IsDead) return;
            
            if(InteractWithCombat()) return;
            InteractWithMovement();
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;
                
                if(!playerAttack.CanAttack(target.gameObject)) continue;
                
                if(Input.GetMouseButton(0))
                {
                    playerAttack.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;
            if (Input.GetMouseButton(0))
            {
                movePlayer.StartMoveAction(hit.point, 1f);
            }
            return true;
        }

        private Ray GetMouseRay()
        {
            return myCam.ScreenPointToRay(Input.mousePosition);
        }
    }
}
