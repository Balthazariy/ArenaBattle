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


    }
}