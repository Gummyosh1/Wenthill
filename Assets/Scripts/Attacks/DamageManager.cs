using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    //as enemies spawn in, they report themselves into the enemyList
    public List<GameObject> enemyList = new List<GameObject>();

    public PlayerHealth playerHealth;
    
    public int addEnemy(GameObject enemyObject)
    {
        enemyList.Add(enemyObject);
        return enemyList.Count - 1;
    }
    
    public void enemyDealDamage(int damage, Vector2 attackerPosition, float knockbackSpeed, float knockbackLift, float knockbackDuration)
    {
        playerHealth.TakeDamage(damage, attackerPosition, knockbackSpeed, knockbackLift, knockbackDuration);
    }

    public void playerDealDamage(float damage, float knockback)
    {
        
    }
}
