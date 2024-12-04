using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    [SerializeField] private GameObject enemyAI2;

    [SerializeField] private GameObject boss = null;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject enemySpawner2;

    [SerializeField] private int maxEnemies = 5;

    [SerializeField] private float enemyInterval = 10f;

    [SerializeField] private int spawnerLifeValue = 5;

    private async void Start()
    {
        await SpawnEnemyAsync(enemyInterval, enemySpawner, enemySpawner2, enemyAI, null, spawnerLifeValue);
        await SpawnEnemyAsync(enemyInterval, enemySpawner2, null, enemyAI2, boss, spawnerLifeValue + 1);
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
            GameObject newEnemy;
            if (spawnerLifeValue == 1)
            {
                newEnemy = Instantiate(boss, enemySpawner.transform.position, Quaternion.identity);
                spawnerLifeValue--;
                StartCoroutine(spawnEnemy(interval, enemySpawner, null, enemy, boss, spawnerLifeValue));
            }
            else
            {
                newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
                spawnerLifeValue--;
                StartCoroutine(spawnEnemy(interval, enemySpawner, null, enemy, boss, spawnerLifeValue));
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

    private Task SpawnEnemyAsync(float interval, GameObject enemySpawner, GameObject nextSpawner, GameObject enemy, GameObject boss, int spawnerLifeValue)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();
        StartCoroutine(SpawnEnemyCoroutine(interval, enemySpawner, nextSpawner, enemy, boss, spawnerLifeValue, taskCompletionSource));
        return taskCompletionSource.Task;
    }

    private IEnumerator SpawnEnemyCoroutine(float interval, GameObject enemySpawner, GameObject nextSpawner, GameObject enemy, GameObject boss, int spawnerLifeValue, TaskCompletionSource<bool> taskCompletionSource)
    {
        // Run the original coroutine
        yield return spawnEnemy(interval, enemySpawner, nextSpawner, enemy, boss, spawnerLifeValue);

        // Signal that the coroutine has completed
        taskCompletionSource.SetResult(true);
    }
}