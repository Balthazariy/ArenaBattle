using Balthazariy.ArenaBattle.Screens;
using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class Main : MonoBehaviour
    {
        public event Action StartGameplayEvent;
        public event Action StopGameplayEvent;

        public static Main Instance;

        public bool GameplayStarted;

        public GameplayPage gameplayPage;
        public MenuPage menuPage;
        public PausePage pausePage;
        public GameOverPage gameOverPage;

        private const float _startGameTimer = 4f;
        private float _currentStartGameTimer;
        private bool _startCountdown;

        private float _gameTimer;
        private bool _isGameTimerActive;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _currentStartGameTimer = _startGameTimer;

            GameplayStarted = false;

            _gameTimer = 0.0f;
            _isGameTimerActive = false;

            _startCountdown = false;
        }

        private void Update()
        {
            UpdatePreStartTimer();

            UpdateGameTimer();
        }

        private void UpdatePreStartTimer()
        {
            if (_startCountdown)
            {
                _currentStartGameTimer -= Time.deltaTime;

                gameplayPage.UpdatePreStartTimerValue((int)_currentStartGameTimer);

                if (_currentStartGameTimer <= 0)
                {
                    _startCountdown = false;
                    gameplayPage.ActivePreStartTimer(false);
                    StartGameplay();

                }
            }
        }

        public void PreStartGame()
        {
            HideAllPages();
            menuPage.Hide();
            gameplayPage.Show();
            _startCountdown = true;
            gameplayPage.Show();
            gameplayPage.ActivePreStartTimer(true);
        }

        public void StartGameplay()
        {
            GameplayStarted = true;
            _isGameTimerActive = true;
            StartGameplayEvent?.Invoke();
        }

        public void StopGameplay()
        {
            GameplayStarted = false;
            _isGameTimerActive = false;
            StopGameplayEvent?.Invoke();
        }

        public void RestartGame()
        {
            StopGameplay();
            StartGameplay();
        }

        private void HideAllPages()
        {
            gameplayPage.Hide();
            menuPage.Hide();
            pausePage.Hide();
            gameOverPage.Hide();
        }

        private void UpdateGameTimer()
        {
            if (_isGameTimerActive)
            {
                _gameTimer += Time.deltaTime;

                int minutes = Mathf.FloorToInt(_gameTimer / 60.0f);
                int seconds = Mathf.FloorToInt(_gameTimer - minutes * 60);
                gameplayPage.UpdateTimeValue(minutes, seconds);
            }
        }

        public void PauseGame(bool isPause)
        {
            Time.timeScale = isPause ? 0 : 1;
        }
    }
}