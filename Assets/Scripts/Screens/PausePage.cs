using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Balthazariy.ArenaBattle.Screens
{
    public class PausePage : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private GameObject _selfObject;

        private void Awake()
        {
            _selfObject = this.gameObject;
            _continueButton.onClick.AddListener(ContinueButtonOnClickHandler);
            _restartButton.onClick.AddListener(RestartButtonOnClickHandler);
            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);
            Hide();
        }

        public void Show()
        {
            _selfObject.SetActive(true);
        }

        public void Hide()
        {
            _selfObject.SetActive(false);
        }

        private void ContinueButtonOnClickHandler()
        {
            Main.Instance.PauseGame(false);
            Hide();
        }

        private void RestartButtonOnClickHandler()
        {
            Main.Instance.PauseGame(false);
            Main.Instance.RestartGame();
            Hide();
        }

        private void ExitButtonOnClickHandler()
        {
            Main.Instance.PauseGame(false);
            Main.Instance.StopGameplay();
            Hide();
            Main.Instance.gameplayPage.Hide();
            Main.Instance.menuPage.Show();
        }

        public void OnContinue(InputValue value)
        {
            ContinueButtonOnClickHandler();
        }

        public void OnRestart(InputValue value)
        {
            RestartButtonOnClickHandler();
        }

        public void OnExit(InputValue value)
        {
            ExitButtonOnClickHandler();
        }
    }
}