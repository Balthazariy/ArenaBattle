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
        private float _rotatingSpeed;

        private bool _isFlyUp;
        private bool _isRotateToPlayer;
        private bool _isFlyToPlayer;

        public RedEnemy(Transform parent,
                        Vector3 startPosition,
                        Player player,
                        float attackCountdownTime,
                        Enemy data) : base(parent, startPosition, player, attackCountdownTime, data)
        {

            _minTimeToFlyUp = 0.5f;
            _maxTimeToFlyUp = 2.0f;

            _energyDrop = 15;
            _scoreDrop = 5;

            _timeToFlyUp = UnityEngine.Random.Range(_minTimeToFlyUp, _maxTimeToFlyUp);
        }

        public override void Update()
        {
            base.Update();


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        private void FlyUp()
        {

        }

        private void RotateToPlayer()
        {

        }

        private void FlyToPlayer()
        {

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