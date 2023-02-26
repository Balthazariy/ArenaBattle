using Balthazariy.ArenaBattle.Factories;
using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

namespace Balthazariy.ArenaBattle.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private FactoryController _factoryController;

        private void Update()
        {

        }

        private void OnMouseClick(InputValue value)
        {
            Debug.Log(123);
            _factoryController.CreateShoot(Factories.Shooting.ShootType.Player);
        }
    }
}