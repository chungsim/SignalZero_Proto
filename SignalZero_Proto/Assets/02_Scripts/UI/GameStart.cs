using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStart : MonoBehaviour
{
    public GameObject buttonObject;
    public GameObject optionObject;

    public Button gameStart;
    public Button options;
    public Button exit;
    
    public bool optionPressed = false;

    void Start()
    {
        gameStart.onClick.AddListener(StartGame);
        options.onClick.AddListener(Option);
        exit.onClick.AddListener(Exit);

        optionObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {
        buttonObject.SetActive(false);
    }

    public void Option()
    {
        optionObject.SetActive(!optionPressed);
		gameStart.interactable = optionPressed;
		exit.interactable = optionPressed;
		optionPressed = !optionPressed;
        if( optionPressed == false )
        {
			UIManager.Instance.option.OptionObject.DOAnchorPos(Vector3.zero, UIManager.Instance.option.moveDuration);
		}
        
    }

    public void Exit()
    {
        Application.Quit();
    }

}
