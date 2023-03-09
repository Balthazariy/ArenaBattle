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

        }

        private void OnDisable()
        {

        }

        private void Awake()
        {
            _selfObject = this.gameObject;
            _startButton.onClick.AddListener(StartButtonOnClickHandler);
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