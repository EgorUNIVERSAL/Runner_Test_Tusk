using UnityEngine;

namespace ButchersGames
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip pickupSfx;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<PlayerStats>().PickCoin();
            audioSource.PlayOneShot(pickupSfx);
            gameObject.SetActive(false);
        }
    }
}