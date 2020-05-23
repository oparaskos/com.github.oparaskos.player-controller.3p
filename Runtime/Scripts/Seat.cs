using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(PlayerInteractable))]
    public class Seat : MonoBehaviour
    {
        public Vector3 exitOffset;
        public OnSitEvent OnSit = new OnSitEvent();
        public OnStandEvent OnStand = new OnStandEvent();
        public PlayerController occupant;

        private PlayerInteractable interactable;

        private void Start()
        {
            interactable = gameObject.GetComponent<PlayerInteractable>();
            interactable.OnInteract.AddListener(ToggleSitting);
        }

        public void ToggleSitting(PlayerController character, PlayerInteractable i)
        {
            if (occupant == null)
            {
                // Player wants to sit.
                occupant = character;
                character.Sit(this);
                Debug.Log("OnSit");
                OnSit.Invoke(character, this);
            }
            else if (occupant == character)
            {
                // Player wants to stand.
                occupant = null;
                character.Stand();
                OnStand.Invoke(character, this);
            }
            else
            {
                // Seat occupied/do nothing.
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(transform.position + exitOffset, "SeatExit.tiff", true);
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "Seat.tiff", true);
        }

        [Serializable]
        public class OnSitEvent : UnityEvent<PlayerController, Seat> { }

        [Serializable]
        public class OnStandEvent : UnityEvent<PlayerController, Seat> { }
    }
}