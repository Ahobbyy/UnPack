using System;
using UnityEditor;
using UnityEngine;

public static class Sliders
{
	private static GUIStyle style;

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
			return Mathf.Log(2f, fader * 2f) * 24f - 24f;
		}
		return -48f + fader * 48f;
	}

	public static float DrawHorizontalSlider(Rect rect, float from, float to, float value, SliderType type)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		if (style == null)
		{
			style = new GUIStyle(GUI.get_skin().GetStyle("label"));
			style.set_fontSize(10);
			style.get_normal().set_textColor(Color.get_white());
			style.set_alignment((TextAnchor)4);
		}
		GUI.set_color(new Color(0f, 0f, 0f, 0f));
		float num;
		switch (type)
		{
		case SliderType.Level:
			num = value;
			break;
		case SliderType.Volume:
			num = DBtoFader(AudioUtils.ValueToDB(value));
			break;
		case SliderType.Linear:
			num = value;
			break;
		case SliderType.Log2:
			num = Mathf.Log(value, 2f);
			from = Mathf.Log(from, 2f);
			to = Mathf.Log(to, 2f);
			break;
		case SliderType.Log10:
			num = Mathf.Log(value, 10f);
			from = Mathf.Log(from, 10f);
			to = Mathf.Log(to, 10f);
			break;
		default:
			throw new InvalidOperationException();
		}
		float num2 = GUI.HorizontalSlider(rect, num, from, to);
		GUI.set_color(Color.get_white());
		EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() + 4f, ((Rect)(ref rect)).get_y() + 4f, ((Rect)(ref rect)).get_width() - 6f, ((Rect)(ref rect)).get_height() - 8f), new Color(0.1f, 0.1f, 0.1f, 1f));
		EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() + 5f, ((Rect)(ref rect)).get_y() + 5f, (((Rect)(ref rect)).get_width() - 8f) * Mathf.InverseLerp(from, to, num2), ((Rect)(ref rect)).get_height() - 10f), new Color(0.25f, 0.5f, 0.25f, 1f));
		float num3;
		switch (type)
		{
		case SliderType.Level:
			num3 = num2;
			break;
		case SliderType.Volume:
			num3 = num2;
			num2 = ((num2 != num) ? AudioUtils.DBToValue(FaderToDB(num2)) : value);
			break;
		case SliderType.Linear:
			num3 = num2;
			break;
		case SliderType.Log2:
			num2 = ((num2 != num) ? Mathf.Pow(2f, num2) : value);
			num3 = num2;
			break;
		case SliderType.Log10:
			num2 = ((num2 != num) ? Mathf.Pow(10f, num2) : value);
			num3 = num2;
			break;
		default:
			throw new InvalidOperationException();
		}
		EditorGUI.LabelField(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), ((Rect)(ref rect)).get_height()), FormatNumber(num3), style);
		return num2;
	}
}
