using System;
using System.Collections;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [NonSerialized] public int towerHealth;

    public SpriteRenderer towerMask;
    private Coroutine damageCoroutine;
    private Color originalColor;

    public void Awake()
    {
        Transform mask = transform.Find("Mask");

        if (mask != null)
            towerMask = mask.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        towerHealth -= damage;

        if (damage <= 0 || towerMask == null)
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
    }

    private IEnumerator flashRed()
    {
        towerMask.color = new Color(1f, 0f, 0f, originalColor.a);
        yield return new WaitForSeconds(0.12f);
        towerMask.color = originalColor;
        damageCoroutine = null;
    }
}
