using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Tooltip("※敵の場合：敵の体力はレイキャストでのみ有効!")]
    [SerializeField, Range(1, 10)] protected int startingHealth = 3;

    protected int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = startingHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }
}