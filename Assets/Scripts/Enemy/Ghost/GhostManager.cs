using System;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    private DamageManager damageManager;
    [NonSerialized] public int listIndex = 0;

    public void Start()
    {
        damageManager = GameObject.Find("GameManager").GetComponent<DamageManager>();
        listIndex = damageManager.addEnemy(gameObject);
    }
}
