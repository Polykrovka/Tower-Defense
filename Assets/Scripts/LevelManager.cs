using UnityEngine;
using UnityEngine.Pool;
public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;
    static int totalEnemies = 0;
    static int numberOfWaves = 4;
    static int wavesEmitted = 0;
    static bool levelOver = false;
    static bool nextWave = false;

    int timeBetweenWaves = 5;


    public ParticleSystem deathParticalePrefab;
    public static IObjectPool<ParticleSystem> deathParticalePool;
    void Start()
    {

        Time.timeScale =  20; 
        GameObject[] spawnP = GameObject.FindGameObjectsWithTag("Spawn");
        spawnPoints = new Spawn[spawnP.Length];
        for(int i = 0; i < spawnP.Length; i++)
        {
            spawnPoints[i] = spawnP[i].GetComponent<Spawn>();
            totalEnemies += spawnPoints[i].maxCountEnemys;
        }

        deathParticalePool = new ObjectPool<ParticleSystem>(
            CreateDeathParticle,
            OnTakeFromPool,
            OnReturnedToPool,
            null,
            true, 10, 30
        );

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
            wavesEmitted++;
            nextWave = true;

            if(wavesEmitted >= numberOfWaves)
            {
                levelOver = true;
                nextWave = false;
            }
        }
    }

    void ResetSpawners()
    {
        foreach(Spawn sp in spawnPoints)
        {
            totalEnemies += sp.maxCountEnemys;
            sp.Restart();
        }
    }

    void Update()
    {
        if(nextWave)
        {
            nextWave = false;
            foreach(Spawn sp in spawnPoints)
            {
                totalEnemies += sp.maxCountEnemys;
                sp.Restart();
            }
            Invoke("ResetSpawners", timeBetweenWaves);

        }
    }
}
