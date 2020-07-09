using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

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
        private SavingWrapper savingWrapper;
        
        private void Start()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
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
            fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOut);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            savingWrapper.Load();
            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeIn);
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
            //Linq alternative way
            //return FindObjectsOfType<Portal>().Where(portal => portal != this).FirstOrDefault(portal => portal.destination == destination);
        }
        
        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        
    }
}