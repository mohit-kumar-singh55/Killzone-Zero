using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerHealth クラスは、プレイヤーの体力管理、UI 更新、ゲームオーバー／勝利イベントを担当するクラス
/// </summary>
public class PlayerHealth : Health
{
    [SerializeField] CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;

    private int _gameOverVirCamPriority = 20;

    public static event Action OnPlayerDie = delegate { };

    void OnEnable()
    {
        WaveManager.OnWin += PlayerWin;
    }

    void OnDisable()
    {
        WaveManager.OnWin -= PlayerWin;
    }

    protected override void Awake()
    {
        base.Awake();
        AdjustShieldUI();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        // changing ui
        AdjustShieldUI();

        if (currentHealth <= 0)
        {
            // 死んでいなければ、リスポーンする
            bool isDead = PlayerManager.Instance.OnLiveLost(transform);

            // 死んだらゲームオーバー
            if (isDead) PlayerGameOver();
            // 死んでいなければ、UIをリセット
            else
            {
                currentHealth = startingHealth;
                AdjustShieldUI();
            }
        }
    }

    void PlayerGameOver()
    {
        OnPlayerDie?.Invoke();  // プレイヤーの死亡を通知

        // カメラをゲームオーバーの仮想カメラに変更する
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = _gameOverVirCamPriority;

        // プレイヤーを破棄する
        Destroy(gameObject);
    }

    void PlayerWin()
    {
        enabled = false;
        if (TryGetComponent(out FirstPersonController fpc)) fpc.enabled = false;
    }

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {

            if (i < currentHealth) shieldBars[i].gameObject.SetActive(true);
            else shieldBars[i].gameObject.SetActive(false);
        }
    }
}