using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public GameObject buttonObject;
    public GameObject optionObject;

    public Button gameStart;
    public Button options;
    public Button exit;
    
    bool optionPressed = false;

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
        
        

    }

    public void Exit()
    {
        Application.Quit();
    }

}
