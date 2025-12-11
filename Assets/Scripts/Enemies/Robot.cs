using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
public class Robot : MonoBehaviour
{
    private FirstPersonController _player;
    private EnemyHealth _enemyHealth;
    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        _player = FindFirstObjectByType<FirstPersonController>();
    }

    void Update()
    {
        if (!_player) return;
        if (_agent.enabled) _agent.SetDestination(_player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER)) _enemyHealth.SelfDestruct();
    }
}
