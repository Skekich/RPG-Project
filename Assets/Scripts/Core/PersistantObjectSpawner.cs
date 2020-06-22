using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistantObjectPrefab;

        private static bool hasSpawned = false;
        private void Awake()
        {
            if(hasSpawned) return;

            SpawnPersistantObjects();
            
            hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            var persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}