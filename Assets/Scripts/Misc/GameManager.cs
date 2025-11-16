using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool menuActive = false;
    private bool gameEnded = false;
    UIManager uiManager;

    public bool MenuActive { get => menuActive; }
    public bool GameEnded { get => gameEnded; set => gameEnded = value; }

    void OnEnable()
    {
        WaveManager.OnWin += TriggerWin;
    }

    void OnDisable()
    {
        WaveManager.OnWin -= TriggerWin;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        uiManager = UIManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded) SetShowMenu();
    }

    public void SetShowMenu()
    {
        menuActive = !menuActive;
        uiManager.ShowMenuUI(menuActive);
        Time.timeScale = menuActive ? 0 : 1;
        ShowCursor(menuActive);
    }

    void ShowCursor(bool show = true)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    public void TriggerWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        uiManager.ShowWinUI(true);
        Time.timeScale = 0;
        ShowCursor(true);
    }

    public void TriggerLose()
    {
        if (gameEnded) return;

        gameEnded = true;
        uiManager.ShowGameOverUI(true);
        Time.timeScale = 0;
        ShowCursor(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1f;
        DifficultyManager.Instance.DestroyDifficultyManager();
        SceneManager.LoadScene(1);
    }

    public void QuitButton() => Application.Quit();
}
