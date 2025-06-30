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
        InvokeRepeating("Spawner", startSpawnDelay, spawnRate);
    }

    void Spawner()
    {
        currentEnemyCount++;
        if(currentEnemyCount >= maxCountEnemys)
        {
            CancelInvoke("Spawner");
        }
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<FindHome>().destination = homeLocation;
    }

}
