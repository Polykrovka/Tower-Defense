using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameObject[] spawnPoints;
    static int totalEnemies = 0;
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        foreach(GameObject sp in spawnPoints)
        {
            totalEnemies += sp.GetComponent<Spawn>().maxCountEnemys;
        }
        Debug.Log("Total enemies to defeat: " + totalEnemies);
    }

    public static void RemoveEnemy()
    {
        totalEnemies--;
        if(totalEnemies <= 0)
        {
            Debug.Log("All enemies defeated!");
        }
    }
}
