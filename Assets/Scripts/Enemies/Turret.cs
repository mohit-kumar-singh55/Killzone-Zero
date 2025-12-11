using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform _playerTargetPoint;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int damage = 2;

    private PlayerHealth _player;

    private void Start()
    {
        // ** 難易度に応じて設定を上書き **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        if (!settings.useTurrets)
        {
            gameObject.SetActive(false);
            return;
        }
        fireRate = settings.turretFireRate;
        damage = settings.turretFireDamage;

        _player = FindFirstObjectByType<PlayerHealth>();

        StartCoroutine(FireRoutine());
    }

    private void Update()
    {
        // 自分をプレイヤーに向ける
        turretHead.LookAt(_playerTargetPoint);
    }

    IEnumerator FireRoutine()
    {
        while (_player)
        {
            yield return new WaitForSeconds(fireRate);
            // TODO: change to object pooling
            Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
            newProjectile.transform.LookAt(_playerTargetPoint);
            newProjectile.Init(damage);
        }
    }
}
