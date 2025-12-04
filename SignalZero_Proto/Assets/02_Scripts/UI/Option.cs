using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
enum OptionType
{
	None,
	Sound,
	Graphics,
	Control
}
public class Option : MonoBehaviour
{
	public RectTransform optionRect;

	public Button sound;
	public Button graphics;
	public Button control;

	public Vector3 originalPosition = Vector3.zero;
	public Vector3 movePosition;

	[Range(0, 10f)] public float moveDuration = 0.5f;

	private OptionType currentOption = OptionType.None;
	public bool isOpen = false;


	public GameObject soundPanel;
	public GameObject graphicsPanel;
	public GameObject controlPanel;

	public GameObject basePanel;

	// Start is called before the first frame updatef
	void Start()
	{
		movePosition = new Vector2(-458f, originalPosition.y);

		sound.onClick.AddListener(ClickSound);
		graphics.onClick.AddListener(ClickGraphics);
		control.onClick.AddListener(ClickControl);

		soundPanel.SetActive(false);
		graphicsPanel.SetActive(false);
		controlPanel.SetActive(false);
		basePanel.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OpenPanel()
	{
		optionRect.DOKill();
		optionRect.DOAnchorPos(movePosition, moveDuration);
		isOpen = true;
		basePanel.SetActive(true);
	}

	void ClosePanel()
	{
		optionRect.DOKill();
		optionRect.DOAnchorPos(originalPosition, moveDuration);
		isOpen = false;
		currentOption = OptionType.None;
		basePanel.SetActive(false);
	}

	void ClickSound()
	{
		HandleOptionClick(OptionType.Sound);
		HandleOptionPanel();
	}
	void ClickGraphics()
	{
		HandleOptionClick(OptionType.Graphics);
		HandleOptionPanel();

	}
	void ClickControl()
	{
		HandleOptionClick(OptionType.Control);
		HandleOptionPanel();
	}

	void HandleOptionClick(OptionType clicked)
	{
		if (!isOpen)
		{
			currentOption = clicked;
			OpenPanel();
			return;
		}

		if (currentOption == clicked)
		{
			ClosePanel();
		}
		else
		{
			currentOption = clicked;
		}
	}

	void HandleOptionPanel()
	{
		switch (currentOption)
		{
			case OptionType.None:
				break;
				case OptionType.Sound:
				if(isOpen == true)
				{
					soundPanel.SetActive(true);
				}
				else if(isOpen == false)
				{
					soundPanel.SetActive(false);
				}
					break;
				case OptionType.Graphics:
				if (isOpen == true)
				{
					graphicsPanel.SetActive(true);
				}
				else if (isOpen == false)
				{
					graphicsPanel.SetActive(false);
				}
				break;
				case OptionType.Control:
				if (isOpen == true)
				{
					controlPanel.SetActive(true);
				}
				else if (isOpen == false)
				{
					controlPanel.SetActive(false);
				}
				break;
			default:
				break;
		}
	}
}
