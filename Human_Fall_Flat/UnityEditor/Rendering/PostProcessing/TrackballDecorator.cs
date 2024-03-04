using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[Decorator(typeof(TrackballAttribute))]
	public sealed class TrackballDecorator : AttributeDecorator
	{
		private static readonly int k_ThumbHash = "colorWheelThumb".GetHashCode();

		private static Material s_Material;

		private bool m_ResetState;

		private Vector2 m_CursorPos;

		public override bool IsAutoProperty()
		{
			return false;
		}

		public override bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if ((int)property.get_propertyType() != 10)
			{
				return false;
			}
			Vector4 value = property.get_vector4Value();
			VerticalScope val = new VerticalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				DisabledScope val2 = new DisabledScope(!overrideState.get_boolValue());
				try
				{
					DrawWheel(ref value, overrideState.get_boolValue(), (TrackballAttribute)attribute);
				}
				finally
				{
					((IDisposable)(DisabledScope)(ref val2)).Dispose();
				}
				DrawLabelAndOverride(title, overrideState);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (m_ResetState)
			{
				value = Vector4.get_zero();
				m_ResetState = false;
			}
			property.set_vector4Value(value);
			return true;
		}

		private void DrawWheel(ref Vector4 value, bool overrideState, TrackballAttribute attr)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Invalid comparison between Unknown and I4
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			Rect aspectRect = GUILayoutUtility.GetAspectRect(1f);
			float width = ((Rect)(ref aspectRect)).get_width();
			float num = width / 2f;
			float num2 = 0.38f * width;
			Vector3 val = default(Vector3);
			Color.RGBToHSV(Color.op_Implicit(value), ref val.x, ref val.y, ref val.z);
			float w = value.w;
			Vector2 val2 = Vector2.get_zero();
			float num3 = val.x * ((float)Math.PI * 2f);
			val2.x = Mathf.Cos(num3 + (float)Math.PI / 2f);
			val2.y = Mathf.Sin(num3 - (float)Math.PI / 2f);
			val2 *= val.y * num2;
			if ((int)Event.get_current().get_type() == 7)
			{
				float pixelsPerPoint = EditorGUIUtility.get_pixelsPerPoint();
				if ((Object)(object)s_Material == (Object)null)
				{
					Material val3 = new Material(Shader.Find("Hidden/PostProcessing/Editor/Trackball"));
					((Object)val3).set_hideFlags((HideFlags)61);
					s_Material = val3;
				}
				RenderTexture active = RenderTexture.get_active();
				RenderTexture temporary = RenderTexture.GetTemporary((int)(width * pixelsPerPoint), (int)(width * pixelsPerPoint), 0, (RenderTextureFormat)0, (RenderTextureReadWrite)1);
				s_Material.SetFloat("_Offset", w);
				s_Material.SetFloat("_DisabledState", overrideState ? 1f : 0.5f);
				s_Material.SetVector("_Resolution", Vector4.op_Implicit(new Vector2(width * pixelsPerPoint, width * pixelsPerPoint / 2f)));
				Graphics.Blit((Texture)null, temporary, s_Material, (!EditorGUIUtility.get_isProSkin()) ? 1 : 0);
				RenderTexture.set_active(active);
				GUI.DrawTexture(aspectRect, (Texture)(object)temporary);
				RenderTexture.ReleaseTemporary(temporary);
				Vector2 wheelThumbSize = Styling.wheelThumbSize;
				Vector2 val4 = wheelThumbSize / 2f;
				Styling.wheelThumb.Draw(new Rect(((Rect)(ref aspectRect)).get_x() + num + val2.x - val4.x, ((Rect)(ref aspectRect)).get_y() + num + val2.y - val4.y, wheelThumbSize.x, wheelThumbSize.y), false, false, false, false);
			}
			Rect bounds = aspectRect;
			((Rect)(ref bounds)).set_x(((Rect)(ref bounds)).get_x() + (num - num2));
			((Rect)(ref bounds)).set_y(((Rect)(ref bounds)).get_y() + (num - num2));
			float width2;
			((Rect)(ref bounds)).set_height(width2 = num2 * 2f);
			((Rect)(ref bounds)).set_width(width2);
			val = GetInput(bounds, val, val2, num2);
			value = Color.op_Implicit(Color.HSVToRGB(val.x, val.y, 1f));
			value.w = w;
			Rect rect = GUILayoutUtility.GetRect(1f, 17f);
			float num4 = ((Rect)(ref rect)).get_width() * 0.05f;
			((Rect)(ref rect)).set_xMin(((Rect)(ref rect)).get_xMin() + num4);
			((Rect)(ref rect)).set_xMax(((Rect)(ref rect)).get_xMax() - num4);
			value.w = GUI.HorizontalSlider(rect, value.w, -1f, 1f);
			if (attr.mode != 0)
			{
				Vector3 val5 = Vector3.get_zero();
				switch (attr.mode)
				{
				case TrackballAttribute.Mode.Lift:
					val5 = ColorUtilities.ColorToLift(value);
					break;
				case TrackballAttribute.Mode.Gamma:
					val5 = ColorUtilities.ColorToInverseGamma(value);
					break;
				case TrackballAttribute.Mode.Gain:
					val5 = ColorUtilities.ColorToGain(value);
					break;
				}
				DisabledGroupScope val6 = new DisabledGroupScope(true);
				try
				{
					Rect rect2 = GUILayoutUtility.GetRect(1f, 17f);
					((Rect)(ref rect2)).set_width(((Rect)(ref rect2)).get_width() / 3f);
					GUI.Label(rect2, val5.x.ToString("F2"), EditorStyles.get_centeredGreyMiniLabel());
					((Rect)(ref rect2)).set_x(((Rect)(ref rect2)).get_x() + ((Rect)(ref rect2)).get_width());
					GUI.Label(rect2, val5.y.ToString("F2"), EditorStyles.get_centeredGreyMiniLabel());
					((Rect)(ref rect2)).set_x(((Rect)(ref rect2)).get_x() + ((Rect)(ref rect2)).get_width());
					GUI.Label(rect2, val5.z.ToString("F2"), EditorStyles.get_centeredGreyMiniLabel());
					((Rect)(ref rect2)).set_x(((Rect)(ref rect2)).get_x() + ((Rect)(ref rect2)).get_width());
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
		}

		private void DrawLabelAndOverride(GUIContent title, SerializedProperty overrideState)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(1f, 17f);
			Vector2 val = Styling.wheelLabel.CalcSize(title);
			Rect val2 = default(Rect);
			((Rect)(ref val2))._002Ector(((Rect)(ref rect)).get_x() + ((Rect)(ref rect)).get_width() / 2f - val.x / 2f, ((Rect)(ref rect)).get_y(), val.x, val.y);
			GUI.Label(val2, title, Styling.wheelLabel);
			EditorUtilities.DrawOverrideCheckbox(new Rect(((Rect)(ref val2)).get_x() - 17f, ((Rect)(ref val2)).get_y() + 3f, 17f, 17f), overrideState);
		}

		private Vector3 GetInput(Rect bounds, Vector3 hsv, Vector2 thumbPos, float radius)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Invalid comparison between Unknown and I4
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Invalid comparison between Unknown and I4
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			int controlID = GUIUtility.GetControlID(k_ThumbHash, (FocusType)2, bounds);
			Vector2 mousePosition = current.get_mousePosition();
			if ((int)current.get_type() == 0 && GUIUtility.get_hotControl() == 0 && ((Rect)(ref bounds)).Contains(mousePosition))
			{
				if (current.get_button() == 0)
				{
					if (Vector2.Distance(new Vector2(((Rect)(ref bounds)).get_x() + radius, ((Rect)(ref bounds)).get_y() + radius), mousePosition) <= radius)
					{
						current.Use();
						m_CursorPos = new Vector2(thumbPos.x + radius, thumbPos.y + radius);
						GUIUtility.set_hotControl(controlID);
						GUI.set_changed(true);
					}
				}
				else if (current.get_button() == 1)
				{
					current.Use();
					GUI.set_changed(true);
					m_ResetState = true;
				}
			}
			else if ((int)current.get_type() == 3 && current.get_button() == 0 && GUIUtility.get_hotControl() == controlID)
			{
				current.Use();
				GUI.set_changed(true);
				m_CursorPos += current.get_delta() * GlobalSettings.trackballSensitivity;
				GetWheelHueSaturation(m_CursorPos.x, m_CursorPos.y, radius, out hsv.x, out hsv.y);
			}
			else if ((int)current.get_rawType() == 1 && current.get_button() == 0 && GUIUtility.get_hotControl() == controlID)
			{
				current.Use();
				GUIUtility.set_hotControl(0);
			}
			return hsv;
		}

		private void GetWheelHueSaturation(float x, float y, float radius, out float hue, out float saturation)
		{
			float num = (x - radius) / radius;
			float num2 = (y - radius) / radius;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			hue = Mathf.Atan2(num, 0f - num2);
			hue = 1f - ((hue > 0f) ? hue : ((float)Math.PI * 2f + hue)) / ((float)Math.PI * 2f);
			saturation = Mathf.Clamp01(num3);
		}
	}
}
