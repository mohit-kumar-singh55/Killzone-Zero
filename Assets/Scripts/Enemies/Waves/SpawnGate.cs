using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] Transform spawnPoint;

    // TODO: change to object pooling
    public void SpawnEnemy() => Instantiate(robotPrefab, spawnPoint.position, transform.rotation);
}
