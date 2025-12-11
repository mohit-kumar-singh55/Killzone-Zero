using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip gunShotSFX;
    public AudioClip enmeyExplosionSFX;

    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGunShotSFX(float volume = .4f) => _audioSource.PlayOneShot(gunShotSFX, volume);

    public void PlayEnemyExplosionSFX(float volume = .4f) => _audioSource.PlayOneShot(enmeyExplosionSFX, volume);

    void ValidateFields()
    {
        Assert.IsNotNull(gunShotSFX, "gunShotSFX cannot be null");
    }
}
