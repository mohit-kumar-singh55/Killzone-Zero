using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのスポーンと残機管理を担当するクラス
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Tooltip("Total no. of lives")]
    [Range(1, 5)][SerializeField] int totalLives = 3;
    [SerializeField] Transform spawnPointsParent;

    private int _livesLeft;
    private List<Transform> _spawnPoints = new();
    private ActiveWeapon _activeWeapon;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // initialize
        _activeWeapon = FindAnyObjectByType<ActiveWeapon>();
        _livesLeft = totalLives;

        if (spawnPointsParent == null)
        {
            Debug.LogError("PlayerManager: spawnPointsParent is null");
            return;
        }

        // caching spawn points
        foreach (Transform spawnPoint in spawnPointsParent) _spawnPoints.Add(spawnPoint);
    }

    public bool OnLiveLost(Transform playerTrans)
    {
        _livesLeft--;

        // プレイヤーをスポーンする
        if (_livesLeft > 0 && spawnPointsParent != null)
        {
            playerTrans.gameObject.SetActive(false);

            // プレイヤーをランダムなスポーンポイントにスポーンする(シーンをリロードしない、死亡後にシーンをリロードする)
            int spawnPointIndex = Random.Range(0, _spawnPoints.Count);
            playerTrans.position = _spawnPoints[spawnPointIndex].position;

            playerTrans.gameObject.SetActive(true);

            // ammoをリセットする
            _activeWeapon.AdjustAmmo(200);
        }

        // ゲームオーバーになるか、リスポーンしてヘルスをリフィルする
        return _livesLeft <= 0;
    }
}
