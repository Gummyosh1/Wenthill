using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class HatProjectile : MonoBehaviour
{
    private PlayerHatAttack playerHatAttack;
    [NonSerialized] public bool right = false;
    private float speed = 4f;
    private bool moving = true;
    private bool bufferDone = false;
    private float spawnBufferTimer = 1f;
    private float timeout = 5f;
    private Coroutine hatAnimation = null;

    public void Start()
    {
        hatAnimation = StartCoroutine(moveHat());
        playerHatAttack = GameObject.Find("Player").GetComponent<PlayerHatAttack>();
        StartCoroutine(spawnBuffer());
        StartCoroutine(hatTimeout());
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            moving = false;
            Debug.Log("Enemy Hit");
            //IMPLEMENT ENEMY HIT LOGIC HERE FOR THE ENEMY
        }
        if (other.gameObject.name == "Player")
        {
            if (bufferDone)
            {
                playerHatAttack.hatInstance = null;
                Destroy(gameObject);
            }
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
    }

    public IEnumerator hatFall()
    {
        yield return null;
    }
    public IEnumerator spawnBuffer()
    {
        yield return new WaitForSeconds(spawnBufferTimer);
        bufferDone = true;
    }

    public IEnumerator hatTimeout()
    {
        yield return new WaitForSeconds(timeout);
        playerHatAttack.hatInstance = null;
        Destroy(gameObject);
    }

    public void dealDamage()
    {
        Debug.Log("Dealing damage!");
    }
}
