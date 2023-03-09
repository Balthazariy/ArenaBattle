using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Balthazariy.ArenaBattle.Screens
{
    public class GameOverPage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private GameObject _selfObject;

        private void Awake()
        {
            _selfObject = this.gameObject;
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

        public void SetScore(int score) => _scoreText.text = "Score: " + score.ToString();
        public void SetTime(int minutes, int seconds) => _timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        private void RestartButtonOnClickHandler()
        {
            Main.Instance.RestartGame();
            Hide();
            Main.Instance.gameplayPage.Show();
        }

        private void ExitButtonOnClickHandler()
        {
            Main.Instance.StopGameplay();
            Hide();
            Main.Instance.gameplayPage.Hide();
            Main.Instance.menuPage.Show();
        }

        public void OnRestart(InputValue value)
        {
            RestartButtonOnClickHandler();
        }

        public void OnExit(InputValue value)
        {
            ExitButtonOnClickHandler();
        }

        public void OnRestartUI()
        {
            OnRestart(null);
        }

        public void OnExitUI()
        {
            OnExit(null);
        }
    }
}