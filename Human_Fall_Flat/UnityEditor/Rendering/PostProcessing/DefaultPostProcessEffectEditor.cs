using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public class DefaultPostProcessEffectEditor : PostProcessEffectBaseEditor
	{
		private List<SerializedParameterOverride> m_Parameters;

		public override void OnEnable()
		{
			m_Parameters = new List<SerializedParameterOverride>();
			foreach (FieldInfo item2 in (from t in ((object)base.target).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where t.FieldType.IsSubclassOf(typeof(ParameterOverride)) && t.Name != "enabled"
				where (t.IsPublic && t.GetCustomAttributes(typeof(NonSerializedAttribute), inherit: false).Length == 0) || t.GetCustomAttributes(typeof(SerializeField), inherit: false).Length != 0
				select t).ToList())
			{
				SerializedProperty property = base.serializedObject.FindProperty(item2.Name);
				Attribute[] attributes = item2.GetCustomAttributes(inherit: false).Cast<Attribute>().ToArray();
				SerializedParameterOverride item = new SerializedParameterOverride(property, attributes);
				m_Parameters.Add(item);
			}
		}

		public override void OnInspectorGUI()
		{
			foreach (SerializedParameterOverride parameter in m_Parameters)
			{
				PropertyField(parameter);
			}
		}
	}
}
