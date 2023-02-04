using UnityEngine;


namespace FFmpeg.Demo.REC
{
    public class RecDemoBall : MonoBehaviour
    {
        public AudioClip ballHitSound;
        public AudioSource source;
        [Range(0, 1)]
        public float voice = 0.5f;
        public Rigidbody body;

        void OnCollisionEnter(Collision collision)
        {
            source.PlayOneShot(ballHitSound, voice);
            body.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
}