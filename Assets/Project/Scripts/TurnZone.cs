using UnityEngine;

namespace ButchersGames
{
    public class TurnZone : MonoBehaviour
    {
        [Tooltip("направление")]
        [SerializeField] private float turnDegrees = 90f;

        [Tooltip("Время поворота")]
        [SerializeField] private float turnDuration = 1f;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[TurnZone] Trigger hit by {other.name}, tag={other.tag}", this);
            if (!other.CompareTag("Player")) return;
            var pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.StartTurn(turnDegrees, turnDuration);
        }
    }
}