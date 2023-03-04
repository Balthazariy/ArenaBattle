using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Objects.Bullets;
using Balthazariy.ArenaBattle.Utilities;
using DG.Tweening;
using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balthazariy.ArenaBattle.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _selfObject;
        [SerializeField] private GameObject _bulletPrefab;

        [SerializeField] private Transform _bulletParent;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _bulletStartPosition;

        [SerializeField] private OnBehaviourHandler _onBehaviourHandler;

        [SerializeField] private TeleportPoints _teleportPoints;

        [SerializeField] FirstPersonController _firstPersonController;

        private List<BulletBase> _bullets;

        private const float _shootCountdownTimer = 0.5f;
        private float _currentShootCountdownTimer;

        private int _health;

        private bool _isShooted;
        private bool _isAlive;

        public int Health
        {
            get => _health;
            private set => _health = value;
        }

        private void Awake()
        {
            _bullets = new List<BulletBase>();

            _firstPersonController.enabled = false;
            _isShooted = true;
            _isAlive = false;

            _onBehaviourHandler.TriggerEntered += OnColliderEnterEventHandler;
            Main.Instance.StartGameplayEvent += StartGameplayEventHandler;
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

                BulletBase bullet = new PlayerBullet(_bulletPrefab, _bulletParent, 50, rotation, startPosition);

                bullet.BulletDestroyEvent += OnBulletDestroyEventHandler;

                _bullets.Add(bullet);

                _currentShootCountdownTimer = _shootCountdownTimer;
                _isShooted = true;
            }
        }

        private void StartGameplayEventHandler()
        {
            _firstPersonController.enabled = true;
            _isAlive = true;
        }

        private void OnBulletDestroyEventHandler(BulletBase currentBullet)
        {
            _bullets.Remove(currentBullet);
        }

        private void OnColliderEnterEventHandler(Collider target)
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
        }

        private void ApplyDamage(int damage) => _health -= damage;

        private bool IsAlive() => _health <= 0;

        public Vector3 GetPlayerPosition() => _selfObject.transform.localPosition;

    }
}