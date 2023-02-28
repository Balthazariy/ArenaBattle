using Balthazariy.ArenaBattle.Factories;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Balthazariy.ArenaBattle.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private FactoryController _factoryController;


        private void Start()
        {
        }

        private void Update()
        {
        }

        private void OnMouseClick(InputValue value)
        {
            _factoryController.CreateShoot(Factories.Shooting.ShootType.Player);
        }
    }
}