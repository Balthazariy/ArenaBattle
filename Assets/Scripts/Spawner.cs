using Balthazariy.ArenaBattle.Objects.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class Spawner : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<EnemyBase> _spawnedEnemies;
        [Space(5)]
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private Players.Player _player;

        private float _spawnTime = 5.0f;
        private float _currentSpawnTIme;
        private bool _isSpawning;

        private void Awake()
        {
            _spawnPoints = new List<Transform>();
            _spawnedEnemies = new List<EnemyBase>();

            _currentSpawnTIme = _spawnTime;
            _isSpawning = false;

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

        private Vector2 GetSpawnPointPosition() => _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position;

        private void SpawnEnemy()
        {
            float chanceToBlueEnemy = UnityEngine.Random.Range(0.0f, 100.0f);

            EnemyBase enemy = null;

            if (chanceToBlueEnemy <= 75.0f)
                //enemy = new RedEnemy();
            else if (chanceToBlueEnemy > 75.0f)
                        Debug.Log("Blue enemy spawned");
            //enemy = null;

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