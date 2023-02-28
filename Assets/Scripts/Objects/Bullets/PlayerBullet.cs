using Balthazariy.Objects.Base;
using UnityEngine;

namespace Balthazariy.Objects.Bullets
{
    public class PlayerBullet : BulletBase
    {
        public PlayerBullet(GameObject prefab, Transform parent) : base(prefab, parent)
        {
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