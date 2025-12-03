using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    public float cooldown;
    public int numPerShot;
    public float spreadAngle;

    public string damageType;
    public int projectileTypeID;

    public int modelID;

    public BulletSO bulletData;

    public AudioData audioData;
}
