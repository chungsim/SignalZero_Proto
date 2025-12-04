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
	[SerializeField] private RectTransform optionRect;

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

	void OpenPanel()
	{
		optionRect.DOKill();
		optionRect.DOAnchorPos(movePosition, moveDuration);
		isOpen = true;
		UpdatePanels();
	}

	void ClosePanel()
	{
		optionRect.DOKill();
		optionRect.DOAnchorPos(originalPosition, moveDuration);
		isOpen = false;
		currentOption = OptionType.None;
		UpdatePanels();
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
			UpdatePanels();
		}
	}

	void UpdatePanels()
	{
		soundPanel.SetActive(isOpen && currentOption == OptionType.Sound);
		graphicsPanel.SetActive(isOpen && currentOption == OptionType.Graphics);
		controlPanel.SetActive(isOpen && currentOption == OptionType.Control);

		basePanel.SetActive(isOpen);
	}


}
