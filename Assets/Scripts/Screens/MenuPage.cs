using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Balthazariy.ArenaBattle.Screens
{
    public class MenuPage : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        private GameObject _selfObject;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartButtonOnClickHandler);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartButtonOnClickHandler);
        }

        private void Awake()
        {
            _selfObject = this.gameObject;

            Show();
        }

        public void Show()
        {
            _selfObject.SetActive(true);
        }

        public void Hide()
        {
            _selfObject.SetActive(false);
        }

        private void StartButtonOnClickHandler()
        {
            Main.Instance.PreStartGame();
        }

        private void OnStart(InputValue input)
        {
            StartButtonOnClickHandler();
        }

        public void OnStartUI()
        {
            OnStart(null);
        }
    }
}