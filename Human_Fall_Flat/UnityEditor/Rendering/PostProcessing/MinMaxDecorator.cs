using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[Decorator(typeof(MinMaxAttribute))]
	public sealed class MinMaxDecorator : AttributeDecorator
	{
		public override bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Invalid comparison between Unknown and I4
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			MinMaxAttribute minMaxAttribute = (MinMaxAttribute)attribute;
			if ((int)property.get_propertyType() == 2)
			{
				float num = EditorGUILayout.FloatField(title, property.get_floatValue(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				property.set_floatValue(Mathf.Clamp(num, minMaxAttribute.min, minMaxAttribute.max));
				return true;
			}
			if ((int)property.get_propertyType() == 0)
			{
				int num2 = EditorGUILayout.IntField(title, property.get_intValue(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				property.set_intValue(Mathf.Clamp(num2, (int)minMaxAttribute.min, (int)minMaxAttribute.max));
				return true;
			}
			if ((int)property.get_propertyType() == 8)
			{
				Vector2 vector2Value = property.get_vector2Value();
				EditorGUILayout.MinMaxSlider(title, ref vector2Value.x, ref vector2Value.y, minMaxAttribute.min, minMaxAttribute.max, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				property.set_vector2Value(vector2Value);
				return true;
			}
			return false;
		}
	}
}
