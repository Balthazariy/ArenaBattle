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

        public const float BULLET_SPEED = 50f;

        public ShootBase(Transform parent, GameObject prefab)
        {
            _selfObject = MonoBehaviour.Instantiate(prefab, parent);

            _modelObject = _selfObject.transform.Find("Model").gameObject;

            _rigidbody = _selfObject.GetComponent<Rigidbody>();
            _collider = _selfObject.GetComponent<Collider>();
            _meshRenderer = _modelObject.GetComponent<MeshRenderer>();

            _behaviourHandler.TriggerEntered += OnTriggerEnterEventHandler;

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
            BulletDestroyEvent?.Invoke(_currentPosition);

            Dispose();
        }

        private void Dispose()
        {
            _isAlive = false;

            _behaviourHandler.TriggerEntered -= OnTriggerEnterEventHandler;

            MonoBehaviour.Destroy(_selfObject);
        }
    }
}