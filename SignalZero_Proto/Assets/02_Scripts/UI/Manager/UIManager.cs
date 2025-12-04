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

	void Start()
	{
        bossHPObject.SetActive(false);
    }

	public void Init()
	{
        if (tutorial != null)
            tutorial.Init();

        if (characterUI != null)
            characterUI.Init();
    }

}
