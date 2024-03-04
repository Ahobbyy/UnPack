using UnityEngine;
using UnityEngine.UI;

public class PaintCursor : MonoBehaviour
{
	public Color color = Color.get_white();

	public float cursorKernel = 10f;

	public float cursorFalloff = 10f;

	private RawImage cursor;

	private Material cursorMaterial;

	private RectTransform cursorParentRect;

	public void Initialize()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		cursor = ((Component)this).GetComponent<RawImage>();
		cursorMaterial = new Material(((Graphic)cursor).get_material());
		((Graphic)cursor).set_material(cursorMaterial);
		cursorParentRect = ((Component)((Component)this).get_transform().get_parent()).GetComponent<RectTransform>();
	}

	public void Process(bool show)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).get_isActiveAndEnabled() != show)
		{
			((Component)this).get_gameObject().SetActive(show);
		}
		if (show)
		{
			cursorMaterial.set_color(color);
			cursorMaterial.SetVector("_PointPos", new Vector4(Input.get_mousePosition().x, Input.get_mousePosition().y, 0f, 0f));
			cursorMaterial.SetVector("_ScreenSize", new Vector4((float)Screen.get_width(), (float)Screen.get_height(), 0f, 0f));
			cursorMaterial.SetFloat("_PointSize", cursorKernel);
			cursorMaterial.SetFloat("_FalloffSize", cursorFalloff);
			float num = (float)(2 * Screen.get_height()) * (cursorFalloff + cursorKernel) + 2f;
			Vector2 val = UICanvas.ScreenPointToLocal(cursorParentRect, Vector2.op_Implicit(Input.get_mousePosition()));
			Vector2 val2 = UICanvas.ScreenPointToLocal(cursorParentRect, Vector2.op_Implicit(Input.get_mousePosition() + new Vector3(1f, 1f, 0f) * num));
			((Graphic)cursor).get_rectTransform().set_anchoredPosition(val);
			((Graphic)cursor).get_rectTransform().SetSizeWithCurrentAnchors((Axis)0, val2.x - val.x);
			((Graphic)cursor).get_rectTransform().SetSizeWithCurrentAnchors((Axis)1, val2.y - val.y);
		}
	}

	public PaintCursor()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
