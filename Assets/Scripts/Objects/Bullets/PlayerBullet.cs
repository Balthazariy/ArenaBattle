using Balthazariy.ArenaBattle.Objects.Base;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Bullets
{
    public class PlayerBullet : BulletBase
    {

        private int _bulletHealth;
        private const float _chanceToNothing = 50.0f;
        private float _chanceToRicochet = 3f;
        private bool _isRicochet;

        public PlayerBullet(GameObject prefab,
                            Transform parent,
                            int damage,
                            Vector3 playerRotation,
                            Vector3 startPosition) : base(prefab, parent, damage, playerRotation, startPosition)
        {
            _bulletHealth = 1;

            InitChancing();

        }

        private void InitChancing()
        {
            int chance = UnityEngine.Random.Range(0, 100);

            if (chance >= _chanceToNothing)
                return;

            //int healthOrRicochetChance = UnityEngine.Random.Range(0, 100);

            //if (healthOrRicochetChance > 0 && healthOrRicochetChance <= 50)
            //{
            //    ++_bulletHealth;
            //}
            //else if (healthOrRicochetChance > 50 && healthOrRicochetChance <= 100)
            //{
            //    _isRicochet = true;
            //}
        }
    }
}