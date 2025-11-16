using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject robotExplosionVFX;
    [Tooltip("Enemy health only works when firing with raycast! (敵の体力はレイキャストでのみ有効!)")]
    [SerializeField] int startingHealth = 3;

    int currentHealth;

    void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0) SelfDestruct();
    }

    public void SelfDestruct()
    {
        WaveManager.Instance.AdjustEnemyCount(-1);      // deleting one enemy
        AudioManager.Instance.PlayEnemyExplosionSFX();      // sfx
        Instantiate(robotExplosionVFX, transform.position, Quaternion.identity);        // vfx
        Destroy(gameObject);
    }

    public void SelfDestructAfterSeconds(float seconds) => Invoke(nameof(SelfDestruct), seconds);
}
