using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform target;
    public Transform triggerTarget;

    private float speed = 3f;
    private float triggerDistance = 0;
    private float towerDistance = 0;
    private float triggerRange = 4f;
    private bool atPlayer = false;


    public void Start()
    {
        target = findTower();
        triggerTarget = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (target == null)
        {
            target = findTower();
            if (target == null)
            {
                return;
            }
        }
        float previousX = transform.position.x;

        triggerDistance = Vector2.Distance(transform.position, triggerTarget.position);
        towerDistance = Vector2.Distance(transform.position, target.position);
        /*Vector2 direction = target.position - transform.position;
        direction.Normalize();
        
        Direction logic not used currently, but might get used later so going to leave it here.
        */

        if (triggerDistance < triggerRange)
        {
            atPlayer = true;
            transform.position = Vector2.MoveTowards(transform.position, triggerTarget.position, speed * Time.deltaTime);
        }
        else
        {
            if (atPlayer == true)
            {
                target = findTower();
                atPlayer = false;
            }
            if (towerDistance > 0.1)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);    
            }
        }

        float movementX = transform.position.x - previousX;
        if (Mathf.Abs(movementX) > 0.0001f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (movementX > 0 ? 1 : -1);
            transform.localScale = scale;
        }
    }


    public Transform findTower()
    {
        float distance = int.MaxValue;
        GameObject towerReturn = null;

        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if (Vector3.Distance(transform.position, tower.transform.position) < distance)
            {
                towerReturn = tower;
                distance = Vector3.Distance(transform.position, tower.transform.position);
            }
        }
        if (towerReturn != null)
        {
            Debug.Log("Found tower!");
            return towerReturn.transform;
        }
        return null;
    }
}
