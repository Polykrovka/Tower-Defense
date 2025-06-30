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
        if(ai.remainingDistance < 0.1f && ai.hasPath)
        {
            Destroy(this.gameObject, 0.1f);
        }
    }

}
