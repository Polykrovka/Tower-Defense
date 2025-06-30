using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject skeletonPrefab;
    public Transform homeLocation;

    void Start()
    {
        InvokeRepeating("Spawner", 1, 0.5f);
    }

    void Spawner()
    {
        GameObject skeleton = Instantiate(skeletonPrefab, transform.position, Quaternion.identity);
        skeleton.GetComponent<FindHome>().destination = homeLocation;
    }

}
