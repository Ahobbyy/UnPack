using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorWheel : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[Serializable]
	public class ColorEvent : UnityEvent<Color>
	{
	}

	public Color color;

	public Graphic colorBox;

	private Texture2D texture;

	public static bool isInside;

	public static bool isTransparent;

	public ColorEvent onColorPreview;

	public UnityEvent onEndPreview;

	public ColorEvent onColorPick;

	public bool isCursorOver
	{
		get
		{
			if (isInside)
			{
				return !isTransparent;
			}
			return false;
		}
	}

	private void OnEnable()
	{
		ref Texture2D val = ref texture;
		Texture obj = ((Component)this).GetComponent<RawImage>().get_texture();
		val = (Texture2D)(object)((obj is Texture2D) ? obj : null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isInside = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isInside)
		{
			isInside = false;
			onEndPreview.Invoke();
		}
	}

	private Color ColorAtScreenPos(Vector2 mousePos)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)this).get_transform();
		Vector2 val = UICanvas.ScreenPointToLocal((RectTransform)(object)((transform is RectTransform) ? transform : null), mousePos);
		Color pixel = texture.GetPixel(Mathf.Clamp((int)val.x, 0, 512), Mathf.Clamp((int)val.y, 0, 512));
		isTransparent = pixel.a < 0.5f;
		return pixel;
	}

	private void Update()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)colorBox != (Object)null)
		{
			colorBox.set_color(color);
		}
		if (!isInside)
		{
			return;
		}
		Color val = ColorAtScreenPos(Vector2.op_Implicit(Input.get_mousePosition()));
		if (!isTransparent && onColorPick != null)
		{
			if (Input.GetMouseButton(0))
			{
				((UnityEvent<Color>)onColorPick).Invoke(val);
			}
			((UnityEvent<Color>)onColorPreview).Invoke(val);
		}
		else
		{
			onEndPreview.Invoke();
		}
	}

	public ColorWheel()
		: this()
	{
	}
}
