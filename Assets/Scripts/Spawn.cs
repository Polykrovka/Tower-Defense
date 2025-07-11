using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject enemyPrefab;
    public Transform homeLocation;

    public float startSpawnDelay = 1f;
    public float spawnRate = 0.5f;
    public int maxCountEnemys = 10;
    private int currentEnemyCount = 0; 

    void Start()
    {
        Restart();
    }

    public void Restart()
    {
        Debug.Log("Restart");
        currentEnemyCount = 0;
        InvokeRepeating("Spawner", startSpawnDelay, spawnRate);
    }

    void Spawner()
    {
        Debug.Log("Spawner " + currentEnemyCount);
        currentEnemyCount++;
        if(currentEnemyCount >= maxCountEnemys)
        {
            CancelInvoke("Spawner");
        }
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<FindHome>().destination = homeLocation;
    }

}
