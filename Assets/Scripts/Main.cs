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

        [SerializeField] private GameplayPage _gameplayPage;
        [SerializeField] private MenuPage _menuPage;
        [SerializeField] private PausePage _pausePage;
        [SerializeField] private GameOverPage _gameOverPage;
        [SerializeField] private GameObject _gameplayPrefab;

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

                _gameplayPage.UpdatePreStartTimerValue((int)_currentStartGameTimer);

                if (_currentStartGameTimer <= 0)
                {
                    _startCountdown = false;
                    _gameplayPage.ActivePreStartTimer(false);
                    StartGameplay();
                }
            }
        }

        public void PreStartGame()
        {
            HideAllPages();

            _startCountdown = true;
            _gameplayPage.Show();
            _gameplayPage.ActivePreStartTimer(true);
        }

        public void StartGameplay()
        {
            GameplayStarted = true;
            _isGameTimerActive = true;
            StartGameplayEvent?.Invoke();
        }

        public void StopGameplay()
        {
            HideAllPages();
            _menuPage.Show();
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
            _gameplayPage.Hide();
            _menuPage.Hide();
            _pausePage.Hide();
            _gameOverPage.Hide();
        }

        private void UpdateGameTimer()
        {
            if (_isGameTimerActive)
            {
                _gameTimer += Time.deltaTime;

                int minutes = Mathf.FloorToInt(_gameTimer / 60.0f);
                int seconds = Mathf.FloorToInt(_gameTimer - minutes * 60);
                _gameplayPage.UpdateTimeValue(minutes, seconds);
            }
        }

        public void PauseGame(bool isPause)
        {
            Time.timeScale = isPause ? 0 : 1;
        }
    }
}