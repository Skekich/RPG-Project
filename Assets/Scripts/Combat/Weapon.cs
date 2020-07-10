using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float range = 0f;

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float Range
        {
            get => range;
            set => range = value;
        }

        public void Spawn(Transform handTransform, Animator anim)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);    
            }

            if (animatorOverride != null)
            {
                anim.runtimeAnimatorController = animatorOverride;    
            }
            
        }
    }
}