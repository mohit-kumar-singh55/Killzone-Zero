using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 500f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // move forward
        rb.AddForce(Camera.main.transform.forward * moveSpeed, ForceMode.Impulse);

        Invoke(nameof(SelfDestroy), 2f);
    }

    // kill enemy
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
            Rigidbody robotRb = collision.gameObject.GetComponent<Rigidbody>();
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            // disable navmesh, so it doesn't get stuck (ナビメッシュを無効化して、スタックしないようにする)
            if (agent) agent.enabled = false;

            // enable physics & add force (物理を有効化して、力を加える)
            if (robotRb)
            {
                robotRb.isKinematic = false;
                robotRb.AddForce((transform.forward - robotRb.transform.forward).normalized * 50f, ForceMode.Impulse);
            }

            // destroy robot after some time (一定時間後にロボットを破壊する)
            if (enemyHealth) enemyHealth.SelfDestructAfterSeconds(2f);

            SelfDestroy();
        }
    }

    void SelfDestroy() => Destroy(gameObject);
}