using Balthazariy.ArenaBattle.Objects.Base;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Objects.Bullets
{
    public class PlayerBullet : BulletBase
    {
        public PlayerBullet(GameObject prefab, Transform parent, int damage, Vector3 playerRotation, Vector3 startPosition) : base(prefab, parent, damage, startPosition)
        {
            _selfTransform.localEulerAngles = playerRotation;
        }

        public override void Update()
        {
            base.Update();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _rigidbody.AddRelativeForce(Vector3.forward * BULLET_SPEED, ForceMode.Impulse);
        }
    }
}