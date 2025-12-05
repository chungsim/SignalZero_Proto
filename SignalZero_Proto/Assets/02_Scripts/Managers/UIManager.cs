using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

	[SerializeField] private GameObject clearPanel;
	[SerializeField] private GameObject overPanel;

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
		
		DeactiveEndPanel();
    }

	public void DeactiveEndPanel()
    {
        clearPanel.SetActive(false);
		overPanel.SetActive(false);
    }

	public void ActiveEndPanel(bool isClear)
    {
        if (isClear)
        {
            clearPanel.SetActive(true);
        }
        else
        {
            overPanel.SetActive(true);
        }
		GameManager.Instance.monsterSpawnManager.ClearAllMonster();
    }

	public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }


}
