using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ButchersGames
{
    public enum GameState { Start, Playing, Win, Lose }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlayerController player;

        public GameState State { get; private set; } = GameState.Start;
        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            player?.StopRunning();
        }

        public void StartGame()
        {
            if (State != GameState.Start) return;
            SetState(GameState.Playing);
            player?.StartRunning();
        }

        public void Win()
        {
            if (State != GameState.Playing) return;
            SetState(GameState.Win);
            Win_Dance();
            player?.StopRunning();
        }

        public void Lose()
        {
            if (State != GameState.Playing) return;
            SetState(GameState.Lose);
            player?.StopRunning();
        }

        public void RestartLevel() => ReloadScene();
        public void NextLevel() => ReloadScene();

        private void SetState(GameState newState)
        {
            State = newState;
            OnStateChanged?.Invoke(newState);
        }

        private void ReloadScene()
        {
            State = GameState.Start;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        [SerializeField] private PlayerAnimator playerAnimator;

        public void Win_Dance()
        {
            if (State != GameState.Playing) return;
            SetState(GameState.Win);
            player?.StopRunning();

            if (playerAnimator != null) playerAnimator.PlayWin();

            LevelManager.CompleteLevelCount = LevelManager.CurrentLevel;
            PlayerPrefs.Save();
        }
    }
}