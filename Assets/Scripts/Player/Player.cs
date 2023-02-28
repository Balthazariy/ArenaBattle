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

        private List<BulletBase> _bullets;

        private void Start()
        {
            _bullets = new List<BulletBase>();
        }

        private void Update()
        {
            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].Update();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].FixedUpdate();
        }

        public void OnMouseClick(InputValue value)
        {
            BulletBase bullet = new PlayerBullet(_bulletPrefab, _bulletParent);

            bullet.BulletDestroyEvent += OnBulletDestroyEventHandler;

            _bullets.Add(bullet);
        }

        private void OnBulletDestroyEventHandler(BulletBase currentBullet)
        {
            _bullets.Remove(currentBullet);
        }
    }
}