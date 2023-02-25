using Balthazariy.ArenaBattle.Shooting.Base;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Shooting
{
    public class PlayerShoot : ShootBase
    {
        public PlayerShoot(Transform parent, GameObject prefab) : base(parent, prefab)
        {
            SetMaterial(Resources.Load<Material>("Materials/PlayerBullet_Material"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _rigidbody.AddForce(Vector3.forward * BULLET_SPEED * Time.fixedDeltaTime);
        }
    }
}