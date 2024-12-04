using System.Collections;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] GameObject enemyAI;
    [SerializeField] GameObject enemyAI2;
    [SerializeField] GameObject boss = null;

    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject enemySpawner2;

    [SerializeField] float enemyInterval = 10f;
    [SerializeField] float officeWorkerInterval = 15f;
    [SerializeField] int spawnerLifeValue = 5;

    void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyAI, enemySpawner, spawnerLifeValue));
        //StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, spawnerLifeValue+1));
    }

    IEnumerator spawnEnemy(float interval, GameObject enemy, GameObject enemySpawner, int spawnerLifeValue)
    {
        if (spawnerLifeValue == 0)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
            spawnerLifeValue--;
            StartCoroutine(spawnEnemy(interval, enemy, enemySpawner,spawnerLifeValue));
        }
    }
}