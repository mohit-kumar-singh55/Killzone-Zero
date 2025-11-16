using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip gunShotSFX;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ValidateFields();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGunShotSFX(float volume = .4f) => audioSource.PlayOneShot(gunShotSFX, volume);

    void ValidateFields()
    {
        Assert.IsNotNull(gunShotSFX, "gunShotSFX cannot be null");
    }
}
