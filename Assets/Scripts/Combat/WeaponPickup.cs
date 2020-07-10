using System;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weaponToEquip = null;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            other.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
            Destroy(gameObject);
        }
    }
}