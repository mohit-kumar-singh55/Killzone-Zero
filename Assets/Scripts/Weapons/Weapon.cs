using Cinemachine;
using UnityEngine;

/// <summary>
/// 現在の射撃タイプに応じてレイキャストまたは弾丸を発射する機能を提供するクラス
/// </summary>
public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask interactionLayers;   // レイキャストで撃てるオブジェクトを判別するため
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;

    private CinemachineImpulseSource _impulseSource;
    private FireTypeManager _fireTypeManager;

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        _fireTypeManager = FireTypeManager.Instance;
    }

    public void Shoot(WeaponSO weaponSO)
    {
        // muzzle flash
        muzzleFlash.Play();

        // impulse to shake camera
        _impulseSource.GenerateImpulse();

        // playing sfx
        AudioManager.Instance.PlayGunShotSFX();

        // raycast shoot
        if (_fireTypeManager.CurrentFireType == FireType.Raycast) HitScanShoot(weaponSO);
        // bullet shoot
        else ProjectileShoot();
    }

    // raycast shoot
    void HitScanShoot(WeaponSO weaponSO)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore))
        {
            // hit vfx
            Instantiate(weaponSO.HitVFXPrefab, hit.point, Quaternion.identity);
            // enemy health
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth) enemyHealth.TakeDamage(weaponSO.Damage);
        }
    }

    // bullet shoot
    void ProjectileShoot() => Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
}
