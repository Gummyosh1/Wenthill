using System.Collections;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int index = 1;
    private float spriteTime = .2f;

    public void Start()
    {
        StartCoroutine(walkAnimation());
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
