using FMOD;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    [SerializeField] private GameObject enemyAI2;
    [SerializeField] private GameObject enemyAI3;
    [SerializeField] private GameObject boss = null;

    [SerializeField] private GameObject enemyMainSpawner;
    [SerializeField] private GameObject enemyMainSpawner2;
    [SerializeField] private GameObject enemyMainSpawner3;

    public int nextSpawnerLocation = 0;

    [SerializeField] private float enemyInterval = 10f;
    [SerializeField] private float officeWorkerInterval = 15f;
    [SerializeField] private int spawnerLifeValue = 5;

    private void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyAI, enemyMainSpawner, enemyMainSpawner2, spawnerLifeValue));
        //StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, enemySpawner2, spawnerLifeValue+1));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy, GameObject enemySpawner, GameObject enemySpawner2, int spawnerLifeValue)
    {
        if (spawnerLifeValue == 0)
        {
            enemySpawner.SetActive(false);
            enemySpawner2.SetActive(true);
            spawnerLifeValue = 5;
            nextSpawnerLocation++;
            StartNextCoroutine();
            yield break;
        }
        else
        {
            /*yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
            spawnerLifeValue--;
            StartCoroutine(spawnEnemy(interval, enemy, enemySpawner,spawnerLifeValue));*/

            do
            {
                yield return new WaitForSeconds(interval);
                GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
                spawnerLifeValue--;
            } while (spawnerLifeValue > 0);

            StartCoroutine(spawnEnemy(interval, enemy, enemySpawner, enemySpawner2, spawnerLifeValue));
        }
    }

    private void StartNextCoroutine()
    {
        if (nextSpawnerLocation == 1) 
        {
            StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, enemyMainSpawner2, enemyMainSpawner3, spawnerLifeValue));
        }
        if (nextSpawnerLocation == 2)
        {
            StartCoroutine(spawnEnemy(enemyInterval, enemyAI3, enemyMainSpawner3, null, spawnerLifeValue));
        }
    }
}