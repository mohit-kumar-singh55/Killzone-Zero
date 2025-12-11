using UnityEngine;

/// <summary>
/// Turretの弾
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject projectileHitVFX;

    private Rigidbody _rb;

    private int _damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ** 難易度に応じて一部の設定を上書きする **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        speed = settings.turretProjectileSpeed;

        _rb.linearVelocity = transform.forward * speed;
    }

    public void Init(int damage) => _damage = damage;

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーにダメージを与える
        if (other.TryGetComponent(out PlayerHealth playerHealth)) playerHealth.TakeDamage(_damage);

        // hit vfx
        Instantiate(projectileHitVFX, transform.position, Quaternion.identity);

        // destroy
        Destroy(gameObject);
    }
}
