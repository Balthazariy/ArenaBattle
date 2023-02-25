using Balthazariy.ArenaBattle.Shooting;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Factories.Shooting
{
    public class ShootFactory
    {
        private Transform _bulletParent;
        private GameObject _bulletPrefab;

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

        public void InitShootByType(ShootType type)
        {
            switch (type)
            {
                case ShootType.Player:
                    {
                        _playerShoot = new PlayerShoot(_bulletParent, _bulletPrefab);
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