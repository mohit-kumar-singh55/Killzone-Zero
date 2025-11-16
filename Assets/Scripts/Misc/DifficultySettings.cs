using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficultySettings", menuName = "Scriptable Objects/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    public int[] enemiesPerWave;
    public bool useTurrets;
    public float turretFireRate = 2f;
    public int turretFireDamage = 2;
    public float turretProjectileSpeed = 15f;
}