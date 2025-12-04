using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
    public GameObject buttonObject;
    public Option optionObject;

    public Button gameStart;
    public Button options;
    public Button exit;
    
    public RectTransform optionRect;

    public bool optionPressed = false;

    void Start()
    {
        gameStart.onClick.AddListener(StartGame);
        options.onClick.AddListener(Option);
        exit.onClick.AddListener(Exit);

        optionObject.gameObject.SetActive(false);
        optionRect = optionObject.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {
        buttonObject.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

    public void Option()
    {
		optionRect.DOKill();
		optionObject.isOpen = false;
	    optionRect.DOAnchorPos(Vector3.zero, 0f);
		
		optionObject.gameObject.SetActive(!optionPressed);
		gameStart.interactable = optionPressed;
		exit.interactable = optionPressed;
		optionPressed = !optionPressed;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
