using UnityEngine;
using UnityEngine.Audio;

namespace ButchersGames
{
    public class Bottle : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip pickupSfx;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<PlayerStats>().PickBottle();
            audioSource.PlayOneShot(pickupSfx);
            gameObject.SetActive(false);
        }
    }
}