using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public class PostProcessEffectBaseEditor
	{
		internal SerializedProperty baseProperty;

		internal SerializedProperty activeProperty;

		private SerializedProperty m_Enabled;

		private Editor m_Inspector;

		internal PostProcessEffectSettings target { get; private set; }

		internal SerializedObject serializedObject { get; private set; }

		internal PostProcessEffectBaseEditor()
		{
		}

		public void Repaint()
		{
			m_Inspector.Repaint();
		}

		internal void Init(PostProcessEffectSettings target, Editor inspector)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			this.target = target;
			m_Inspector = inspector;
			serializedObject = new SerializedObject((Object)(object)target);
			m_Enabled = serializedObject.FindProperty("enabled.value");
			activeProperty = serializedObject.FindProperty("active");
			OnEnable();
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		internal void OnInternalInspectorGUI()
		{
			serializedObject.Update();
			TopRowFields();
			OnInspectorGUI();
			EditorGUILayout.Space();
			serializedObject.ApplyModifiedProperties();
		}

		public virtual void OnInspectorGUI()
		{
		}

		public virtual string GetDisplayTitle()
		{
			return ObjectNames.NicifyVariableName(((object)target).GetType().Name);
		}

		private void TopRowFields()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			HorizontalScope val = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				if (GUILayout.Button(EditorUtilities.GetContent("All|Toggle all overrides on. To maximize performances you should only toggle overrides that you actually need."), Styling.miniLabelButton, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(17f),
					GUILayout.ExpandWidth(false)
				}))
				{
					SetAllOverridesTo(state: true);
				}
				if (GUILayout.Button(EditorUtilities.GetContent("None|Toggle all overrides off."), Styling.miniLabelButton, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(32f),
					GUILayout.ExpandWidth(false)
				}))
				{
					SetAllOverridesTo(state: false);
				}
				GUILayout.FlexibleSpace();
				bool boolValue = m_Enabled.get_boolValue();
				boolValue = GUILayout.Toggle(boolValue, EditorUtilities.GetContent("On|Enable this effect."), EditorStyles.get_miniButtonLeft(), (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(35f),
					GUILayout.ExpandWidth(false)
				});
				boolValue = !GUILayout.Toggle(!boolValue, EditorUtilities.GetContent("Off|Disable this effect."), EditorStyles.get_miniButtonRight(), (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(35f),
					GUILayout.ExpandWidth(false)
				});
				m_Enabled.set_boolValue(boolValue);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private void SetAllOverridesTo(bool state)
		{
			Undo.RecordObject((Object)(object)target, "Toggle All");
			target.SetAllOverridesTo(state);
			serializedObject.Update();
		}

		protected void PropertyField(SerializedParameterOverride property)
		{
			GUIContent content = EditorUtilities.GetContent(property.displayName);
			PropertyField(property, content);
		}

		protected void PropertyField(SerializedParameterOverride property, GUIContent title)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Invalid comparison between Unknown and I4
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Invalid comparison between Unknown and I4
			DisplayNameAttribute attribute = property.GetAttribute<DisplayNameAttribute>();
			if (attribute != null)
			{
				title.set_text(attribute.displayName);
			}
			if (string.IsNullOrEmpty(title.get_tooltip()))
			{
				TooltipAttribute attribute2 = property.GetAttribute<TooltipAttribute>();
				if (attribute2 != null)
				{
					title.set_tooltip(attribute2.tooltip);
				}
			}
			AttributeDecorator attributeDecorator = null;
			Attribute attribute3 = null;
			Attribute[] attributes = property.attributes;
			foreach (Attribute attribute4 in attributes)
			{
				if (attributeDecorator == null)
				{
					attributeDecorator = EditorUtilities.GetDecorator(attribute4.GetType());
					attribute3 = attribute4;
				}
				if (attribute4 is PropertyAttribute)
				{
					if (attribute4 is SpaceAttribute)
					{
						EditorGUILayout.GetControlRect(false, ((SpaceAttribute)((attribute4 is SpaceAttribute) ? attribute4 : null)).height, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					}
					else if (attribute4 is HeaderAttribute)
					{
						Rect val = EditorGUILayout.GetControlRect(false, 24f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						((Rect)(ref val)).set_y(((Rect)(ref val)).get_y() + 8f);
						val = EditorGUI.IndentedRect(val);
						EditorGUI.LabelField(val, ((HeaderAttribute)((attribute4 is HeaderAttribute) ? attribute4 : null)).header, Styling.headerLabel);
					}
				}
			}
			bool flag = false;
			if (attributeDecorator != null && !attributeDecorator.IsAutoProperty())
			{
				if (attributeDecorator.OnGUI(property.value, property.overrideState, title, attribute3))
				{
					return;
				}
				flag = true;
			}
			HorizontalScope val2 = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				Rect rect = GUILayoutUtility.GetRect(17f, 17f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				((Rect)(ref rect)).set_yMin(((Rect)(ref rect)).get_yMin() + 4f);
				EditorUtilities.DrawOverrideCheckbox(rect, property.overrideState);
				DisabledScope val3 = new DisabledScope(!property.overrideState.get_boolValue());
				try
				{
					if (attributeDecorator == null || flag || !attributeDecorator.OnGUI(property.value, property.overrideState, title, attribute3))
					{
						if (property.value.get_hasVisibleChildren() && (int)property.value.get_propertyType() != 8 && (int)property.value.get_propertyType() != 9)
						{
							GUILayout.Space(12f);
							EditorGUILayout.PropertyField(property.value, title, true, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						}
						else
						{
							EditorGUILayout.PropertyField(property.value, title, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						}
					}
				}
				finally
				{
					((IDisposable)(DisabledScope)(ref val3)).Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}
}
