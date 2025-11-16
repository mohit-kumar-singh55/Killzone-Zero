using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform playerTargetPoint;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int damage = 2;

    PlayerHealth player;

    private void Start()
    {
        // ** overriding some settings as per difficulty (難易度に応じて設定を上書き) **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        if (!settings.useTurrets)       // change it later
        {
            gameObject.SetActive(false);
            return;
        }
        fireRate = settings.turretFireRate;
        damage = settings.turretFireDamage;

        player = FindFirstObjectByType<PlayerHealth>();

        StartCoroutine(FireRoutine());
    }

    private void Update()
    {
        // rotates towards player to shoot (自分をプレイヤーに向ける)
        turretHead.LookAt(playerTargetPoint);
    }

    IEnumerator FireRoutine()
    {
        while (player)
        {
            yield return new WaitForSeconds(fireRate);
            Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
            newProjectile.transform.LookAt(playerTargetPoint);
            newProjectile.Init(damage);
        }
    }
}
