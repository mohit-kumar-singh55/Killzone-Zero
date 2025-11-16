using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject projectileHitVFX;

    Rigidbody rb;

    int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ** overriding some settings as per difficulty **
        DifficultySettings settings = DifficultyManager.Instance?.CurrentSettings;
        speed = settings.turretProjectileSpeed;

        rb.linearVelocity = transform.forward * speed;
    }

    public void Init(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        // player health
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage);

        // hit vfx
        Instantiate(projectileHitVFX, transform.position, Quaternion.identity);

        // destroy
        Destroy(gameObject);
    }
}
