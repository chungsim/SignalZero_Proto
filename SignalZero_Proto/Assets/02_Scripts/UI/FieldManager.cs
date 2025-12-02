using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public List<GameObject> fields;
    public event Action spawn;

    // Start is called before the first frame update
    void Start()
    {
        ShuffleList(fields);
        fieldReorder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fieldReorder()
    {  
		float y = -1f;
        for(int i = 0;  i < 200; i++)
		{
            int q = UnityEngine.Random.Range(0, fields.Count);
            GameObject spawnField = Instantiate(fields[q],this.transform);
			float x = (i % 10) * 200f;
            float z = (i / 10) * 200f;
            spawnField.transform.position = new Vector3(x, y, z);
           


		}
    }

    void ShuffleList(List<GameObject> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i,list.Count);
			GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
		}
    }

}
