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

	// Start is called before the first frame updatef
	void Start()
	{
		movePosition = new Vector2(-458f, originalPosition.y);

		sound.onClick.AddListener(ClickSound);
		graphics.onClick.AddListener(ClickGraphics);
		control.onClick.AddListener(ClickControl);
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
	}

	void ClosePanel()
	{
		optionRect.DOKill();
		optionRect.DOAnchorPos(originalPosition, moveDuration);
		isOpen = false;
		currentOption = OptionType.None;
	}

	void ClickSound()
	{
		HandleOptionClick(OptionType.Sound);
	}
	void ClickGraphics()
	{
		HandleOptionClick(OptionType.Graphics);
	}
	void ClickControl()
	{
		HandleOptionClick(OptionType.Control);
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
}
