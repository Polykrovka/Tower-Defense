using UnityEngine;
using UnityEngine.Pool;
public class LevelManager : MonoBehaviour
{
    GameObject[] spawnPoints;
    static int totalEnemies = 0;
    public ParticleSystem deathParticalePrefab;
    public static IObjectPool<ParticleSystem> deathParticalePool;
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        foreach(GameObject sp in spawnPoints)
        {
            totalEnemies += sp.GetComponent<Spawn>().maxCountEnemys;
        }

        deathParticalePool = new ObjectPool<ParticleSystem>(
            CreateDeathParticle,
            OnTakeFromPool,
            OnReturnedToPool,
            null,
            true, 10, 30
        );

        Debug.Log("Total enemies to defeat: " + totalEnemies);
    }

    ParticleSystem CreateDeathParticle()
    {
        ParticleSystem particle = Instantiate(deathParticalePrefab);
        particle.Stop();
        
        return particle;
    }

    void OnReturnedToPool(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
    }

    void OnTakeFromPool(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);

    }

    public static void DisplayDeathExplosion(Vector3 position)
    {
        ParticleSystem particle = deathParticalePool.Get();
        if(particle)
        {
            particle.transform.position = position;
            particle.Play();
        }
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
