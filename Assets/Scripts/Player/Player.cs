using Balthazariy.Objects.Base;
using Balthazariy.Objects.Bullets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Balthazariy.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _bulletStartPosition;

        private List<BulletBase> _bullets;

        private const float _shootCountdownTimer = 0.5f;
        private float _currentShootCountdownTimer;
        private bool _isShooted;

        private void Start()
        {
            _bullets = new List<BulletBase>();
        }

        private void Update()
        {
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

        public void OnMouseClick(InputValue value)
        {
            if (!_isShooted)
            {
                var rotation = _cameraRoot.localEulerAngles + transform.localEulerAngles;
                var startPosition = _bulletStartPosition.position;

                BulletBase bullet = new PlayerBullet(_bulletPrefab, _bulletParent, 30, rotation, startPosition);

                bullet.BulletDestroyEvent += OnBulletDestroyEventHandler;

                _bullets.Add(bullet);

                _currentShootCountdownTimer = _shootCountdownTimer;
                _isShooted = true;
            }
        }

        private void OnBulletDestroyEventHandler(BulletBase currentBullet)
        {
            _bullets.Remove(currentBullet);
        }
    }
}