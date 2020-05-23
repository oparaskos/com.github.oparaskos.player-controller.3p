using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class BaseController : MonoBehaviour
    {
        public Vector2 movementAxis = Vector2.zero;
        public Vector3 lookAtPosition;

        public Transform lookAt;
        public bool lookAtInvert = true;
        public bool lookAtClampXZ = true;
        public float lookRotationSpeed = 2.0f;
        public Transform weapon;
        public Transform hand;
        public Transform holster;
        public OnInputEvent OnInteractPressed = new OnInputEvent();

        protected bool weaponDrawn = false;
        protected Seat seat = null;
        protected Transform originalParent = null;
        protected CharacterController controller;
        protected Animator animator;
        protected bool firingWeapon = false;

        public float impactScale = 3f;
        private bool attacking;
        private Ladder climbingLadder;
        private float exitDistance = 2f;

        public Vector3 top()
        {
            return transform.TransformPoint(controller.center + (Vector3.up * controller.height / 2));
        }
        public Vector3 bottom()
        {
            return transform.TransformPoint(controller.center - (Vector3.up * controller.height / 2));
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.rigidbody != null && controller.velocity.sqrMagnitude > 0)
            {
                Vector3 impactForce = -hit.normal * impactScale;
                hit.rigidbody.AddForceAtPosition(impactForce, hit.point, ForceMode.Impulse);
                hit.gameObject.SendMessageUpwards("PlayImpactNoise", SendMessageOptions.DontRequireReceiver);
            }
        }
        void Update()
        {
            if (seat == null)
            {
                UpdateLocomotion();
            }
        }

        public void OnFire()
        {
            attacking = true;
        }

        public void OnClimbLadder(Ladder ladder)
        {
            climbingLadder = ladder;
            Vector3 ladderPos = ladder.top();
            transform.position = new Vector3(ladderPos.x, transform.position.y, ladderPos.z);
            transform.rotation = Quaternion.LookRotation(-climbingLadder.transform.forward, Vector3.up);
            controller.enabled = false;
        }

        private void DismountLadder(Vector3 exitPosition)
        {
            climbingLadder = null;
            transform.position = exitPosition;
            controller.enabled = true;
        }

        protected void UpdateLocomotion()
        {
            animator.SetBool("Attack", attacking);
            attacking = false;
            animator.SetBool("Sitting", false);
            animator.SetBool("Weapon Drawn", weaponDrawn);
            animator.SetFloat("XVelocity", movementAxis.x);
            animator.SetFloat("YVelocity", movementAxis.y);
            if (climbingLadder != null)
            {
                UpdateClimbingLocomotion();
            }
            else
            {
                UpdateNormalLocomotion();
            }

        }

        private void UpdateClimbingLocomotion()
        {
            animator.SetBool("Climbing Ladder", true);
            if (movementAxis.y > 0.1)
            {
                Ray r = new Ray(top(), transform.forward);
                if (!Physics.Raycast(r, 1))
                {
                    DismountLadder(top() + transform.forward * exitDistance);
                }
            }
            if (movementAxis.y < -0.1)
            {
                Ray r = new Ray(bottom(), -transform.up);
                if (!Physics.Raycast(r, 1))
                {
                    DismountLadder(bottom() - transform.forward * exitDistance);
                }
            }
        }

        private void UpdateNormalLocomotion()
        {
            controller.Move(Vector3.down * 9.8f * Time.deltaTime);
            animator.SetBool("Climbing Ladder", false);
            if (lookAt != null)
            {
                lookAtPosition = lookAt.transform.position;
            }
            Vector3 targetLookDirection = Vector3.Normalize(lookAtPosition - transform.position);
            if (lookAtInvert)
            {
                targetLookDirection = -targetLookDirection;
            }
            if (lookAtClampXZ)
            {
                targetLookDirection = new Vector3(targetLookDirection.x, 0, targetLookDirection.z);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetLookDirection, Vector3.up), lookRotationSpeed * movementAxis.y * Time.deltaTime);
        }

        public void Stand()
        {
            transform.parent = originalParent;
            if (seat != null)
            {
                transform.position = seat.transform.position + seat.exitOffset;
                transform.rotation = seat.transform.rotation;
            }
            seat = null;
            controller.detectCollisions = true;
            controller.enabled = true;
        }

        public void Sit(Seat seat)
        {
            this.seat = seat;
            controller.detectCollisions = false;
            controller.enabled = false;
            transform.parent = seat.transform;
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation;
            animator.SetBool("Sitting", true);
            animator.SetFloat("XVelocity", 0f);
            animator.SetFloat("YVelocity", 0f);
        }

        public void WeaponHandAttach()
        {
            if (weapon != null && hand != null && holster != null)
            {
                if (weaponDrawn)
                {
                    weapon.parent = hand;
                    weapon.SetPositionAndRotation(hand.position, hand.rotation);
                }
                else
                {
                    weapon.parent = holster;
                    weapon.SetPositionAndRotation(holster.position, holster.rotation);
                }
            }
        }


        [Serializable]
        public class OnInputEvent : UnityEvent { }
    }
}
