using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health = 10;
    private SpriteRenderer spriteRenderer;
    private Coroutine takeDamageAnimationInstance;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        takeDamageAnimationInstance = StartCoroutine(takeDamageAnimation());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
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
}
