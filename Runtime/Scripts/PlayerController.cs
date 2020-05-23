using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : BaseController
    {
        public Vector2 mouseSensitivity = Vector2.one;

        private bool wasWeaponDrawn = false;
        private bool wasInteractPressed = false;

        void Start()
        {
            if (lookAt == null)
            {
                lookAt = Camera.main.transform;
            }
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            originalParent = transform.parent;
        }


        public void OnMove(InputValue value)
        {
            movementAxis = value.Get<Vector2>();
        }

        private void OnInteract()
        {
            OnInteractPressed.Invoke();
        }

        public void OnAim()
        {
            weaponDrawn = !weaponDrawn;
        }
    }
}