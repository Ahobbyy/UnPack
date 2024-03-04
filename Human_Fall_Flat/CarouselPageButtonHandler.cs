using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarouselPageButtonHandler : MonoBehaviour
{
	public Button leftButton;

	public TextMeshProUGUI leftLabel;

	public Button rightButton;

	public TextMeshProUGUI rightLabel;

	private void Start()
	{
		leftLabel.text = "Q/ℂ";
		rightLabel.text = "℅/E";
	}

	private bool Left()
	{
		return false;
	}

	private bool Right()
	{
		return false;
	}

	private void Update()
	{
		if (Left())
		{
			((UnityEvent)leftButton.get_onClick()).Invoke();
		}
		if (Right())
		{
			((UnityEvent)rightButton.get_onClick()).Invoke();
		}
	}

	public CarouselPageButtonHandler()
		: this()
	{
	}
}
