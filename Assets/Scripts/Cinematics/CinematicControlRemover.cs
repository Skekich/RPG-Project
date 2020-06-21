using RPG.Control;
using RPG.Core;
using RuntimeSet;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField] private GameObjectRuntimeSet playerRuntimeSet;

        private GameObject currentPlayer;
        private PlayerController playerController;
        
        private void Start()
        {
            currentPlayer = playerRuntimeSet.GetItemIndex(0);
            playerController = currentPlayer.GetComponent<PlayerController>();
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            currentPlayer.GetComponent<ActionSceduler>().CancelCurrentAction();
            playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            playerController.enabled = true;
        }
    }
}