using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerRunner : MonoBehaviour
{
    [Header("MENU ELEMENTS")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    [Header("GAME ELEMENTS")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _highestScoreText;

    [Header("GAME END ELEMENTS")]
    [SerializeField] private GameObject _gameEndPanel;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private TextMeshProUGUI _gameEndScoreText;
    [SerializeField] private TextMeshProUGUI _gameEndCoinText;
    [SerializeField] private TextMeshProUGUI _gameEndTotalScoreText;
    [SerializeField] private TextMeshProUGUI _highestScoreFeedBackText;

    [Header("Broadcast Events")]
    [SerializeField] private VoidEventChannelSO _gameStarted;
    [SerializeField] private VoidEventChannelSO _gameReStarted;
    [Header("Listening Events")]
    [SerializeField] private IntEventChannelSO _playerScoreChanged;
    [SerializeField] private IntEventChannelSO _playerCollectedCoinChanged;
    [SerializeField] private VoidEventChannelSO _playerDied;

    private int _totalCoin;
    private int _currentScore;
    private int _highestScore;

    private const string HIGHEST_SCORE_KEY = "HighestScore";
    private void Awake()
    {
        _startButton.onClick.AddListener(OnGameStarted);
        _quitButton.onClick.AddListener(QuitButtonClicked);
        _playAgainButton.onClick.AddListener(RestartGame);
        _playerScoreChanged.OnEventRaised += UpdateScore;
        _playerCollectedCoinChanged.OnEventRaised += UpdateCoin;
        _playerDied.OnEventRaised += ActivateGameEndPanel;
        _highestScore = PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);
    }

    private void RestartGame()
    {
        SetActivePanel(_menuPanel.name);
        _gameReStarted.RaiseEvent();
        _totalCoin = 0;
        _currentScore = 0;
        UpdateCoin(_totalCoin);
        UpdateScore(_currentScore);
        _highestScoreFeedBackText.gameObject.SetActive(false);
    }

    private void UpdateCoin(int coinCount)
    {
        _totalCoin += coinCount;
        _coinText.text = "Coin: " + _totalCoin.ToString(); ;
    }

    private void UpdateScore(int currentZ)
    {
        if (currentZ >= 0)
        {
            _currentScore = currentZ;
            _currentScoreText.text = "Score: " + _currentScore.ToString();
        }

    }
    private void ActivateGameEndPanel()
    {
       SetActivePanel(_gameEndPanel.name);
        CalculateScore();
    }

    private void CalculateScore()
    {
        _gameEndScoreText.text = "Score: " + _currentScore.ToString();
        _gameEndCoinText.text = "Coin  : " + _totalCoin.ToString();
        int totalScore = _currentScore + _totalCoin;

        DOTween.To(() => 0, x => _gameEndTotalScoreText.text = x.ToString(), totalScore, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (totalScore > _highestScore)
                {
                    SaveScore(HIGHEST_SCORE_KEY, totalScore);
                    _highestScore = totalScore;
                    _highestScoreFeedBackText.transform.localScale = Vector3.zero;
                    _highestScoreFeedBackText.gameObject.SetActive(true);
                    _highestScoreFeedBackText.transform.DOScale(Vector3.one, 0.5f)
                        .SetEase(Ease.OutBounce);
                }
            });
    }

    private void SaveScore(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private void OnGameStarted()
    {
        _highestScoreText.text = "Highest Score \n" + _highestScore.ToString();
        _gameStarted.RaiseEvent();
        SetActivePanel(_gamePanel.name);
    }

    public void QuitButtonClicked()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    private void SetActivePanel(string activatePanel)
    {
        _menuPanel.gameObject.SetActive(activatePanel.Equals(_menuPanel.name));
        _gamePanel.gameObject.SetActive(activatePanel.Equals(_gamePanel.name));
        _gameEndPanel.gameObject.SetActive(activatePanel.Equals(_gameEndPanel.name));
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(OnGameStarted);
        _quitButton.onClick.RemoveListener(QuitButtonClicked);
        _playerScoreChanged.OnEventRaised -= UpdateScore;
        _playerCollectedCoinChanged.OnEventRaised -= UpdateCoin;
        _playerDied.OnEventRaised -= ActivateGameEndPanel;
    }
}
