using FMOD;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyAI;
    private GameObject enemyAI2;
    private GameObject enemyAI3;
    private GameObject enemyAI4;
    [SerializeField] private GameObject boss = null;

    [SerializeField] private GameObject enemyMainSpawner;
    [SerializeField] private GameObject enemyMainSpawner2;
    private GameObject enemyMainSpawner3;
    private GameObject enemyMainSpawner4;

    public int nextSpawnerLocation = 0;

    [SerializeField] private float enemyInterval = 10f;
    [SerializeField] private float officeWorkerInterval = 15f;
    [SerializeField] private int spawnerLifeValue = 5;
    [SerializeField] private bool bossSpawner = true;

    private void Start()
    {
        StartCoroutine(spawnEnemy(enemyInterval, enemyAI, enemyMainSpawner, enemyMainSpawner2, spawnerLifeValue, bossSpawner));
        //StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, enemySpawner2, spawnerLifeValue+1));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy, GameObject enemySpawner, GameObject enemySpawner2, int lifeValue, bool bossSpawner)
    {
        if (spawnerLifeValue == 0)
        {
            enemySpawner.SetActive(false);
            enemySpawner2.SetActive(true);
            spawnerLifeValue = lifeValue;
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
                print("relevant" + (bossSpawner));
                print("relevant" + (spawnerLifeValue == 1) );
                if (bossSpawner == true && spawnerLifeValue == 1)
                {

                    yield return new WaitForSeconds(interval);
                    GameObject newEnemy = Instantiate(boss, enemySpawner.transform.position, Quaternion.identity);
                    spawnerLifeValue--;
                    //StartCoroutine(spawnEnemy(interval, enemy, enemySpawner, enemySpawner2, spawnerLifeValue, message));
                }
                else
                {
                    yield return new WaitForSeconds(interval);
                    GameObject newEnemy = Instantiate(enemy, enemySpawner.transform.position, Quaternion.identity);
                    spawnerLifeValue--;
                    print("relevant" + spawnerLifeValue);
                }
            } while (spawnerLifeValue > 0);

            StartCoroutine(spawnEnemy(interval, enemy, enemySpawner, enemySpawner2, spawnerLifeValue, bossSpawner));
        }
    }

    private void StartNextCoroutine()
    {
        if (nextSpawnerLocation == 1) 
        {
            StartCoroutine(spawnEnemy(enemyInterval, enemyAI2, enemyMainSpawner2, enemyMainSpawner3, 6, true));
        }
        if (nextSpawnerLocation == 2)
        {
            StartCoroutine(spawnEnemy(enemyInterval, enemyAI3, enemyMainSpawner3, enemyMainSpawner4, 5, true));
        }
        if (nextSpawnerLocation == 3)
        {
            StartCoroutine(spawnEnemy(enemyInterval, enemyAI4, enemyMainSpawner4, null, 5, false));
        }
    }
}