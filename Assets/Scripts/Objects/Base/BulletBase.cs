using Balthazariy.ArenaBattle.Utilities;
using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Base
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

        protected const float BULLET_SPEED = 4.0f;

        public string bulletName;

        private bool _isAlive;

        #region Bullet Live Timer
        private const float LIVE_TIME = 5.0f;
        private float _currentLiveTime;
        #endregion

        private int _bulletDamage;
        private int _bulletHealth;
        private const float _chanceToNothing = 50.0f;
        private bool _isRicochet;

        public BulletBase(GameObject prefab, Transform parent, int bulletDamage, Vector3 playerRotation, Vector3 startPosition)
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

            _selfObject.SetActive(false);

            _selfTransform.localEulerAngles = playerRotation;
            _selfTransform.position = startPosition;

            _currentLiveTime = LIVE_TIME;

            _bulletDamage = bulletDamage;
            _bulletHealth = 1;

            bulletName = "Bullet[" + UnityEngine.Random.Range(0, 9999).ToString() + "]";
            _selfObject.name = bulletName;

            InitChancing();

            _selfObject.SetActive(true);

            _isAlive = true;
        }

        public virtual void Update()
        {
            if (!_isAlive)
                return;

            _currentLiveTime -= Time.deltaTime;

            if (_currentLiveTime <= 0)
                Dispose(false);
        }

        public virtual void FixedUpdate()
        {
            if (!_isAlive)
                return;

            _rigidbody.AddRelativeForce(Vector3.forward * BULLET_SPEED, ForceMode.Impulse);
        }

        public void Dispose(bool isEnemy)
        {
            if (isEnemy)
            {
                --_bulletHealth;

                if (_bulletHealth <= 0)
                    Dead();
            }
            else
                Dead();
        }

        private void TriggerEnteredEventHandler(Collider target)
        {
            if (!_isAlive)
                return;
            Dispose(target.transform.tag != "Ground");
        }

        private void OnCollisionEnterEventHandler(Collision target)
        {
            if (!_isAlive)
                return;
            Dispose(target.transform.tag != "Ground");
        }

        private void Dead()
        {
            _isAlive = false;

            _onBehaviourHandler.TriggerEntered -= TriggerEnteredEventHandler;
            _onBehaviourHandler.CollisionEnter -= OnCollisionEnterEventHandler;

            BulletDestroyEvent?.Invoke(this);

            MonoBehaviour.Destroy(_selfObject);
        }

        private void InitChancing()
        {
            int chance = UnityEngine.Random.Range(0, 100);

            if (chance >= _chanceToNothing)
                return;

            int healthOrRicochetChance = UnityEngine.Random.Range(0, 100);

            if (healthOrRicochetChance > 0 && healthOrRicochetChance <= 50)
            {
                ++_bulletHealth;
            }
            else if (healthOrRicochetChance > 50 && healthOrRicochetChance <= 100)
            {
                _isRicochet = true;
            }
        }

        public int GetBulletDamage() => _bulletDamage;
    }
}