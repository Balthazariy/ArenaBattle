using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Objects.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class Spawner : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform _spawnPointParent;
        [SerializeField] private List<EnemyBase> _spawnedEnemies;
        [Space(5)]
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private Players.Player _player;

        private List<Transform> _spawnPoints;

        private float _spawnTime = 5.0f;
        private float _currentSpawnTIme;
        private bool _isSpawning;

        private void Awake()
        {
            _spawnPoints = new List<Transform>();
            _spawnedEnemies = new List<EnemyBase>();

            _currentSpawnTIme = _spawnTime;
            _isSpawning = false;

            for (int i = 0; i < _spawnPointParent.childCount; i++)
                _spawnPoints.Add(_spawnPointParent.GetChild(i));

            Main.Instance.StartGameplayEvent += StartGameplayEventHandler;
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
            {
                Debug.Log("Blue enemy spawned");
                enemy = new RedEnemy(_enemyParent, GetSpawnPointPosition(), _player, 10f, 1f, 1f, data.GetEnemyByType(EnemyType.Red));
            }
            _spawnedEnemies.Add(enemy);
        }

        private void StartGameplayEventHandler()
        {
            _isSpawning = true;
        }
    }

    public enum EnemyType
    {
        Unknown,

        Red,
        Blue,
    }
}