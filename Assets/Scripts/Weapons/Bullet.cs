using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 生成時に前方へ移動し、ロボットの物理挙動と体力に影響を与え、
/// 衝突時、一定時間経過で自己破壊する弾丸を定義するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 500f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 前進
        _rb.AddForce(Camera.main.transform.forward * moveSpeed, ForceMode.Impulse);

        Invoke(nameof(SelfDestroy), 2f);
    }

    // ロボットに衝突
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TAGS.ROBOT))
        {
            Rigidbody robotRb = collision.gameObject.GetComponent<Rigidbody>();
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            // ナビメッシュを無効化して、スタックしないようにする
            if (agent) agent.enabled = false;

            // 物理を有効化して、力を加える
            if (robotRb)
            {
                robotRb.isKinematic = false;
                robotRb.AddForce((transform.forward - robotRb.transform.forward).normalized * 50f, ForceMode.Impulse);
            }

            // 一定時間後にロボットを破壊する
            if (enemyHealth) enemyHealth.SelfDestructAfterSeconds(2f);

            SelfDestroy();
        }
    }

    void SelfDestroy() => Destroy(gameObject);
}