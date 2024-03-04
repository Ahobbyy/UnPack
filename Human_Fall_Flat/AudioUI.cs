using System;
using UnityEngine;

public static class AudioUI
{
	private static readonly Texture2D backgroundTexture = Texture2D.get_whiteTexture();

	private static readonly GUIStyle textureStyle;

	public static GUIStyle style;

	public static GUIStyle buttonStyle;

	public static void DrawRect(Rect rect, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Color backgroundColor = GUI.get_backgroundColor();
		GUI.set_backgroundColor(color);
		GUI.Box(rect, GUIContent.none, textureStyle);
		GUI.set_backgroundColor(backgroundColor);
	}

	public static string FormatNumber(float num, int len = 5)
	{
		if (float.IsInfinity(num))
		{
			return "-Inf";
		}
		return num.ToString("0.0");
	}

	private static float DBtoFader(float db)
	{
		if (db < -24f)
		{
			return Mathf.Pow(2f, (db + 24f) / 24f) / 2f;
		}
		return (db + 48f) / 48f;
	}

	private static float FaderToDB(float fader)
	{
		if (fader < 0.5f)
		{
			return Mathf.Log(fader * 2f, 2f) * 24f - 24f;
		}
		return -48f + fader * 48f;
	}

	public static void EnsureStyle()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		if (style == null)
		{
			style = new GUIStyle(GUI.get_skin().GetStyle("label"));
			style.set_fontSize(10);
			style.get_normal().set_textColor(Color.get_white());
			style.set_alignment((TextAnchor)4);
		}
		if (buttonStyle == null)
		{
			buttonStyle = new GUIStyle(GUI.get_skin().GetStyle("button"));
			buttonStyle.set_fontSize(10);
		}
	}

	public static float DrawHorizontalSlider(Rect rect, float from, float to, float def, float value, AudioSliderType type)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		EnsureStyle();
		GUI.set_color(new Color(0f, 0f, 0f, 0f));
		bool flag = Event.get_current().get_button() == 1 && ((Rect)(ref rect)).Contains(Event.get_current().get_mousePosition());
		if (flag)
		{
			value = def;
		}
		float num;
		switch (type)
		{
		case AudioSliderType.Level:
			num = value;
			break;
		case AudioSliderType.Volume:
			num = DBtoFader(AudioUtils.ValueToDB(value));
			break;
		case AudioSliderType.Pitch:
			num = AudioUtils.RatioToCents(value);
			break;
		case AudioSliderType.Linear:
			num = value;
			break;
		case AudioSliderType.Log2:
			num = Mathf.Log(value, 2f);
			from = Mathf.Log(from, 2f);
			to = Mathf.Log(to, 2f);
			break;
		case AudioSliderType.Log10:
			num = Mathf.Log(value, 10f);
			from = Mathf.Log(from, 10f);
			to = Mathf.Log(to, 10f);
			break;
		default:
			throw new InvalidOperationException();
		}
		float num2 = ((!flag) ? GUI.HorizontalSlider(rect, num, from, to) : num);
		GUI.set_color(Color.get_white());
		DrawRect(new Rect(((Rect)(ref rect)).get_x() + 4f, ((Rect)(ref rect)).get_y() + 4f, ((Rect)(ref rect)).get_width() - 6f, ((Rect)(ref rect)).get_height() - 8f), new Color(0.1f, 0.1f, 0.1f, 1f));
		DrawRect(new Rect(((Rect)(ref rect)).get_x() + 5f, ((Rect)(ref rect)).get_y() + 5f, (((Rect)(ref rect)).get_width() - 8f) * Mathf.InverseLerp(from, to, num2), ((Rect)(ref rect)).get_height() - 10f), new Color(0.25f, 0.5f, 0.25f, 1f));
		float num3;
		switch (type)
		{
		case AudioSliderType.Level:
			num3 = num2;
			break;
		case AudioSliderType.Volume:
			num3 = FaderToDB(num2);
			num2 = ((num2 != num) ? AudioUtils.DBToValue(FaderToDB(num2)) : value);
			break;
		case AudioSliderType.Pitch:
			num3 = num2;
			num2 = ((num2 != num) ? AudioUtils.CentsToRatio(num2) : value);
			break;
		case AudioSliderType.Linear:
			num3 = num2;
			break;
		case AudioSliderType.Log2:
			num2 = ((num2 != num) ? Mathf.Pow(2f, num2) : value);
			num3 = num2;
			break;
		case AudioSliderType.Log10:
			num2 = ((num2 != num) ? Mathf.Pow(10f, num2) : value);
			num3 = num2;
			break;
		default:
			throw new InvalidOperationException();
		}
		GUI.Label(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), ((Rect)(ref rect)).get_height()), FormatNumber(num3), style);
		return num2;
	}

	static AudioUI()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002a: Expected O, but got Unknown
		GUIStyle val = new GUIStyle();
		GUIStyleState val2 = new GUIStyleState();
		val2.set_background(backgroundTexture);
		val.set_normal(val2);
		textureStyle = val;
	}
}
