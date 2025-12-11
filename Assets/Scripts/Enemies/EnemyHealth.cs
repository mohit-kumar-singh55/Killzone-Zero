using UnityEngine;

/// <summary>
/// このクラスは敵の体力を管理し、ダメージ処理と体力がゼロになった際の自己破壊を行う
/// </summary>
public class EnemyHealth : Health
{
    [SerializeField] GameObject robotExplosionVFX;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (currentHealth <= 0) SelfDestruct();
    }

    public void SelfDestruct()
    {
        WaveManager.Instance.AdjustEnemyCount(-1);      // 敵を1体削除する
        AudioManager.Instance.PlayEnemyExplosionSFX();      // sfx
        Instantiate(robotExplosionVFX, transform.position, Quaternion.identity);        // vfx
        Destroy(gameObject);
    }

    public void SelfDestructAfterSeconds(float seconds) => Invoke(nameof(SelfDestruct), seconds);
}
