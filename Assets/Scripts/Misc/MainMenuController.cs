using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UIDocument mainMenuUI;

    [Header("Element Names")]
    [SerializeField] string mainMenu = "MainMenu";
    [SerializeField] string optionsMenu = "OptionsMenu";
    [SerializeField] string startButton = "StartButton";
    [SerializeField] string quitButton = "QuitButton";
    [SerializeField] string optionsButton = "OptionsButton";
    [SerializeField] string backButton = "BackButton";
    [SerializeField] string unlimitedBulletsCheckbox = "UnlimitedBulletsToggle";
    [SerializeField] string fireTypeDropdownText = "FireType";

    private VisualElement root;

    void Start()
    {
        if (mainMenuUI == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        // Access the root VisualElement
        root = mainMenuUI.rootVisualElement;

        // fetch checkpoint toggle
        Toggle unlimitedBulletsToggle = root.Q<Toggle>(unlimitedBulletsCheckbox);

        // resetting options menu
        ShowOptions(false);

        // ** adding listeners **
        GetButton(startButton).clicked += LoadNewGame;
        GetButton(quitButton).clicked += Quit;
        GetButton(optionsButton).clicked += () => ShowOptions(true);
        GetButton(backButton).clicked += () => ShowOptions(false);

        // ** setting unlimitedBullets toggle **
        unlimitedBulletsToggle.value = PlayerPrefs.GetInt(UnlimitedBulletsManager.UNLIMITED_BULLETS_KEY, 1) == 1;    // 0=off, 1=on
        unlimitedBulletsToggle.RegisterValueChangedCallback(evt => PlayerPrefs.SetInt(UnlimitedBulletsManager.UNLIMITED_BULLETS_KEY, evt.newValue ? 1 : 0));

        // ** setting fire type dropdown **
        DropdownField fireTypeDropdown = root.Q<DropdownField>(fireTypeDropdownText);

        fireTypeDropdown.index = PlayerPrefs.GetInt(FireTypeManager.FIRE_TYPE_KEY, 0);
        fireTypeDropdown.RegisterValueChangedCallback(evt =>
        {
            string val = evt.newValue;
            int idx = fireTypeDropdown.choices.IndexOf(val);

            PlayerPrefs.SetInt(FireTypeManager.FIRE_TYPE_KEY, idx);
            PlayerPrefs.Save();
        });

        // final save
        PlayerPrefs.Save();
    }

    Button GetButton(string name) => root.Q<Button>(name);

    public void LoadNewGame() => SceneLoader.LoadScene(2);

    void ShowOptions(bool show)
    {
        root.Q(mainMenu).style.display = show ? DisplayStyle.None : DisplayStyle.Flex;
        root.Q(optionsMenu).style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void Quit() => Application.Quit();
}
