using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] GameObject WinUI;
    [SerializeField] TMP_Text currentWaveCountText;
    [SerializeField] GameObject waveCountdownContainer;
    [SerializeField] TMP_Text waveCountdownText;
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject gameOverUI;

    const string ENEMIES_LEFT_STRING = "Enemies Left: ";
    const string CURRENT_WAVE_COUNT_STRING = "Wave: ";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetEnemyLeftText(int amount) => enemiesLeftText.text = ENEMIES_LEFT_STRING + amount.ToString();

    public void ShowWinUI(bool show) => WinUI.SetActive(show);

    public void ShowGameOverUI(bool show) => gameOverUI.SetActive(show);

    public void SetCurrentWaveCountText(int count) => currentWaveCountText.text = CURRENT_WAVE_COUNT_STRING + count.ToString();

    public void ShowWaveCountdown(bool show) => waveCountdownContainer.SetActive(show);

    public void SetWaveCountdownText(int count) => waveCountdownText.text = count.ToString();

    public void ShowMenuUI(bool show) => menuUI.SetActive(show);
}
