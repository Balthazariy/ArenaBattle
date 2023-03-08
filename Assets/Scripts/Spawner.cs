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
        [SerializeField] private Transform _spawnPointParent;
        [SerializeField] private List<EnemyBase> _spawnedEnemies;
        [Space(5)]
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private Players.Player _player;
        [SerializeField] private float _decreaseSpawnTimeModificator = 0.1f;

        private List<Transform> _spawnPoints;

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
            _spawnPoints = new List<Transform>();
            _spawnedEnemies = new List<EnemyBase>();

            _currentSpawnTIme = _spawnTime;
            _isSpawning = false;

            for (int i = 0; i < _spawnPointParent.childCount; i++)
                _spawnPoints.Add(_spawnPointParent.GetChild(i));

            Main.Instance.StartGameplayEvent += StartGameplayEventHandler;
            Main.Instance.StopGameplayEvent += StopGameplayEventHandler;
            _player.UltaActivated += UltaActivatedEventHandler;
        }

        private void UltaActivatedEventHandler()
        {
            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].Dispose();
        }

        private void StopGameplayEventHandler()
        {
            for (int i = 0; i < _spawnedEnemies.Count; i++)
                _spawnedEnemies[i].Dispose();

            _isSpawning = false;
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

        private Vector3 GetSpawnPointPosition() => _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].localPosition;

        private void SpawnEnemy()
        {
            EnemiesData data = Resources.Load<EnemiesData>("Models/EnemiesData");
            float chanceToBlueEnemy = UnityEngine.Random.Range(0.0f, 100.0f);

            EnemyBase enemy = null;

            if (chanceToBlueEnemy <= 75.0f)
                enemy = new RedEnemy(_enemyParent, GetSpawnPointPosition(), _player, 10f, 1f, 1f, data.GetEnemyByType(EnemyType.Red));
            else if (chanceToBlueEnemy > 75.0f)
                enemy = new BlueEnemy(_enemyParent, GetSpawnPointPosition(), _player, 10f, 1f, 1f, data.GetEnemyByType(EnemyType.Blue));

            enemy.EnemyDestroyEvent += EnemyDestroyedEventHandler;

            _spawnedEnemies.Add(enemy);
        }

        private void EnemyDestroyedEventHandler(EnemyBase enemy)
        {
            _spawnedEnemies.Remove(enemy);

            _player.AddEnergy(enemy.GetEnergyDrop());
            _player.AddScore(enemy.GetScoreDrop());
        }

        private void StartGameplayEventHandler()
        {
            _isSpawning = true;
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