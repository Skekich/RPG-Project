using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        
        static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();
        
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var savable in GetComponents<ISaveable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>)state;
            foreach (var savable in GetComponents<ISaveable>())
            {
                var typeString = savable.GetType().ToString();
                if (!stateDict.ContainsKey(typeString)) return;
                savable.RestoreState(stateDict[typeString]);
            }
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if(string.IsNullOrEmpty(gameObject.scene.path)) return;
            
            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif
        
        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;
            if (globalLookup[candidate] == this) return true;
            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }
            if (globalLookup[candidate].GetUniqueIdentifier() == candidate) return false;
            globalLookup.Remove(candidate);
            return true;
        }
    }
}