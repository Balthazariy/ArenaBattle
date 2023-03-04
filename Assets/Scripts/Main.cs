using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class Main : MonoBehaviour
    {
        public event Action StartGameplayEvent;

        public static Main Instance;

        public bool GameplayStarted;

        private const float _startGameTimer = 5f;
        private float _currentStartGameTimer;
        private bool _startCountdown;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _currentStartGameTimer = _startGameTimer;

            GameplayStarted = false;

            _startCountdown = true;
        }

        private void Update()
        {
            if (_startCountdown)
            {
                _currentStartGameTimer -= Time.deltaTime;

                if (_currentStartGameTimer <= 0)
                {
                    _startCountdown = false;

                    GameplayStarted = true;

                    StartGameplayEvent?.Invoke();
                }
            }
        }
    }
}