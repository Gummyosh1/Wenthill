using System.Collections;
using UnityEngine;

public class PlayerAnimateMove : MonoBehaviour
{
    public PlayerMove playerMove;
    public Sprite[] sprites;
    public Sprite idle;
    public SpriteRenderer spriteRenderer;
    private float timer = 0.1f;
    private int index = 1;
    private float spriteTime = .1f;
    private Coroutine walkAnimationRoutine = null;


    public void Update()
    {
        if (walkAnimationRoutine == null && playerMove.isMoving)
        {
            walkAnimationRoutine = StartCoroutine(walkAnimation());
        }
        else if (walkAnimationRoutine != null && !playerMove.isMoving)
        {
            StopCoroutine(walkAnimationRoutine);
            walkAnimationRoutine = null;
            spriteRenderer.sprite = idle;
        }
    }

    public IEnumerator walkAnimation()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= spriteTime)
            {
                spriteRenderer.sprite = sprites[index];
                index++;
                if (index >= sprites.Length)
                {
                    index = 0;
                }
                timer = 0;
            }
            yield return null;
        }
        
    }
}
