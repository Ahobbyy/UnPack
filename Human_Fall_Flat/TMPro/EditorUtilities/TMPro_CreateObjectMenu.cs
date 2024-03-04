using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro.EditorUtilities
{
	public static class TMPro_CreateObjectMenu
	{
		private const string kUILayerName = "UI";

		private const string kStandardSpritePath = "UI/Skin/UISprite.psd";

		private const string kBackgroundSpritePath = "UI/Skin/Background.psd";

		private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";

		private const string kKnobPath = "UI/Skin/Knob.psd";

		private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";

		private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";

		private const string kMaskPath = "UI/Skin/UIMask.psd";

		private static TMP_DefaultControls.Resources s_StandardResources;

		[MenuItem("GameObject/3D Object/TextMeshPro - Text", false, 30)]
		private static void CreateTextMeshProObjectPerform(MenuCommand command)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			GameObject val = new GameObject("TextMeshPro");
			TextMeshPro textMeshPro = val.AddComponent<TextMeshPro>();
			textMeshPro.text = "Sample text";
			textMeshPro.alignment = TextAlignmentOptions.TopLeft;
			Undo.RegisterCreatedObjectUndo((Object)(object)val, "Create " + ((Object)val).get_name());
			Object context = command.context;
			GameObject val2 = (GameObject)(object)((context is GameObject) ? context : null);
			if ((Object)(object)val2 != (Object)null)
			{
				GameObjectUtility.SetParentAndAlign(val, val2);
				Undo.SetTransformParent(val.get_transform(), val2.get_transform(), "Parent " + ((Object)val).get_name());
			}
			Selection.set_activeGameObject(val);
		}

		[MenuItem("GameObject/UI/TextMeshPro - Text", false, 2001)]
		private static void CreateTextMeshProGuiObjectPerform(MenuCommand command)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			Canvas val = Object.FindObjectOfType<Canvas>();
			if ((Object)(object)val == (Object)null)
			{
				GameObject val2 = new GameObject("Canvas");
				val = val2.AddComponent<Canvas>();
				val.set_renderMode((RenderMode)0);
				((Component)val).get_gameObject().AddComponent<GraphicRaycaster>();
				Undo.RegisterCreatedObjectUndo((Object)(object)val2, "Create " + ((Object)val2).get_name());
			}
			GameObject val3 = new GameObject("TextMeshPro Text");
			RectTransform val4 = val3.AddComponent<RectTransform>();
			Undo.RegisterCreatedObjectUndo((Object)(object)val3, "Create " + ((Object)val3).get_name());
			Object context = command.context;
			GameObject val5 = (GameObject)(object)((context is GameObject) ? context : null);
			if ((Object)(object)val5 == (Object)null)
			{
				GameObjectUtility.SetParentAndAlign(val3, ((Component)val).get_gameObject());
				TextMeshProUGUI textMeshProUGUI = val3.AddComponent<TextMeshProUGUI>();
				textMeshProUGUI.text = "New Text";
				textMeshProUGUI.alignment = TextAlignmentOptions.TopLeft;
			}
			else if ((Object)(object)val5.GetComponent<Button>() != (Object)null)
			{
				val4.set_sizeDelta(Vector2.get_zero());
				val4.set_anchorMin(Vector2.get_zero());
				val4.set_anchorMax(Vector2.get_one());
				GameObjectUtility.SetParentAndAlign(val3, val5);
				TextMeshProUGUI textMeshProUGUI2 = val3.AddComponent<TextMeshProUGUI>();
				textMeshProUGUI2.text = "Button";
				textMeshProUGUI2.fontSize = 24f;
				textMeshProUGUI2.alignment = TextAlignmentOptions.Center;
			}
			else
			{
				GameObjectUtility.SetParentAndAlign(val3, val5);
				TextMeshProUGUI textMeshProUGUI3 = val3.AddComponent<TextMeshProUGUI>();
				textMeshProUGUI3.text = "New Text";
				textMeshProUGUI3.alignment = TextAlignmentOptions.TopLeft;
			}
			if (!Object.op_Implicit((Object)(object)Object.FindObjectOfType<EventSystem>()))
			{
				GameObject val6 = new GameObject("EventSystem", new Type[1] { typeof(EventSystem) });
				val6.AddComponent<StandaloneInputModule>();
				Undo.RegisterCreatedObjectUndo((Object)(object)val6, "Create " + ((Object)val6).get_name());
			}
			Selection.set_activeGameObject(val3);
		}

		[MenuItem("GameObject/UI/TextMeshPro - Input Field", false, 2037)]
		private static void AddTextMeshProInputField(MenuCommand menuCommand)
		{
			PlaceUIElementRoot(TMP_DefaultControls.CreateInputField(GetStandardResources()), menuCommand);
		}

		[MenuItem("GameObject/UI/TextMeshPro - Dropdown", false, 2036)]
		public static void AddDropdown(MenuCommand menuCommand)
		{
			PlaceUIElementRoot(TMP_DefaultControls.CreateDropdown(GetStandardResources()), menuCommand);
		}

		private static TMP_DefaultControls.Resources GetStandardResources()
		{
			if ((Object)(object)s_StandardResources.standard == (Object)null)
			{
				s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
				s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
				s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
				s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
				s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
				s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");
				s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
			}
			return s_StandardResources;
		}

		private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			SceneView val = SceneView.get_lastActiveSceneView();
			if ((Object)(object)val == (Object)null && SceneView.get_sceneViews().Count > 0)
			{
				object obj = SceneView.get_sceneViews()[0];
				val = (SceneView)((obj is SceneView) ? obj : null);
			}
			if (!((Object)(object)val == (Object)null) && !((Object)(object)val.get_camera() == (Object)null))
			{
				Camera camera = val.get_camera();
				Vector3 zero = Vector3.get_zero();
				Vector2 val2 = default(Vector2);
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2((float)(camera.get_pixelWidth() / 2), (float)(camera.get_pixelHeight() / 2)), camera, ref val2))
				{
					val2.x += canvasRTransform.get_sizeDelta().x * canvasRTransform.get_pivot().x;
					val2.y += canvasRTransform.get_sizeDelta().y * canvasRTransform.get_pivot().y;
					val2.x = Mathf.Clamp(val2.x, 0f, canvasRTransform.get_sizeDelta().x);
					val2.y = Mathf.Clamp(val2.y, 0f, canvasRTransform.get_sizeDelta().y);
					zero.x = val2.x - canvasRTransform.get_sizeDelta().x * itemTransform.get_anchorMin().x;
					zero.y = val2.y - canvasRTransform.get_sizeDelta().y * itemTransform.get_anchorMin().y;
					Vector3 val3 = default(Vector3);
					val3.x = canvasRTransform.get_sizeDelta().x * (0f - canvasRTransform.get_pivot().x) + itemTransform.get_sizeDelta().x * itemTransform.get_pivot().x;
					val3.y = canvasRTransform.get_sizeDelta().y * (0f - canvasRTransform.get_pivot().y) + itemTransform.get_sizeDelta().y * itemTransform.get_pivot().y;
					Vector3 val4 = default(Vector3);
					val4.x = canvasRTransform.get_sizeDelta().x * (1f - canvasRTransform.get_pivot().x) - itemTransform.get_sizeDelta().x * itemTransform.get_pivot().x;
					val4.y = canvasRTransform.get_sizeDelta().y * (1f - canvasRTransform.get_pivot().y) - itemTransform.get_sizeDelta().y * itemTransform.get_pivot().y;
					zero.x = Mathf.Clamp(zero.x, val3.x, val4.x);
					zero.y = Mathf.Clamp(zero.y, val3.y, val4.y);
				}
				itemTransform.set_anchoredPosition(Vector2.op_Implicit(zero));
				((Transform)itemTransform).set_localRotation(Quaternion.get_identity());
				((Transform)itemTransform).set_localScale(Vector3.get_one());
			}
		}

		private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
		{
			Object context = menuCommand.context;
			GameObject val = (GameObject)(object)((context is GameObject) ? context : null);
			if ((Object)(object)val == (Object)null || (Object)(object)val.GetComponentInParent<Canvas>() == (Object)null)
			{
				val = GetOrCreateCanvasGameObject();
			}
			string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling(val.get_transform(), ((Object)element).get_name());
			((Object)element).set_name(uniqueNameForSibling);
			Undo.RegisterCreatedObjectUndo((Object)(object)element, "Create " + ((Object)element).get_name());
			Undo.SetTransformParent(element.get_transform(), val.get_transform(), "Parent " + ((Object)element).get_name());
			GameObjectUtility.SetParentAndAlign(element, val);
			if ((Object)(object)val != menuCommand.context)
			{
				SetPositionVisibleinSceneView(val.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
			}
			Selection.set_activeGameObject(element);
		}

		public static GameObject CreateNewUI()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			GameObject val = new GameObject("Canvas");
			val.set_layer(LayerMask.NameToLayer("UI"));
			val.AddComponent<Canvas>().set_renderMode((RenderMode)0);
			val.AddComponent<CanvasScaler>();
			val.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo((Object)(object)val, "Create " + ((Object)val).get_name());
			CreateEventSystem(select: false);
			return val;
		}

		private static void CreateEventSystem(bool select)
		{
			CreateEventSystem(select, null);
		}

		private static void CreateEventSystem(bool select, GameObject parent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			EventSystem val = Object.FindObjectOfType<EventSystem>();
			if ((Object)(object)val == (Object)null)
			{
				GameObject val2 = new GameObject("EventSystem");
				GameObjectUtility.SetParentAndAlign(val2, parent);
				val = val2.AddComponent<EventSystem>();
				val2.AddComponent<StandaloneInputModule>();
				Undo.RegisterCreatedObjectUndo((Object)(object)val2, "Create " + ((Object)val2).get_name());
			}
			if (select && (Object)(object)val != (Object)null)
			{
				Selection.set_activeGameObject(((Component)val).get_gameObject());
			}
		}

		public static GameObject GetOrCreateCanvasGameObject()
		{
			GameObject activeGameObject = Selection.get_activeGameObject();
			Canvas val = (((Object)(object)activeGameObject != (Object)null) ? activeGameObject.GetComponentInParent<Canvas>() : null);
			if ((Object)(object)val != (Object)null && ((Component)val).get_gameObject().get_activeInHierarchy())
			{
				return ((Component)val).get_gameObject();
			}
			Object obj = Object.FindObjectOfType(typeof(Canvas));
			val = (Canvas)(object)((obj is Canvas) ? obj : null);
			if ((Object)(object)val != (Object)null && ((Component)val).get_gameObject().get_activeInHierarchy())
			{
				return ((Component)val).get_gameObject();
			}
			return CreateNewUI();
		}
	}
}
