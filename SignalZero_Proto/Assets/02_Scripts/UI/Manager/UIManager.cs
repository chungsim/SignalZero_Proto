using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public CharacterUI characterUI;
	public Tutorial tutorial;

	public Image bossHPBackground;
	public Image bossHPBar;
	public GameObject bossHPObject;


	private static UIManager instance;
	public static UIManager Instance { get { return instance; } }
	// Start is called before the first frame update
	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		bossHPObject.SetActive(false);

	}

	public void Init()
	{
		tutorial = FindObjectOfType<Tutorial>(true);
		characterUI = FindObjectOfType<CharacterUI>(true);
		characterUI.Init();
		tutorial.Init();
	
	}

}
