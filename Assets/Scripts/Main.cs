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

        [SerializeField] private MainPage _mainPage;

        private const float _startGameTimer = 3f;
        private float _currentStartGameTimer;
        private bool _startCountdown;

        private float _gameTimer;
        private bool _isGameTimerActive;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _currentStartGameTimer = _startGameTimer;

            GameplayStarted = false;

            _gameTimer = 0.0f;
            _isGameTimerActive = false;

            _startCountdown = true;
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

                _mainPage.UpdatePreStartTimerValue((int)_currentStartGameTimer);

                if (_currentStartGameTimer <= 0)
                {
                    _startCountdown = false;

                    StartGameplay();
                }
            }
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
        }

        private void UpdateGameTimer()
        {
            if (_isGameTimerActive)
            {
                _gameTimer += Time.deltaTime;

                int minutes = Mathf.FloorToInt(_gameTimer / 60.0f);
                int seconds = Mathf.FloorToInt(_gameTimer - minutes * 60);
                _mainPage.UpdateTimeValue(minutes, seconds);
            }
        }
    }
}