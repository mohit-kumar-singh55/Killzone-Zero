using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UIDocument mainMenuUI;

    [Header("Element Names")]
    [SerializeField] string mainMenu = "MainMenu";
    [SerializeField] string optionsMenu = "OptionsMenu";
    [SerializeField] string instructionsMenu = "InstructionsUI";
    [SerializeField] string startButton = "StartButton";
    [SerializeField] string quitButton = "QuitButton";
    [SerializeField] string optionsButton = "OptionsButton";
    [SerializeField] string instructionsButton = "InstructionsButton";
    [SerializeField] string backButton = "BackButton";
    [SerializeField] string unlimitedBulletsCheckbox = "UnlimitedBulletsToggle";
    [SerializeField] string fireTypeDropdownText = "FireType";

    private VisualElement _root;
    private VisualElement _menuUI;
    private VisualElement _optionsUI;
    private VisualElement _instructionsUI;

    void Start()
    {
        if (mainMenuUI == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        // _root VisualElement にアクセスする
        _root = mainMenuUI.rootVisualElement;

        // チェックボックス用トグルを取得
        Toggle unlimitedBulletsToggle = _root.Q<Toggle>(unlimitedBulletsCheckbox);

        // ** UI のセットアップ **
        _menuUI = _root.Q(mainMenu);
        _optionsUI = _root.Q(optionsMenu);
        _instructionsUI = _root.Q(instructionsMenu);

        // メニューをリセットする
        Back();

        // ** リスナーのセットアップ **
        GetButton(startButton).clicked += LoadNewGame;
        GetButton(quitButton).clicked += Quit;
        GetButton(optionsButton).clicked += () => ShowUI(_optionsUI);
        GetButton(instructionsButton).clicked += () => ShowUI(_instructionsUI);
        List<Button> backButtons = GetButtons(backButton);
        backButtons.ForEach(b => b.clicked += Back);

        // ** unlimitedBullets トグルを設定する **
        unlimitedBulletsToggle.value = PlayerPrefs.GetInt(PLAYER_PREFS.UNLIMITED_BULLETS_KEY, 1) == 1;    // 0=off, 1=on
        unlimitedBulletsToggle.RegisterValueChangedCallback(evt => PlayerPrefs.SetInt(PLAYER_PREFS.UNLIMITED_BULLETS_KEY, evt.newValue ? 1 : 0));

        // ** 射撃タイプのドロップダウンを設定する **
        DropdownField fireTypeDropdown = _root.Q<DropdownField>(fireTypeDropdownText);

        fireTypeDropdown.index = PlayerPrefs.GetInt(PLAYER_PREFS.FIRE_TYPE_KEY, 0);
        fireTypeDropdown.RegisterValueChangedCallback(evt =>
        {
            string val = evt.newValue;
            int idx = fireTypeDropdown.choices.IndexOf(val);

            PlayerPrefs.SetInt(PLAYER_PREFS.FIRE_TYPE_KEY, idx);
            PlayerPrefs.Save();
        });

        // final save
        PlayerPrefs.Save();
    }

    private Button GetButton(string name) => _root.Q<Button>(name);
    private List<Button> GetButtons(string name) => _root.Query<Button>(name).ToList();

    public void LoadNewGame() => SceneLoader.LoadScene(SCENES.MAIN_LEVEL);

    private void ShowUI(VisualElement ui)
    {
        _menuUI.style.display = DisplayStyle.None;
        ui.style.display = DisplayStyle.Flex;
    }

    public void Back()
    {
        _instructionsUI.style.display = DisplayStyle.None;
        _optionsUI.style.display = DisplayStyle.None;
        _menuUI.style.display = DisplayStyle.Flex;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}