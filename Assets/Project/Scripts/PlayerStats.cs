using System;
using UnityEngine;

namespace ButchersGames
{
    public class PlayerStats : MonoBehaviour
    {
        
        public const int CoinValue = 1;
        public const int BottleValue = -10;

        [Header("Skins")]
        [SerializeField] private GameObject poor;
        [SerializeField] private GameObject med;
        [SerializeField] private GameObject rich;

        [Header("Стартовые значения")]
        [SerializeField] private int startValue = 1;
        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 999;

        public int Value { get; private set; }
        public int Coins { get; private set; }

        public event Action<int> OnValueChanged;
        public event Action<int> OnCoinsChanged;

        public int MinValue => minValue;
        public int MaxValue => maxValue;

        private void Start()
        {
            Value = startValue;
            Coins = 0;

            OnValueChanged?.Invoke(Value);
            OnCoinsChanged?.Invoke(Coins);
        }

        private void FixedUpdate()
        {
            ChangeSkin();
        }

        public void Add(int amount)
        {
            if (amount == 0) return;
            Value = Mathf.Clamp(Value + amount, minValue, maxValue);
            OnValueChanged?.Invoke(Value);
        }

        public void Subtract(int amount) => Add(-amount);

        public void Multiply(int factor)
        {
            if (factor <= 0) return;
            Value = Mathf.Clamp(Value * factor, minValue, maxValue);
            OnValueChanged?.Invoke(Value);
        }

        public void Divide(int divisor)
        {
            if (divisor <= 0) return;
            Value = Mathf.Clamp(Value / divisor, minValue, maxValue);
            OnValueChanged?.Invoke(Value);
        }

        public void AddCoin(int amount = 1)
        {
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
        }

        
        public void PickCoin() { AddCoin(CoinValue); Add(CoinValue); }
        public void PickBottle() { Add(BottleValue); }

        public void ResetStats()
        {
            Value = startValue;
            Coins = 0;
            OnValueChanged?.Invoke(Value);
            OnCoinsChanged?.Invoke(Coins);
        }

        public void ChangeSkin()
        {
            if (Value < 50)
            {
                poor.gameObject.SetActive(true);
                med.gameObject.SetActive(false);
                rich.gameObject.SetActive(false);
            }

            if (Value >50 & Value < 150)
            {
                poor.gameObject.SetActive(false);
                med.gameObject.SetActive(true);
                rich.gameObject.SetActive(false);
            }

            if (Value >150)
            {
                poor.gameObject.SetActive(false);
                med.gameObject.SetActive(false);
                rich.gameObject.SetActive(true);
            }
        }
    }
}