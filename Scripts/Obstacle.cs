using UnityEngine;

namespace ButchersGames
{
    public class Obstacle : MonoBehaviour
    {
        [Tooltip("Мгновенная смерть")]
        [SerializeField] private bool killOnHit = true;

        [Tooltip("Сколько отнимает")]
        [SerializeField] private int damage = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (killOnHit)
            {
                GameManager.Instance.Lose();
                return;
            }

            other.GetComponent<PlayerStats>().Subtract(damage);
            gameObject.SetActive(false);
        }
    }
}