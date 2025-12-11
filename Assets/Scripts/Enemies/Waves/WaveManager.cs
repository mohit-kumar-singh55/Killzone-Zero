using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ウェーブ開始、敵数の調整、プレイヤー勝利判定など、敵ウェーブ全体を管理するクラス
/// </summary>
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Tooltip("総ウェーブ数と各ウェーブに対応する敵数")]
    [SerializeField] int[] enemiesPerWave;                  // 難易度設定によって上書きされる
    [Range(1f, 10f)][SerializeField] float timeBetweenEnemiesSpawn = 2f;
    [Tooltip("最終ウェーブ終了から次のウェーブ開始までの時間")]
    [SerializeField] int timeBetweenWaves = 10;

    private PlayerHealth _player;
    private UIManager _uIManager;

    private SpawnGate[] _spawnGates;
    private int _totalNoOfWaves;
    private int _currentWave;
    private bool _isUnderWave = false;
    private int _timer = 0;
    private int _enemyCount = 0;

    public static event Action OnWin = delegate { };

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
        // ** 難易度に応じて一部の設定を上書きする **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        enemiesPerWave = settings.enemiesPerWave;

        // initialize
        _totalNoOfWaves = enemiesPerWave.Length;
        _currentWave = 0;        // no wave

        _uIManager = UIManager.Instance;

        _player = FindFirstObjectByType<PlayerHealth>();
        _spawnGates = FindObjectsByType<SpawnGate>(FindObjectsSortMode.None);

        // 最初の波
        StartNextWave();
    }

    void LateUpdate()
    {
        // 死んだら止める
        if (_player == null) StopAllCoroutines();

        // 全ての敵が死んだら次の波を始める
        if (_currentWave < _totalNoOfWaves && !_isUnderWave && _enemyCount <= 0 && _timer <= 0) StartCoroutine(StartTimerAndNextWave());
        // win
        else if (_currentWave >= _totalNoOfWaves && _enemyCount <= 0) OnPlayerWin();
    }

    // 次の波
    void StartNextWave()
    {
        if (_currentWave >= _totalNoOfWaves || _player == null) return;

        _currentWave++;
        StartCoroutine(StartWave(_currentWave - 1));

        _uIManager.SetCurrentWaveCountText(_currentWave);
    }

    // 敵数の調整
    public void AdjustEnemyCount(int amount)
    {
        _enemyCount += amount;
        _uIManager.SetEnemyLeftText(_enemyCount);
    }

    void OnPlayerWin()
    {
        OnWin?.Invoke();
        enabled = false;
    }

    IEnumerator StartWave(int waveIndex)
    {
        _isUnderWave = true;

        for (int i = 0; i < enemiesPerWave[waveIndex]; i++)
        {
            // ランダムに敵を出現させる
            _spawnGates[UnityEngine.Random.Range(0, _spawnGates.Length)].SpawnEnemy();
            AdjustEnemyCount(1);
            yield return new WaitForSeconds(timeBetweenEnemiesSpawn);
        }

        _isUnderWave = false;
    }

    IEnumerator StartTimerAndNextWave()
    {
        _uIManager.ShowWaveCountdown(true);
        _timer = 0;

        while (_timer < timeBetweenWaves)
        {
            _timer++;
            _uIManager.SetWaveCountdownText(timeBetweenWaves - _timer);
            yield return new WaitForSeconds(1f);
        }

        // 次の波を始める
        StartNextWave();

        _timer = 0;
        _uIManager.ShowWaveCountdown(false);
    }
}
