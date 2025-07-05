using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTurget;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && currentTurget == null)
        {
            currentTurget = other.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentTurget)
            currentTurget = null;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(currentTurget != null)
            this.transform.LookAt(currentTurget.transform.position);
    }
}
