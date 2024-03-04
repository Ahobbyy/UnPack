using System;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	[Decorator(typeof(RangeAttribute))]
	public sealed class RangeDecorator : AttributeDecorator
	{
		public override bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			RangeAttribute val = (RangeAttribute)attribute;
			if ((int)property.get_propertyType() == 2)
			{
				property.set_floatValue(EditorGUILayout.Slider(title, property.get_floatValue(), val.min, val.max, (GUILayoutOption[])(object)new GUILayoutOption[0]));
				return true;
			}
			if ((int)property.get_propertyType() == 0)
			{
				property.set_intValue(EditorGUILayout.IntSlider(title, property.get_intValue(), (int)val.min, (int)val.max, (GUILayoutOption[])(object)new GUILayoutOption[0]));
				return true;
			}
			return false;
		}
	}
}
