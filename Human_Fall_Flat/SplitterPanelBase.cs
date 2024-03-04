using UnityEditor;
using UnityEngine;

public abstract class SplitterPanelBase
{
	public enum CollapseMode
	{
		Normal,
		CollapsedA,
		CollapsedB
	}

	public struct InitialConfig
	{
		public bool UpDownMode;

		public bool SwitchPanels;

		public bool CanSnapAClosed;

		public CollapseMode Collapse;

		public float PaneASize;

		public Rect InitialBounds;

		public float MinSizeA;

		public float MinSizeB;
	}

	protected bool _SwitchPanels;

	protected bool _CanSnapAClosed;

	protected bool _IsSnappedClosed;

	protected CollapseMode _CollapseMode;

	protected float _MinSizeA;

	protected float _MinSizeB;

	protected float SizeA = 150f;

	protected float SizeD = 10f;

	protected float ActiveSizeA;

	protected float ActiveSizeB;

	protected float ActiveSizeD;

	public bool debugRender;

	protected Rect _Bounds;

	private bool dragging;

	private float dragSize;

	private float dragRefPt;

	public Rect RectPaneA { get; protected set; }

	public Rect RectPaneB { get; protected set; }

	public Rect RectDivider { get; protected set; }

	public bool DrawPaneA { get; protected set; }

	public bool DrawPaneB { get; protected set; }

	public bool DrawDivider { get; protected set; }

	public bool UpDownMode { get; protected set; }

	public bool SwitchPanels
	{
		get
		{
			return _SwitchPanels;
		}
		set
		{
			if (_SwitchPanels != value)
			{
				_SwitchPanels = value;
				dragging = false;
				RefreshOutputs();
			}
		}
	}

	public CollapseMode CollapseState
	{
		get
		{
			return _CollapseMode;
		}
		set
		{
			if (_CollapseMode != value)
			{
				_CollapseMode = value;
				dragging = false;
				UpdateRange();
				RefreshOutputs();
			}
		}
	}

	public bool CanSnapAClosed
	{
		get
		{
			return _CanSnapAClosed;
		}
		set
		{
			if (_CanSnapAClosed != value)
			{
				_CanSnapAClosed = value;
				UpdateRange();
				RefreshOutputs();
			}
		}
	}

	public float MinSizeA
	{
		get
		{
			return _MinSizeA;
		}
		set
		{
			if (_MinSizeA != value)
			{
				_MinSizeA = value;
				UpdateMinBounds();
				UpdateRange();
				RefreshOutputs();
			}
		}
	}

	public float MinSizeB
	{
		get
		{
			return _MinSizeB;
		}
		set
		{
			if (_MinSizeB != value)
			{
				_MinSizeB = value;
				UpdateMinBounds();
				UpdateRange();
				RefreshOutputs();
			}
		}
	}

