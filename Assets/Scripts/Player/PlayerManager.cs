using System.Collections.Generic;
using UnityEngine;

// responsible for spawning and lives management of the player
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Tooltip("Total no. of lives")]
    [Range(1, 5)][SerializeField] int totalLives = 3;
    [SerializeField] Transform spawnPointsParent;

    private int livesLeft;
    private List<Transform> spawnPoints = new();
    private ActiveWeapon activeWeapon;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // initialize
        activeWeapon = FindAnyObjectByType<ActiveWeapon>();
        livesLeft = totalLives;

        if (spawnPointsParent == null)
        {
            Debug.LogError("PlayerManager: spawnPointsParent is null");
            return;
        }

        // caching spawn points
        foreach (Transform spawnPoint in spawnPointsParent) spawnPoints.Add(spawnPoint);
    }

    public bool OnLiveLost(Transform playerTrans)
    {
        livesLeft--;

        // spawn player
        if (livesLeft > 0 && spawnPointsParent != null)
        {
            playerTrans.gameObject.SetActive(false);

            // spawn player at a random spawn point (don't reload the scene, reload scene only after death)
            // プレイヤーをランダムなスポーンポイントにスポーンする(シーンをリロードしない、死亡後にシーンをリロードする)
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            playerTrans.position = spawnPoints[spawnPointIndex].position;

            playerTrans.gameObject.SetActive(true);

            // reset ammo
            activeWeapon.AdjustAmmo(200);
        }

        // game over if no lives left else respawn and refill the health
        // ゲームオーバーになるか、リスポーンしてヘルスをリフィルする
        return livesLeft <= 0;
    }
}
