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

        private OnBehaviourHandler _onBehaviourHandler;

        private int _health;
        private int _damage;

        private float _moveSpeed;
        private float _rotatingSpeed;

        private bool _isAlive;

        private bool _isAttackState;
        private bool _isWalkingState;

        private float _attackCountdownTimer = 0.5f;
        private float _currenattackCountdownTimer;
        private bool _isAttacked;

        public EnemyBase(Transform parent,
                         Vector3 startPosition,
                         Player player,
                         float moveSpeed,
                         float rotatingSpeed,
                         float attackCountdownTime,
                         Enemy data)
        {
            _data = data;

            _selfObject = MonoBehaviour.Instantiate(_data.prefab, parent);
            _selfTransform = _selfObject.transform;

            _rigidbody = _selfObject.GetComponent<Rigidbody>();
            _collider = _selfObject.GetComponent<Collider>();
            _onBehaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();

            _modelObject = _selfObject.transform.Find("Model").gameObject;
            _modelMeshRenderer = _modelObject.GetComponent<MeshRenderer>();

            _onBehaviourHandler.TriggerEntered += TriggerEnteredEventHandler;
            _onBehaviourHandler.CollisionEnter += OnCollisionEnterEventHandler;

            _selfTransform.position = startPosition;

            _player = player;
            _health = _data.health;
            _damage = _data.damage;
            _moveSpeed = moveSpeed;
            _rotatingSpeed = rotatingSpeed;
            _attackCountdownTimer = attackCountdownTime;

            _isAlive = true;
        }

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

        private void TriggerEnteredEventHandler(Collider target)
        {

        }

        private void OnCollisionEnterEventHandler(Collision target)
        {

        }

        private void Dispose()
        {
            _isAlive = false;

            _onBehaviourHandler.TriggerEntered -= TriggerEnteredEventHandler;

            EnemyDestroyEvent?.Invoke(this);

            MonoBehaviour.Destroy(_selfObject);
        }
    }
}