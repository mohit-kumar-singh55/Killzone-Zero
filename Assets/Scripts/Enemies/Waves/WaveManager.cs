using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Tooltip("Total no. of waves and enemies related to each wave")]
    [SerializeField] int[] enemiesPerWave;                  // will be overwritten by difficulty setting
    [Range(1f, 10f)][SerializeField] float timeBetweenEnemiesSpawn = 2f;
    [Tooltip("Time between last wave ends and next wave starts")]
    [SerializeField] int timeBetweenWaves = 10;

    PlayerHealth player;
    UIManager uIManager;

    SpawnGate[] spawnGates;
    int totalNoOfWaves;
    int currentWave;
    bool isUnderWave = false;
    int timer = 0;
    int enemyCount = 0;

    public delegate void WinSequence();
    public static event WinSequence OnWin;

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
        // ** overriding some settings as per difficulty **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        enemiesPerWave = settings.enemiesPerWave;

        // initialize
        totalNoOfWaves = enemiesPerWave.Length;
        currentWave = 0;        // no wave

        uIManager = UIManager.Instance;

        player = FindFirstObjectByType<PlayerHealth>();
        spawnGates = FindObjectsByType<SpawnGate>(FindObjectsSortMode.None);

        // first wave (最初の波)
        StartNextWave();
    }

    void LateUpdate()
    {
        // stop if player is dead (死んだら止める)
        if (player == null) StopAllCoroutines();

        // if all enemies are dead and not last wave, start next wave (全ての敵が死んだら次の波を始める)
        if (currentWave < totalNoOfWaves && !isUnderWave && enemyCount <= 0 && timer <= 0) StartCoroutine(StartTimerAndNextWave());
        // win
        else if (currentWave >= totalNoOfWaves && enemyCount <= 0) OnPlayerWin();
    }

    void StartNextWave()
    {
        if (currentWave >= totalNoOfWaves || player == null) return;

        currentWave++;
        StartCoroutine(StartWave(currentWave - 1));

        uIManager.SetCurrentWaveCountText(currentWave);
    }

    public void AdjustEnemyCount(int amount)
    {
        enemyCount += amount;
        UIManager.Instance.SetEnemyLeftText(enemyCount);
    }

    void OnPlayerWin()
    {
        OnWin?.Invoke();
        enabled = false;
    }

    IEnumerator StartWave(int waveIndex)
    {
        isUnderWave = true;

        for (int i = 0; i < enemiesPerWave[waveIndex]; i++)
        {
            // change it to object pooling later (後でオブジェクトプールに変更する)
            spawnGates[Random.Range(0, spawnGates.Length)].SpawnEnemy();
            AdjustEnemyCount(1);
            yield return new WaitForSeconds(timeBetweenEnemiesSpawn);
        }

        isUnderWave = false;
    }

    IEnumerator StartTimerAndNextWave()
    {
        uIManager.ShowWaveCountdown(true);
        timer = 0;

        while (timer < timeBetweenWaves)
        {
            timer++;
            uIManager.SetWaveCountdownText(timeBetweenWaves - timer);
            yield return new WaitForSeconds(1f);
        }

        // start next wave (次の波を始める)
        StartNextWave();

        timer = 0;
        uIManager.ShowWaveCountdown(false);
    }
}
