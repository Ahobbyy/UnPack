using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	public static class TMP_DefaultControls
	{
		public struct Resources
		{
			public Sprite standard;

			public Sprite background;

			public Sprite inputField;

			public Sprite knob;

			public Sprite checkmark;

			public Sprite dropdown;

			public Sprite mask;
		}

		private const float kWidth = 160f;

		private const float kThickHeight = 30f;

		private const float kThinHeight = 20f;

		private static Vector2 s_ThickElementSize = new Vector2(160f, 30f);

		private static Vector2 s_ThinElementSize = new Vector2(160f, 20f);

		private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);

		private static Color s_TextColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

		private static GameObject CreateUIElementRoot(string name, Vector2 size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			GameObject val = new GameObject(name);
			val.AddComponent<RectTransform>().set_sizeDelta(size);
			return val;
		}

		private static GameObject CreateUIObject(string name, GameObject parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0015: Expected O, but got Unknown
			GameObject val = new GameObject(name);
			val.AddComponent<RectTransform>();
			SetParentAndAlign(val, parent);
			return val;
		}

		private static void SetDefaultTextValues(TMP_Text lbl)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Graphic)lbl).set_color(s_TextColor);
			lbl.fontSize = 14f;
		}

		private static void SetDefaultColorTransitionValues(Selectable slider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			ColorBlock colors = slider.get_colors();
			((ColorBlock)(ref colors)).set_highlightedColor(new Color(0.882f, 0.882f, 0.882f));
			((ColorBlock)(ref colors)).set_pressedColor(new Color(0.698f, 0.698f, 0.698f));
			((ColorBlock)(ref colors)).set_disabledColor(new Color(0.521f, 0.521f, 0.521f));
		}

		private static void SetParentAndAlign(GameObject child, GameObject parent)
		{
			if (!((Object)(object)parent == (Object)null))
			{
				child.get_transform().SetParent(parent.get_transform(), false);
				SetLayerRecursively(child, parent.get_layer());
			}
		}

		private static void SetLayerRecursively(GameObject go, int layer)
		{
			go.set_layer(layer);
			Transform transform = go.get_transform();
			for (int i = 0; i < transform.get_childCount(); i++)
			{
				SetLayerRecursively(((Component)transform.GetChild(i)).get_gameObject(), layer);
			}
		}

		public static GameObject CreateScrollbar(Resources resources)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = CreateUIElementRoot("Scrollbar", s_ThinElementSize);
			GameObject val2 = CreateUIObject("Sliding Area", val);
			GameObject obj = CreateUIObject("Handle", val2);
			Image obj2 = val.AddComponent<Image>();
			obj2.set_sprite(resources.background);
			obj2.set_type((Type)1);
			((Graphic)obj2).set_color(s_DefaultSelectableColor);
			Image val3 = obj.AddComponent<Image>();
			val3.set_sprite(resources.standard);
			val3.set_type((Type)1);
			((Graphic)val3).set_color(s_DefaultSelectableColor);
			RectTransform component = val2.GetComponent<RectTransform>();
			component.set_sizeDelta(new Vector2(-20f, -20f));
			component.set_anchorMin(Vector2.get_zero());
			component.set_anchorMax(Vector2.get_one());
			RectTransform component2 = obj.GetComponent<RectTransform>();
			component2.set_sizeDelta(new Vector2(20f, 20f));
			Scrollbar obj3 = val.AddComponent<Scrollbar>();
			obj3.set_handleRect(component2);
			((Selectable)obj3).set_targetGraphic((Graphic)(object)val3);
			SetDefaultColorTransitionValues((Selectable)(object)obj3);
			return val;
		}

		public static GameObject CreateInputField(Resources resources)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = CreateUIElementRoot("TextMeshPro - InputField", s_ThickElementSize);
			GameObject val2 = CreateUIObject("Text Area", val);
			GameObject val3 = CreateUIObject("Placeholder", val2);
			GameObject obj = CreateUIObject("Text", val2);
			Image obj2 = val.AddComponent<Image>();
			obj2.set_sprite(resources.inputField);
			obj2.set_type((Type)1);
			((Graphic)obj2).set_color(s_DefaultSelectableColor);
			TMP_InputField tMP_InputField = val.AddComponent<TMP_InputField>();
			SetDefaultColorTransitionValues((Selectable)(object)tMP_InputField);
			val2.AddComponent<RectMask2D>();
			RectTransform component = val2.GetComponent<RectTransform>();
			component.set_anchorMin(Vector2.get_zero());
			component.set_anchorMax(Vector2.get_one());
			component.set_sizeDelta(Vector2.get_zero());
			component.set_offsetMin(new Vector2(10f, 6f));
			component.set_offsetMax(new Vector2(-10f, -7f));
			TextMeshProUGUI textMeshProUGUI = obj.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = "";
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.extraPadding = true;
			textMeshProUGUI.richText = true;
			SetDefaultTextValues(textMeshProUGUI);
			TextMeshProUGUI textMeshProUGUI2 = val3.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI2.text = "Enter text...";
			textMeshProUGUI2.fontSize = 14f;
			textMeshProUGUI2.fontStyle = FontStyles.Italic;
			textMeshProUGUI2.enableWordWrapping = false;
			textMeshProUGUI2.extraPadding = true;
			Color color = ((Graphic)textMeshProUGUI).get_color();
			color.a *= 0.5f;
			((Graphic)textMeshProUGUI2).set_color(color);
			RectTransform component2 = obj.GetComponent<RectTransform>();
			component2.set_anchorMin(Vector2.get_zero());
			component2.set_anchorMax(Vector2.get_one());
			component2.set_sizeDelta(Vector2.get_zero());
			component2.set_offsetMin(new Vector2(0f, 0f));
			component2.set_offsetMax(new Vector2(0f, 0f));
			RectTransform component3 = val3.GetComponent<RectTransform>();
			component3.set_anchorMin(Vector2.get_zero());
			component3.set_anchorMax(Vector2.get_one());
			component3.set_sizeDelta(Vector2.get_zero());
			component3.set_offsetMin(new Vector2(0f, 0f));
			component3.set_offsetMax(new Vector2(0f, 0f));
			tMP_InputField.textViewport = component;
			tMP_InputField.textComponent = textMeshProUGUI;
			tMP_InputField.placeholder = (Graphic)(object)textMeshProUGUI2;
			tMP_InputField.fontAsset = textMeshProUGUI.font;
			return val;
		}

		public static GameObject CreateDropdown(Resources resources)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Expected O, but got Unknown
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = CreateUIElementRoot("Dropdown", s_ThickElementSize);
			GameObject obj = CreateUIObject("Label", val);
			GameObject val2 = CreateUIObject("Arrow", val);
			GameObject val3 = CreateUIObject("Template", val);
			GameObject val4 = CreateUIObject("Viewport", val3);
			GameObject val5 = CreateUIObject("Content", val4);
			GameObject val6 = CreateUIObject("Item", val5);
			GameObject val7 = CreateUIObject("Item Background", val6);
			GameObject val8 = CreateUIObject("Item Checkmark", val6);
			GameObject val9 = CreateUIObject("Item Label", val6);
			GameObject obj2 = CreateScrollbar(resources);
			((Object)obj2).set_name("Scrollbar");
			SetParentAndAlign(obj2, val3);
			Scrollbar component = obj2.GetComponent<Scrollbar>();
			component.SetDirection((Direction)2, true);
			RectTransform component2 = obj2.GetComponent<RectTransform>();
			component2.set_anchorMin(Vector2.get_right());
			component2.set_anchorMax(Vector2.get_one());
			component2.set_pivot(Vector2.get_one());
			component2.set_sizeDelta(new Vector2(component2.get_sizeDelta().x, 0f));
			TextMeshProUGUI textMeshProUGUI = val9.AddComponent<TextMeshProUGUI>();
			SetDefaultTextValues(textMeshProUGUI);
			textMeshProUGUI.alignment = TextAlignmentOptions.Left;
			Image val10 = val7.AddComponent<Image>();
			((Graphic)val10).set_color(Color32.op_Implicit(new Color32((byte)245, (byte)245, (byte)245, byte.MaxValue)));
			Image val11 = val8.AddComponent<Image>();
			val11.set_sprite(resources.checkmark);
			Toggle obj3 = val6.AddComponent<Toggle>();
			((Selectable)obj3).set_targetGraphic((Graphic)(object)val10);
			obj3.graphic = (Graphic)(object)val11;
			obj3.set_isOn(true);
			Image obj4 = val3.AddComponent<Image>();
			obj4.set_sprite(resources.standard);
			obj4.set_type((Type)1);
			ScrollRect obj5 = val3.AddComponent<ScrollRect>();
			obj5.set_content((RectTransform)val5.get_transform());
			obj5.set_viewport((RectTransform)val4.get_transform());
			obj5.set_horizontal(false);
			obj5.set_movementType((MovementType)2);
			obj5.set_verticalScrollbar(component);
			obj5.set_verticalScrollbarVisibility((ScrollbarVisibility)2);
			obj5.set_verticalScrollbarSpacing(-3f);
			val4.AddComponent<Mask>().set_showMaskGraphic(false);
			Image obj6 = val4.AddComponent<Image>();
			obj6.set_sprite(resources.mask);
			obj6.set_type((Type)1);
			TextMeshProUGUI textMeshProUGUI2 = obj.AddComponent<TextMeshProUGUI>();
			SetDefaultTextValues(textMeshProUGUI2);
			textMeshProUGUI2.alignment = TextAlignmentOptions.Left;
			val2.AddComponent<Image>().set_sprite(resources.dropdown);
			Image val12 = val.AddComponent<Image>();
			val12.set_sprite(resources.standard);
			((Graphic)val12).set_color(s_DefaultSelectableColor);
			val12.set_type((Type)1);
			TMP_Dropdown tMP_Dropdown = val.AddComponent<TMP_Dropdown>();
			((Selectable)tMP_Dropdown).set_targetGraphic((Graphic)(object)val12);
			SetDefaultColorTransitionValues((Selectable)(object)tMP_Dropdown);
			tMP_Dropdown.template = val3.GetComponent<RectTransform>();
			tMP_Dropdown.captionText = textMeshProUGUI2;
			tMP_Dropdown.itemText = textMeshProUGUI;
			textMeshProUGUI.text = "Option A";
			tMP_Dropdown.options.Add(new TMP_Dropdown.OptionData
			{
				text = "Option A"
			});
			tMP_Dropdown.options.Add(new TMP_Dropdown.OptionData
			{
				text = "Option B"
			});
			tMP_Dropdown.options.Add(new TMP_Dropdown.OptionData
			{
				text = "Option C"
			});
			tMP_Dropdown.RefreshShownValue();
			RectTransform component3 = obj.GetComponent<RectTransform>();
			component3.set_anchorMin(Vector2.get_zero());
			component3.set_anchorMax(Vector2.get_one());
			component3.set_offsetMin(new Vector2(10f, 6f));
			component3.set_offsetMax(new Vector2(-25f, -7f));
			RectTransform component4 = val2.GetComponent<RectTransform>();
			component4.set_anchorMin(new Vector2(1f, 0.5f));
			component4.set_anchorMax(new Vector2(1f, 0.5f));
			component4.set_sizeDelta(new Vector2(20f, 20f));
			component4.set_anchoredPosition(new Vector2(-15f, 0f));
			RectTransform component5 = val3.GetComponent<RectTransform>();
			component5.set_anchorMin(new Vector2(0f, 0f));
			component5.set_anchorMax(new Vector2(1f, 0f));
			component5.set_pivot(new Vector2(0.5f, 1f));
			component5.set_anchoredPosition(new Vector2(0f, 2f));
			component5.set_sizeDelta(new Vector2(0f, 150f));
			RectTransform component6 = val4.GetComponent<RectTransform>();
			component6.set_anchorMin(new Vector2(0f, 0f));
			component6.set_anchorMax(new Vector2(1f, 1f));
			component6.set_sizeDelta(new Vector2(-18f, 0f));
			component6.set_pivot(new Vector2(0f, 1f));
			RectTransform component7 = val5.GetComponent<RectTransform>();
			component7.set_anchorMin(new Vector2(0f, 1f));
			component7.set_anchorMax(new Vector2(1f, 1f));
			component7.set_pivot(new Vector2(0.5f, 1f));
			component7.set_anchoredPosition(new Vector2(0f, 0f));
			component7.set_sizeDelta(new Vector2(0f, 28f));
			RectTransform component8 = val6.GetComponent<RectTransform>();
			component8.set_anchorMin(new Vector2(0f, 0.5f));
			component8.set_anchorMax(new Vector2(1f, 0.5f));
			component8.set_sizeDelta(new Vector2(0f, 20f));
			RectTransform component9 = val7.GetComponent<RectTransform>();
			component9.set_anchorMin(Vector2.get_zero());
			component9.set_anchorMax(Vector2.get_one());
			component9.set_sizeDelta(Vector2.get_zero());
			RectTransform component10 = val8.GetComponent<RectTransform>();
			component10.set_anchorMin(new Vector2(0f, 0.5f));
			component10.set_anchorMax(new Vector2(0f, 0.5f));
			component10.set_sizeDelta(new Vector2(20f, 20f));
			component10.set_anchoredPosition(new Vector2(10f, 0f));
			RectTransform component11 = val9.GetComponent<RectTransform>();
			component11.set_anchorMin(Vector2.get_zero());
			component11.set_anchorMax(Vector2.get_one());
			component11.set_offsetMin(new Vector2(20f, 1f));
			component11.set_offsetMax(new Vector2(-10f, -2f));
			val3.SetActive(false);
			return val;
		}
	}
}
