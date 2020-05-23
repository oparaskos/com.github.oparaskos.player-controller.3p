using UnityEngine;

namespace Zeno.PlayerController
{
    public class Footsteps : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] footstepSounds;
        public float minPitch = 0.75f;
        public float maxPitch = 1.25f;
        public float minVolume = 0.75f;
        public float maxVolume = 1.25f;
        private static System.Random rnd = new System.Random();

        void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        public void Footstep()
        {
            if (footstepSounds.Length > 0)
            {
                audioSource.volume = GetRandomNumber(minVolume, maxVolume);
                audioSource.pitch = GetRandomNumber(minPitch, maxPitch);
                audioSource.PlayOneShot(footstepSounds[rnd.Next(0, footstepSounds.Length)]);
            }
        }

        public float GetRandomNumber(float minimum, float maximum)
        {
            return (float)rnd.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}