using System;
using UnityEngine;

namespace RuntimeSet
{
    public class AddGameObjectToRuntimeSet : MonoBehaviour
    {
        public GameObjectRuntimeSet gameObjectRuntimeSet;
        private void OnEnable()
        {
            gameObjectRuntimeSet.AddToList(this.gameObject);
        }

        private void OnDisable()
        {
            gameObjectRuntimeSet.RemoveFromList(this.gameObject);
        }
    }
}
