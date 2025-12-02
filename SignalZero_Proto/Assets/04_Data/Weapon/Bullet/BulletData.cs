using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "Weapons/Bullet")]
public class BulletSO : ScriptableObject
{
    public float range;
    public float bulletSpeed;
    public float bulletSize;

    public float damagePerShot;

    public string damageType;
    public string modelID;
}

