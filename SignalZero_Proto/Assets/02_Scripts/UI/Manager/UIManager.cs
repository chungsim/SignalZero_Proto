using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameStart gameStart;
    public Option option;
    public CharacterUI characterUI;

    private static UIManager instance;
    public static UIManager Instance {  get { return instance; } }
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
			instance = this;
		}
        gameStart = GetComponentInChildren<GameStart>();
        option = GetComponentInChildren<Option>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
