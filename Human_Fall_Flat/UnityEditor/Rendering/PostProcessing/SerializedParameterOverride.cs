using System;
using System.Linq;

namespace UnityEditor.Rendering.PostProcessing
{
	public sealed class SerializedParameterOverride
	{
		internal SerializedProperty baseProperty;

		public SerializedProperty overrideState { get; private set; }

		public SerializedProperty value { get; private set; }

		public Attribute[] attributes { get; private set; }

		public string displayName => baseProperty.get_displayName();

		internal SerializedParameterOverride(SerializedProperty property, Attribute[] attributes)
		{
			baseProperty = property.Copy();
			SerializedProperty val = baseProperty.Copy();
			val.Next(true);
			overrideState = val.Copy();
			val.Next(false);
			value = val.Copy();
			this.attributes = attributes;
		}

		public T GetAttribute<T>() where T : Attribute
		{
			return (T)attributes.FirstOrDefault((Attribute x) => x is T);
		}
	}
}
