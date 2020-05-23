using UnityEngine;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Ladder : MonoBehaviour
    {
        CapsuleCollider capsule;
        public Vector3 ladderOffset;

        private void Awake()
        {
            capsule = GetComponent<CapsuleCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Vector2 vectorBetween = transform.position.XZ() - other.transform.position.XZ();
                Vector2 otherFwd = other.transform.forward.XZ();
                if (Vector2.Dot(vectorBetween.normalized, otherFwd.normalized) > 0.9)
                {
                    other.GetComponent<BaseController>().OnClimbLadder(this);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (capsule == null) Awake();
            Gizmos.DrawIcon(top(), "Ladder.png", false);
        }
        public Vector3 top() => transform.TransformPoint(capsule.center + (Vector3.up * capsule.height / 2) + ladderOffset);
    }
}