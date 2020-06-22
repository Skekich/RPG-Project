using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private float fadeOut = 2f;
        [SerializeField] private float fadeIn = 2f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private enum DestinationPortal
        {
            A, B, C, D, E
        }
        
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationPortal destination;

        private Fader fader;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
            fader = FindObjectOfType<Fader>();
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            
            yield return fader.FadeOut(fadeOut);
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeWaitTime);
            
            yield return fader.FadeIn(fadeIn);
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
               if(portal == this) continue;
               if(portal.destination != destination) continue;
               
               return portal;
            }
            return null;
        }
        
        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
        
    }
}