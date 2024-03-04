using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSlider : Slider
{
	private RectTransform hitRect;

	protected override void OnEnable()
	{
		((Slider)this).OnEnable();
		hitRect = ((Component)((Transform)((Slider)this).get_fillRect()).get_parent()).GetComponent<RectTransform>();
	}

	private bool IsRightOfSlider(Vector2 screenPos, Camera camera)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(hitRect, screenPos, camera, ref val);
		Vector2 val2 = val;
		Rect rect = hitRect.get_rect();
		val = val2 - ((Rect)(ref rect)).get_position();
		return val.x < 0f;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		if (IsRightOfSlider(eventData.get_position(), eventData.get_pressEventCamera()))
		{
			IntPtr functionPointer = typeof(Selectable).GetMethod("OnPointerDown").MethodHandle.GetFunctionPointer();
			((Action<PointerEventData>)Activator.CreateInstance(typeof(Action<PointerEventData>), this, functionPointer))(eventData);
		}
		else
		{
			((Slider)this).OnPointerDown(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		if (!IsRightOfSlider(eventData.get_pressPosition(), eventData.get_pressEventCamera()))
		{
			((Slider)this).OnDrag(eventData);
		}
	}

	public MenuSlider()
		: this()
	{
	}
}
