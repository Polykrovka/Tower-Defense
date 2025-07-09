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

    public void Hit(float power)
    {
        if(healthBar)
        {
            healthBar.value -= power;
            if(healthBar.value <= 0 && ai.hasPath)
            {
                LevelManager.RemoveEnemy();//Just decrease enemis count
                ai.ResetPath();

                Destroy(healthBar.gameObject);
                Destroy(this.gameObject, 0.1f);
            }
        }
    }


    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            LevelManager.RemoveEnemy();//Just decrease enemis count
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
