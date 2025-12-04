using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

	public GameObject tutorialPanel;
	public Button lineWindow;
	public TextMeshProUGUI lineText;
	// Start is called before the first frame update
	public int lineCount = 0;

	bool isSaw;

	string line;

	private void Start()
	{
		
		
	}

	public void Init()
	{
		Time.timeScale = 0f;
		tutorialPanel.SetActive(true);
		lineWindow.onClick.AddListener(ClickWindow);
		lineText.text = "본격적인 의뢰를 내주기 전에 테스트를 통과해라.";
	}

	void ClickWindow()
	{
		if (lineCount >= 0 && lineCount <= 5)
		{
			ChangeLine();
			lineText.text = line;
			lineCount++;
		}
		else
		{
			ChangeLine();
			tutorialPanel.SetActive(false);
			Time.timeScale = 1;
		}
	}

	void ChangeLine()
	{
		switch (lineCount)
		{
			case 0:
				line = "지금 네 녀석이 있는 영역을 주름잡는 해적선 정도면 충분하겠지.";
				break;
			case 1:
				line = "호위기의 조작은 기억하나? 좌측은 공격, 우측은 버스트 기능이다.";
				break;
			case 2:
				line = "버스트를 유지하면 부스터가 작동된다. 게이지를 사용하니 주의하도록.";
				break;
			case 3:
				line = "주변을 탐색하다 보면 다르게 생긴 녀석이 있을 거다.";
				break;
			case 4:
				line = "그 놈을 세 대 정도 파괴하면 목표가 직접 행차할테니 거기까진 알아서 해라.";
				break;
			case 5:
				line = "행운을 빌지.";
				break;
			default:
				line = "";
				isSaw = true;
				break;
		}
	}

	void StartTutorial()
	{
		line = "본격적인 의뢰를 내주기 전에 테스트를 통과해라.";
	}



}
