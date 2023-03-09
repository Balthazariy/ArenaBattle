using Balthazariy.ArenaBattle.Objects.Base;
using Balthazariy.ArenaBattle.Players;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Bullets
{
    public class EnemyBullet : BulletBase
    {
        private Player _player;
        public EnemyBullet(GameObject prefab,
            Transform parent,
            int bulletDamage,
            Vector3 playerRotation,
            Player player,
            Vector3 startPosition) : base(prefab, parent, bulletDamage, playerRotation, startPosition)
        {
            _player = player;
        }

        public override void Update()
        {
            base.Update();

            var playerPosition = _player.GetPlayerPosition();
            _selfTransform.LookAt(new Vector3(playerPosition.x, playerPosition.y + 1, playerPosition.z));
            _selfTransform.position += _selfTransform.forward * (BULLET_SPEED * 3) * Time.deltaTime;
        }
    }
}