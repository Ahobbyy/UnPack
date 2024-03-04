using System;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	[Decorator(typeof(ColorUsageAttribute))]
	public sealed class ColorUsageDecorator : AttributeDecorator
	{
		public override bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			ColorUsageAttribute val = (ColorUsageAttribute)attribute;
			if ((int)property.get_propertyType() != 4)
			{
				return false;
			}
			ColorPickerHDRConfig val2 = null;
			if (val.hdr)
			{
				val2 = new ColorPickerHDRConfig(val.minBrightness, val.maxBrightness, val.minExposureValue, val.maxExposureValue);
			}
			property.set_colorValue(EditorGUILayout.ColorField(title, property.get_colorValue(), true, val.showAlpha, val.hdr, val2, (GUILayoutOption[])(object)new GUILayoutOption[0]));
			return true;
		}
	}
}
