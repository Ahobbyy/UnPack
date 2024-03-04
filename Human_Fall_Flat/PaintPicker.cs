using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PaintPicker : MonoBehaviour
{
	[Serializable]
	public class ColorEvent : UnityEvent<Color>
	{
	}

	public Color color;

	public Graphic colorPreviewBox;

	private RectTransform rect;

	private RectTransform parentRect;

	private Camera mainCamera;

	private Texture2D readPixTex;

	private bool allowPicking;

	public ColorEvent onColorPreview;

	public ColorEvent onColorPick;

	public void Initialize()
	{
		rect = ((Component)this).GetComponent<RectTransform>();
		parentRect = ((Component)((Transform)rect).get_parent()).GetComponent<RectTransform>();
		mainCamera = CustomizationController.instance.mainCamera;
	}

	private IEnumerator PickColor()
	{
		yield return (object)new WaitForEndOfFrame();
		int num = 1;
		int num2 = 1;
		if ((Object)(object)readPixTex == (Object)null)
		{
			readPixTex = new Texture2D(num, num2, (TextureFormat)3, false);
		}
		readPixTex.ReadPixels(new Rect(Input.get_mousePosition().x, Input.get_mousePosition().y, (float)num, (float)num2), 0, 0);
		readPixTex.Apply();
		Color pixel = readPixTex.GetPixel(0, 0);
		((UnityEvent<Color>)onColorPick).Invoke(pixel);
	}

	public void Process(bool show, bool pick)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).get_isActiveAndEnabled() != show)
		{
			((Component)this).get_gameObject().SetActive(show);
		}
		if (allowPicking != pick)
		{
			allowPicking = pick;
			if (allowPicking)
			{
				MenuCameraEffects.SuspendEffects(suspend: true);
				mainCamera.SetReplacementShader(Shaders.instance.customizeUnlitShader, "RenderType");
				mainCamera.set_renderingPath((RenderingPath)1);
			}
			else
			{
				MenuCameraEffects.SuspendEffects(suspend: false);
				mainCamera.ResetReplacementShader();
				mainCamera.set_renderingPath((RenderingPath)(-1));
			}
		}
		if (show)
		{
			Vector2 anchoredPosition = UICanvas.ScreenPointToLocal(parentRect, Vector2.op_Implicit(Input.get_mousePosition()));
			rect.set_anchoredPosition(anchoredPosition);
			if (allowPicking)
			{
				((MonoBehaviour)this).StartCoroutine(PickColor());
			}
			colorPreviewBox.set_color(color);
		}
	}

	public PaintPicker()
		: this()
	{
	}
}
