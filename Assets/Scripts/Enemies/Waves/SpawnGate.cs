using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] Transform spawnPoint;

    public void SpawnEnemy() => Instantiate(robotPrefab, spawnPoint.position, transform.rotation);
}
