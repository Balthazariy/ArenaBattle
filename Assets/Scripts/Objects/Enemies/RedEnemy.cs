using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Players;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Enemies
{
    public class RedEnemy : EnemyBase
    {
        private float _minTimeToFlyUp;
        private float _maxTimeToFlyUp;

        private float _timeToFlyUp;

        private bool _isFlyUp;
        private bool _isRotateToPlayer;
        private bool _isFlyToPlayer;

        public RedEnemy(Transform parent,
                        Vector3 startPosition,
                        Player player,
                        float attackCountdownTime,
                        Enemy data) : base(parent, startPosition, player, attackCountdownTime, data)
        {

            _minTimeToFlyUp = 1.0f;
            _maxTimeToFlyUp = 3.0f;

            _energyDrop = 15;

            _timeToFlyUp = UnityEngine.Random.Range(_minTimeToFlyUp, _maxTimeToFlyUp);

            ChangeState(0);
        }

        public override void Update()
        {
            base.Update();

            FlyUp();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            RotateToPlayer();
            FlyToPlayer();
        }

        private void FlyUp()
        {
            if (_isFlyUp)
            {
                _timeToFlyUp -= Time.deltaTime;

                if (_timeToFlyUp <= 0)
                {
                    ChangeState(1);
                }

                _rigidbody.position += Vector3.up * 3f * Time.deltaTime;
            }
        }

        private void RotateToPlayer()
        {
            if (_isRotateToPlayer)
            {
                var playerPosition = _player.GetPlayerPosition();
                _selfTransform.LookAt(new Vector3(playerPosition.x, playerPosition.y + 1, playerPosition.z));
                ChangeState(2);
            }
        }

        private void FlyToPlayer()
        {
            if (_isFlyToPlayer)
            {
                var playerPosition = _player.GetPlayerPosition();

                _selfTransform.LookAt(new Vector3(playerPosition.x, playerPosition.y + 1, playerPosition.z));
                _selfTransform.position += _selfTransform.forward * 6f * Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// 0 - fly up; 1 - rotate to player; 2 - fly to player
        /// </summary>
        private void ChangeState(byte state)
        {
            switch (state)
            {
                case 0:
                    {
                        _isFlyUp = true;
                        _isRotateToPlayer = false;
                        _isFlyToPlayer = false;
                    }
                    break;
                case 1:
                    {
                        _isFlyUp = false;
                        _isRotateToPlayer = true;
                        _isFlyToPlayer = false;
                    }
                    break;
                case 2:
                    {
                        _isFlyUp = false;
                        _isRotateToPlayer = false;
                        _isFlyToPlayer = true;
                    }
                    break;
                default: break;
            }
        }
    }
}