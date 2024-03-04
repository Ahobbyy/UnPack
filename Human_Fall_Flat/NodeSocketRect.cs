using HumanAPI;
using UnityEditor;
using UnityEngine;

public class NodeSocketRect
{
	public Rect rect;

	public NodeSocket socket;

	public NodeRect nodeRect;

	public bool allowEdit;

	private Rect localRect;

	private Rect hitRect;

	public Vector2 connectPoint
	{
		get
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (socket is NodeInput || socket is NodeEntry)
			{
				return new Vector2(((Rect)(ref rect)).get_xMin(), ((Rect)(ref rect)).get_center().y);
			}
			return new Vector2(((Rect)(ref rect)).get_xMax(), ((Rect)(ref rect)).get_center().y);
		}
	}

	public void UpdateLayout(Rect rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		localRect = rect;
		hitRect = localRect;
		if (socket is NodeInput || socket is NodeEntry)
		{
			((Rect)(ref hitRect)).set_width(16f);
		}
		else
		{
			((Rect)(ref hitRect)).set_x(((Rect)(ref hitRect)).get_width() - 16f);
			((Rect)(ref hitRect)).set_width(16f);
		}
		((Rect)(ref rect)).set_x(((Rect)(ref rect)).get_x() + ((Rect)(ref nodeRect.rect)).get_x());
		((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + ((Rect)(ref nodeRect.rect)).get_y());
		this.rect = rect;
	}

	public bool HitTest(Vector2 localPos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ((Rect)(ref hitRect)).Contains(localPos);
	}

	public void Render()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		Rect val = localRect;
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + 16f);
		((Rect)(ref val)).set_width(((Rect)(ref val)).get_width() - 32f);
		float num = 0f;
		if (EditorApplication.get_isPlaying())
		{
			if (socket is NodeInput)
			{
				num = (socket as NodeInput).value;
			}
			if (socket is NodeOutput)
			{
				num = (socket as NodeOutput).value;
			}
		}
		else
		{
			if (socket is NodeInput)
			{
				num = (socket as NodeInput).initialValue;
			}
			if (socket is NodeOutput)
			{
				num = (socket as NodeOutput).initialValue;
			}
		}
		if (socket is NodeInput || socket is NodeEntry)
		{
			GUI.Label(val, socket.name + ":" + num.ToString("0.###"));
		}
		else
		{
			GUI.get_skin().get_label().set_alignment((TextAnchor)2);
			GUI.Label(val, socket.name + ":" + num.ToString("0.###"));
			GUI.get_skin().get_label().set_alignment((TextAnchor)0);
		}
		Rect val2 = hitRect;
		((Rect)(ref val2)).set_x(((Rect)(ref val2)).get_x() + 2f);
		((Rect)(ref val2)).set_y(((Rect)(ref val2)).get_y() + 2f);
		((Rect)(ref val2)).set_width(((Rect)(ref val2)).get_width() - 4f);
		((Rect)(ref val2)).set_height(((Rect)(ref val2)).get_height() - 4f);
		GUI.set_color((num > 0.5f) ? Color.get_green() : Color.get_white());
		NodeWindow.DrawTextureGUI(val2, NodeWindow.circle);
		GUI.set_color(Color.get_white());
	}
}
