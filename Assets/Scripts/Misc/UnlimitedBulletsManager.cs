using UnityEngine;

public class UnlimitedBulletsManager : MonoBehaviour
{
    public static UnlimitedBulletsManager Instance { get; private set; }

    private bool _unlimitedBullets = false;

    public bool UnlimitedBullets => _unlimitedBullets;

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
        _unlimitedBullets = PlayerPrefs.GetInt(PLAYER_PREFS.UNLIMITED_BULLETS_KEY, 1) == 1;
    }
}
