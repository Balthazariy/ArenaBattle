using Balthazariy.ArenaBattle.Factories.Shooting;
using UnityEngine;

namespace Balthazariy.ArenaBattle.Factories
{
    public class FactoryController : MonoBehaviour
    {
        private ShootFactory _shootFactory;

        private void Awake()
        {

        }

        private void Start()
        {
            _shootFactory = new ShootFactory();
        }

        public void CreateShoot(ShootType type)
        {
            _shootFactory.InitShootByType(type);
        }

        private void Update()
        {
            if (_shootFactory == null)
                return;

            _shootFactory.Update();
        }

        private void FixedUpdate()
        {
            if (_shootFactory == null)
                return;

            _shootFactory.FixedUpdate();
        }
    }
}