using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Zeno.PlayerController
{
    public class CinemachineInputSystemAdapter : MonoBehaviour
    {
        private Vector2 LookCamera; // your LookDelta
        private void Awake()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        public void OnLook(InputValue value)
        {
            LookCamera = value.Get<Vector2>().normalized;
        }

        public float GetAxisCustom(string axisName)
        {
            if (axisName == "X")
            {
                return LookCamera.x;
            }

            else if (axisName == "Y")
            {
                return LookCamera.y;
            }

            return 0;
        }
    }
}