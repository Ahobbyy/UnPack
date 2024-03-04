using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : Button
{
	private enum MousePlace
	{
		None,
		Button,
		Left,
		Right
	}

	[Serializable]
	public class SelectorEvent : UnityEvent<int>
	{
	}

	public string[] options;

	public TextMeshProUGUI[] optionLabels;

	public int selectedIndex;

	public TextMeshProUGUI valueLabel;

	public Image leftArrow;

	public Image rightArrow;

	private PointerEventData cachedEventData;

	private MousePlace currentArrow;

	public SelectorEvent onValueChanged;

	private int optionsCount
	{
		get
		{
			if (options.Length == 0)
			{
				return optionLabels.Length;
			}
			return options.Length;
		}
	}

	protected override void Start()
	{
		((UIBehaviour)this).Start();
	}

	public void SelectIndex(int index)
	{
		selectedIndex = index;
		RebindValue();
	}

	public void RebindValue()
	{
		if (selectedIndex >= 0 && selectedIndex < optionsCount)
		{
			if (options.Length != 0)
			{
				valueLabel.text = options[selectedIndex];
				return;
			}
			for (int i = 0; i < optionsCount; i++)
			{
				((Component)optionLabels[i]).get_gameObject().SetActive(i == selectedIndex);
			}
		}
		else
		{
			Debug.LogError((object)"Option missing", (Object)(object)this);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		((Selectable)this).OnPointerEnter(eventData);
		cachedEventData = eventData;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		((Selectable)this).OnPointerExit(eventData);
		cachedEventData = null;
		SetActiveArrow(MousePlace.Button);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		((Component)leftArrow).get_gameObject().SetActive(true);
		((Component)rightArrow).get_gameObject().SetActive(true);
		SetActiveArrow(MousePlace.None);
		((Selectable)this).OnSelect(eventData);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		((Component)leftArrow).get_gameObject().SetActive(false);
		((Component)rightArrow).get_gameObject().SetActive(false);
		((Selectable)this).OnDeselect(eventData);
	}

	private MousePlace GetPointPlacement(Vector2 screenPos, Camera camera)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		RectTransform component = ((Component)((Component)leftArrow).get_transform().get_parent()).GetComponent<RectTransform>();
		Vector2 val = default(Vector2);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(component, screenPos, camera, ref val);
		Vector2 val2 = val;
		Rect rect = component.get_rect();
		val = val2 - ((Rect)(ref rect)).get_position();
		float x = val.x;
		rect = component.get_rect();
		float num = x / ((Rect)(ref rect)).get_width();
		if (num < 0f)
		{
			return MousePlace.Button;
		}
		if (num > 0.5f)
		{
			return MousePlace.Right;
		}
		return MousePlace.Left;
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		NextValue();
		((Button)this).OnSubmit(eventData);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		((Button)this).OnPointerClick(eventData);
		switch (GetPointPlacement(eventData.get_position(), eventData.get_pressEventCamera()))
		{
		case MousePlace.Button:
			NextValue();
			break;
		case MousePlace.Left:
			PrevValue();
			break;
		case MousePlace.Right:
			NextValue();
			break;
		}
	}

	private void NextValue()
	{
		if (optionsCount != 0)
		{
			selectedIndex = (selectedIndex + 1) % optionsCount;
			RebindValue();
			((UnityEvent<int>)onValueChanged).Invoke(selectedIndex);
			Canvas.ForceUpdateCanvases();
		}
	}

	private void PrevValue()
	{
		if (optionsCount != 0)
		{
			selectedIndex = (selectedIndex + optionsCount - 1) % optionsCount;
			RebindValue();
			((UnityEvent<int>)onValueChanged).Invoke(selectedIndex);
			Canvas.ForceUpdateCanvases();
		}
	}

	private void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Input.get_mousePresent() && cachedEventData != null)
		{
			MousePlace pointPlacement = GetPointPlacement(Vector2.op_Implicit(Input.get_mousePosition()), cachedEventData.get_enterEventCamera());
			SetActiveArrow(pointPlacement);
		}
	}

	private void SetActiveArrow(MousePlace place)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (place == MousePlace.None)
		{
			((Graphic)leftArrow).get_canvasRenderer().SetColor(new Color(1f, 1f, 1f, 0.5f));
			((Graphic)rightArrow).get_canvasRenderer().SetColor(new Color(1f, 1f, 1f, 0.5f));
		}
		else
		{
			if (place == currentArrow)
			{
				return;
			}
			((Graphic)leftArrow).CrossFadeAlpha((place == MousePlace.Left) ? 1f : 0.5f, 0.1f, true);
			((Graphic)rightArrow).CrossFadeAlpha((place == MousePlace.Right) ? 1f : 0.5f, 0.1f, true);
		}
		currentArrow = place;
	}

	public override void OnMove(AxisEventData eventData)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		((Selectable)this).OnMove(eventData);
		if ((int)eventData.get_moveDir() == 0)
		{
			PrevValue();
		}
		if ((int)eventData.get_moveDir() == 2)
		{
			NextValue();
		}
	}

	public MenuSelector()
		: this()
	{
	}
}
