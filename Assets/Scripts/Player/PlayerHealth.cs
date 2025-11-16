using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 5;
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;

    int currentHealth;
    int gameOverVirCamPriority = 20;
    GameManager gameManager;

    void OnEnable()
    {
        WaveManager.OnWin += PlayerWin;
    }

    void OnDisable()
    {
        WaveManager.OnWin -= PlayerWin;
    }

    void Awake()
    {
        currentHealth = startingHealth;
        AdjustShieldUI();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void TakeDamage(int amount)
    {
        if (gameManager.GameEnded) return;

        currentHealth -= amount;

        // changing ui
        AdjustShieldUI();

        if (currentHealth <= 0)
        {
            bool isDead = PlayerManager.Instance.OnLiveLost(transform);

            // dead
            if (isDead) PlayerGameOver();
            // refill health for respawn (死んだらリスポーンする)
            else
            {
                currentHealth = startingHealth;
                AdjustShieldUI();
            }
        }
    }

    void PlayerGameOver()
    {
        gameManager.TriggerLose();

        // transitioning camera to game over (death) virtual camera (カメラをゲームオーバーの仮想カメラに変更する)
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverVirCamPriority;

        // unlocking cursor
        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetsInputs.SetCursorState(false);

        // destroying player
        Destroy(gameObject);
    }

    void PlayerWin() => enabled = false;

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {

            if (i < currentHealth) shieldBars[i].gameObject.SetActive(true);
            else shieldBars[i].gameObject.SetActive(false);
        }
    }
}
