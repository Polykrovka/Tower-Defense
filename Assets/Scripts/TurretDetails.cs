using UnityEngine;

[CreateAssetMenu(fileName = "TurretDetails", menuName = "Scriptable Objects/TurretDetails", order = 2)]
public class TurretDetails : ScriptableObject
{
    public enum TurretType { RocketLauncher, Gatling, Flamer, Trash }
    public TurretType turretType;
    public float damage;
    public float accuracy;
    public float RotationSpeed;
    public float reloadTime;
    public float angleAccuracy;
    public float aoeRadius = 3f;
    public float moneyCost;
    public float upgradeMoneyCost = 20;
}
