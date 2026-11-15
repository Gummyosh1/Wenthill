using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private DamageManager damageManager;
    private int damage = 1;
    private float knockbackSpeed = 8f;
    private float knockbackLift = 5f;
    private float knockbackDuration = 0.25f;

    public void Start()
    {
        damageManager = GameObject.Find("GameManager").GetComponent<DamageManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        if (collision.gameObject.name == "Player")
        {
            damageManager.enemyDealDamage(damage, transform.position, knockbackSpeed, knockbackLift, knockbackDuration);
        }
    }
}