	public Rect Bounds
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _Bounds;
		}
		set
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (((Rect)(ref _Bounds)).get_x() != ((Rect)(ref value)).get_x() || ((Rect)(ref _Bounds)).get_y() != ((Rect)(ref value)).get_y() || ((Rect)(ref _Bounds)).get_width() != ((Rect)(ref value)).get_width() || ((Rect)(ref _Bounds)).get_height() != ((Rect)(ref value)).get_height())
			{
				_Bounds = value;
				UpdateMinBounds();
				UpdateRange();
				RefreshOutputs();
			}
		}
	}

	public float MinBoundsX { get; protected set; }

	public float MinBoundsY { get; protected set; }

	public SplitterPanelBase()
	{
	}

	public void CompleteInit()
	{
		UpdateMinBounds();
		UpdateRange();
		RefreshOutputs();
		dragging = false;
	}

	public void InitState(InitialConfig config, bool deferRecalc = false)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		UpDownMode = config.UpDownMode;
		_SwitchPanels = config.SwitchPanels;
		_CanSnapAClosed = config.CanSnapAClosed;
		_CollapseMode = config.Collapse;
		SizeA = config.PaneASize;
		_Bounds = config.InitialBounds;
		_MinSizeA = config.MinSizeA;
		_MinSizeB = config.MinSizeB;
		if (!deferRecalc)
		{
			CompleteInit();
		}
	}

	protected void UpdateMinBounds()
	{
		if (UpDownMode)
		{
			MinBoundsX = ((Rect)(ref _Bounds)).get_xMax();
			MinBoundsY = ((Rect)(ref _Bounds)).get_yMin() + _MinSizeA + _MinSizeB + SizeD;
		}
		else
		{
			MinBoundsX = ((Rect)(ref _Bounds)).get_xMin() + _MinSizeA + _MinSizeB + SizeD;
			MinBoundsY = ((Rect)(ref _Bounds)).get_yMax();
		}
	}

	protected void UpdateRange()
	{
		float num = (UpDownMode ? ((Rect)(ref _Bounds)).get_height() : ((Rect)(ref _Bounds)).get_width());
		if (num < 0f)
		{
			num = 0f;
		}
		switch (_CollapseMode)
		{
		case CollapseMode.CollapsedA:
			ActiveSizeA = 0f;
			ActiveSizeD = 0f;
			ActiveSizeB = num;
			return;
		case CollapseMode.CollapsedB:
			ActiveSizeB = 0f;
			ActiveSizeD = 0f;
			ActiveSizeA = num;
			return;
		}
		ActiveSizeA = SizeA;
		ActiveSizeD = SizeD;
		float num2 = ClampActiveA(num);
		if (num2 > 0f)
		{
			ActiveSizeA -= num2;
			num2 = ClampActiveA(num);
			if (num2 > 0f)
			{
				ActiveSizeA = 0f;
				num2 = ClampActiveA(num);
				if (num2 > 0f)
				{
					ActiveSizeA = 0f;
					ActiveSizeD = 0f;
					ActiveSizeB = num;
				}
			}
		}
		_IsSnappedClosed = _CanSnapAClosed && ActiveSizeA <= 0f;
	}

	private float ClampActiveA(float bounds)
	{
		if (_CanSnapAClosed)
		{
			if (ActiveSizeA <= 0f || (ActiveSizeA < _MinSizeA * 0.5f && _IsSnappedClosed))
			{
				ActiveSizeA = 0f;
			}
			else if (ActiveSizeA < _MinSizeA)
			{
				ActiveSizeA = _MinSizeA;
			}
		}
		else if (ActiveSizeA < _MinSizeA)
		{
			ActiveSizeA = _MinSizeA;
		}
		ActiveSizeB = bounds - ActiveSizeA - ActiveSizeD;
		if (ActiveSizeB < _MinSizeB)
		{
			ActiveSizeB = _MinSizeB;
		}
		return ActiveSizeA + ActiveSizeB + ActiveSizeD - bounds;
	}

	private void MoveDivider(float newSizeA)
	{
		SizeA = newSizeA;
		UpdateRange();
		RefreshOutputs();
	}

	protected void RefreshOutputs()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		if (UpDownMode)
		{
			if (SwitchPanels)
			{
				RectPaneB = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y(), ((Rect)(ref _Bounds)).get_width(), ActiveSizeB);
				RectDivider = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y() + ActiveSizeB, ((Rect)(ref _Bounds)).get_width(), ActiveSizeD);
				RectPaneA = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y() + ActiveSizeB + ActiveSizeD, ((Rect)(ref _Bounds)).get_width(), ActiveSizeA);
			}
			else
			{
				RectPaneA = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y(), ((Rect)(ref _Bounds)).get_width(), ActiveSizeA);
				RectDivider = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y() + ActiveSizeA, ((Rect)(ref _Bounds)).get_width(), ActiveSizeD);
				RectPaneB = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y() + ActiveSizeA + ActiveSizeD, ((Rect)(ref _Bounds)).get_width(), ActiveSizeB);
			}
		}
		else if (SwitchPanels)
		{
			RectPaneB = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y(), ActiveSizeB, ((Rect)(ref _Bounds)).get_height());
			RectDivider = new Rect(((Rect)(ref _Bounds)).get_x() + ActiveSizeB, ((Rect)(ref _Bounds)).get_y(), ActiveSizeD, ((Rect)(ref _Bounds)).get_height());
			RectPaneA = new Rect(((Rect)(ref _Bounds)).get_x() + ActiveSizeB + ActiveSizeD, ((Rect)(ref _Bounds)).get_y(), ActiveSizeA, ((Rect)(ref _Bounds)).get_height());
		}
		else
		{
			RectPaneA = new Rect(((Rect)(ref _Bounds)).get_x(), ((Rect)(ref _Bounds)).get_y(), ActiveSizeA, ((Rect)(ref _Bounds)).get_height());
			RectDivider = new Rect(((Rect)(ref _Bounds)).get_x() + ActiveSizeA, ((Rect)(ref _Bounds)).get_y(), ActiveSizeD, ((Rect)(ref _Bounds)).get_height());
			RectPaneB = new Rect(((Rect)(ref _Bounds)).get_x() + ActiveSizeA + ActiveSizeD, ((Rect)(ref _Bounds)).get_y(), ActiveSizeB, ((Rect)(ref _Bounds)).get_height());
		}
		Rect val = RectPaneA;
		int drawPaneA;
		if (((Rect)(ref val)).get_width() > 0f)
		{
			val = RectPaneA;
			if (((Rect)(ref val)).get_height() > 0f)
			{
				drawPaneA = ((_CollapseMode != CollapseMode.CollapsedA) ? 1 : 0);
				goto IL_0316;
			}
		}
		drawPaneA = 0;
		goto IL_0316;
		IL_0391:
		int drawDivider;
		DrawDivider = (byte)drawDivider != 0;
		if (!DrawDivider)
		{
			dragging = false;
		}
		return;
		IL_0316:
		DrawPaneA = (byte)drawPaneA != 0;
		val = RectPaneB;
		int drawPaneB;
		if (((Rect)(ref val)).get_width() > 0f)
		{
			val = RectPaneB;
			if (((Rect)(ref val)).get_height() > 0f)
			{
				drawPaneB = ((_CollapseMode != CollapseMode.CollapsedB) ? 1 : 0);
				goto IL_0355;
			}
		}
		drawPaneB = 0;
		goto IL_0355;
		IL_0355:
		DrawPaneB = (byte)drawPaneB != 0;
		val = RectDivider;
		if (((Rect)(ref val)).get_width() > 0f)
		{
			val = RectDivider;
			if (((Rect)(ref val)).get_height() > 0f)
			{
				drawDivider = ((_CollapseMode == CollapseMode.Normal) ? 1 : 0);
				goto IL_0391;
			}
		}
		drawDivider = 0;
		goto IL_0391;
	}

	protected abstract void RenderDivider();

	public bool OnGUI(int dividerControlHint = -99)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected I4, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		Event current = Event.get_current();
		int controlID = GUIUtility.GetControlID(dividerControlHint, (FocusType)2, RectDivider);
		if (GUIUtility.get_hotControl() == controlID)
		{
			if (!dragging)
			{
				GUIUtility.set_hotControl(0);
				current.Use();
			}
		}
		else
		{
			dragging = false;
		}
		EventType type = current.get_type();
		switch ((int)type)
		{
		case 1:
			if (current.get_button() == 0 && GUIUtility.get_hotControl() == controlID)
			{
				dragging = false;
				GUIUtility.set_hotControl(0);
			}
			if (GUIUtility.get_hotControl() == controlID)
			{
				current.Use();
				if (!dragging)
				{
					GUIUtility.set_hotControl(0);
				}
			}
			break;
		case 0:
		case 3:
		{
			if (current.get_button() != 0 && !dragging)
			{
				break;
			}
			if (!dragging && GUIUtility.get_hotControl() == 0)
			{
				if (!DrawDivider)
				{
					break;
				}
				Rect rectDivider = RectDivider;
				if (!((Rect)(ref rectDivider)).Contains(current.get_mousePosition()))
				{
					break;
				}
				GUIUtility.set_hotControl(controlID);
				dragging = true;
				dragSize = ActiveSizeA;
				dragRefPt = (UpDownMode ? current.get_mousePosition().y : current.get_mousePosition().x);
			}
			if (GUIUtility.get_hotControl() != controlID)
			{
				dragging = false;
				break;
			}
			current.Use();
			float num = (UpDownMode ? current.get_mousePosition().y : current.get_mousePosition().x) - dragRefPt;
			if (_SwitchPanels)
			{
				num = 0f - num;
			}
			MoveDivider(dragSize + num);
			result = true;
			break;
		}
		}
		if (debugRender)
		{
			Rect rectPaneA = RectPaneA;
			Rect rectDivider2 = RectDivider;
			Rect rectPaneB = RectPaneB;
			if (DrawPaneA)
			{
				EditorGUI.DrawRect(rectPaneA, Color.get_red());
			}
			if (DrawPaneB)
			{
				EditorGUI.DrawRect(rectPaneB, Color.get_green());
			}
			if (DrawDivider)
			{
				EditorGUI.DrawRect(rectDivider2, Color.get_blue());
			}
		}
		if (DrawDivider)
		{
			RenderDivider();
		}
		EditorGUIUtility.AddCursorRect(RectDivider, (MouseCursor)(UpDownMode ? 18 : 19), controlID);
		return result;
	}
}
