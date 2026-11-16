using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private DamageManager damageManager;
    private EnemyHealth enemyHealth;
    private int damage = 1;
    private int towerDamage = 1;
    private float knockbackSpeed = 8f;
    private float knockbackLift = 5f;
    private float knockbackDuration = 0.25f;

    public void Start()
    {
        damageManager = GameObject.Find("GameManager").GetComponent<DamageManager>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        if (collision.gameObject.name == "Player")
        {
            damageManager.enemyDealDamage(damage, transform.position, knockbackSpeed, knockbackLift, knockbackDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Mask")
        {
            if (enemyHealth.IsDying)
            {
                return;
            }
            Debug.Log("Wasp entered Mask trigger");
            damageManager.enemyDealTowerDamage(towerDamage);
            enemyHealth.Die();
            
        }
    }
}
