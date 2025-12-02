using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option : MonoBehaviour
{
    public RectTransform OptionObject;
    
    public Button sound;
    public Button graphics;
    public Button control;

    public Vector3 originalPosition = Vector3.zero;
    public Vector3 movePosition;

    [Range(0,10f)]public float moveDuration = 0.5f;

    bool isClicked = false;

    // Start is called before the first frame updatef
    void Start()
    {
        sound.onClick.AddListener(clickSound);
        graphics.onClick.AddListener(clickGraphics);
        control.onClick.AddListener(clickControl);
    }

    // Update is called once per frame
    void Update()
    {
	}

    public void animation()
    {
        Vector3 moveVector = new Vector3 (-458f, originalPosition.y, originalPosition.z);
        movePosition = moveVector;
        if (isClicked == true)
        {
			OptionObject.DOAnchorPos(movePosition, moveDuration);
		}
        else if( isClicked == false)
        {
			OptionObject.DOAnchorPos(originalPosition, moveDuration);
        }
    }

    void clickSound()
    {
		animation();
		isClicked = ! isClicked;
	}
    void clickGraphics()
    {
		animation();
		isClicked = !isClicked;
	}
    void clickControl()
    {
		animation();
		isClicked = !isClicked;
	}
     
}
