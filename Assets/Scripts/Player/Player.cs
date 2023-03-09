using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Objects.Bullets;
using Balthazariy.ArenaBattle.Screens;
using Balthazariy.ArenaBattle.Utilities;
using DG.Tweening;
using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balthazariy.ArenaBattle.Players
{
    public class Player : MonoBehaviour
    {
        public event Action UltaActivated;

        [SerializeField] private GameObject _selfObject;
        [SerializeField] private GameObject _bulletPrefab;

        [SerializeField] private Transform _bulletParent;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _bulletStartPosition;

        [SerializeField] private OnBehaviourHandler _onBehaviourHandler;

        [SerializeField] private TeleportPoints _teleportPoints;

        [SerializeField] FirstPersonController _firstPersonController;

        [SerializeField] private int _energyLimit;
        [SerializeField] private int _healthLimit;

        [SerializeField] private GameplayPage _mainPage;

        private List<BulletBase> _bullets;
        private BulletBase _currentBullet;
        private EnemiesData _enemyData;

        private const float _shootCountdownTimer = 0.5f;
        private float _currentShootCountdownTimer;

        private int _health;
        private int _energy;
        private int _score;

        private bool _isShooted;
        private bool _isAlive;

        private int _chanceToRicochet;
        private int _chanceToAddHealth;

        public int Health
        {
            get => _health;
            private set => _health = value;
        }

        public int Energy
        {
            get => _energy;
            private set => _energy = value;
        }

        public int Score
        {
            get => _score;
            private set => _score = value;
        }

        private void Start()
        {
            _bullets = new List<BulletBase>();

            _firstPersonController.enabled = false;
            _isShooted = true;
            _isAlive = false;

            _enemyData = Resources.Load<EnemiesData>("Models/EnemiesData");

            _chanceToAddHealth = 3;
            _chanceToRicochet = 3;

            _onBehaviourHandler.TriggerEntered += OnTriggerEnterEventHandler;
            Main.Instance.StartGameplayEvent += StartGameplayEventHandler;
            Main.Instance.StopGameplayEvent += StopGameplayEventHandler;
        }

        public void ResetStats()
        {
            _health = 100;
            _energy = 0;
            _score = 0;

            AddEnergy(_energy);
            AddHealth(0);
            AddScore(0);
        }

        public void AddEnergy(int value)
        {
            _energy += value;

            if (_energy >= _energyLimit)
                _energy = _energyLimit;

            if (_energy <= 0)
                _energy = 0;

            _mainPage.UpdateEnergyValue(_energy, _energyLimit);
        }

        public void RemoveEnergy(int value)
        {
            AddEnergy(value * -1);
        }

        public void AddHealth(int value)
        {
            _health += value;

            if (_health >= _healthLimit)
                _health = _healthLimit;

            if (_health <= 0)
                _health = 0;

            _mainPage.UpdateHealthValue(_health, _healthLimit);
        }

        public void AddScore(int value)
        {
            _score += value;

            _mainPage.UpdateScoreValue(_score);
        }

        private void Update()
        {
            if (!Main.Instance.GameplayStarted)
                return;

            if (!_isAlive)
                return;

            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].Update();

            if (_isShooted)
            {
                _currentShootCountdownTimer -= Time.deltaTime;

                if (_currentShootCountdownTimer <= 0)
                {
                    _currentShootCountdownTimer = _shootCountdownTimer;
                    _isShooted = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!Main.Instance.GameplayStarted)
                return;

            if (!_isAlive)
                return;

            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].FixedUpdate();
        }

        public void OnFire(InputValue value)
        {
            if (!_isAlive)
                return;

            if (!_isShooted)
            {
                var rotation = _cameraRoot.localEulerAngles + transform.localEulerAngles;
                var startPosition = _bulletStartPosition.position;

                int bulletHealth = CalculateBulletHealth();

                bool isRicochet = CalculateBulletRicochet();

                BulletBase bullet = new PlayerBullet(_bulletPrefab, _bulletParent, 50, rotation, bulletHealth, isRicochet, startPosition);
                bullet.BulletDestroyEvent += OnBulletDestroyEventHandler;
                _currentBullet = bullet;

                _bullets.Add(bullet);

                _currentShootCountdownTimer = _shootCountdownTimer;
                _isShooted = true;
            }
        }

        private int CalculateBulletHealth()
        {
            var healthDifference = _healthLimit - _health;
            var totalChance = _chanceToAddHealth + healthDifference;

            if (totalChance > _healthLimit)
                totalChance = _healthLimit;

            var chancing = UnityEngine.Random.Range(0, 100);
            var bulletHealth = 1;

            if (chancing < totalChance)
                ++bulletHealth;
            return bulletHealth;
        }

        private bool CalculateBulletRicochet()
        {
            var healthDifference = (_healthLimit - _health) / 2;
            var totalChance = _chanceToAddHealth + healthDifference;

            if (totalChance > _healthLimit)
                totalChance = _healthLimit;

            var chancing = UnityEngine.Random.Range(0, 100);

            if (chancing < totalChance)
                return true;

            return false;
        }

        public void OnFireUI()
        {
            OnFire(null);
        }

        public void OnUltaUI()
        {
            OnUlta(null);
        }

        public void OnUlta(InputValue value)
        {
            if (!_isAlive)
                return;

            if (_energy == _energyLimit)
            {
                _energy = 0;
                AddEnergy(0);
                UltaActivated?.Invoke();
            }
        }

        private void StartGameplayEventHandler()
        {
            ResetStats();
            _firstPersonController.enabled = false;
            _firstPersonController.enabled = true;
            _isAlive = true;
        }

        private void StopGameplayEventHandler()
        {
            ResetStats();
            _firstPersonController.enabled = false;
            _isAlive = false;

            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].Dispose(false);
        }

        private void OnBulletDestroyEventHandler(BulletBase currentBullet)
        {
            _bullets.Remove(currentBullet);
        }

        private void OnTriggerEnterEventHandler(Collider target)
        {
            if (target.transform.tag == "Zone")
                TeleportToRandomPointOfMap();
        }

        private void TeleportToRandomPointOfMap()
        {
            _selfObject.transform.DOMove(_teleportPoints.GetRandomPoint(), 0.3f);
        }

        public void ApplyDamageAndCheckIsAlive(int damage)
        {
            ApplyDamage(damage);

            _isAlive = IsAlive();

            if (!_isAlive)
            {
                Main.Instance.gameOverPage.Show();
                Main.Instance.StopGameplay();
                _firstPersonController.enabled = false;
            }
        }

        public BulletBase GetBulletByName(string name)
        {
            if (_currentBullet != null)
                return _currentBullet;

            foreach (var bullet in _bullets)
                if (String.Compare(bullet.bulletName, name) == 1)
                    return bullet;

            return null;
        }

        private void ApplyDamage(int damage)
        {
            _health -= damage;

            if (_health <= 0)
                _health = 0;

            _mainPage.UpdateHealthValue(_health, _healthLimit);
        }

        private bool IsAlive() => _health > 0;

        public Vector3 GetPlayerPosition() => _selfObject.transform.localPosition;

    }
}