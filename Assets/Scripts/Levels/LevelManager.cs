using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [NonSerialized] public int currentLevel = 1;
    public Transform rightSpawnTransform;
    public Transform leftSpawnTransform;
    public Transform enemyHolder;

    public GameObject waspPrefab;
    public GameObject ghostPrefab;

    public Sprite[] levelSprites;
    public Image levelHolder;
    public Sprite gameOverSprite;

    private float spawnRate = 0.1f;

    public Coroutine gameplayCoroutine = null;

    Dictionary<int, int> enemySpawnCounts = new Dictionary<int, int>
    {
        {1, 1},
        {2, 2},
        {3, 6},
        {4, 10},
        {5, 10}
    };

    private bool rightSpawn = true;

    public void StartGame()
    {
        if (gameplayCoroutine == null)
        {
            gameplayCoroutine = StartCoroutine(PlayGame());
        }
    }


    private IEnumerator PlayGame()
    {
        while (true)
        {
            levelHolder.sprite = levelSprites[currentLevel - 1];
            levelHolder.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            levelHolder.gameObject.SetActive(false);

            while (enemySpawnCounts[currentLevel] > 0)
            {
                if (rightSpawn)
                {
                    Instantiate(waspPrefab, rightSpawnTransform.position, Quaternion.identity, enemyHolder);
                }
                else
                {
                    Instantiate(waspPrefab, leftSpawnTransform.position, Quaternion.identity, enemyHolder);
                }
                enemySpawnCounts[currentLevel]--;
                rightSpawn = !rightSpawn;
                yield return new WaitForSeconds(spawnRate);
            }
            while (enemyHolder.childCount > 0)
            {
                yield return new WaitForSeconds(0.12f);
            }
            nextLevel(); 
        }
    }

    public void endGame()
    {
        levelHolder.sprite = gameOverSprite;
        levelHolder.gameObject.SetActive(true);
        int totalEnemies = enemyHolder.childCount;
        StopCoroutine(gameplayCoroutine);

        for (int i = 0; i < totalEnemies; i++)
        {
            Destroy(enemyHolder.GetChild(i).gameObject);
        }

    }

    public void nextLevel()
    {
        currentLevel++;
    }
}
