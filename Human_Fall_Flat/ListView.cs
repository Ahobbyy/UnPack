using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListView : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	public delegate void PointerClickAction<T>(T obj, int clickCount, InputButton button);

	public Button itemTemplate;

	public GameObject itemContainer;

	private List<Button> buttons = new List<Button>();

	private GameObject newSelectedObject;

	private int targetItemIndex;

	private bool forceSnap;

	public bool disableHoverSelect;

	private float fromAnchor;

	private float toAnchor;

	public float transitionDuration = 0.25f;

	private float transitionPhase = 1f;

	private bool ignoreOnSelect;

	public Action<ListViewItem> onSubmit;

	public Action<ListViewItem> onSelect;

	public Action<ListViewItem> onDeSelect;

	public PointerClickAction<ListViewItem> onPointerClick;

	private RectTransform containerRect;

	private RectTransform scrollRect;

	private float itemSize;

	private int maxItemsOnScreen;

	private int orginalItemsInList;

	[NonSerialized]
	public float itemSpacing;

	private bool isHorizontal;

	[NonSerialized]
	public bool isCarousel;

	private float scrollSpeed = 100f;

	private float anchoredPosition
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (!isHorizontal)
			{
				return 0f - containerRect.get_anchoredPosition().y;
			}
			return containerRect.get_anchoredPosition().x;
		}
	}

	private float spaceAfterItem
	{
		get
		{
			if (!isHorizontal)
			{
				return 0f;
			}
			return (itemSize + itemSpacing) / 2f;
		}
	}

	public int GetNumberItems => orginalItemsInList;

	public void Bind(IList list)
	{
		Clear();
		orginalItemsInList = list.Count;
		if (orginalItemsInList == 0)
		{
			return;
		}
		GameObject val = null;
		int num = 0;
		int num2;
		if (orginalItemsInList > maxItemsOnScreen)
		{
			num2 = maxItemsOnScreen + orginalItemsInList;
			num = (isHorizontal ? Math.Max(orginalItemsInList - maxItemsOnScreen, 0) : 0);
			isCarousel = true;
		}
		else
		{
			num2 = orginalItemsInList;
			isCarousel = false;
		}
		int[] array = new int[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = num;
			num++;
			if (num >= list.Count)
			{
				num = 0;
			}
		}
		for (int j = 0; j < num2; j++)
		{
			val = Object.Instantiate<GameObject>(((Component)itemTemplate).get_gameObject(), itemContainer.get_transform(), false);
			buttons.Add(val.GetComponent<Button>());
			ListViewItem component = val.GetComponent<ListViewItem>();
			int index = array[j];
			component.Bind(j, list[index]);
			val.SetActive(true);
		}
	}

	public GameObject GetButton(int index)
	{
		return ((Component)buttons[index]).get_gameObject();
	}

	public void FocusItem(int index)
	{
		if (isCarousel)
		{
			index += maxItemsOnScreen;
			if (index > orginalItemsInList)
			{
				index -= orginalItemsInList;
			}
		}
		FocusItem(((Component)buttons[index]).GetComponent<ListViewItem>());
	}

	public void FocusItem(ListViewItem item)
	{
		EventSystem.get_current().SetSelectedGameObject(((Component)item).get_gameObject());
		BringItemToStart(item);
	}

	private void JumpToTarget()
	{
		ListViewItem component = ((Component)buttons[targetItemIndex]).GetComponent<ListViewItem>();
		newSelectedObject = ((Component)component).get_gameObject();
		forceSnap = true;
		BringItemToStart(component);
		if (onSelect != null)
		{
			onSelect(component);
		}
	}

	public void PageUp()
	{
		if (isCarousel)
		{
			targetItemIndex += maxItemsOnScreen - 2;
			if (targetItemIndex > orginalItemsInList)
			{
				targetItemIndex -= orginalItemsInList;
			}
			JumpToTarget();
		}
	}

	public void PageDown()
	{
		if (isCarousel)
		{
			targetItemIndex -= maxItemsOnScreen - 2;
			if (targetItemIndex <= 0)
			{
				targetItemIndex += orginalItemsInList;
			}
			JumpToTarget();
		}
	}

	private void CenterItem(ListViewItem item)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)item.index * (itemSize + itemSpacing) + itemSize / 2f;
		Rect rect;
		float num2;
		if (!isHorizontal)
		{
			rect = scrollRect.get_rect();
			num2 = ((Rect)(ref rect)).get_height();
		}
		else
		{
			rect = scrollRect.get_rect();
			num2 = ((Rect)(ref rect)).get_width();
		}
		ScrollToOffset(num - num2 / 2f);
	}

	private void BringItemToStart(ListViewItem item)
	{
		ScrollToOffset((float)item.index * (itemSize + itemSpacing) - spaceAfterItem);
	}

	private void BringItemToEnd(ListViewItem item)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)item.index * (itemSize + itemSpacing) + itemSize;
		Rect rect;
		float num2;
		if (!isHorizontal)
		{
			rect = scrollRect.get_rect();
			num2 = ((Rect)(ref rect)).get_height();
		}
		else
		{
			rect = scrollRect.get_rect();
			num2 = ((Rect)(ref rect)).get_width();
		}
		ScrollToOffset(num - num2 + spaceAfterItem);
	}

	private void ScrollToItem(ListViewItem item)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		((Component)item).GetComponent<RectTransform>();
		int num = item.index;
		if (isHorizontal && isCarousel && (item.index >= maxItemsOnScreen + orginalItemsInList - 1 || item.index <= 0))
		{
			int num2 = ((item.index > 0) ? 1 : (-1));
			float num3 = (float)orginalItemsInList * (itemSize + itemSpacing);
			num = item.index - orginalItemsInList * num2;
			containerRect.set_anchoredPosition(new Vector2(containerRect.get_anchoredPosition().x + num3 * (float)num2, containerRect.get_anchoredPosition().y));
			newSelectedObject = ((Component)buttons[num]).get_gameObject();
		}
		targetItemIndex = num;
		float num4 = (float)num * (itemSize + itemSpacing);
		float num5 = num4 + itemSize;
		num4 -= spaceAfterItem;
		num5 += spaceAfterItem;
		Rect rect;
		float num6;
		if (!isHorizontal)
		{
			rect = scrollRect.get_rect();
			num6 = ((Rect)(ref rect)).get_height();
		}
		else
		{
			rect = scrollRect.get_rect();
			num6 = ((Rect)(ref rect)).get_width();
		}
		float num7 = num6;
		float num8 = 0f - anchoredPosition;
		if (num4 < num8)
		{
			ScrollToOffset(num4);
		}
		if (num5 > num8 + num7)
		{
			ScrollToOffset(num5 - num7);
		}
	}

	private void ScrollToOffset(float offset)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f - spaceAfterItem;
		float num2 = (float)(buttons.Count - 1) * (itemSize + itemSpacing) + itemSize;
		Rect rect;
		float num3;
		if (!isHorizontal)
		{
			rect = scrollRect.get_rect();
			num3 = ((Rect)(ref rect)).get_height();
		}
		else
		{
			rect = scrollRect.get_rect();
			num3 = ((Rect)(ref rect)).get_width();
		}
		float num4 = num2 - num3 + spaceAfterItem;
		if (offset > num4)
		{
			offset = num4;
		}
		if (offset < num)
		{
			offset = num;
		}
		fromAnchor = anchoredPosition;
		toAnchor = 0f - offset;
		transitionPhase = 0f;
	}

	private void Update()
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)newSelectedObject != (Object)null)
		{
			ignoreOnSelect = true;
			EventSystem.get_current().SetSelectedGameObject(newSelectedObject);
			newSelectedObject = null;
		}
		if (transitionPhase < 1f || forceSnap)
		{
			if (forceSnap)
			{
				transitionPhase = 1f;
				forceSnap = false;
			}
			else
			{
				transitionPhase += Time.get_deltaTime() / transitionDuration;
			}
			if (transitionPhase >= 1f)
			{
				transitionPhase = 1f;
			}
			float num = Ease.easeOutQuad(0f, 1f, transitionPhase);
			if (isHorizontal)
			{
				containerRect.set_anchoredPosition(new Vector2(Mathf.Lerp(fromAnchor, toAnchor, num), containerRect.get_anchoredPosition().y));
			}
			else
			{
				containerRect.set_anchoredPosition(new Vector2(containerRect.get_anchoredPosition().x, Mathf.Lerp(0f - fromAnchor, 0f - toAnchor, num)));
			}
		}
	}

	public void OnPointerClick(ListViewItem item, int clickCount, InputButton button)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (onPointerClick != null)
		{
			onPointerClick(item, clickCount, button);
		}
	}

	public void OnSubmit(ListViewItem item)
	{
		if (onSubmit != null)
		{
			onSubmit(item);
		}
	}

	public void OnSelect(ListViewItem item)
	{
		if (ignoreOnSelect)
		{
			ignoreOnSelect = false;
			return;
		}
		ScrollToItem(item);
		if (!disableHoverSelect)
		{
			if (onSelect != null)
			{
				onSelect(item);
			}
			if (MenuSystem.instance.GetCurrentInputDevice() == MenuSystem.eInputDeviceType.Mouse)
			{
				EventSystem.get_current().SetSelectedGameObject(((Component)item).get_gameObject());
			}
		}
	}

	public void OnDeSelect(ListViewItem item)
	{
		if (!((Object)(object)this == (Object)null) && onSubmit != null && onDeSelect != null && (Object)(object)item != (Object)null)
		{
			onDeSelect(item);
		}
	}

	private void Awake()
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		scrollRect = ((Component)this).GetComponent<RectTransform>();
		containerRect = itemContainer.GetComponent<RectTransform>();
		isHorizontal = itemContainer.GetComponent<HorizontalOrVerticalLayoutGroup>() is HorizontalLayoutGroup;
		itemSpacing = itemContainer.GetComponent<HorizontalOrVerticalLayoutGroup>().get_spacing();
		itemSize = (isHorizontal ? ((Component)itemTemplate).GetComponent<LayoutElement>().get_preferredWidth() : ((Component)itemTemplate).GetComponent<LayoutElement>().get_preferredHeight());
		((Component)itemTemplate).get_gameObject().SetActive(false);
		if (isHorizontal)
		{
			Rect rect = scrollRect.get_rect();
			float num = ((Rect)(ref rect)).get_width() / (itemSize + itemSpacing);
			maxItemsOnScreen = (int)Mathf.Floor(num + 0.999f);
			maxItemsOnScreen++;
		}
		else
		{
			maxItemsOnScreen = 0;
		}
	}

	public void Clear()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			Object.Destroy((Object)(object)((Component)buttons[i]).get_gameObject());
		}
		buttons.Clear();
		isCarousel = false;
	}

	public void OnScroll(PointerEventData eventData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		ScrollToOffset(0f - toAnchor - eventData.get_scrollDelta().y * scrollSpeed);
	}

	public ListView()
		: this()
	{
	}
}
