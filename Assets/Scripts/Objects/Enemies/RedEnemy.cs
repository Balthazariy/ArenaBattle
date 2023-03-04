using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Players;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Enemies
{
    public class RedEnemy : EnemyBase
    {
        public RedEnemy(GameObject prefab,
                        Transform parent,
                        Vector3 startPosition,
                        Player player,
                        int health,
                        int damage,
                        float moveSpeed,
                        float rotatingSpeed,
                        float attackCountdownTime) : base(prefab, parent, startPosition, player, health, damage, moveSpeed, rotatingSpeed, attackCountdownTime)
        {

        }

        public override void Update()
        {
            base.Update();


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();


        }
    }
}