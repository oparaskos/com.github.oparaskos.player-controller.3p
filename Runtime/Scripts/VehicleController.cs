using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Zeno.PlayerController
{
    [RequireComponent(typeof(PlayerInteractable))]
    public class VehicleController : MonoBehaviour
    {
        public WheelData[] wheels = new WheelData[0];
        public bool driving = false;
        public float motorTorque = 300f;
        public float steer = 30f;
        public Seat driversSeat;
        public CinemachineVirtualCameraBase carCamera;
        public CinemachineBrain cameraBrain;
        public AudioSource audioSource;
        public AudioClip startup;
        public AudioClip idle;

        private GameObject originalCamera;

        private void Start()
        {
            if (cameraBrain == null)
            {
                cameraBrain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
            }
            if (audioSource == null)
            {
                audioSource = gameObject.GetComponent<AudioSource>();
            }
            driversSeat.OnSit.AddListener(BeginDrive);
            driversSeat.OnStand.AddListener(EndDrive);
        }

        public void BeginDrive(PlayerController player, Seat seat)
        {
            Debug.Log("BeginDrive");
            originalCamera = cameraBrain.ActiveVirtualCamera.VirtualCameraGameObject;
            originalCamera.SetActive(false);
            carCamera.gameObject.SetActive(true);
            StartEngine();
            StartCoroutine(WaitForSound(() =>
            {
                audioSource.clip = idle;
                audioSource.loop = true;
                audioSource.Play();
            }));
            driving = true;
        }

        public void StartEngine()
        {
            audioSource.clip = startup;
            audioSource.loop = false;
            audioSource.Play();
        }

        IEnumerator WaitForSound(Action onFinish)
        {
            yield return new WaitForSeconds(audioSource.clip.length - audioSource.time);
            onFinish.Invoke();
        }

        public void EndDrive(PlayerController player, Seat seat)
        {
            carCamera.gameObject.SetActive(false);
            originalCamera.gameObject.SetActive(true);
            driving = false;
        }

        void FixedUpdate()
        {
            if (driving)
            {
                foreach (WheelData wheel in wheels)
                {
                    if (wheel.drive)
                    {
                        wheel.collider.motorTorque = motorTorque * driversSeat.occupant.movementAxis.y;
                    }
                    if (wheel.steer)
                    {
                        wheel.collider.steerAngle = steer * driversSeat.occupant.movementAxis.x;
                    }
                    Vector3 position = wheel.transform.position;
                    Quaternion rotation = wheel.transform.rotation;
                    wheel.collider.GetWorldPose(out position, out rotation);
                    wheel.transform.position = position;
                    wheel.transform.rotation = rotation;
                }
            }
        }

        [Serializable]
        public class WheelData
        {
            public Transform transform;
            public WheelCollider collider;
            public bool drive;
            public bool steer;
            public WheelData(Transform t, WheelCollider c, bool d, bool s)
            {
                transform = t;
                collider = c;
                drive = d;
                steer = s;
            }
        }
    }
}