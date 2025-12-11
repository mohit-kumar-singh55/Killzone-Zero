using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool menuActive = false;
    private bool gameEnded = false;
    UIManager uiManager;

    public bool MenuActive { get => menuActive; }

    void OnEnable()
    {
        WaveManager.OnWin += TriggerWin;
        PlayerHealth.OnPlayerDie += TriggerLose;
    }

    void OnDisable()
    {
        WaveManager.OnWin -= TriggerWin;
        PlayerHealth.OnPlayerDie -= TriggerLose;
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
        // メニューを開く
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded) SetShowMenu();
    }

    private void SetShowMenu()
    {
        menuActive = !menuActive;
        uiManager.ShowMenuUI(menuActive);
        Time.timeScale = menuActive ? 0 : 1;
        ShowCursor(menuActive);
    }

    private void ShowCursor(bool show = true)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    private void TriggerWin()
    {
        if (gameEnded) return;

        GameOverSequence();
        uiManager.ShowWinUI(true);
    }

    private void TriggerLose()
    {
        if (gameEnded) return;

        GameOverSequence();
        uiManager.ShowGameOverUI(true);
    }

    private void GameOverSequence()
    {
        gameEnded = true;
        Time.timeScale = 0;
        ShowCursor(true);
    }

    // *** UI から呼び出される ***
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1f;
        DifficultyManager.Instance.DestroyDifficultyManager();
        SceneManager.LoadScene(SCENES.MAIN_MENU);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
