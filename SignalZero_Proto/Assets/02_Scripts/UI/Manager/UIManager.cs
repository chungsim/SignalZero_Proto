using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
