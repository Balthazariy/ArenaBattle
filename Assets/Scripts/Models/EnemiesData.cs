using System;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Models
{
    [CreateAssetMenu(fileName = "EnemiesData", menuName = "Balthazariy/ArenaBattle/EnemiesData", order = 0)]
    public class EnemiesData : ScriptableObject
    {
        public Enemy[] enemies;

        public Enemy GetEnemyByType(EnemyType type)
        {
            foreach (Enemy enemy in enemies)
                if (enemy.type == type)
                    return enemy;

            return null;
        }
    }

    [Serializable]
    public class Enemy
    {
        public GameObject prefab;
        public EnemyType type;
        public int health;
        public int damage;
    }
}