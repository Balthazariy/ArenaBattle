using Balthazariy.ArenaBattle.Models;
using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Players;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Enemies
{
    public class RedEnemy : EnemyBase
    {
        public RedEnemy(Transform parent,
                        Vector3 startPosition,
                        Player player,
                        float moveSpeed,
                        float rotatingSpeed,
                        float attackCountdownTime,
                        Enemy data) : base(parent, startPosition, player, moveSpeed, rotatingSpeed, attackCountdownTime, data)
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