using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[Decorator(typeof(MaxAttribute))]
	public sealed class MaxDecorator : AttributeDecorator
	{
		public override bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			MaxAttribute maxAttribute = (MaxAttribute)attribute;
			if ((int)property.get_propertyType() == 2)
			{
				float num = EditorGUILayout.FloatField(title, property.get_floatValue(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				property.set_floatValue(Mathf.Min(num, maxAttribute.max));
				return true;
			}
			if ((int)property.get_propertyType() == 0)
			{
				int num2 = EditorGUILayout.IntField(title, property.get_intValue(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				property.set_intValue(Mathf.Min(num2, (int)maxAttribute.max));
				return true;
			}
			return false;
		}
	}
}
