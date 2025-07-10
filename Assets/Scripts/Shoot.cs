using System.Collections.Generic;
using UnityEngine;
using static FindHome;

public class Shoot:MonoBehaviour
{
    GameObject currentTurget;
    FindHome currentTargetScript;
    public GameObject core;
    public GameObject gun;
    public TurretDetails turretDetails;
    public AudioSource shootSound;
    public List<ParticleSystem> shootParticles;
    public int particleCount = 15;
    Quaternion coreStartRotation;
    Quaternion gunStartRotation;

    List<GameObject> enemiesInRange = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null && other.CompareTag("Enemy") && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
            CleanupDeadEnemies();
            if(currentTurget == null)
                UpdateTarget();
        }
    }

    void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject);
        if(other.gameObject == currentTurget)
        {
            currentTurget = null;
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        CleanupDeadEnemies();

        if(enemiesInRange.Count > 0)
        {
            currentTurget = enemiesInRange[0];
            if(currentTurget != null)
                currentTargetScript = currentTurget.GetComponent<FindHome>();
            else
            {
                currentTurget = null;
                currentTargetScript = null;
            }
        }
        else
        {
            currentTurget = null;
            currentTargetScript = null;
        }
    }

    void CleanupDeadEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);
    }

    void Start()
    {
        coreStartRotation = core.transform.rotation;
        gunStartRotation = gun.transform.localRotation;
        if(shootParticles != null)
        {
            foreach(var particle in shootParticles)
                particle.Stop();
        }

        FindHome.OnEnemyDeath += OnEnemyDeathHandler;
    }

    void OnDestroy()
    {
        FindHome.OnEnemyDeath -= OnEnemyDeathHandler;
    }

    void OnEnemyDeathHandler(GameObject enemy)
    {
        if(enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);

        if(enemy == currentTurget)
        {
            currentTurget = null;
            currentTargetScript = null;
            UpdateTarget();
        }
    }

    bool shootColdown = false;

    void ShootColdown()
    {
        shootColdown = false;
    }

    void ShootTarget()
    {
        if (currentTurget && !shootColdown)
        {
            currentTargetScript.GetComponent<FindHome>().Hit(turretDetails.damage, turretDetails.turretType);
            shootSound.Play();
            if (shootParticles != null)
            {
                foreach (var particle in shootParticles)
                    particle.Play();
            }
            Invoke("stopParticales", turretDetails.reloadTime > 1 ? turretDetails.reloadTime : 1);
            shootColdown = true;
            Invoke("ShootColdown", turretDetails.reloadTime);
        }
    }

    void stopParticales()
    {
        if(shootParticles != null)
        {

            foreach(var particle in shootParticles)
                particle.Stop();
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
                Time.deltaTime * turretDetails.RotationSpeed
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
                Time.deltaTime * turretDetails.RotationSpeed
            );


            Vector3 directionToTrget = currentTurget.transform.position - gun.transform.position;
            if(Vector3.Angle(directionToTrget, gun.transform.forward) < turretDetails.angleAccuracy)  // If the target is within a certain angle, shoot
            {
                if(Random.Range(0, 100) < turretDetails.accuracy)
                    ShootTarget();
            }

        }
        else
        {
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation, coreStartRotation, Time.deltaTime * (turretDetails.RotationSpeed / 2));
            gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation, gunStartRotation, Time.deltaTime * (turretDetails.RotationSpeed / 2));
        }
    }
}
