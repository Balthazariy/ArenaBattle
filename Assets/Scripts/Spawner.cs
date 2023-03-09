using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Objects.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class Spawner : MonoBehaviour
    {
        public event Action<int> GetEnergyEvent;

        public static Spawner Instance;

        [Header("General")]
        [SerializeField] private Transform _topSpawnPointParent;
        [SerializeField] private Transform _bottomSpawnPointParent;
        [SerializeField] private List<EnemyBase> _spawnedEnemies;
        [Space(5)]
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private Players.Player _player;
        [SerializeField] private float _decreaseSpawnTimeModificator = 0.1f;
        [SerializeField] private float _spawnTimeLimit;
        [SerializeField] private float _spawnedEnemiesLimit;
        [Space(5)]
        [Header("Blue enemy settings")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletParent;

        private List<Transform> _topSpawnPoints;
        private List<Transform> _bottomSpawnPoints;

        private float _spawnTime = 5.0f;
        private float _currentSpawnTIme;
        private bool _isSpawning;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _topSpawnPoints = new List<Transform>();
            _bottomSpawnPoints = new List<Transform>();
            _spawnedEnemies = new List<EnemyBase>();

            _currentSpawnTIme = _spawnTime;
            _isSpawning = false;

            InitSpawnPoints();

            Main.Instance.StartGameplayEvent += StartGameplayEventHandler;
            Main.Instance.StopGameplayEvent += StopGameplayEventHandler;
            _player.UltaActivated += UltaActivatedEventHandler;
        }

        private void InitSpawnPoints()
        {
            for (int i = 0; i < _topSpawnPointParent.childCount; i++)
                _topSpawnPoints.Add(_topSpawnPointParent.GetChild(i));

            for (int i = 0; i < _bottomSpawnPointParent.childCount; i++)
                _bottomSpawnPoints.Add(_bottomSpawnPointParent.GetChild(i));
        }

        private void UltaActivatedEventHandler()
        {
            RemoveAllEnemies();
        }

        private void StopGameplayEventHandler()
        {

            _isSpawning = false;
            RemoveAllEnemies();
        }

        private void RemoveAllEnemies()
        {
            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].Dispose();

            for (int i = 0; i < _enemyParent.childCount; i++)
                MonoBehaviour.Destroy(_enemyParent.GetChild(i).gameObject);

            _spawnedEnemies.Clear();
        }

        private void Update()
        {
            if (!Main.Instance.GameplayStarted)
                return;

            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].Update();

            if (_isSpawning)
            {
                _currentSpawnTIme -= Time.deltaTime;

                if (_currentSpawnTIme <= 0)
                {
                    _spawnTime -= _decreaseSpawnTimeModificator;

                    if (_spawnTime <= _spawnTimeLimit)
                        _spawnTime = _spawnTimeLimit;

                    _currentSpawnTIme = _spawnTime;
                    SpawnEnemy();
                }
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].FixedUpdate();
        }

        private Vector3 GetBottomSpawnPointPosition() => _bottomSpawnPoints[UnityEngine.Random.Range(0, _bottomSpawnPoints.Count)].localPosition;
        private Vector3 GetTopSpawnPointPosition() => _topSpawnPoints[UnityEngine.Random.Range(0, _topSpawnPoints.Count)].localPosition;

        private void SpawnEnemy()
        {
            if (_spawnedEnemies.Count >= _spawnedEnemiesLimit)
                return;

            EnemiesData data = Resources.Load<EnemiesData>("Models/EnemiesData");
            float chanceToBlueEnemy = UnityEngine.Random.Range(0.0f, 100.0f);

            EnemyBase enemy = null;

            if (chanceToBlueEnemy <= 75.0f)
                enemy = new RedEnemy(_enemyParent, GetBottomSpawnPointPosition(), _player, 1f, data.GetEnemyByType(EnemyType.Red));
            else if (chanceToBlueEnemy > 75.0f)
                enemy = new BlueEnemy(_enemyParent, GetTopSpawnPointPosition(), _player, _bulletPrefab, _bulletParent, 3f, data.GetEnemyByType(EnemyType.Blue));

            enemy.EnemyDestroyEvent += EnemyDestroyedEventHandler;

            _spawnedEnemies.Add(enemy);
        }

        private void EnemyDestroyedEventHandler(EnemyBase enemy)
        {
            _spawnedEnemies.Remove(enemy);

            _player.AddEnergy(enemy.GetEnergyDrop());
            _player.AddScore(1);
        }

        private void StartGameplayEventHandler()
        {
            _isSpawning = true;

            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].Dispose();
        }

        public EnemyBase GetEnemyByName(string name)
        {
            foreach (var enemy in _spawnedEnemies)
                if (String.Compare(enemy.enemyName, name) == 1)
                    return enemy;

            return null;
        }
    }

    public enum EnemyType
    {
        Unknown,

        Red,
        Blue,
    }
}