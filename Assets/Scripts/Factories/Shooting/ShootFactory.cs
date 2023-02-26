using Balthazariy.ArenaBattle.Shooting;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Factories.Shooting
{
    public class ShootFactory
    {
        private PlayerShoot _playerShoot;

        public ShootFactory()
        {

        }

        public void Update()
        {
            if (_playerShoot == null)
                return;

            _playerShoot.Update();
        }

        public void FixedUpdate()
        {
            if (_playerShoot == null)
                return;

            _playerShoot.FixedUpdate();
        }

        public void InitShootByType(ShootType type)
        {
            var bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletBase");

            switch (type)
            {
                case ShootType.Player:
                    {
                        _playerShoot = new PlayerShoot(GameObject.Find("[GAMEPLAY]/Objects").transform, bulletPrefab);
                    }
                    break;
                case ShootType.BlueEnemy:
                    {

                    }
                    break;
                default:
                    {
                        Debug.LogWarning($"Failed to init shoot with type {type}");
                    }
                    break;
            }
        }
    }

    public enum ShootType
    {
        Unknown,

        Player,
        BlueEnemy,
    }
}