using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask interactionLayers;   // to know what objects can be shooted with raycast
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;

    CinemachineImpulseSource impulseSource;
    FireTypeManager fireTypeManager;

    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        fireTypeManager = FireTypeManager.Instance;
    }

    public void Shoot(WeaponSO weaponSO)
    {
        // muzzle flash
        muzzleFlash.Play();

        // impulse to shake camera
        impulseSource.GenerateImpulse();

        // playing sfx
        AudioManager.Instance.PlayGunShotSFX();

        // raycast shoot
        if (fireTypeManager.CurrentFireType == FireType.Raycast) HitScanShoot(weaponSO);
        // bullet shoot
        else ProjectileShoot();
    }

    // raycast shoot
    void HitScanShoot(WeaponSO weaponSO)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore))
        {
            // hit vfx
            Instantiate(weaponSO.HitVFXPrefab, hit.point, Quaternion.identity);
            // enemy health
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            enemyHealth?.TakeDamage(weaponSO.Damage);
        }
    }

    // bullet shoot
    // TODO: change this to object pooling
    void ProjectileShoot() => Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
}
