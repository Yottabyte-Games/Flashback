using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    [SerializeField] private GameObject enemyAI2;
    [SerializeField] private GameObject boss = null;

    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject enemySpawner2;

    [SerializeField] private float enemyInterval = 10f;
    [SerializeField] private float officeWorkerInterval = 15f;
    [SerializeField] private int spawnerLifeValue = 5;

    private void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyAI, enemySpawner, spawnerLifeValue));
        //StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, spawnerLifeValue+1));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy, GameObject enemySpawner, int spawnerLifeValue)
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