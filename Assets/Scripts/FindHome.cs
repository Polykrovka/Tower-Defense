using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FindHome : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent ai;
    public EnemyDetails enemyDetails;
    float currentHealth;
    public Slider healthBarPrefab;
    Slider healthBar;
    public ParticleSystem rocketHitEffectPrefab;

    public delegate void EnemyDeathHandler(GameObject enemy);
    public static event EnemyDeathHandler OnEnemyDeath;


    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.SetDestination(destination.position);
        ai.speed = enemyDetails.speed;
        currentHealth = enemyDetails.maxHealth;
        healthBar = Instantiate(healthBarPrefab, this.transform.position, Quaternion.identity);
        healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
        healthBar.maxValue = enemyDetails.maxHealth;
        healthBar.value = enemyDetails.maxHealth;
    }

    public void Hit(TurretDetails turretDetails)
    {
        float power = turretDetails.damage;
        float aoeRadius = turretDetails.aoeRadius;

        if(turretDetails.turretType == TurretDetails.TurretType.RocketLauncher)
        {
            ParticleSystem ps = Object.Instantiate(rocketHitEffectPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            ps.Play();
        }

        if(aoeRadius > 0f)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
            foreach(var hit in hitColliders)
            {
                if(hit.CompareTag("Enemy"))
                {
                    FindHome enemy = hit.GetComponent<FindHome>();
                    if(enemy != null)
                        enemy.ReduceHealth(power);
                }
            }
        }
        else
        {
            ReduceHealth(power);
        }
    }

    public void ReduceHealth(float power)
    {
        if(healthBar)
        {
            healthBar.value -= power;
            if(healthBar.value <= 0 && ai.hasPath)
            {
                LevelManager.DisplayDeathExplosion(this.transform.position + new Vector3(0, 1, 0));
                LevelManager.totalMoney += enemyDetails.moneyReward;
                LevelManager.RemoveEnemy();//Just decrease enemis count
                ai.ResetPath();

                Destroy(healthBar.gameObject);
                Destroy(this.gameObject, 0.1f);
                OnEnemyDeath?.Invoke(this.gameObject); // Notify subscribers about enemy death
            }
        }
    }

    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            Debug.Log("RemoveEnemy");
            LevelManager.RemoveEnemy();
            LevelManager.RemoveLive();
            ai.ResetPath();

            Destroy(healthBar.gameObject);
            Destroy(this.gameObject, 0.1f);
        }
        if(healthBar)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.7f);
        }
    }

}
