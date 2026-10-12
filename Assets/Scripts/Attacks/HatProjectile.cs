using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class HatProjectile : MonoBehaviour
{
    [NonSerialized] public bool right = false;
    private float speed = 4f;
    private bool moving = true;
    private Coroutine hatAnimation = null;

    public void Start()
    {
        hatAnimation = StartCoroutine(moveHat());
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            moving = false;
            Debug.Log("Enemy Hit");
            //IMPLEMENT ENEMY HIT LOGIC HERE FOR THE ENEMY
        }
    }


    public IEnumerator moveHat()
    {
        while (moving)
        {
            if (right)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                yield return null;
            }
            else
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                yield return null;
            }
            
        }
        yield return new WaitForSeconds(0.3f);
        dealDamage();
        hatAnimation = null;
        Destroy(gameObject);
    }

    public void dealDamage()
    {
        Debug.Log("Dealing damage!");
    }
}
