using System;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	public abstract class AttributeDecorator
	{
		public virtual bool IsAutoProperty()
		{
			return true;
		}

		public abstract bool OnGUI(SerializedProperty property, SerializedProperty overrideState, GUIContent title, Attribute attribute);
	}
}
