using UnityEngine;

public enum FireType { Raycast, Projectile };

public class FireTypeManager : MonoBehaviour
{
    public static FireTypeManager Instance { get; private set; }

    private FireType currentFireType;

    public FireType CurrentFireType { get => currentFireType; }

    private void Awake()
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
        // PlayerPrefs から読み込む／なければレイキャストをデフォルトとして使用する
        currentFireType = (FireType)PlayerPrefs.GetInt(PLAYER_PREFS.FIRE_TYPE_KEY, (int)FireType.Raycast);
    }
}
