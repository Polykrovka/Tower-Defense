using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTurget;
    FindHome currentTargetScript;
    public GameObject core;
    public GameObject gun;
    Quaternion coreStartRotation;
    Quaternion gunStartRotation;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && currentTurget == null)
        {
            currentTurget = other.gameObject;
            currentTargetScript = currentTurget.GetComponent<FindHome>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentTurget)
            currentTurget = null;
    }

    void Start()
    {
        coreStartRotation = core.transform.rotation;
        gunStartRotation = gun.transform.rotation;
    }

    void ShootTarget()
    {
        if(currentTurget)
        {
            currentTargetScript.GetComponent<FindHome>().Hit(1);
        }
    }

    void Update()
    {
        if(currentTurget != null)
        {
            Vector3 aimAt = new Vector3(currentTurget.transform.position.x, core.transform.position.y, currentTurget.transform.position.z);

            //gun.transform.LookAt(currentTurget. transform.position);

            float distanceToTarget = Vector3.Distance(aimAt, gun.transform.position);
            Vector3 relativeTargetPosition = gun.transform.position + (gun.transform.forward * distanceToTarget);

            relativeTargetPosition = new Vector3(relativeTargetPosition.x, currentTurget.transform.position.y, relativeTargetPosition.z);

            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, Quaternion.LookRotation(relativeTargetPosition - gun.transform.position), Time.deltaTime);

            //core.transform.LookAt(aimAt);

            core.transform.rotation = Quaternion.Slerp(core.transform.rotation, Quaternion.LookRotation(aimAt - core.transform.position), Time.deltaTime);


            ShootTarget();

        }
        else
        {
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation, coreStartRotation, Time.deltaTime );
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunStartRotation, Time.deltaTime );
        }
    }
}
