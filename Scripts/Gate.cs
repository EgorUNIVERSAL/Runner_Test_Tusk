using TMPro;
using UnityEngine;

namespace ButchersGames
{
    public enum GateOp
    { Add, Subtract, Multiply, Divide }

    public class Gate : MonoBehaviour
    {
        [SerializeField] private GateOp operation = GateOp.Multiply;
        [SerializeField] private int value = 2;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var stats = other.GetComponent<PlayerStats>();
            switch (operation)
            {
                case GateOp.Add: stats.Add(value); break;
                case GateOp.Subtract: stats.Subtract(value); break;
                case GateOp.Multiply: stats.Multiply(value); break;
                case GateOp.Divide: stats.Divide(value); break;
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}