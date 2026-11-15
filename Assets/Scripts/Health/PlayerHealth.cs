using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PlayerMove playerMove;
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
    private Vector2 attackerPosition;

    private float heartAnimationRate = .75f;

    [Min(0f)] private float postKnockbackIFrames = 0.25f;
    private float hitInvulnerableUntil;

    //LOW HEALTH
    [SerializeField] private Image lowHealthVignette;
    [SerializeField] private CameraFollow cameraFollow;
    private int lowHealthThreshold = 2;
    private bool wasLowHealth;


    public void Start()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = heartSprites[0];
            heartAnimationRoutines[i] = StartCoroutine(heartAnimation(hearts[i]));
        }
        playerMove = GetComponent<PlayerMove>();
    }

    private void LateUpdate()
    {
        bool isLowHealth = isAlive && currentHealth <= lowHealthThreshold;

        Color color = lowHealthVignette.color;
        float targetAlpha = isLowHealth ? 0.55f : 0f;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, 3f * Time.deltaTime);
        lowHealthVignette.color = color;

        if (isLowHealth && !wasLowHealth)
            cameraFollow.PlayLowHealthShake();

        wasLowHealth = isLowHealth;
    }

    public void TakeDamage(int damage, Vector2 attackerPosition, float knockbackSpeed, float knockbackLift, float knockbackDuration)
    {
        if (isInvincible || Time.time < hitInvulnerableUntil)
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

        // The attack supplies attackerPosition. Pass this direction to PlayerMove.
        Vector2 hitDirection = (Vector2)transform.position - attackerPosition;
        hitInvulnerableUntil = Time.time + knockbackDuration + postKnockbackIFrames;
        playerMove.ApplyKnockback(hitDirection, knockbackDuration, knockbackSpeed, knockbackLift);
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
