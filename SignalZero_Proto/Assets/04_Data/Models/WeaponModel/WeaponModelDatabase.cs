using UnityEngine;
using System;

[Serializable]
public class WeaponModelEntry
{
    public int modelID;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "WeaponModelDB", menuName = "Weapons/WeaponModelDatabase")]
public class WeaponModelDatabase : ScriptableObject
{
    public WeaponModelEntry[] entries;

    public GameObject Get(int id)
    {
        foreach (var e in entries)
            if (e.modelID == id)
                return e.prefab;

        Debug.LogWarning($"모델 ID {id} 를 찾을 수 없습니다.");
        return null;
    }
}

