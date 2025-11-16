using UnityEngine;

public class UnlimitedBulletsManager : MonoBehaviour
{
    public static UnlimitedBulletsManager Instance { get; private set; }

    public static readonly string UNLIMITED_BULLETS_KEY = "UnlimitedBullets";

    private bool unlimitedBullets = false;

    public bool UnlimitedBullets => unlimitedBullets;

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
        unlimitedBullets = PlayerPrefs.GetInt(UNLIMITED_BULLETS_KEY, 1) == 1;
    }
}
