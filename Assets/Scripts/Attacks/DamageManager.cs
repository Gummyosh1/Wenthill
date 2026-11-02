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
    
    public void enemyDealDamage(int damage, float knockback = 0)
    {
        playerHealth.TakeDamage(damage);
    }

    public void playerDealDamage(float damage, float knockback)
    {
        
    }
}
