using UnityEditor;
using UnityEngine;

public class SplitterPanel : SplitterPanelBase
{
	protected float LineW = 2f;

	protected float MarginY;

	public Color LeftEdgeColor = new Color(0.25f, 0.25f, 0.25f, 1f);

	public Color RightEdgeColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	public static readonly string[] CollapseSymbols = new string[4] { "►", "◄", "▼", "▲" };

	private static GUIStyle style_Button = null;

	public SplitterPanel()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		InitDividerShape();
	}

	public void InitDividerShape(float divW = 6f, float lineW = 3f, float marginY = 15f)
	{
		SizeD = divW;
		LineW = lineW;
		MarginY = marginY;
		CompleteInit();
	}

	protected override void RenderDivider()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		Rect rectDivider;
		if (base.UpDownMode)
		{
			rectDivider = base.RectDivider;
			float num = (((Rect)(ref rectDivider)).get_height() - LineW) * 0.5f;
			rectDivider = base.RectDivider;
			float num2 = ((Rect)(ref rectDivider)).get_x() + MarginY;
			rectDivider = base.RectDivider;
			float num3 = ((Rect)(ref rectDivider)).get_y() + num;
			rectDivider = base.RectDivider;
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(num2, num3, ((Rect)(ref rectDivider)).get_width() - MarginY * 2f, LineW * 0.5f);
			EditorGUI.DrawRect(val, LeftEdgeColor);
			((Rect)(ref val)).set_y(((Rect)(ref val)).get_y() + ((Rect)(ref val)).get_height());
			EditorGUI.DrawRect(val, RightEdgeColor);
		}
		else
		{
			rectDivider = base.RectDivider;
			float num4 = (((Rect)(ref rectDivider)).get_width() - LineW) * 0.5f;
			rectDivider = base.RectDivider;
			float num5 = ((Rect)(ref rectDivider)).get_x() + num4;
			rectDivider = base.RectDivider;
			float num6 = ((Rect)(ref rectDivider)).get_y() + MarginY;
			float num7 = LineW * 0.5f;
			rectDivider = base.RectDivider;
			Rect val2 = default(Rect);
			((Rect)(ref val2))._002Ector(num5, num6, num7, ((Rect)(ref rectDivider)).get_height() - MarginY * 2f);
			EditorGUI.DrawRect(val2, LeftEdgeColor);
			((Rect)(ref val2)).set_x(((Rect)(ref val2)).get_x() + ((Rect)(ref val2)).get_width());
			EditorGUI.DrawRect(val2, RightEdgeColor);
		}
	}

	public string DetermineCollapseTip(bool right)
	{
		if (base.CollapseState == CollapseMode.CollapsedA)
		{
			return "Open side panel";
		}
		if (right ? (!base.SwitchPanels) : base.SwitchPanels)
		{
			return "Move side panel to this side";
		}
		return "Close side panel";
	}

	public int DetermineCollapseSymbol(bool right)
	{
		bool flag = base.CollapseState == CollapseMode.CollapsedA || (right ? (!base.SwitchPanels) : base.SwitchPanels);
		if (right)
		{
			flag = !flag;
		}
		if (base.UpDownMode)
		{
			if (!flag)
			{
				return 3;
			}
			return 2;
		}
		if (!flag)
		{
			return 1;
		}
		return 0;
	}

	public void ProcessCollapseButton(bool right)
	{
		bool flag = base.SwitchPanels;
		if (right)
		{
			flag = !flag;
		}
		if (flag)
		{
			base.SwitchPanels = !base.SwitchPanels;
			base.CollapseState = CollapseMode.Normal;
		}
		else if (base.CollapseState != 0)
		{
			base.CollapseState = CollapseMode.Normal;
		}
		else
		{
			base.CollapseState = CollapseMode.CollapsedA;
		}
	}

	public bool DefaultCollapeButtons(float size = 16f, float margin1 = 4f, float margin2 = 4f)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		if (!base.DrawPaneB)
		{
			return false;
		}
		if (style_Button == null)
		{
			style_Button = new GUIStyle(EditorStyles.get_miniButton());
			style_Button.set_fontSize(7);
		}
		bool result = false;
		Rect rectPaneB = base.RectPaneB;
		float num = ((Rect)(ref rectPaneB)).get_x() + margin1;
		rectPaneB = base.RectPaneB;
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(num, ((Rect)(ref rectPaneB)).get_y() + margin1, size, size);
		if (base.UpDownMode)
		{
			rectPaneB = base.RectPaneB;
			((Rect)(ref val)).set_x(((Rect)(ref rectPaneB)).get_xMax() - margin2 - size);
		}
		GUIContent val2 = new GUIContent(CollapseSymbols[DetermineCollapseSymbol(right: false)], DetermineCollapseTip(right: false));
		if (GUI.Button(val, val2, style_Button))
		{
			ProcessCollapseButton(right: false);
			result = true;
		}
		if (base.UpDownMode)
		{
			rectPaneB = base.RectPaneB;
			((Rect)(ref val)).set_y(((Rect)(ref rectPaneB)).get_yMax() - margin2 - size);
		}
		else
		{
			rectPaneB = base.RectPaneB;
			((Rect)(ref val)).set_x(((Rect)(ref rectPaneB)).get_xMax() - margin2 - size);
		}
		val2 = new GUIContent(CollapseSymbols[DetermineCollapseSymbol(right: true)], DetermineCollapseTip(right: true));
		if (GUI.Button(val, val2, style_Button))
		{
			ProcessCollapseButton(right: true);
			result = true;
		}
		return result;
	}
}
