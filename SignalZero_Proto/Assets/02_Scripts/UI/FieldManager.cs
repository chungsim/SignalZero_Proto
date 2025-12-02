using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public List<GameObject> fields;

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
        for(int i = 0;  i < fields.Count; i++)
		{
			float x = (i % 5) * 200f;
            float z = (i / 5) * 200f;
            fields[i].transform.position = new Vector3(x, y, z);
		}
        
    }

    void ShuffleList(List<GameObject> list)
    {
        
        for(int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i,list.Count);
			GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
		}
    }

}
