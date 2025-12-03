using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum FieldType
{
	radioShip,
    common
}
public class Field : MonoBehaviour
{
    public FieldType type;

	public bool isSpawned;
	private void Awake()
	{
		GetRandomType();
	}


	void GetRandomType()
	{
		int r = Random.Range(0,100);

		if (r < 70)
		{
			type = FieldType.common;
		}
		else
		{
			type = FieldType.radioShip;
		}
	}



}
