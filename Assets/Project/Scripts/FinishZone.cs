using UnityEngine;

namespace ButchersGames
{
    public class FinishZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameManager.Instance.Win();
        }
    }
}