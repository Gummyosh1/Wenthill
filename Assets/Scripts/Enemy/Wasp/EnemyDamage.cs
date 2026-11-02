using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private DamageManager damageManager;
    private int damage = 1;
    private float knockback = 1.5f;

    public void Start()
    {
        damageManager = GameObject.Find("GameManager").GetComponent<DamageManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        if (collision.gameObject.name == "Player")
        {
            damageManager.enemyDealDamage(damage, knockback);
        }
    }
}
