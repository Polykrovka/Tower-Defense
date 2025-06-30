using UnityEngine;
using UnityEngine.AI;

public class FindHome : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent ai;
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            LevelManager.RemoveEnemy();//Just decrease enemis count
            ai.ResetPath();

            Destroy(this.gameObject, 0.1f);
        }
    }

}
