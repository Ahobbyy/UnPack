using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public static class EditorUtilities
	{
		private static Dictionary<string, GUIContent> s_GUIContentCache;

		private static Dictionary<Type, AttributeDecorator> s_AttributeDecorators;

		private static PostProcessEffectSettings s_ClipboardContent;

		public static bool isTargetingConsoles
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Invalid comparison between Unknown and I4
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				BuildTarget activeBuildTarget = EditorUserBuildSettings.get_activeBuildTarget();
				if ((int)activeBuildTarget != 31 && (int)activeBuildTarget != 33)
				{
					return (int)activeBuildTarget == 38;
				}
				return true;
			}
		}

		public static bool isTargetingMobiles
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Invalid comparison between Unknown and I4
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Invalid comparison between Unknown and I4
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Invalid comparison between Unknown and I4
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Invalid comparison between Unknown and I4
				BuildTarget activeBuildTarget = EditorUserBuildSettings.get_activeBuildTarget();
				if ((int)activeBuildTarget != 13 && (int)activeBuildTarget != 9 && (int)activeBuildTarget != 37 && (int)activeBuildTarget != 29 && (int)activeBuildTarget != 35)
				{
					return (int)activeBuildTarget == 30;
				}
				return true;
			}
		}

		public static bool isTargetingConsolesOrMobiles
		{
			get
			{
				if (!isTargetingConsoles)
				{
					return isTargetingMobiles;
				}
				return true;
			}
		}

		static EditorUtilities()
		{
			s_GUIContentCache = new Dictionary<string, GUIContent>();
			s_AttributeDecorators = new Dictionary<Type, AttributeDecorator>();
			ReloadDecoratorTypes();
		}

		[DidReloadScripts]
		private static void OnEditorReload()
		{
			ReloadDecoratorTypes();
		}

		private static void ReloadDecoratorTypes()
		{
			s_AttributeDecorators.Clear();
			foreach (Type item in from t in RuntimeUtilities.GetAllAssemblyTypes()
				where t.IsSubclassOf(typeof(AttributeDecorator)) && t.IsDefined(typeof(DecoratorAttribute), inherit: false) && !t.IsAbstract
				select t)
			{
				DecoratorAttribute attribute = item.GetAttribute<DecoratorAttribute>();
				AttributeDecorator value = (AttributeDecorator)Activator.CreateInstance(item);
				s_AttributeDecorators.Add(attribute.attributeType, value);
			}
		}

		internal static AttributeDecorator GetDecorator(Type attributeType)
		{
			if (s_AttributeDecorators.TryGetValue(attributeType, out var value))
			{
				return value;
			}
			return null;
		}

		public static GUIContent GetContent(string textAndTooltip)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			if (string.IsNullOrEmpty(textAndTooltip))
			{
				return GUIContent.none;
			}
			if (!s_GUIContentCache.TryGetValue(textAndTooltip, out var value))
			{
				string[] array = textAndTooltip.Split('|');
				value = new GUIContent(array[0]);
				if (array.Length > 1 && !string.IsNullOrEmpty(array[1]))
				{
					value.set_tooltip(array[1]);
				}
				s_GUIContentCache.Add(textAndTooltip, value);
			}
			return value;
		}

		public static void DrawFixMeBox(string text, Action action)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			Assert.IsNotNull<Action>(action);
			EditorGUILayout.HelpBox(text, (MessageType)2);
			GUILayout.Space(-32f);
			HorizontalScope val = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Fix", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(60f) }))
				{
					action();
				}
				GUILayout.Space(8f);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			GUILayout.Space(11f);
		}

		public static void DrawSplitter()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Invalid comparison between Unknown and I4
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(1f, 1f);
			((Rect)(ref rect)).set_xMin(0f);
			((Rect)(ref rect)).set_width(((Rect)(ref rect)).get_width() + 4f);
			if ((int)Event.get_current().get_type() == 7)
			{
				EditorGUI.DrawRect(rect, Styling.splitter);
			}
		}

		public static void DrawOverrideCheckbox(Rect rect, SerializedProperty property)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			property.set_boolValue(GUI.Toggle(rect, property.get_boolValue(), GetContent("|Override this setting for this volume."), Styling.smallTickbox));
		}

		public static void DrawHeaderLabel(string title)
		{
			EditorGUILayout.LabelField(title, Styling.headerLabel, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}

		public static bool DrawHeader(string title, bool state)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(1f, 17f);
			Rect val = rect;
			((Rect)(ref val)).set_xMin(((Rect)(ref val)).get_xMin() + 16f);
			((Rect)(ref val)).set_xMax(((Rect)(ref val)).get_xMax() - 20f);
			Rect val2 = rect;
			((Rect)(ref val2)).set_y(((Rect)(ref val2)).get_y() + 1f);
			((Rect)(ref val2)).set_width(13f);
			((Rect)(ref val2)).set_height(13f);
			((Rect)(ref rect)).set_xMin(0f);
			((Rect)(ref rect)).set_width(((Rect)(ref rect)).get_width() + 4f);
			EditorGUI.DrawRect(rect, Styling.headerBackground);
			EditorGUI.LabelField(val, GetContent(title), EditorStyles.get_boldLabel());
			state = GUI.Toggle(val2, state, GUIContent.none, EditorStyles.get_foldout());
			Event current = Event.get_current();
			if ((int)current.get_type() == 0 && ((Rect)(ref rect)).Contains(current.get_mousePosition()) && current.get_button() == 0)
			{
				state = !state;
				current.Use();
			}
			return state;
		}

		public static bool DrawHeader(string title, SerializedProperty group, SerializedProperty activeField, PostProcessEffectSettings target, Action resetAction, Action removeAction)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			Assert.IsNotNull<SerializedProperty>(group);
			Assert.IsNotNull<SerializedProperty>(activeField);
			Assert.IsNotNull<PostProcessEffectSettings>(target);
			Rect rect = GUILayoutUtility.GetRect(1f, 17f);
			Rect val = rect;
			((Rect)(ref val)).set_xMin(((Rect)(ref val)).get_xMin() + 32f);
			((Rect)(ref val)).set_xMax(((Rect)(ref val)).get_xMax() - 20f);
			Rect val2 = rect;
			((Rect)(ref val2)).set_y(((Rect)(ref val2)).get_y() + 1f);
			((Rect)(ref val2)).set_width(13f);
			((Rect)(ref val2)).set_height(13f);
			Rect val3 = rect;
			((Rect)(ref val3)).set_x(((Rect)(ref val3)).get_x() + 16f);
			((Rect)(ref val3)).set_y(((Rect)(ref val3)).get_y() + 2f);
			((Rect)(ref val3)).set_width(13f);
			((Rect)(ref val3)).set_height(13f);
			Texture2D paneOptionsIcon = Styling.paneOptionsIcon;
			Rect val4 = default(Rect);
			((Rect)(ref val4))._002Ector(((Rect)(ref val)).get_xMax() + 4f, ((Rect)(ref val)).get_y() + 4f, (float)((Texture)paneOptionsIcon).get_width(), (float)((Texture)paneOptionsIcon).get_height());
			((Rect)(ref rect)).set_xMin(0f);
			((Rect)(ref rect)).set_width(((Rect)(ref rect)).get_width() + 4f);
			EditorGUI.DrawRect(rect, Styling.headerBackground);
			DisabledScope val5 = default(DisabledScope);
			((DisabledScope)(ref val5))._002Ector(!activeField.get_boolValue());
			try
			{
				EditorGUI.LabelField(val, GetContent(title), EditorStyles.get_boldLabel());
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val5)).Dispose();
			}
			group.get_serializedObject().Update();
			group.set_isExpanded(GUI.Toggle(val2, group.get_isExpanded(), GUIContent.none, EditorStyles.get_foldout()));
			group.get_serializedObject().ApplyModifiedProperties();
			activeField.get_serializedObject().Update();
			activeField.set_boolValue(GUI.Toggle(val3, activeField.get_boolValue(), GUIContent.none, Styling.smallTickbox));
			activeField.get_serializedObject().ApplyModifiedProperties();
			GUI.DrawTexture(val4, (Texture)(object)paneOptionsIcon);
			Event current = Event.get_current();
			if ((int)current.get_type() == 0)
			{
				if (((Rect)(ref val4)).Contains(current.get_mousePosition()))
				{
					ShowHeaderContextMenu(new Vector2(((Rect)(ref val4)).get_x(), ((Rect)(ref val4)).get_yMax()), target, resetAction, removeAction);
					current.Use();
				}
				else if (((Rect)(ref val)).Contains(current.get_mousePosition()))
				{
					if (current.get_button() == 0)
					{
						group.set_isExpanded(!group.get_isExpanded());
					}
					else
					{
						ShowHeaderContextMenu(current.get_mousePosition(), target, resetAction, removeAction);
					}
					current.Use();
				}
			}
			return group.get_isExpanded();
		}

		private static void ShowHeaderContextMenu(Vector2 position, PostProcessEffectSettings target, Action resetAction, Action removeAction)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			Assert.IsNotNull<Action>(resetAction);
			Assert.IsNotNull<Action>(removeAction);
			GenericMenu val = new GenericMenu();
			val.AddItem(GetContent("Reset"), false, (MenuFunction)delegate
			{
				resetAction();
			});
			val.AddItem(GetContent("Remove"), false, (MenuFunction)delegate
			{
				removeAction();
			});
			val.AddSeparator(string.Empty);
			val.AddItem(GetContent("Copy Settings"), false, (MenuFunction)delegate
			{
				CopySettings(target);
			});
			if (CanPaste(target))
			{
				val.AddItem(GetContent("Paste Settings"), false, (MenuFunction)delegate
				{
					PasteSettings(target);
				});
			}
			else
			{
				val.AddDisabledItem(GetContent("Paste Settings"));
			}
			val.DropDown(new Rect(position, Vector2.get_zero()));
		}

		private static void CopySettings(PostProcessEffectSettings target)
		{
			Assert.IsNotNull<PostProcessEffectSettings>(target);
			if ((Object)(object)s_ClipboardContent != (Object)null)
			{
				RuntimeUtilities.Destroy((Object)(object)s_ClipboardContent);
				s_ClipboardContent = null;
			}
			s_ClipboardContent = (PostProcessEffectSettings)(object)ScriptableObject.CreateInstance(((object)target).GetType());
			EditorUtility.CopySerializedIfDifferent((Object)(object)target, (Object)(object)s_ClipboardContent);
		}

		private static void PasteSettings(PostProcessEffectSettings target)
		{
			Assert.IsNotNull<PostProcessEffectSettings>(target);
			Assert.IsNotNull<PostProcessEffectSettings>(s_ClipboardContent);
			Assert.AreEqual<Type>(((object)s_ClipboardContent).GetType(), ((object)target).GetType());
			Undo.RecordObject((Object)(object)target, "Paste Settings");
			EditorUtility.CopySerializedIfDifferent((Object)(object)s_ClipboardContent, (Object)(object)target);
		}

		private static bool CanPaste(PostProcessEffectSettings target)
		{
			if ((Object)(object)s_ClipboardContent != (Object)null)
			{
				return ((object)s_ClipboardContent).GetType() == ((object)target).GetType();
			}
			return false;
		}
	}
}
