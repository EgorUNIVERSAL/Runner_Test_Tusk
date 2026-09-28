using UnityEngine;

namespace ButchersGames
{
    public class TestStats : MonoBehaviour
    {
        [SerializeField] private PlayerStats stats;

        private void Start()
        {
            stats.OnValueChanged += v => Debug.Log($"[Stats] Value = {v}");
            stats.OnCoinsChanged += c => Debug.Log($"[Stats] Coins = {c}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) stats.PickCoin();
            if (Input.GetKeyDown(KeyCode.Alpha2)) stats.PickBottle();
            if (Input.GetKeyDown(KeyCode.Alpha3)) stats.Multiply(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) stats.Divide(2);
            if (Input.GetKeyDown(KeyCode.Alpha5)) stats.AddCoin();
            if (Input.GetKeyDown(KeyCode.Alpha6)) stats.ResetStats();
        }
    }
}