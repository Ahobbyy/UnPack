using UnityEngine;
using UnityEngine.EventSystems;

public class ListViewItem : MonoBehaviour, IScrollHandler, IEventSystemHandler, ISubmitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler
{
	public int index;

	public object data;

	public WorkshopItemSource type;

	public virtual void Bind(int index, object data)
	{
		this.index = index;
		this.data = data;
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).GetComponentInParent<ListView>().OnPointerClick(this, pointerEventData.get_clickCount(), pointerEventData.get_button());
	}

	public virtual void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (MenuSystem.instance.GetCurrentInputDevice() == MenuSystem.eInputDeviceType.Mouse)
		{
			((Component)this).GetComponentInParent<ListView>().OnSelect(this);
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (MenuSystem.instance.GetCurrentInputDevice() != 0)
		{
			((Component)this).GetComponentInParent<ListView>().OnSelect(this);
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		((Component)this).GetComponentInParent<ListView>().OnSubmit(this);
	}

	public void OnScroll(PointerEventData eventData)
	{
		((Component)this).GetComponentInParent<ListView>().OnScroll(eventData);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (!((Object)(object)this == (Object)null))
		{
			ListView componentInParent = ((Component)this).GetComponentInParent<ListView>();
			if (Object.op_Implicit((Object)(object)componentInParent))
			{
				componentInParent.OnDeSelect(this);
			}
		}
	}

	public ListViewItem()
		: this()
	{
	}
}
