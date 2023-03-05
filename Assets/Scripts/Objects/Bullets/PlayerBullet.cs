using Balthazariy.ArenaBattle.Objects.Base;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Bullets
{
    public class PlayerBullet : BulletBase
    {

        public PlayerBullet(GameObject prefab,
                            Transform parent,
                            int damage,
                            Vector3 playerRotation,
                            Vector3 startPosition) : base(prefab, parent, damage, playerRotation, startPosition)
        {
        }
    }
}