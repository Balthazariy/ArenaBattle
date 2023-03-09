using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Objects.Bullets;
using Balthazariy.ArenaBattle.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Enemies
{
    public class BlueEnemy : EnemyBase
    {
        private List<BulletBase> _bullets;

        private GameObject _bulletPrefab;
        private Transform _bulletParent;

        private float _shootCountdownTimer;
        private float _currentShootCountdownTimer;

        private bool _isShooted;

        public BlueEnemy(Transform parent,
                         Vector3 startPosition,
                         Player player,
                         GameObject bulletPrefab,
                         Transform bulletParent,
                         float attackCountdownTime,
                         Enemy data) : base(parent, startPosition, player, attackCountdownTime, data)
        {
            _bullets = new List<BulletBase>();
            _isShooted = true;

            _bulletPrefab = bulletPrefab;
            _bulletParent = bulletParent;

            _shootCountdownTimer = attackCountdownTime;
            _currentShootCountdownTimer = _shootCountdownTimer;

            _energyDrop = 50;
            _scoreDrop = 15;
        }

        public override void Update()
        {
            base.Update();

            RotateBody();

            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].Update();

            if (_isShooted)
            {
                _currentShootCountdownTimer -= Time.deltaTime;

                if (_currentShootCountdownTimer <= 0)
                {
                    _currentShootCountdownTimer = _shootCountdownTimer;
                    _isShooted = false;
                    Shoot();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            for (int i = 0; i < _bullets.Count; i++)
                _bullets[i].FixedUpdate();
        }

        public void Shoot()
        {
            if (!_isShooted)
            {
                BulletBase bullet = new EnemyBullet(_bulletPrefab, _bulletParent, _data.damage, Vector3.zero, _player, _selfTransform.Find("ShootPivot").transform.position);
                bullet.BulletDestroyEvent += OnBulletDestroyEventHandler;

                _bullets.Add(bullet);

                _currentShootCountdownTimer = _shootCountdownTimer;
                _isShooted = true;
            }
        }

        private void OnBulletDestroyEventHandler(BulletBase currentBullet)
        {
            _bullets.Remove(currentBullet);

            _player.RemoveEnergy(currentBullet.GetBulletDamage());
        }

        private void RotateBody()
        {
            _selfTransform.LookAt(_player.GetPlayerPosition());

            _selfTransform.localEulerAngles = new Vector3(0, _selfTransform.localEulerAngles.y, 0);
        }
    }
}