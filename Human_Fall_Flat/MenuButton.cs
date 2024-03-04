using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : Button
{
	private bool _isOn;

	public TextMeshProUGUI label;

	[NonSerialized]
	public Text[] textLabel;

	private int frameUpdated;

	public bool isOn
	{
		get
		{
			return _isOn;
		}
		set
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_isOn = value;
			((Selectable)this).DoStateTransition(((Selectable)this).get_currentSelectionState(), false);
		}
	}

	public void SetLabel(Text textLabelPara)
	{
		textLabel = (Text[])(object)new Text[1] { textLabelPara };
		label = null;
	}

	public void SetLabel(TextMeshProUGUI label)
	{
		this.label = label;
		textLabel = null;
	}

	protected override void Awake()
	{
		((Selectable)this).Awake();
		if ((Object)(object)label == (Object)null)
		{
			label = ((Component)this).GetComponentInChildren<TextMeshProUGUI>();
		}
		if ((Object)(object)label == (Object)null)
		{
			textLabel = ((Component)this).GetComponentsInChildren<Text>();
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected I4, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		Color val = default(Color);
		Color val2;
		ColorBlock colors;
		if (isOn)
		{
			if ((int)state == 1)
			{
				((Color)(ref val))._002Ector(0f, 0f, 0f, 1f);
			}
			else
			{
				((Color)(ref val))._002Ector(0f, 0f, 0f, 0.9f);
			}
			val2 = Color.get_white();
		}
		else
		{
			switch ((int)state)
			{
			case 0:
				colors = ((Selectable)this).get_colors();
				((Color)(ref val))._002Ector(1f, 1f, 1f, ((ColorBlock)(ref colors)).get_normalColor().a);
				val2 = Color.get_black();
				break;
			case 1:
				((Color)(ref val))._002Ector(0f, 0f, 0f, 0.75f);
				val2 = Color.get_white();
				break;
			case 2:
				((Color)(ref val))._002Ector(0f, 0f, 0f, 0.75f);
				val2 = Color.get_white();
				break;
			case 3:
				colors = ((Selectable)this).get_colors();
				val = ((ColorBlock)(ref colors)).get_disabledColor();
				val2 = Color.get_black();
				break;
			default:
				val = Color.get_black();
				val2 = Color.get_white();
				break;
			}
		}
		if (frameUpdated == Time.get_renderedFrameCount())
		{
			instant = true;
		}
		frameUpdated = Time.get_renderedFrameCount();
		if (!((Component)this).get_gameObject().get_activeInHierarchy())
		{
			return;
		}
		Graphic targetGraphic = ((Selectable)this).get_targetGraphic();
		Color val3 = val;
		colors = ((Selectable)this).get_colors();
		Color val4 = val3 * ((ColorBlock)(ref colors)).get_colorMultiplier();
		float num;
		if (!instant)
		{
			colors = ((Selectable)this).get_colors();
			num = ((ColorBlock)(ref colors)).get_fadeDuration();
		}
		else
		{
			num = 0f;
		}
		targetGraphic.CrossFadeColor(val4, num, true, true);
		if ((Object)(object)label != (Object)null)
		{
			((Graphic)label).set_color(Color.get_white());
			TextMeshProUGUI textMeshProUGUI = label;
			Color val5 = val2;
			colors = ((Selectable)this).get_colors();
			Color val6 = val5 * ((ColorBlock)(ref colors)).get_colorMultiplier();
			float num2;
			if (!instant)
			{
				colors = ((Selectable)this).get_colors();
				num2 = ((ColorBlock)(ref colors)).get_fadeDuration();
			}
			else
			{
				num2 = 0f;
			}
			((Graphic)textMeshProUGUI).CrossFadeColor(val6, num2, true, true);
		}
		if (textLabel == null)
		{
			return;
		}
		for (int i = 0; i < textLabel.Length; i++)
		{
			((Graphic)textLabel[i]).set_color(Color.get_white());
			Text obj = textLabel[i];
			Color val7 = val2;
			colors = ((Selectable)this).get_colors();
			Color val8 = val7 * ((ColorBlock)(ref colors)).get_colorMultiplier();
			float num3;
			if (!instant)
			{
				colors = ((Selectable)this).get_colors();
				num3 = ((ColorBlock)(ref colors)).get_fadeDuration();
			}
			else
			{
				num3 = 0f;
			}
			((Graphic)obj).CrossFadeColor(val8, num3, true, true);
		}
	}

	public MenuButton()
		: this()
	{
	}
}
