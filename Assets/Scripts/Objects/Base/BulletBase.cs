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

        protected const float BULLET_SPEED = 5.0f;

        private bool _isAlive;

        #region Bullet Live Timer
        private const float LIVE_TIME = 2.0f;
        private float _currentLiveTime;
        #endregion

        public BulletBase(GameObject prefab, Transform parent)
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

            _currentLiveTime = LIVE_TIME;

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

            _onBehaviourHandler.TriggerEntered -= TriggerEnteredEventHandler;

            BulletDestroyEvent?.Invoke(this);

            MonoBehaviour.Destroy(_selfObject);
        }

        private void TriggerEnteredEventHandler(Collider target)
        {
            Dispose();
        }

        private void OnCollisionEnterEventHandler(Collision target)
        {
            Dispose();
        }
    }
}