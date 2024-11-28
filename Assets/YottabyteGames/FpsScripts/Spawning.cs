using System.Collections;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    [SerializeField] private GameObject enemySpawner;

    [SerializeField] private int maxEnemies = 5;

    [SerializeField] private float enemyInterval = 10f;

    private void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyAI));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

}