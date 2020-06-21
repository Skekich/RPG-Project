using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if(isTriggered) return;
            if(!other.CompareTag("Player")) return;
            GetComponent<PlayableDirector>().Play();
            isTriggered = true;
        }
    }
}