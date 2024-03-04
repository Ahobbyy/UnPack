using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TMP Dropdown", 35)]
	[RequireComponent(typeof(RectTransform))]
	public class TMP_Dropdown : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
	{
		protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ICancelHandler
		{
			[SerializeField]
			private TMP_Text m_Text;

			[SerializeField]
			private Image m_Image;

			[SerializeField]
			private RectTransform m_RectTransform;

			[SerializeField]
			private Toggle m_Toggle;

			public TMP_Text text
			{
				get
				{
					return m_Text;
				}
				set
				{
					m_Text = value;
				}
			}

			public Image image
			{
				get
				{
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}

			public RectTransform rectTransform
			{
				get
				{
					return m_RectTransform;
				}
				set
				{
					m_RectTransform = value;
				}
			}

			public Toggle toggle
			{
				get
				{
					return m_Toggle;
				}
				set
				{
					m_Toggle = value;
				}
			}

			public virtual void OnPointerEnter(PointerEventData eventData)
			{
				EventSystem.get_current().SetSelectedGameObject(((Component)this).get_gameObject());
			}

			public virtual void OnCancel(BaseEventData eventData)
			{
				TMP_Dropdown componentInParent = ((Component)this).GetComponentInParent<TMP_Dropdown>();
				if (Object.op_Implicit((Object)(object)componentInParent))
				{
					componentInParent.Hide();
				}
			}

			public DropdownItem()
				: this()
			{
			}
		}

		[Serializable]
		public class OptionData
		{
			[SerializeField]
			private string m_Text;

			[SerializeField]
			private Sprite m_Image;

			public string text
			{
				get
				{
					return m_Text;
				}
				set
				{
					m_Text = value;
				}
			}

			public Sprite image
			{
				get
				{
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}

			public OptionData()
			{
			}

			public OptionData(string text)
			{
				this.text = text;
			}

			public OptionData(Sprite image)
			{
				this.image = image;
			}

			public OptionData(string text, Sprite image)
			{
				this.text = text;
				this.image = image;
			}
		}

		[Serializable]
		public class OptionDataList
		{
			[SerializeField]
			private List<OptionData> m_Options;

			public List<OptionData> options
			{
				get
				{
					return m_Options;
				}
				set
				{
					m_Options = value;
				}
			}

			public OptionDataList()
			{
				options = new List<OptionData>();
			}
		}

		[Serializable]
		public class DropdownEvent : UnityEvent<int>
		{
		}

		[SerializeField]
		private RectTransform m_Template;

		[SerializeField]
		private TMP_Text m_CaptionText;

		[SerializeField]
		private Image m_CaptionImage;

		[Space]
		[SerializeField]
		private TMP_Text m_ItemText;

		[SerializeField]
		private Image m_ItemImage;

		[Space]
		[SerializeField]
		private int m_Value;

		[Space]
		[SerializeField]
		private OptionDataList m_Options = new OptionDataList();

		[Space]
		[SerializeField]
		private DropdownEvent m_OnValueChanged = new DropdownEvent();

		private GameObject m_Dropdown;

		private GameObject m_Blocker;

		private List<DropdownItem> m_Items = new List<DropdownItem>();

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		private bool validTemplate;

		private static OptionData s_NoOptionData = new OptionData();

		public RectTransform template
		{
			get
			{
				return m_Template;
			}
			set
			{
				m_Template = value;
				RefreshShownValue();
			}
		}

		public TMP_Text captionText
		{
			get
			{
				return m_CaptionText;
			}
			set
			{
				m_CaptionText = value;
				RefreshShownValue();
			}
		}

		public Image captionImage
		{
			get
			{
				return m_CaptionImage;
			}
			set
			{
				m_CaptionImage = value;
				RefreshShownValue();
			}
		}

		public TMP_Text itemText
		{
			get
			{
				return m_ItemText;
			}
			set
			{
				m_ItemText = value;
				RefreshShownValue();
			}
		}

		public Image itemImage
		{
			get
			{
				return m_ItemImage;
			}
			set
			{
				m_ItemImage = value;
				RefreshShownValue();
			}
		}

		public List<OptionData> options
		{
			get
			{
				return m_Options.options;
			}
			set
			{
				m_Options.options = value;
				RefreshShownValue();
			}
		}

		public DropdownEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		public int value
		{
			get
			{
				return m_Value;
			}
			set
			{
				if (!Application.get_isPlaying() || (value != m_Value && options.Count != 0))
				{
					m_Value = Mathf.Clamp(value, 0, options.Count - 1);
					RefreshShownValue();
					((UnityEvent<int>)m_OnValueChanged).Invoke(m_Value);
				}
			}
		}

		public bool IsExpanded => (Object)(object)m_Dropdown != (Object)null;

		protected TMP_Dropdown()
			: this()
		{
		}

		protected override void Awake()
		{
			if (Application.get_isPlaying())
			{
				m_AlphaTweenRunner = new TweenRunner<FloatTween>();
				m_AlphaTweenRunner.Init((MonoBehaviour)(object)this);
				if (Object.op_Implicit((Object)(object)m_CaptionImage))
				{
					((Behaviour)m_CaptionImage).set_enabled((Object)(object)m_CaptionImage.get_sprite() != (Object)null);
				}
				if (Object.op_Implicit((Object)(object)m_Template))
				{
					((Component)m_Template).get_gameObject().SetActive(false);
				}
			}
		}

		protected override void OnValidate()
		{
			((Selectable)this).OnValidate();
			if (((UIBehaviour)this).IsActive())
			{
				RefreshShownValue();
			}
		}

		public void RefreshShownValue()
		{
			OptionData optionData = s_NoOptionData;
			if (options.Count > 0)
			{
				optionData = options[Mathf.Clamp(m_Value, 0, options.Count - 1)];
			}
			if (Object.op_Implicit((Object)(object)m_CaptionText))
			{
				if (optionData != null && optionData.text != null)
				{
					m_CaptionText.text = optionData.text;
				}
				else
				{
					m_CaptionText.text = "";
				}
			}
			if (Object.op_Implicit((Object)(object)m_CaptionImage))
			{
				if (optionData != null)
				{
					m_CaptionImage.set_sprite(optionData.image);
				}
				else
				{
					m_CaptionImage.set_sprite((Sprite)null);
				}
				((Behaviour)m_CaptionImage).set_enabled((Object)(object)m_CaptionImage.get_sprite() != (Object)null);
			}
		}

		public void AddOptions(List<OptionData> options)
		{
			this.options.AddRange(options);
			RefreshShownValue();
		}

		public void AddOptions(List<string> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			RefreshShownValue();
		}

		public void AddOptions(List<Sprite> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			RefreshShownValue();
		}

		public void ClearOptions()
		{
			options.Clear();
			RefreshShownValue();
		}

		private void SetupTemplate()
		{
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			validTemplate = false;
			if (!Object.op_Implicit((Object)(object)m_Template))
			{
				Debug.LogError((object)"The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", (Object)(object)this);
				return;
			}
			GameObject gameObject = ((Component)m_Template).get_gameObject();
			gameObject.SetActive(true);
			Toggle componentInChildren = ((Component)m_Template).GetComponentInChildren<Toggle>();
			validTemplate = true;
			if (!Object.op_Implicit((Object)(object)componentInChildren) || (Object)(object)((Component)componentInChildren).get_transform() == (Object)(object)template)
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", (Object)(object)template);
			}
			else if (!(((Component)componentInChildren).get_transform().get_parent() is RectTransform))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", (Object)(object)template);
			}
			else if ((Object)(object)itemText != (Object)null && !itemText.transform.IsChildOf(((Component)componentInChildren).get_transform()))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", (Object)(object)template);
			}
			else if ((Object)(object)itemImage != (Object)null && !((Component)itemImage).get_transform().IsChildOf(((Component)componentInChildren).get_transform()))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", (Object)(object)template);
			}
			if (!validTemplate)
			{
				gameObject.SetActive(false);
				return;
			}
			DropdownItem dropdownItem = ((Component)componentInChildren).get_gameObject().AddComponent<DropdownItem>();
			dropdownItem.text = m_ItemText;
			dropdownItem.image = m_ItemImage;
			dropdownItem.toggle = componentInChildren;
			dropdownItem.rectTransform = (RectTransform)((Component)componentInChildren).get_transform();
			Canvas orAddComponent = TMP_Dropdown.GetOrAddComponent<Canvas>(gameObject);
			orAddComponent.set_overrideSorting(true);
			orAddComponent.set_sortingOrder(30000);
			TMP_Dropdown.GetOrAddComponent<GraphicRaycaster>(gameObject);
			TMP_Dropdown.GetOrAddComponent<CanvasGroup>(gameObject);
			gameObject.SetActive(false);
			validTemplate = true;
		}

		private static T GetOrAddComponent<T>(GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if (!Object.op_Implicit((Object)(object)val))
			{
				val = go.AddComponent<T>();
			}
			return val;
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			Show();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			Show();
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			Hide();
		}

		public void Show()
		{
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			if (!((UIBehaviour)this).IsActive() || !((Selectable)this).IsInteractable() || (Object)(object)m_Dropdown != (Object)null)
			{
				return;
			}
			if (!validTemplate)
			{
				SetupTemplate();
				if (!validTemplate)
				{
					return;
				}
			}
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			((Component)this).get_gameObject().GetComponentsInParent<Canvas>(false, list);
			if (list.Count == 0)
			{
				return;
			}
			Canvas val = list[0];
			TMP_ListPool<Canvas>.Release(list);
			((Component)m_Template).get_gameObject().SetActive(true);
			m_Dropdown = CreateDropdownList(((Component)m_Template).get_gameObject());
			((Object)m_Dropdown).set_name("Dropdown List");
			m_Dropdown.SetActive(true);
			Transform transform = m_Dropdown.get_transform();
			RectTransform val2 = (RectTransform)(object)((transform is RectTransform) ? transform : null);
			((Transform)val2).SetParent(((Component)m_Template).get_transform().get_parent(), false);
			DropdownItem componentInChildren = m_Dropdown.GetComponentInChildren<DropdownItem>();
			Transform transform2 = ((Component)((Transform)componentInChildren.rectTransform).get_parent()).get_gameObject().get_transform();
			RectTransform val3 = (RectTransform)(object)((transform2 is RectTransform) ? transform2 : null);
			((Component)componentInChildren.rectTransform).get_gameObject().SetActive(true);
			Rect rect = val3.get_rect();
			Rect rect2 = componentInChildren.rectTransform.get_rect();
			Vector2 val4 = ((Rect)(ref rect2)).get_min() - ((Rect)(ref rect)).get_min() + Vector2.op_Implicit(((Transform)componentInChildren.rectTransform).get_localPosition());
			Vector2 val5 = ((Rect)(ref rect2)).get_max() - ((Rect)(ref rect)).get_max() + Vector2.op_Implicit(((Transform)componentInChildren.rectTransform).get_localPosition());
			Vector2 size = ((Rect)(ref rect2)).get_size();
			m_Items.Clear();
			Toggle val6 = null;
			for (int i = 0; i < options.Count; i++)
			{
				OptionData data = options[i];
				DropdownItem item = AddItem(data, value == i, componentInChildren, m_Items);
				if (!((Object)(object)item == (Object)null))
				{
					item.toggle.set_isOn(value == i);
					((UnityEvent<bool>)(object)item.toggle.onValueChanged).AddListener((UnityAction<bool>)delegate
					{
						OnSelectItem(item.toggle);
					});
					if (item.toggle.get_isOn())
					{
						((Selectable)item.toggle).Select();
					}
					if ((Object)(object)val6 != (Object)null)
					{
						Navigation navigation = ((Selectable)val6).get_navigation();
						Navigation navigation2 = ((Selectable)item.toggle).get_navigation();
						((Navigation)(ref navigation)).set_mode((Mode)4);
						((Navigation)(ref navigation2)).set_mode((Mode)4);
						((Navigation)(ref navigation)).set_selectOnDown((Selectable)(object)item.toggle);
						((Navigation)(ref navigation)).set_selectOnRight((Selectable)(object)item.toggle);
						((Navigation)(ref navigation2)).set_selectOnLeft((Selectable)(object)val6);
						((Navigation)(ref navigation2)).set_selectOnUp((Selectable)(object)val6);
						((Selectable)val6).set_navigation(navigation);
						((Selectable)item.toggle).set_navigation(navigation2);
					}
					val6 = item.toggle;
				}
			}
			Vector2 sizeDelta = val3.get_sizeDelta();
			sizeDelta.y = size.y * (float)m_Items.Count + val4.y - val5.y;
			val3.set_sizeDelta(sizeDelta);
			Rect rect3 = val2.get_rect();
			float height = ((Rect)(ref rect3)).get_height();
			rect3 = val3.get_rect();
			float num = height - ((Rect)(ref rect3)).get_height();
			if (num > 0f)
			{
				val2.set_sizeDelta(new Vector2(val2.get_sizeDelta().x, val2.get_sizeDelta().y - num));
			}
			Vector3[] array = (Vector3[])(object)new Vector3[4];
			val2.GetWorldCorners(array);
			Transform transform3 = ((Component)val).get_transform();
			RectTransform val7 = (RectTransform)(object)((transform3 is RectTransform) ? transform3 : null);
			Rect rect4 = val7.get_rect();
			for (int j = 0; j < 2; j++)
			{
				bool flag = false;
				int num2 = 0;
				while (num2 < 4)
				{
					Vector3 val8 = ((Transform)val7).InverseTransformPoint(array[num2]);
					float num3 = ((Vector3)(ref val8)).get_Item(j);
					Vector2 val9 = ((Rect)(ref rect4)).get_min();
					if (!(num3 < ((Vector2)(ref val9)).get_Item(j)))
					{
						float num4 = ((Vector3)(ref val8)).get_Item(j);
						val9 = ((Rect)(ref rect4)).get_max();
						if (!(num4 > ((Vector2)(ref val9)).get_Item(j)))
						{
							num2++;
							continue;
						}
					}
					flag = true;
					break;
				}
				if (flag)
				{
					RectTransformUtility.FlipLayoutOnAxis(val2, j, false, false);
				}
			}
			for (int k = 0; k < m_Items.Count; k++)
			{
				RectTransform rectTransform = m_Items[k].rectTransform;
				rectTransform.set_anchorMin(new Vector2(rectTransform.get_anchorMin().x, 0f));
				rectTransform.set_anchorMax(new Vector2(rectTransform.get_anchorMax().x, 0f));
				rectTransform.set_anchoredPosition(new Vector2(rectTransform.get_anchoredPosition().x, val4.y + size.y * (float)(m_Items.Count - 1 - k) + size.y * rectTransform.get_pivot().y));
				rectTransform.set_sizeDelta(new Vector2(rectTransform.get_sizeDelta().x, size.y));
			}
			AlphaFadeList(0.15f, 0f, 1f);
			((Component)m_Template).get_gameObject().SetActive(false);
			((Component)componentInChildren).get_gameObject().SetActive(false);
			m_Blocker = CreateBlocker(val);
		}

		protected virtual GameObject CreateBlocker(Canvas rootCanvas)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ad: Expected O, but got Unknown
			GameObject val = new GameObject("Blocker");
			RectTransform obj = val.AddComponent<RectTransform>();
			((Transform)obj).SetParent(((Component)rootCanvas).get_transform(), false);
			obj.set_anchorMin(Vector2.op_Implicit(Vector3.get_zero()));
			obj.set_anchorMax(Vector2.op_Implicit(Vector3.get_one()));
			obj.set_sizeDelta(Vector2.get_zero());
			Canvas obj2 = val.AddComponent<Canvas>();
			obj2.set_overrideSorting(true);
			Canvas component = m_Dropdown.GetComponent<Canvas>();
			obj2.set_sortingLayerID(component.get_sortingLayerID());
			obj2.set_sortingOrder(component.get_sortingOrder() - 1);
			val.AddComponent<GraphicRaycaster>();
			((Graphic)val.AddComponent<Image>()).set_color(Color.get_clear());
			((UnityEvent)val.AddComponent<Button>().get_onClick()).AddListener(new UnityAction(Hide));
			return val;
		}

		protected virtual void DestroyBlocker(GameObject blocker)
		{
			Object.Destroy((Object)(object)blocker);
		}

		protected virtual GameObject CreateDropdownList(GameObject template)
		{
			return Object.Instantiate<GameObject>(template);
		}

		protected virtual void DestroyDropdownList(GameObject dropdownList)
		{
			Object.Destroy((Object)(object)dropdownList);
		}

		protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			return Object.Instantiate<DropdownItem>(itemTemplate);
		}

		protected virtual void DestroyItem(DropdownItem item)
		{
		}

		private DropdownItem AddItem(OptionData data, bool selected, DropdownItem itemTemplate, List<DropdownItem> items)
		{
			DropdownItem dropdownItem = CreateItem(itemTemplate);
			((Transform)dropdownItem.rectTransform).SetParent(((Transform)itemTemplate.rectTransform).get_parent(), false);
			((Component)dropdownItem).get_gameObject().SetActive(true);
			((Object)((Component)dropdownItem).get_gameObject()).set_name("Item " + items.Count + ((data.text != null) ? (": " + data.text) : ""));
			if ((Object)(object)dropdownItem.toggle != (Object)null)
			{
				dropdownItem.toggle.set_isOn(false);
			}
			if (Object.op_Implicit((Object)(object)dropdownItem.text))
			{
				dropdownItem.text.text = data.text;
			}
			if (Object.op_Implicit((Object)(object)dropdownItem.image))
			{
				dropdownItem.image.set_sprite(data.image);
				((Behaviour)dropdownItem.image).set_enabled((Object)(object)dropdownItem.image.get_sprite() != (Object)null);
			}
			items.Add(dropdownItem);
			return dropdownItem;
		}

		private void AlphaFadeList(float duration, float alpha)
		{
			CanvasGroup component = m_Dropdown.GetComponent<CanvasGroup>();
			AlphaFadeList(duration, component.get_alpha(), alpha);
		}

		private void AlphaFadeList(float duration, float start, float end)
		{
			if (!end.Equals(start))
			{
				FloatTween floatTween = default(FloatTween);
				floatTween.duration = duration;
				floatTween.startValue = start;
				floatTween.targetValue = end;
				FloatTween info = floatTween;
				info.AddOnChangedCallback(SetAlpha);
				info.ignoreTimeScale = true;
				m_AlphaTweenRunner.StartTween(info);
			}
		}

		private void SetAlpha(float alpha)
		{
			if (Object.op_Implicit((Object)(object)m_Dropdown))
			{
				m_Dropdown.GetComponent<CanvasGroup>().set_alpha(alpha);
			}
		}

		public void Hide()
		{
			if ((Object)(object)m_Dropdown != (Object)null)
			{
				AlphaFadeList(0.15f, 0f);
				if (((UIBehaviour)this).IsActive())
				{
					((MonoBehaviour)this).StartCoroutine(DelayedDestroyDropdownList(0.15f));
				}
			}
			if ((Object)(object)m_Blocker != (Object)null)
			{
				DestroyBlocker(m_Blocker);
			}
			m_Blocker = null;
			((Selectable)this).Select();
		}

		private IEnumerator DelayedDestroyDropdownList(float delay)
		{
			yield return (object)new WaitForSecondsRealtime(delay);
			for (int i = 0; i < m_Items.Count; i++)
			{
				if ((Object)(object)m_Items[i] != (Object)null)
				{
					DestroyItem(m_Items[i]);
				}
				m_Items.Clear();
			}
			if ((Object)(object)m_Dropdown != (Object)null)
			{
				DestroyDropdownList(m_Dropdown);
			}
			m_Dropdown = null;
		}

		private void OnSelectItem(Toggle toggle)
		{
			if (!toggle.get_isOn())
			{
				toggle.set_isOn(true);
			}
			int num = -1;
			Transform transform = ((Component)toggle).get_transform();
			Transform parent = transform.get_parent();
			for (int i = 0; i < parent.get_childCount(); i++)
			{
				if ((Object)(object)parent.GetChild(i) == (Object)(object)transform)
				{
					num = i - 1;
					break;
				}
			}
			if (num >= 0)
			{
				value = num;
				Hide();
			}
		}
	}
}
