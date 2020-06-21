using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeSet
{
    public class GenericRuntimeSet<T> : ScriptableObject
    {
        private List<T> items = new List<T>();

        private void Initialize()
        {
            items.Clear();
        }

        public T GetItemIndex(int index)
        {
            return items[index];
        }

        public void AddToList(T thingsToAdd)
        {
            if (!items.Contains(thingsToAdd))
            {
                items.Add(thingsToAdd);
            }
        }

        public void RemoveFromList(T thingsToRemove)
        {
            if (items.Contains(thingsToRemove))
            {
                items.Remove(thingsToRemove);
            }
        }
    }
}