using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health = 10;
    private SpriteRenderer spriteRenderer;
    private Coroutine takeDamageAnimationInstance;
    private EnemyMove enemyMove;
    private GhostMove ghostMove;

    private SpriteRenderer explosionRenderer;
    public Sprite[] explosionList;
    private float explosionFrameTime = 0.06f;
    private bool isDying;


    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameObject.name == "Wasp")
        {
            enemyMove = GetComponent<EnemyMove>();
        }
        else
        {
            ghostMove = GetComponent<GhostMove>();
        }

        Transform explosion = transform.Find("BlowUp");

        if (explosion != null)
            explosionRenderer = explosion.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying)
        {
            return;
        }

        health -= damage;
        
        if (health <= 0)
        {
            Die();
            return;
        }

        takeDamageAnimationInstance = StartCoroutine(takeDamageAnimation());
    }

    public void Die()
    {
        isDying = true;
        if (enemyMove != null)
        {
            enemyMove.DeathSlow();
        }
        else
        {
            ghostMove.DeathSlow();
        }
        StartCoroutine(PlayExplosion());
    }

    public IEnumerator takeDamageAnimation()
    {
        float transitionTime = 0.08f;
        Color startColor = spriteRenderer.color;
        Color red = new Color(1f, 0f, 0f, startColor.a);

        // Quickly fade to red.
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(startColor, red, t / transitionTime);
            yield return null;
        }

        spriteRenderer.color = red;

        // Fade back to the normal color.
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(red, startColor, t / transitionTime);
            yield return null;
        }

        spriteRenderer.color = startColor;
    }


    private IEnumerator PlayExplosion()
    {
        explosionRenderer.gameObject.SetActive(true);

        WaitForSeconds frameDelay = new WaitForSeconds(explosionFrameTime);
        int index = 0;
        foreach (Sprite frame in explosionList)
        {
            explosionRenderer.sprite = frame;
            index ++;

            if (index == 5)
            {
                spriteRenderer.enabled = false;
            }
            yield return frameDelay;
        }

        Destroy(gameObject);
    }
}
