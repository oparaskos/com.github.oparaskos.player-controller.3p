using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zeno.PlayerController
{
    public class PlayerInteractable : MonoBehaviour
    {
        public InteractEvent OnInteract = new InteractEvent();
        public string playerTag = "Player";

        private bool canInteract = true;
        private PlayerController player = null;

        private void OnTriggerEnter(Collider other)
        {
            // Check the thing that entered range was a "Player"
            if (player == null && other.CompareTag(playerTag))
            {
                player = other.GetComponent<PlayerController>();
                player.OnInteractPressed.AddListener(DoInteract);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            // Check the thing that left range was a the same player
            if (other.transform == player)
            {
                player = null;
                player.OnInteractPressed.RemoveListener(DoInteract);
            }
        }

        private void DoInteract()
        {
            if (canInteract)
            {
                // Call the configured event.
                OnInteract.Invoke(player, this);
            }
        }

        void OnEnable()
        {
            canInteract = true;
        }
        void OnDisable()
        {
            canInteract = false;
        }

        [Serializable]
        public class InteractEvent : UnityEvent<PlayerController, PlayerInteractable> { }
    }
}