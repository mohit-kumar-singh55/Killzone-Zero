using UnityEngine;

public enum FireType { Raycast, Projectile };

public class FireTypeManager : MonoBehaviour
{
    public static FireTypeManager Instance { get; private set; }

    public static readonly string FIRE_TYPE_KEY = "FireType";

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
        // load from player prefs or default to raycast
        currentFireType = (FireType)PlayerPrefs.GetInt(FIRE_TYPE_KEY, (int)FireType.Raycast);
    }
}
