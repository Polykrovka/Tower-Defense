using UnityEngine;
using UnityEngine.Pool;
public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;
    public static int totalEnemies = 0;
    public static int numberOfWaves = 4;
    public static int wavesEmitted = 0;
    public static int totalMoney = 500;
    public static int totalLives = 10;
    public static bool levelOver = false;
    public static bool nextWave = false;
    public GameObject gameOverPanel;

    int timeBetweenWaves = 5;


    public ParticleSystem deathParticalePrefab;
    public static IObjectPool<ParticleSystem> deathParticalePool;
    void Start()
    {

        Time.timeScale =  1; 
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

    public static void setGameSpeed(int speed) 
    { 
        Time.timeScale = speed;
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

    public static void RemoveLive()
    {
        totalLives--;
        if(totalLives <= 0)
        {
            levelOver = true;
            nextWave = false;
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
            Invoke("ResetSpawners", timeBetweenWaves);
        }

        if(levelOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
