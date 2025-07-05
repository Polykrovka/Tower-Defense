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
        gunStartRotation = gun.transform.localRotation;
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

            // Horizontal rotation for core
            core.transform.rotation = Quaternion.Slerp(
                core.transform.rotation,
                Quaternion.LookRotation(aimAt - core.transform.position),
                Time.deltaTime
            );

            // Vertical rotation for gun (relative to core)
            Vector3 gunLocalTarget = core.transform.InverseTransformPoint(currentTurget.transform.position);
            float angle = -Mathf.Atan2(
                gunLocalTarget.y,
                new Vector2(gunLocalTarget.x, gunLocalTarget.z).magnitude
            ) * Mathf.Rad2Deg;
            Quaternion gunTargetRot = Quaternion.Euler(angle, 0, 0);
            gun.transform.localRotation = Quaternion.Slerp(
                gun.transform.localRotation,
                gunTargetRot,
                Time.deltaTime
            );

            ShootTarget();
        }
        else
        {
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation, coreStartRotation, Time.deltaTime);
            gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation, gunStartRotation, Time.deltaTime);
        }
    }
}
