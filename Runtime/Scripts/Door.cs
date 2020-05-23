using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(PlayerInteractable))]
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        public bool open = false;
        public OnOpenEvent OnOpen = new OnOpenEvent();
        public OnCloseEvent OnClose = new OnCloseEvent();
        private PlayerInteractable interactable;
        private Animator animator;

        private void Start()
        {
            interactable = gameObject.GetComponent<PlayerInteractable>();
            interactable.OnInteract.AddListener(ToggleDoor);
            animator = gameObject.GetComponent<Animator>();
        }

        public void ToggleDoor(PlayerController character, PlayerInteractable i)
        {
            open = !open;
            animator.SetBool("Open", open);
            if (open)
            {
                OnOpen.Invoke(character, this);
            }
            else
            {
                OnClose.Invoke(character, this);
            }
        }


        [Serializable]
        public class OnOpenEvent : UnityEvent<PlayerController, Door> { }

        [Serializable]
        public class OnCloseEvent : UnityEvent<PlayerController, Door> { }
    }
}