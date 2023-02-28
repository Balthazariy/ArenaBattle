using Balthazariy.Utilities;
using System;
using UnityEngine;

namespace Balthazariy.Objects.Base
{
    public class BulletBase
    {
        public event Action<BulletBase> BulletDestroyEvent;

        protected GameObject _selfObject;
        protected Transform _selfTransform;

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected OnBehaviourHandler _onBehaviourHandler;

        protected GameObject _modelObject;
        protected MeshRenderer _modelMeshRenderer;

        protected const float BULLET_SPEED = 1.0f;

        private bool _isAlive;

        #region Bullet Live Timer
        private const float LIVE_TIME = 5.0f;
        private float _currentLiveTime;
        #endregion

        private int _bulletDamage;
        private int _bulletHealth;
        private const float _chanceToGetSecongLife = 25.0f;
        private const float _chanceToRikochet = 25.0f;
        private const float _chanceToNothing = 50.0f;

        public BulletBase(GameObject prefab, Transform parent, int bulletDamage, Vector3 startPosition)
        {
            _selfObject = MonoBehaviour.Instantiate(prefab, parent);
            _selfTransform = _selfObject.transform;

            _rigidbody = _selfObject.GetComponent<Rigidbody>();
            _collider = _selfObject.GetComponent<Collider>();
            _onBehaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();

            _modelObject = _selfObject.transform.Find("Model").gameObject;
            _modelMeshRenderer = _modelObject.GetComponent<MeshRenderer>();

            _onBehaviourHandler.TriggerEntered += TriggerEnteredEventHandler;
            _onBehaviourHandler.CollisionEnter += OnCollisionEnterEventHandler;

            _selfTransform.position = startPosition;

            _currentLiveTime = LIVE_TIME;

            _bulletDamage = bulletDamage;
            _bulletHealth = 1;

            InitChancing();

            _isAlive = true;
        }

        public virtual void Update()
        {
            if (!_isAlive)
                return;

            _currentLiveTime -= Time.deltaTime;

            if (_currentLiveTime <= 0)
                Dispose();
        }

        public virtual void FixedUpdate()
        {
            if (!_isAlive)
                return;
        }

        public void Dispose()
        {
            _isAlive = false;

            --_bulletHealth;

            if (_bulletHealth <= 0)
            {
                _onBehaviourHandler.TriggerEntered -= TriggerEnteredEventHandler;

                BulletDestroyEvent?.Invoke(this);

                MonoBehaviour.Destroy(_selfObject);
            }
        }

        private void TriggerEnteredEventHandler(Collider target)
        {
            Dispose();
        }

        private void OnCollisionEnterEventHandler(Collision target)
        {
            Dispose();
        }

        private void InitChancing()
        {
            float chance = UnityEngine.Random.Range(0.0f, 100f);

            if (chance >= _chanceToNothing)
                return;
        }
    }
}