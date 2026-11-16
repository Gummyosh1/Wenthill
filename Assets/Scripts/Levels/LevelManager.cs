using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [NonSerialized] public int currentLevel = 1;
    public Transform rightSpawnTransform;
    public Transform leftSpawnTransform;
    public Transform enemyHolder;

    public GameObject waspPrefab;
    public GameObject ghostPrefab;

    private float spawnRate = 2f;

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
            if (currentLevel > 1)
            {
                yield return new WaitForSeconds(3f);
            }
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
                yield return new WaitForSeconds(5);
            }
            while (enemyHolder.childCount > 0)
            {
                yield return new WaitForSeconds(0.12f);
            }
            nextLevel(); 
        }
    }

    public void nextLevel()
    {
        currentLevel++;
    }
}
