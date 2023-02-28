using Balthazariy.ArenaBattle.Shooting;
using Balthazariy.ArenaBattle.Shooting.Base;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Factories.Shooting
{
    public class ShootFactory
    {


        private ShootBase _playerShoot;

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
            var bulletParent = GameObject.Find("[GAMEPLAY]/Objects").transform;
            var startPosition = Vector3.zero;

            switch (type)
            {
                case ShootType.Player:
                    {
                        startPosition = GameObject.Find("[GAMEPLAY]/PlayerCapsule/PlayerCameraRoot/Shoot_Pivot").transform.position;
                        _playerShoot = new PlayerShoot(bulletParent, bulletPrefab, startPosition);
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