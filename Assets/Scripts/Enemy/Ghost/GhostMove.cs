using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    public Transform target;
    public Transform triggerTarget;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Collider2D playerCollider;

    private float speed = 3f;
    private float triggerDistance = 0;
    private float towerDistance = 0;
    private float triggerRange = 4f;
    private bool atPlayer = false;


    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
        target = findTower();
        triggerTarget = GameObject.Find("Player").transform;
        StartCoroutine(phaseOut());
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

    public IEnumerator phaseOut()
    {
        while (true)
        {
            float priorOpacity = spriteRenderer.color.a;
            spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.05f);
            SetColliderInvisible(true);
            yield return new WaitForSeconds(1.5f);
            spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, priorOpacity);
            SetColliderInvisible(false);
            yield return new WaitForSeconds(3f);
        }
        
    }

    private void SetColliderInvisible(bool invisible)
    {
        Physics2D.IgnoreCollision(
            boxCollider,
            playerCollider,
            invisible
        );

        // Your visibility changes here.
    }
}
