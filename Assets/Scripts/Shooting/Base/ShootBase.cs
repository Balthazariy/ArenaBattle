using Balthazariy.Utilities;
using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Shooting.Base
{
    public class ShootBase
    {
        protected GameObject _selfObject;
        protected Transform _selftTransform;

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected MeshRenderer _meshRenderer;

        public event Action<Vector3> BulletDestroyEvent;

        private GameObject _modelObject;

        private Vector3 _currentPosition;

        private OnBehaviourHandler _behaviourHandler;

        private bool _isAlive;

        public const float BULLET_SPEED = 5f;

        public ShootBase(Transform parent, GameObject prefab, Vector3 spawnPosition)
        {
            _selfObject = MonoBehaviour.Instantiate(prefab, parent);
            _selftTransform = _selfObject.transform;

            _modelObject = _selfObject.transform.Find("Model").gameObject;

            _rigidbody = _selfObject.GetComponent<Rigidbody>();
            _collider = _selfObject.GetComponent<Collider>();
            _behaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();
            _meshRenderer = _modelObject.GetComponent<MeshRenderer>();

            _selfObject.transform.position = spawnPosition;

            _behaviourHandler.TriggerEntered += OnTriggerEnterEventHandler;
            _behaviourHandler.CollisionEnter += OnCollisionEnterEventHandler;

            _isAlive = true;
        }

        public virtual void Update()
        {
            if (!_isAlive)
                return;

            _currentPosition = _selftTransform.position;
        }

        public virtual void FixedUpdate()
        {
            if (!_isAlive)
                return;
        }

        public virtual void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        private void OnTriggerEnterEventHandler(Collider target)
        {
            Dispose();
        }

        private void OnCollisionEnterEventHandler(Collision target)
        {
            Dispose();
        }

        private void Dispose()
        {
            _isAlive = false;

            BulletDestroyEvent?.Invoke(_currentPosition);

            _behaviourHandler.TriggerEntered -= OnTriggerEnterEventHandler;

            MonoBehaviour.Destroy(_selfObject);
        }
    }
}