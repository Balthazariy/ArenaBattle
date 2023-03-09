using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Players;
using Balthazariy.ArenaBattle.Utilities;
using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Base
{
    public class EnemyBase
    {
        public event Action<EnemyBase> EnemyDestroyEvent;

        protected GameObject _selfObject;
        protected Transform _selfTransform;

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected GameObject _modelObject;
        protected MeshRenderer _modelMeshRenderer;

        protected Enemy _data;

        protected Player _player;

        protected int _energyDrop;

        protected int _scoreDrop;

        private OnBehaviourHandler _onBehaviourHandler;

        private int _health;
        private int _damage;

        private bool _isAlive;

        public string enemyName;

        public EnemyBase(Transform parent,
                         Vector3 startPosition,
                         Player player,
                         float attackCountdownTime,
                         Enemy data)
        {
            _data = data;

            _selfObject = MonoBehaviour.Instantiate(_data.prefab, parent);

            enemyName = "Enemy[" + UnityEngine.Random.Range(0, 9999).ToString() + "]";
            _selfObject.name = enemyName;

            _selfTransform = _selfObject.transform;

            _rigidbody = _selfObject.GetComponent<Rigidbody>();
            _collider = _selfObject.GetComponent<Collider>();
            _onBehaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();

            _modelObject = _selfObject.transform.Find("Model").gameObject;
            _modelMeshRenderer = _modelObject.GetComponent<MeshRenderer>();

            _onBehaviourHandler.TriggerEntered += TriggerEnteredEventHandler;

            _selfTransform.position = startPosition;

            _player = player;
            _health = _data.health;
            _damage = _data.damage;

            _isAlive = true;
        }

        public int GetDamage() => _damage;

        public virtual void Update()
        {
            if (!_isAlive)
                return;
        }

        public virtual void FixedUpdate()
        {
            if (!_isAlive)
                return;
        }

        public virtual void Hit()
        {
            _player.ApplyDamageAndCheckIsAlive(_damage);
            Dispose();
        }

        private void TriggerEnteredEventHandler(Collider target)
        {
            InterractWithPlayerBullet(target);
            InterractWithPlayerBody(target);

            if (target.transform.tag == "Ground")
                Dispose();
        }

        public virtual void Dispose()
        {
            _isAlive = false;

            _onBehaviourHandler.TriggerEntered -= TriggerEnteredEventHandler;

            EnemyDestroyEvent?.Invoke(this);

            MonoBehaviour.Destroy(_selfObject);
        }

        public void ApplyDamageAndCheckIsAlive(int damage)
        {
            ApplyDamage(damage);

            if (IsAlive())
                Dispose();
        }

        private void InterractWithPlayerBullet(Collider target)
        {
            if (target.transform.tag == "PlayerBullet")
            {
                BulletBase playerBullet = _player.GetBulletByName(target.name);
                ApplyDamageAndCheckIsAlive(playerBullet.GetBulletDamage());
                if (playerBullet.IsRicochet())
                    _player.AddHealth(15);
            }
        }

        private void InterractWithPlayerBody(Collider target)
        {
            if (target.transform.tag == "Player")
                Hit();
        }

        public int GetEnergyDrop() => _energyDrop;
        public int GetScoreDrop() => _scoreDrop;

        private void ApplyDamage(int damage) => _health -= damage;

        private bool IsAlive() => _health <= 0;
    }
}