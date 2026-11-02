using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [NonSerialized] public int currentHealth;
    [NonSerialized] public int maxHealth = 7;
    [NonSerialized] public bool isAlive = true;
    [NonSerialized] public bool isInvincible;
    [NonSerialized] public float invincibilityDuration;
    [NonSerialized] public float invincibilityTimer;

    public Image[] hearts;
    public Sprite[] heartSprites;
    public Sprite emptyHeartSprite;
    private Coroutine[] heartAnimationRoutines = new Coroutine[10];

    private float heartAnimationRate = .75f;

    public void Start()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = heartSprites[0];
            heartAnimationRoutines[i] = StartCoroutine(heartAnimation(hearts[i]));
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        Debug.Log("Current health is " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }

        for (int i = currentHealth; i < maxHealth; i++)
        {
            if (hearts[i].sprite != emptyHeartSprite)
            {
                hearts[i].sprite = emptyHeartSprite;
            }

            if (heartAnimationRoutines[i] != null)
            {
                StopCoroutine(heartAnimationRoutines[i]);
            }
            heartAnimationRoutines[i] = null;
        }
    }

    public IEnumerator heartAnimation(Image heart)
    {
        float heartTimer = 0;
        int index = 1;
        while (true)
        {
            heartTimer += Time.deltaTime;
            if (heartTimer >= heartAnimationRate)
            {
                heart.sprite = heartSprites[index];
                index++;
                if (index >= heartSprites.Length)
                {
                    index = 0;
                }
                heartTimer = 0;
            }
            yield return null;
        }
        
    }
}
