using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Balthazariy.ArenaBattle.Screens
{
    public class GameplayPage : MonoBehaviour
    {
        [SerializeField] private Image _healthImage;
        [SerializeField] private Image _energyImage;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _healthValueText;
        [SerializeField] private TextMeshProUGUI _energyValueText;
        [SerializeField] private TextMeshProUGUI _preStartTimerText;

        [SerializeField] private Button _pauseButton;

        [SerializeField] private PausePage _pausePage;

        private GameObject _selfObject;

        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseButtonOnClickHandler);
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(PauseButtonOnClickHandler);
        }

        private void Awake()
        {
            _selfObject = this.gameObject;

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

        public void UpdateHealthValue(int value, int limitValue)
        {
            _healthValueText.text = "Health: " + value.ToString() + "/" + limitValue.ToString();
            _healthImage.fillAmount = value * 0.01f;
        }

        public void UpdateEnergyValue(int value, int limitValue)
        {
            _energyValueText.text = "Energy: " + value.ToString() + "/" + limitValue.ToString();
            _energyImage.fillAmount = value * 0.002f;
        }

        public void UpdateScoreValue(int value) => _scoreText.text = "Score: " + value.ToString();
        public void UpdateTimeValue(int minutes, int seconds) => _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        public void UpdatePreStartTimerValue(int value) => _preStartTimerText.text = value.ToString();
        public void ActivePreStartTimer(bool isActive) => _preStartTimerText.gameObject.SetActive(isActive);

        private void PauseButtonOnClickHandler()
        {
            Main.Instance.PauseGame(true);

            _pausePage.Show();
        }
    }
}