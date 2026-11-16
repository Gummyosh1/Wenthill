using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    [NonSerialized] public int towerHealth;
    private int maxTowerHealth = 7;

    public SpriteRenderer towerMask;
    private Coroutine damageCoroutine;
    private Color originalColor;
    public Image[] hearts;

    public Sprite[] heartAnimation;

    public Sprite deadHeart;
    private float animationRate = 1f;

    private Coroutine[] heartAnimationRoutines = new Coroutine[10];



    public void Awake()
    {
        towerHealth = maxTowerHealth;
        renderHealth();
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = heartAnimation[0];
            heartAnimationRoutines[i] = StartCoroutine(heartIdle(hearts[i]));
        }
        Transform mask = transform.Find("Mask");

        if (mask != null)
            towerMask = mask.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        towerHealth -= damage;


        if (damage < 0)
        {
            return;
        }

        if (damageCoroutine == null)
        {
            originalColor = towerMask.color;
        }
        else
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        damageCoroutine = StartCoroutine(flashRed());
        renderHealth();
    }

    private IEnumerator flashRed()
    {
        towerMask.color = new Color(1f, 0f, 0f, originalColor.a);
        yield return new WaitForSeconds(0.12f);
        towerMask.color = originalColor;
        damageCoroutine = null;
    }

    private void renderHealth()
    {
        for (int i = towerHealth; i < maxTowerHealth; i++)
        {
            if (i < 0)
            {
                return;
            }
            hearts[i].sprite = deadHeart;
            if (heartAnimationRoutines[i] != null)
            {
                StopCoroutine(heartAnimationRoutines[i]);
                heartAnimationRoutines[i] = null;
            }
        }
    }

    private IEnumerator heartIdle(Image heart)
    {
        float heartTimer = 0;
        int index = 1;
        while (true)
        {
            heartTimer += Time.deltaTime;
            if (heartTimer >= animationRate)
            {
                heart.sprite = heartAnimation[index];
                index++;
                if (index >= heartAnimation.Length)
                {
                    index = 0;
                }
                heartTimer = 0;
            }
            yield return null;
        }
    }
}
