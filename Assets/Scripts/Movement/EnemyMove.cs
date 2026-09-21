using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform target;
    public Transform triggerTarget;

    private float speed = 3f;
    private float triggerDistance = 0;
    private float towerDistance = 0;
    private float triggerRange = 4f;


    void Update()
    {
        triggerDistance = Vector2.Distance(transform.position, triggerTarget.position);
        towerDistance = Vector2.Distance(transform.position, target.position);
        /*Vector2 direction = target.position - transform.position;
        direction.Normalize();
        
        Direction logic not used currently, but might get used later so going to leave it here.
        */

        if (triggerDistance < triggerRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, triggerTarget.position, speed * Time.deltaTime);
        }
        else
        {
            if (towerDistance > 0.1)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);    
            }
            
        }
    }
}
