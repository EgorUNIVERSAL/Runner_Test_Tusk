using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ButchersGames
{
    public class UIManager : MonoBehaviour
    {
        [Header("Ёкраны")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject gameplayScreen;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;

        [Header("HUD")]
        [SerializeField] private Slider wealthBar;
        [SerializeField] private TextMeshProUGUI coinText;

        [Header("—сылки")]
        [SerializeField] private PlayerStats stats;

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStateChanged += OnStateChanged;

            if (stats != null)
            {
                stats.OnValueChanged += OnValueChanged;
                stats.OnCoinsChanged += OnCoinsChanged;
                OnValueChanged(stats.Value);
                OnCoinsChanged(stats.Coins);
            }

            ShowOnly(startScreen);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStateChanged -= OnStateChanged;
            if (stats != null)
            {
                stats.OnValueChanged -= OnValueChanged;
                stats.OnCoinsChanged -= OnCoinsChanged;
            }
        }

        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Start: ShowOnly(startScreen); break;
                case GameState.Playing: ShowOnly(gameplayScreen); break;
                case GameState.Win: ShowOnly(winScreen); break;
                case GameState.Lose: ShowOnly(loseScreen); break;
            }
        }

        private void ShowOnly(GameObject screen)
        {
            if (startScreen != null) startScreen.SetActive(screen == startScreen);
            if (gameplayScreen != null) gameplayScreen.SetActive(screen == gameplayScreen);
            if (winScreen != null) winScreen.SetActive(screen == winScreen);
            if (loseScreen != null) loseScreen.SetActive(screen == loseScreen);
        }

        private void OnValueChanged(int v)
        {
      
            if (wealthBar != null && stats != null)
                wealthBar.value = Mathf.InverseLerp(stats.MinValue, stats.MaxValue, v);
        }

        private void OnCoinsChanged(int c)
        {
            if (coinText != null) coinText.text = c.ToString();
        }

        //  нопки
        public void OnRestartPressed() => GameManager.Instance.RestartLevel();
        public void OnNextPressed() => GameManager.Instance.NextLevel();
    }
}