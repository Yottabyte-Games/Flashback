using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    [SerializeField] private GameObject enemyAI2;
    [SerializeField] private GameObject boss = null;
    //[SerializeField] private GameObject enemyAI3 = null;
    //[SerializeField] private GameObject enemyAI4 = null;


    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject enemySpawner2;

    [SerializeField] private int maxEnemies = 5;

    [SerializeField] private float enemyInterval = 10f;

    [SerializeField] private int spawnerLifeValue = 5;

    private void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemySpawner, enemySpawner2, enemyAI, null, spawnerLifeValue));
        StartCoroutine(spawnEnemy(enemyInterval, enemySpawner2, null, enemyAI2, boss, spawnerLifeValue+1));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemySpawner, GameObject nextSpawner, GameObject enemy, GameObject boss, int spawnerLifeValue)
    {
        if (spawnerLifeValue == 0)
        {
            nextSpawner.SetActive(true);
            yield break;
        }
        else if (boss != null)
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
            spawnerLifeValue--;
            if (spawnerLifeValue == 1)
            {
                StartCoroutine(spawnEnemy(interval, enemySpawner, null, enemy, boss, spawnerLifeValue));
            }
            else
            {
                StartCoroutine(spawnEnemy(interval, enemySpawner, null, enemy, null, spawnerLifeValue));
            }
        }
        else
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
            spawnerLifeValue--;
            StartCoroutine(spawnEnemy(interval, enemySpawner, nextSpawner, enemy, null, spawnerLifeValue));
        }
    }

}