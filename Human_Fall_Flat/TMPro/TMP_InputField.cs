using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TextMeshPro - Input Field", 11)]
	public class TMP_InputField : Selectable, IUpdateSelectedHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, IScrollHandler
	{
		public enum ContentType
		{
			Standard,
			Autocorrected,
			IntegerNumber,
			DecimalNumber,
			Alphanumeric,
			Name,
			EmailAddress,
			Password,
			Pin,
			Custom
		}

		public enum InputType
		{
			Standard,
			AutoCorrect,
			Password
		}

		public enum CharacterValidation
		{
			None,
			Digit,
			Integer,
			Decimal,
			Alphanumeric,
			Name,
			Regex,
			EmailAddress,
			CustomValidator
		}

		public enum LineType
		{
			SingleLine,
			MultiLineSubmit,
			MultiLineNewline
		}

		public delegate char OnValidateInput(string text, int charIndex, char addedChar);

		[Serializable]
		public class SubmitEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class OnChangeEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class SelectionEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class TextSelectionEvent : UnityEvent<string, int, int>
		{
		}

		protected enum EditState
		{
			Continue,
			Finish
		}

		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[6] { ' ', '.', ',', '\t', '\r', '\n' };

		[SerializeField]
		protected RectTransform m_TextViewport;

		[SerializeField]
		protected TMP_Text m_TextComponent;

		protected RectTransform m_TextComponentRectTransform;

		[SerializeField]
		protected Graphic m_Placeholder;

		[SerializeField]
		protected Scrollbar m_VerticalScrollbar;

		[SerializeField]
		protected TMP_ScrollbarEventHandler m_VerticalScrollbarEventHandler;

		private float m_ScrollPosition;

		[SerializeField]
		protected float m_ScrollSensitivity = 1f;

		[SerializeField]
		private ContentType m_ContentType;

		[SerializeField]
		private InputType m_InputType;

		[SerializeField]
		private char m_AsteriskChar = '*';

		[SerializeField]
		private TouchScreenKeyboardType m_KeyboardType;

		[SerializeField]
		private LineType m_LineType;

		[SerializeField]
		private bool m_HideMobileInput;

		[SerializeField]
		private CharacterValidation m_CharacterValidation;

		[SerializeField]
		private string m_RegexValue = string.Empty;

		[SerializeField]
		private float m_GlobalPointSize = 14f;

		[SerializeField]
		private int m_CharacterLimit;

		[SerializeField]
		private SubmitEvent m_OnEndEdit = new SubmitEvent();

		[SerializeField]
		private SubmitEvent m_OnSubmit = new SubmitEvent();

		[SerializeField]
		private SelectionEvent m_OnSelect = new SelectionEvent();

		[SerializeField]
		private SelectionEvent m_OnDeselect = new SelectionEvent();

		[SerializeField]
		private TextSelectionEvent m_OnTextSelection = new TextSelectionEvent();

		[SerializeField]
		private TextSelectionEvent m_OnEndTextSelection = new TextSelectionEvent();

		[SerializeField]
		private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

		[SerializeField]
		private OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor;

		[SerializeField]
		private Color m_SelectionColor = new Color(56f / 85f, 206f / 255f, 1f, 64f / 85f);

		[SerializeField]
		protected string m_Text = string.Empty;

		[SerializeField]
		[Range(0f, 4f)]
		private float m_CaretBlinkRate = 0.85f;

		[SerializeField]
		[Range(1f, 5f)]
		private int m_CaretWidth = 1;

		[SerializeField]
		private bool m_ReadOnly;

		[SerializeField]
		private bool m_RichText = true;

		protected int m_StringPosition;

		protected int m_StringSelectPosition;

		protected int m_CaretPosition;

		protected int m_CaretSelectPosition;

		private RectTransform caretRectTrans;

		protected UIVertex[] m_CursorVerts;

		private CanvasRenderer m_CachedInputRenderer;

		private Vector2 m_DefaultTransformPosition;

		private Vector2 m_LastPosition;

		[NonSerialized]
		protected Mesh m_Mesh;

		private bool m_AllowInput;

		private bool m_ShouldActivateNextUpdate;

		private bool m_UpdateDrag;

		private bool m_DragPositionOutOfBounds;

		private const float kHScrollSpeed = 0.05f;

		private const float kVScrollSpeed = 0.1f;

		protected bool m_CaretVisible;

		private Coroutine m_BlinkCoroutine;

		private float m_BlinkStartTime;

		private Coroutine m_DragCoroutine;

		private string m_OriginalText = "";

		private bool m_WasCanceled;

		private bool m_HasDoneFocusTransition;

		private bool m_IsScrollbarUpdateRequired;

		private bool m_IsUpdatingScrollbarValues;

		private bool m_isLastKeyBackspace;

		private float m_ClickStartTime;

		private float m_DoubleClickDelay = 0.5f;

		private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

		[SerializeField]
		protected TMP_FontAsset m_GlobalFontAsset;

		[SerializeField]
		protected bool m_OnFocusSelectAll = true;

		protected bool m_isSelectAll;

		[SerializeField]
		protected bool m_ResetOnDeActivation = true;

		[SerializeField]
		private bool m_RestoreOriginalTextOnEscape = true;

		[SerializeField]
		protected bool m_isRichTextEditingAllowed = true;

		[SerializeField]
		protected TMP_InputValidator m_InputValidator;

		private bool m_isSelected;

		private bool isStringPositionDirty;

		private bool m_forceRectTransformAdjustment;

		private Event m_ProcessingEvent = new Event();

		protected Mesh mesh
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				if ((Object)(object)m_Mesh == (Object)null)
				{
					m_Mesh = new Mesh();
				}
				return m_Mesh;
			}
		}

		public bool shouldHideMobileInput
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Invalid comparison between Unknown and I4
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Invalid comparison between Unknown and I4
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Invalid comparison between Unknown and I4
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Invalid comparison between Unknown and I4
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Invalid comparison between Unknown and I4
				RuntimePlatform platform = Application.get_platform();
				if ((int)platform <= 11)
				{
					if ((int)platform == 8 || (int)platform == 11)
					{
						goto IL_0020;
					}
				}
				else if ((int)platform == 23 || (int)platform == 31)
				{
					goto IL_0020;
				}
				return true;
				IL_0020:
				return m_HideMobileInput;
			}
			set
			{
				SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
			}
		}

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (text == value)
				{
					return;
				}
				m_Text = value;
				if (!Application.get_isPlaying())
				{
					SendOnValueChangedAndUpdateLabel();
					return;
				}
				if (m_Keyboard != null)
				{
					m_Keyboard.set_text(m_Text);
				}
				if (m_StringPosition > m_Text.Length)
				{
					m_StringPosition = (m_StringSelectPosition = m_Text.Length);
				}
				AdjustTextPositionRelativeToViewport(0f);
				m_forceRectTransformAdjustment = true;
				SendOnValueChangedAndUpdateLabel();
			}
		}

		public bool isFocused => m_AllowInput;

		public float caretBlinkRate
		{
			get
			{
				return m_CaretBlinkRate;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value) && m_AllowInput)
				{
					SetCaretActive();
				}
			}
		}

		public int caretWidth
		{
			get
			{
				return m_CaretWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretWidth, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public RectTransform textViewport
		{
			get
			{
				return m_TextViewport;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextViewport, value);
			}
		}

		public TMP_Text textComponent
		{
			get
			{
				return m_TextComponent;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextComponent, value);
			}
		}

		public Graphic placeholder
		{
			get
			{
				return m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_Placeholder, value);
			}
		}

		public Scrollbar verticalScrollbar
		{
			get
			{
				return m_VerticalScrollbar;
			}
			set
			{
				if ((Object)(object)m_VerticalScrollbar != (Object)null)
				{
					((UnityEvent<float>)(object)m_VerticalScrollbar.get_onValueChanged()).RemoveListener((UnityAction<float>)OnScrollbarValueChange);
				}
				SetPropertyUtility.SetClass(ref m_VerticalScrollbar, value);
				if (Object.op_Implicit((Object)(object)m_VerticalScrollbar))
				{
					((UnityEvent<float>)(object)m_VerticalScrollbar.get_onValueChanged()).AddListener((UnityAction<float>)OnScrollbarValueChange);
				}
			}
		}

		public float scrollSensitivity
		{
			get
			{
				return m_ScrollSensitivity;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_ScrollSensitivity, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public Color caretColor
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				if (!customCaretColor)
				{
					return ((Graphic)textComponent).get_color();
				}
				return m_CaretColor;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				if (SetPropertyUtility.SetColor(ref m_CaretColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public bool customCaretColor
		{
			get
			{
				return m_CustomCaretColor;
			}
			set
			{
				if (m_CustomCaretColor != value)
				{
					m_CustomCaretColor = value;
					MarkGeometryAsDirty();
				}
			}
		}

		public Color selectionColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_SelectionColor;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				if (SetPropertyUtility.SetColor(ref m_SelectionColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public SubmitEvent onEndEdit
		{
			get
			{
				return m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndEdit, value);
			}
		}

		public SubmitEvent onSubmit
		{
			get
			{
				return m_OnSubmit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnSubmit, value);
			}
		}

		public SelectionEvent onSelect
		{
			get
			{
				return m_OnSelect;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnSelect, value);
			}
		}

		public SelectionEvent onDeselect
		{
			get
			{
				return m_OnDeselect;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnDeselect, value);
			}
		}

		public TextSelectionEvent onTextSelection
		{
			get
			{
				return m_OnTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnTextSelection, value);
			}
		}

		public TextSelectionEvent onEndTextSelection
		{
			get
			{
				return m_OnEndTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndTextSelection, value);
			}
		}

		public OnChangeEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValueChanged, value);
			}
		}

		public OnValidateInput onValidateInput
		{
			get
			{
				return m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValidateInput, value);
			}
		}

		public int characterLimit
		{
			get
			{
				return m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
				{
					UpdateLabel();
				}
			}
		}

		public float pointSize
		{
			get
			{
				return m_GlobalPointSize;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_GlobalPointSize, Math.Max(0f, value)))
				{
					SetGlobalPointSize(m_GlobalPointSize);
					UpdateLabel();
				}
			}
		}

		public TMP_FontAsset fontAsset
		{
			get
			{
				return m_GlobalFontAsset;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_GlobalFontAsset, value))
				{
					SetGlobalFontAsset(m_GlobalFontAsset);
					UpdateLabel();
				}
			}
		}

		public bool onFocusSelectAll
		{
			get
			{
				return m_OnFocusSelectAll;
			}
			set
			{
				m_OnFocusSelectAll = value;
			}
		}

		public bool resetOnDeActivation
		{
			get
			{
				return m_ResetOnDeActivation;
			}
			set
			{
				m_ResetOnDeActivation = value;
			}
		}

		public bool restoreOriginalTextOnEscape
		{
			get
			{
				return m_RestoreOriginalTextOnEscape;
			}
			set
			{
				m_RestoreOriginalTextOnEscape = value;
			}
		}

		public bool isRichTextEditingAllowed
		{
			get
			{
				return m_isRichTextEditingAllowed;
			}
			set
			{
				m_isRichTextEditingAllowed = value;
			}
		}

		public ContentType contentType
		{
			get
			{
				return m_ContentType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_ContentType, value))
				{
					EnforceContentType();
				}
			}
		}

		public LineType lineType
		{
			get
			{
				return m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_LineType, value))
				{
					SetTextComponentWrapMode();
				}
				SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
			}
		}

		public InputType inputType
		{
			get
			{
				return m_InputType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_InputType, value))
				{
					SetToCustom();
				}
			}
		}

		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_KeyboardType;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				if (SetPropertyUtility.SetStruct<TouchScreenKeyboardType>(ref m_KeyboardType, value))
				{
					SetToCustom();
				}
			}
		}

		public CharacterValidation characterValidation
		{
			get
			{
				return m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value))
				{
					SetToCustom();
				}
			}
		}

		public TMP_InputValidator inputValidator
		{
			get
			{
				return m_InputValidator;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_InputValidator, value))
				{
					SetToCustom(CharacterValidation.CustomValidator);
				}
			}
		}

		public bool readOnly
		{
			get
			{
				return m_ReadOnly;
			}
			set
			{
				m_ReadOnly = value;
			}
		}

		public bool richText
		{
			get
			{
				return m_RichText;
			}
			set
			{
				m_RichText = value;
				SetTextComponentRichTextMode();
			}
		}

		public bool multiLine
		{
			get
			{
				if (m_LineType != LineType.MultiLineNewline)
				{
					return lineType == LineType.MultiLineSubmit;
				}
				return true;
			}
		}

		public char asteriskChar
		{
			get
			{
				return m_AsteriskChar;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_AsteriskChar, value))
				{
					UpdateLabel();
				}
			}
		}

		public bool wasCanceled => m_WasCanceled;

		protected int caretPositionInternal
		{
			get
			{
				return m_CaretPosition + Input.get_compositionString().Length;
			}
			set
			{
				m_CaretPosition = value;
				ClampCaretPos(ref m_CaretPosition);
			}
		}

		protected int stringPositionInternal
		{
			get
			{
				return m_StringPosition + Input.get_compositionString().Length;
			}
			set
			{
				m_StringPosition = value;
				ClampStringPos(ref m_StringPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return m_CaretSelectPosition + Input.get_compositionString().Length;
			}
			set
			{
				m_CaretSelectPosition = value;
				ClampCaretPos(ref m_CaretSelectPosition);
			}
		}

		protected int stringSelectPositionInternal
		{
			get
			{
				return m_StringSelectPosition + Input.get_compositionString().Length;
			}
			set
			{
				m_StringSelectPosition = value;
				ClampStringPos(ref m_StringSelectPosition);
			}
		}

		private bool hasSelection => stringPositionInternal != stringSelectPositionInternal;

		public int caretPosition
		{
			get
			{
				return caretSelectPositionInternal;
			}
			set
			{
				selectionAnchorPosition = value;
				selectionFocusPosition = value;
				isStringPositionDirty = true;
			}
		}

		public int selectionAnchorPosition
		{
			get
			{
				return caretPositionInternal;
			}
			set
			{
				if (Input.get_compositionString().Length == 0)
				{
					caretPositionInternal = value;
					isStringPositionDirty = true;
				}
			}
		}

		public int selectionFocusPosition
		{
			get
			{
				return caretSelectPositionInternal;
			}
			set
			{
				if (Input.get_compositionString().Length == 0)
				{
					caretSelectPositionInternal = value;
					isStringPositionDirty = true;
				}
			}
		}

		public int stringPosition
		{
			get
			{
				return stringSelectPositionInternal;
			}
			set
			{
				selectionStringAnchorPosition = value;
				selectionStringFocusPosition = value;
			}
		}

		public int selectionStringAnchorPosition
		{
			get
			{
				return stringPositionInternal;
			}
			set
			{
				if (Input.get_compositionString().Length == 0)
				{
					stringPositionInternal = value;
				}
			}
		}

		public int selectionStringFocusPosition
		{
			get
			{
				return stringSelectPositionInternal;
			}
			set
			{
				if (Input.get_compositionString().Length == 0)
				{
					stringSelectPositionInternal = value;
				}
			}
		}

		private static string clipboard
		{
			get
			{
				return GUIUtility.get_systemCopyBuffer();
			}
			set
			{
				GUIUtility.set_systemCopyBuffer(value);
			}
		}

		protected TMP_InputField()
			: this()
		{
		}//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown


		protected void ClampStringPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > text.Length)
			{
				pos = text.Length;
			}
		}

		protected void ClampCaretPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > m_TextComponent.textInfo.characterCount - 1)
			{
				pos = m_TextComponent.textInfo.characterCount - 1;
			}
		}

		protected override void OnValidate()
		{
			((Selectable)this).OnValidate();
			EnforceContentType();
			m_CharacterLimit = Math.Max(0, m_CharacterLimit);
			if (((UIBehaviour)this).IsActive())
			{
				SetTextComponentRichTextMode();
				UpdateLabel();
				if (m_AllowInput)
				{
					SetCaretActive();
				}
			}
		}

		protected override void OnEnable()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			((Selectable)this).OnEnable();
			if (m_Text == null)
			{
				m_Text = string.Empty;
			}
			if (Application.get_isPlaying() && (Object)(object)m_CachedInputRenderer == (Object)null && (Object)(object)m_TextComponent != (Object)null)
			{
				GameObject val = new GameObject(((Object)((Component)this).get_transform()).get_name() + " Input Caret", new Type[1] { typeof(RectTransform) });
				((Graphic)val.AddComponent<TMP_SelectionCaret>()).set_color(Color.get_clear());
				((Object)val).set_hideFlags((HideFlags)52);
				val.get_transform().SetParent(m_TextComponent.transform.get_parent());
				val.get_transform().SetAsFirstSibling();
				val.set_layer(((Component)this).get_gameObject().get_layer());
				caretRectTrans = val.GetComponent<RectTransform>();
				m_CachedInputRenderer = val.GetComponent<CanvasRenderer>();
				m_CachedInputRenderer.SetMaterial(Graphic.get_defaultGraphicMaterial(), (Texture)(object)Texture2D.get_whiteTexture());
				val.AddComponent<LayoutElement>().set_ignoreLayout(true);
				AssignPositioningIfNeeded();
			}
			if ((Object)(object)m_CachedInputRenderer != (Object)null)
			{
				m_CachedInputRenderer.SetMaterial(Graphic.get_defaultGraphicMaterial(), (Texture)(object)Texture2D.get_whiteTexture());
			}
			if ((Object)(object)m_TextComponent != (Object)null)
			{
				((Graphic)m_TextComponent).RegisterDirtyVerticesCallback(new UnityAction(MarkGeometryAsDirty));
				((Graphic)m_TextComponent).RegisterDirtyVerticesCallback(new UnityAction(UpdateLabel));
				m_TextComponent.ignoreRectMaskCulling = true;
				m_DefaultTransformPosition = Vector2.op_Implicit(((Transform)m_TextComponent.rectTransform).get_localPosition());
				if ((Object)(object)m_VerticalScrollbar != (Object)null)
				{
					((UnityEvent<float>)(object)m_VerticalScrollbar.get_onValueChanged()).AddListener((UnityAction<float>)OnScrollbarValueChange);
				}
				UpdateLabel();
			}
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
		}

		protected override void OnDisable()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			m_BlinkCoroutine = null;
			DeactivateInputField();
			if ((Object)(object)m_TextComponent != (Object)null)
			{
				((Graphic)m_TextComponent).UnregisterDirtyVerticesCallback(new UnityAction(MarkGeometryAsDirty));
				((Graphic)m_TextComponent).UnregisterDirtyVerticesCallback(new UnityAction(UpdateLabel));
				if ((Object)(object)m_VerticalScrollbar != (Object)null)
				{
					((UnityEvent<float>)(object)m_VerticalScrollbar.get_onValueChanged()).RemoveListener((UnityAction<float>)OnScrollbarValueChange);
				}
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild((ICanvasElement)(object)this);
			if ((Object)(object)m_CachedInputRenderer != (Object)null)
			{
				m_CachedInputRenderer.Clear();
			}
			if ((Object)(object)m_Mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_Mesh);
			}
			m_Mesh = null;
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
			((Selectable)this).OnDisable();
		}

		private void ON_TEXT_CHANGED(Object obj)
		{
			if (obj == (Object)(object)m_TextComponent && Application.get_isPlaying())
			{
				caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
				caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			}
		}

		private IEnumerator CaretBlink()
		{
			m_CaretVisible = true;
			yield return null;
			while (m_CaretBlinkRate > 0f)
			{
				float num = 1f / m_CaretBlinkRate;
				bool flag = (Time.get_unscaledTime() - m_BlinkStartTime) % num < num / 2f;
				if (m_CaretVisible != flag)
				{
					m_CaretVisible = flag;
					if (!hasSelection)
					{
						MarkGeometryAsDirty();
					}
				}
				yield return null;
			}
			m_BlinkCoroutine = null;
		}

		private void SetCaretVisible()
		{
			if (m_AllowInput)
			{
				m_CaretVisible = true;
				m_BlinkStartTime = Time.get_unscaledTime();
				SetCaretActive();
			}
		}

		private void SetCaretActive()
		{
			if (!m_AllowInput)
			{
				return;
			}
			if (m_CaretBlinkRate > 0f)
			{
				if (m_BlinkCoroutine == null)
				{
					m_BlinkCoroutine = ((MonoBehaviour)this).StartCoroutine(CaretBlink());
				}
			}
			else
			{
				m_CaretVisible = true;
			}
		}

		protected void OnFocus()
		{
			if (m_OnFocusSelectAll)
			{
				SelectAll();
			}
		}

		protected void SelectAll()
		{
			m_isSelectAll = true;
			stringPositionInternal = text.Length;
			stringSelectPositionInternal = 0;
		}

		public void MoveTextEnd(bool shift)
		{
			if (m_isRichTextEditingAllowed)
			{
				int length = text.Length;
				if (shift)
				{
					stringSelectPositionInternal = length;
				}
				else
				{
					stringPositionInternal = length;
					stringSelectPositionInternal = stringPositionInternal;
				}
			}
			else
			{
				int num = m_TextComponent.textInfo.characterCount - 1;
				if (!shift)
				{
					int num4 = (caretPositionInternal = (caretSelectPositionInternal = num));
					num4 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(num)));
				}
				else
				{
					caretSelectPositionInternal = num;
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(num);
				}
			}
			UpdateLabel();
		}

		public void MoveTextStart(bool shift)
		{
			if (m_isRichTextEditingAllowed)
			{
				int num = 0;
				if (shift)
				{
					stringSelectPositionInternal = num;
				}
				else
				{
					stringPositionInternal = num;
					stringSelectPositionInternal = stringPositionInternal;
				}
			}
			else
			{
				int num2 = 0;
				if (!shift)
				{
					int num5 = (caretPositionInternal = (caretSelectPositionInternal = num2));
					num5 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(num2)));
				}
				else
				{
					caretSelectPositionInternal = num2;
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(num2);
				}
			}
			UpdateLabel();
		}

		public void MoveToEndOfLine(bool shift, bool ctrl)
		{
			int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
			int num = (ctrl ? (m_TextComponent.textInfo.characterCount - 1) : m_TextComponent.textInfo.lineInfo[lineNumber].lastCharacterIndex);
			num = GetStringIndexFromCaretPosition(num);
			if (shift)
			{
				stringSelectPositionInternal = num;
			}
			else
			{
				stringPositionInternal = num;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		public void MoveToStartOfLine(bool shift, bool ctrl)
		{
			int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
			int num = ((!ctrl) ? m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex : 0);
			num = GetStringIndexFromCaretPosition(num);
			if (shift)
			{
				stringSelectPositionInternal = num;
			}
			else
			{
				stringPositionInternal = num;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.get_isSupported();
		}

		protected virtual void LateUpdate()
		{
			if (m_ShouldActivateNextUpdate)
			{
				if (!isFocused)
				{
					ActivateInputFieldInternal();
					m_ShouldActivateNextUpdate = false;
					return;
				}
				m_ShouldActivateNextUpdate = false;
			}
			if (m_IsScrollbarUpdateRequired)
			{
				UpdateScrollbar();
				m_IsScrollbarUpdateRequired = false;
			}
			if (InPlaceEditing() || !isFocused)
			{
				return;
			}
			AssignPositioningIfNeeded();
			if (m_Keyboard == null || !m_Keyboard.get_active())
			{
				if (m_Keyboard != null)
				{
					if (!m_ReadOnly)
					{
						this.text = m_Keyboard.get_text();
					}
					if (m_Keyboard.get_wasCanceled())
					{
						m_WasCanceled = true;
					}
					if (m_Keyboard.get_done())
					{
						OnSubmit(null);
					}
				}
				((Selectable)this).OnDeselect((BaseEventData)null);
				return;
			}
			string text = m_Keyboard.get_text();
			if (m_Text != text)
			{
				if (m_ReadOnly)
				{
					m_Keyboard.set_text(m_Text);
				}
				else
				{
					m_Text = "";
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c == '\r' || c == '\u0003')
						{
							c = '\n';
						}
						if (onValidateInput != null)
						{
							c = onValidateInput(m_Text, m_Text.Length, c);
						}
						else if (characterValidation != 0)
						{
							c = Validate(m_Text, m_Text.Length, c);
						}
						if (lineType == LineType.MultiLineSubmit && c == '\n')
						{
							m_Keyboard.set_text(m_Text);
							OnSubmit(null);
							((Selectable)this).OnDeselect((BaseEventData)null);
							return;
						}
						if (c != 0)
						{
							m_Text += c;
						}
					}
					if (characterLimit > 0 && m_Text.Length > characterLimit)
					{
						m_Text = m_Text.Substring(0, characterLimit);
					}
					int num2 = (stringPositionInternal = (stringSelectPositionInternal = m_Text.Length));
					if (m_Text != text)
					{
						m_Keyboard.set_text(m_Text);
					}
					SendOnValueChangedAndUpdateLabel();
				}
			}
			if (m_Keyboard.get_done())
			{
				if (m_Keyboard.get_wasCanceled())
				{
					m_WasCanceled = true;
				}
				((Selectable)this).OnDeselect((BaseEventData)null);
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (((UIBehaviour)this).IsActive() && ((Selectable)this).IsInteractable() && (int)eventData.get_button() == 0 && (Object)(object)m_TextComponent != (Object)null)
			{
				return m_Keyboard == null;
			}
			return false;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = true;
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			if (MayDrag(eventData))
			{
				CaretPosition cursor;
				int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, Vector2.op_Implicit(eventData.get_position()), eventData.get_pressEventCamera(), out cursor);
				switch (cursor)
				{
				case CaretPosition.Left:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
					break;
				case CaretPosition.Right:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
					break;
				}
				caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
				MarkGeometryAsDirty();
				m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textViewport, eventData.get_position(), eventData.get_pressEventCamera());
				if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
				{
					m_DragCoroutine = ((MonoBehaviour)this).StartCoroutine(MouseDragOutsideRect(eventData));
				}
				((AbstractEventData)eventData).Use();
			}
		}

		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			Vector2 val = default(Vector2);
			while (m_UpdateDrag && m_DragPositionOutOfBounds)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(textViewport, eventData.get_position(), eventData.get_pressEventCamera(), ref val);
				Rect rect = textViewport.get_rect();
				if (multiLine)
				{
					if (val.y > ((Rect)(ref rect)).get_yMax())
					{
						MoveUp(shift: true, goToFirstChar: true);
					}
					else if (val.y < ((Rect)(ref rect)).get_yMin())
					{
						MoveDown(shift: true, goToLastChar: true);
					}
				}
				else if (val.x < ((Rect)(ref rect)).get_xMin())
				{
					MoveLeft(shift: true, ctrl: false);
				}
				else if (val.x > ((Rect)(ref rect)).get_xMax())
				{
					MoveRight(shift: true, ctrl: false);
				}
				UpdateLabel();
				float num = (multiLine ? 0.1f : 0.05f);
				yield return (object)new WaitForSeconds(num);
			}
			m_DragCoroutine = null;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = false;
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			if (!MayDrag(eventData))
			{
				return;
			}
			EventSystem.get_current().SetSelectedGameObject(((Component)this).get_gameObject(), (BaseEventData)(object)eventData);
			bool allowInput = m_AllowInput;
			((Selectable)this).OnPointerDown(eventData);
			if (!InPlaceEditing() && (m_Keyboard == null || !m_Keyboard.get_active()))
			{
				((Selectable)this).OnSelect((BaseEventData)(object)eventData);
				return;
			}
			bool flag = Input.GetKey((KeyCode)304) || Input.GetKey((KeyCode)303);
			bool flag2 = false;
			float unscaledTime = Time.get_unscaledTime();
			if (m_ClickStartTime + m_DoubleClickDelay > unscaledTime)
			{
				flag2 = true;
			}
			m_ClickStartTime = unscaledTime;
			if (allowInput || !m_OnFocusSelectAll)
			{
				CaretPosition cursor;
				int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, Vector2.op_Implicit(eventData.get_position()), eventData.get_pressEventCamera(), out cursor);
				if (flag)
				{
					switch (cursor)
					{
					case CaretPosition.Left:
						stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
						break;
					case CaretPosition.Right:
						stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
						break;
					}
				}
				else
				{
					switch (cursor)
					{
					case CaretPosition.Left:
					{
						int num3 = (stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition)));
						break;
					}
					case CaretPosition.Right:
					{
						int num3 = (stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1));
						break;
					}
					}
				}
				if (!flag2)
				{
					int num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal)));
				}
				else
				{
					int num6 = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Vector2.op_Implicit(eventData.get_position()), eventData.get_pressEventCamera());
					if (num6 != -1)
					{
						caretPositionInternal = m_TextComponent.textInfo.wordInfo[num6].firstCharacterIndex;
						caretSelectPositionInternal = m_TextComponent.textInfo.wordInfo[num6].lastCharacterIndex + 1;
						stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
						stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
					}
					else
					{
						caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
						stringSelectPositionInternal++;
						caretSelectPositionInternal = caretPositionInternal + 1;
						caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
					}
				}
			}
			UpdateLabel();
			((AbstractEventData)eventData).Use();
		}

		protected EditState KeyPressed(Event evt)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Invalid comparison between Unknown and I4
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Invalid comparison between Unknown and I4
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Invalid comparison between Unknown and I4
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Invalid comparison between Unknown and I4
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Invalid comparison between Unknown and I4
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Invalid comparison between Unknown and I4
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Invalid comparison between Unknown and I4
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Invalid comparison between Unknown and I4
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Invalid comparison between Unknown and I4
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Invalid comparison between Unknown and I4
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Invalid comparison between Unknown and I4
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Invalid comparison between Unknown and I4
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected I4, but got Unknown
			EventModifiers modifiers = evt.get_modifiers();
			RuntimePlatform platform = Application.get_platform();
			bool flag = (((int)platform == 0 || (int)platform == 1) ? ((modifiers & 8) > 0) : ((modifiers & 2) > 0));
			bool flag2 = (modifiers & 1) > 0;
			bool flag3 = (modifiers & 4) > 0;
			bool flag4 = flag && !flag3 && !flag2;
			KeyCode keyCode = evt.get_keyCode();
			if ((int)keyCode <= 97)
			{
				if ((int)keyCode <= 13)
				{
					if ((int)keyCode == 8)
					{
						Backspace();
						return EditState.Continue;
					}
					if ((int)keyCode == 13)
					{
						goto IL_01cc;
					}
				}
				else
				{
					if ((int)keyCode == 27)
					{
						m_WasCanceled = true;
						return EditState.Finish;
					}
					if ((int)keyCode == 97 && flag4)
					{
						SelectAll();
						return EditState.Continue;
					}
				}
			}
			else if ((int)keyCode <= 118)
			{
				if ((int)keyCode != 99)
				{
					if ((int)keyCode == 118 && flag4)
					{
						Append(clipboard);
						return EditState.Continue;
					}
				}
				else if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = "";
					}
					return EditState.Continue;
				}
			}
			else
			{
				if ((int)keyCode != 120)
				{
					if ((int)keyCode != 127)
					{
						switch (keyCode - 271)
						{
						case 7:
							MoveToStartOfLine(flag2, flag);
							return EditState.Continue;
						case 8:
							MoveToEndOfLine(flag2, flag);
							return EditState.Continue;
						case 5:
							MoveLeft(flag2, flag);
							return EditState.Continue;
						case 4:
							MoveRight(flag2, flag);
							return EditState.Continue;
						case 2:
							MoveUp(flag2);
							return EditState.Continue;
						case 3:
							MoveDown(flag2);
							return EditState.Continue;
						case 9:
							MovePageUp(flag2);
							return EditState.Continue;
						case 10:
							MovePageDown(flag2);
							return EditState.Continue;
						case 0:
							break;
						default:
							goto IL_01e0;
						}
						goto IL_01cc;
					}
					ForwardSpace();
					return EditState.Continue;
				}
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = "";
					}
					Delete();
					SendOnValueChangedAndUpdateLabel();
					return EditState.Continue;
				}
			}
			goto IL_01e0;
			IL_01cc:
			if (lineType != LineType.MultiLineNewline)
			{
				return EditState.Finish;
			}
			goto IL_01e0;
			IL_01e0:
			char c = evt.get_character();
			if (!multiLine && (c == '\t' || c == '\r' || c == '\n'))
			{
				return EditState.Continue;
			}
			if (c == '\r' || c == '\u0003')
			{
				c = '\n';
			}
			if (IsValidChar(c))
			{
				Append(c);
			}
			if (c == '\0' && Input.get_compositionString().Length > 0)
			{
				UpdateLabel();
			}
			return EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			switch (c)
			{
			case '\u007f':
				return false;
			case '\t':
			case '\n':
				return true;
			default:
				return m_TextComponent.font.HasCharacter(c, searchFallbacks: true);
			}
		}

		public void ProcessEvent(Event e)
		{
			KeyPressed(e);
		}

		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Invalid comparison between Unknown and I4
			if (!isFocused)
			{
				return;
			}
			bool flag = false;
			while (Event.PopEvent(m_ProcessingEvent))
			{
				if ((int)m_ProcessingEvent.get_rawType() == 4)
				{
					flag = true;
					if (KeyPressed(m_ProcessingEvent) == EditState.Finish)
					{
						SendOnSubmit();
						DeactivateInputField();
						break;
					}
				}
				EventType type = m_ProcessingEvent.get_type();
				if (type - 13 <= 1 && m_ProcessingEvent.get_commandName() == "SelectAll")
				{
					SelectAll();
					flag = true;
				}
			}
			if (flag)
			{
				UpdateLabel();
			}
			((AbstractEventData)eventData).Use();
		}

		public virtual void OnScroll(PointerEventData eventData)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			float preferredHeight = m_TextComponent.preferredHeight;
			Rect rect = m_TextViewport.get_rect();
			if (!(preferredHeight < ((Rect)(ref rect)).get_height()))
			{
				float num = 0f - eventData.get_scrollDelta().y;
				m_ScrollPosition += 1f / (float)m_TextComponent.textInfo.lineCount * num * m_ScrollSensitivity;
				m_ScrollPosition = Mathf.Clamp01(m_ScrollPosition);
				AdjustTextPositionRelativeToViewport(m_ScrollPosition);
				m_AllowInput = false;
				if (Object.op_Implicit((Object)(object)m_VerticalScrollbar))
				{
					m_IsUpdatingScrollbarValues = true;
					m_VerticalScrollbar.set_value(m_ScrollPosition);
				}
			}
		}

		private string GetSelectedString()
		{
			if (!hasSelection)
			{
				return "";
			}
			int num = stringPositionInternal;
			int num2 = stringSelectPositionInternal;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (stringSelectPositionInternal + 1 >= text.Length)
			{
				return text.Length;
			}
			int num = text.IndexOfAny(kSeparators, stringSelectPositionInternal + 1);
			if (num == -1)
			{
				return text.Length;
			}
			return num + 1;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = (stringPositionInternal = (stringSelectPositionInternal = Mathf.Max(stringPositionInternal, stringSelectPositionInternal)));
				num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
				return;
			}
			int num5 = (ctrl ? FindtNextWordBegin() : ((!m_isRichTextEditingAllowed) ? GetStringIndexFromCaretPosition(caretSelectPositionInternal + 1) : (stringSelectPositionInternal + 1)));
			if (shift)
			{
				stringSelectPositionInternal = num5;
				caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			}
			else
			{
				int num3 = (stringSelectPositionInternal = (stringPositionInternal = num5));
				num3 = (caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
			}
		}

		private int FindtPrevWordBegin()
		{
			if (stringSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = text.LastIndexOfAny(kSeparators, stringSelectPositionInternal - 2);
			if (num == -1)
			{
				return 0;
			}
			return num + 1;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = (stringPositionInternal = (stringSelectPositionInternal = Mathf.Min(stringPositionInternal, stringSelectPositionInternal)));
				num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
				return;
			}
			int num5 = (ctrl ? FindtPrevWordBegin() : ((!m_isRichTextEditingAllowed) ? GetStringIndexFromCaretPosition(caretSelectPositionInternal - 1) : (stringSelectPositionInternal - 1)));
			if (shift)
			{
				stringSelectPositionInternal = num5;
				caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			}
			else
			{
				int num3 = (stringSelectPositionInternal = (stringPositionInternal = num5));
				num3 = (caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
			}
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				if (!goToFirstChar)
				{
					return originalPos;
				}
				return 0;
			}
			int num = m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex; i < num; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f && num6 <= 1f)
				{
					if (num6 < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num3)
				{
					num2 = i;
					num3 = num5;
					num4 = num6;
				}
			}
			if (num2 == -1)
			{
				return num;
			}
			if (num4 < 0.5f)
			{
				return num2;
			}
			return num2 + 1;
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				return m_TextComponent.textInfo.characterCount - 1;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
			{
				if (!goToLastChar)
				{
					return originalPos;
				}
				return m_TextComponent.textInfo.characterCount - 1;
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
			int num = -1;
			float num2 = 32767f;
			float num3 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num4 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num5 = num4 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num5 >= 0f && num5 <= 1f)
				{
					if (num5 < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
				num4 = Mathf.Abs(num4);
				if (num4 < num2)
				{
					num = i;
					num2 = num4;
					num3 = num5;
				}
			}
			if (num == -1)
			{
				return lastCharacterIndex;
			}
			if (num3 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		private int PageUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				if (!goToFirstChar)
				{
					return originalPos;
				}
				return 0;
			}
			Rect rect = m_TextViewport.get_rect();
			float height = ((Rect)(ref rect)).get_height();
			int num = lineNumber - 1;
			while (num > 0 && !(m_TextComponent.textInfo.lineInfo[num].baseline > m_TextComponent.textInfo.lineInfo[lineNumber].baseline + height))
			{
				num--;
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[num].lastCharacterIndex;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[num].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f && num6 <= 1f)
				{
					if (num6 < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num3)
				{
					num2 = i;
					num3 = num5;
					num4 = num6;
				}
			}
			if (num2 == -1)
			{
				return lastCharacterIndex;
			}
			if (num4 < 0.5f)
			{
				return num2;
			}
			return num2 + 1;
		}

		private int PageDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				return m_TextComponent.textInfo.characterCount - 1;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
			{
				if (!goToLastChar)
				{
					return originalPos;
				}
				return m_TextComponent.textInfo.characterCount - 1;
			}
			Rect rect = m_TextViewport.get_rect();
			float height = ((Rect)(ref rect)).get_height();
			int i;
			for (i = lineNumber + 1; i < m_TextComponent.textInfo.lineCount - 1 && !(m_TextComponent.textInfo.lineInfo[i].baseline < m_TextComponent.textInfo.lineInfo[lineNumber].baseline - height); i++)
			{
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[i].lastCharacterIndex;
			int num = -1;
			float num2 = 32767f;
			float num3 = 0f;
			for (int j = m_TextComponent.textInfo.lineInfo[i].firstCharacterIndex; j < lastCharacterIndex; j++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[j];
				float num4 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num5 = num4 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num5 >= 0f && num5 <= 1f)
				{
					if (num5 < 0.5f)
					{
						return j;
					}
					return j + 1;
				}
				num4 = Mathf.Abs(num4);
				if (num4 < num2)
				{
					num = j;
					num2 = num4;
					num3 = num5;
				}
			}
			if (num == -1)
			{
				return lastCharacterIndex;
			}
			if (num3 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		private void MoveDown(bool shift)
		{
			MoveDown(shift, goToLastChar: true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = (caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal)));
			}
			int num4 = (multiLine ? LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar) : (m_TextComponent.textInfo.characterCount - 1));
			if (shift)
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			else
			{
				int num3 = (caretSelectPositionInternal = (caretPositionInternal = num4));
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
		}

		private void MoveUp(bool shift)
		{
			MoveUp(shift, goToFirstChar: true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = (caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal)));
			}
			int num4 = (multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0);
			if (shift)
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			else
			{
				int num3 = (caretSelectPositionInternal = (caretPositionInternal = num4));
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
		}

		private void MovePageUp(bool shift)
		{
			MovePageUp(shift, goToFirstChar: true);
		}

		private void MovePageUp(bool shift, bool goToFirstChar)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			if (hasSelection && !shift)
			{
				int num3 = (caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal)));
			}
			int num4 = (multiLine ? PageUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0);
			if (!shift)
			{
				int num3 = (caretSelectPositionInternal = (caretPositionInternal = num4));
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
			else
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			if (m_LineType != 0)
			{
				Rect rect = m_TextViewport.get_rect();
				float height = ((Rect)(ref rect)).get_height();
				float y = ((Transform)m_TextComponent.rectTransform).get_position().y;
				Bounds textBounds = m_TextComponent.textBounds;
				float num8 = y + ((Bounds)(ref textBounds)).get_max().y;
				float y2 = ((Transform)m_TextViewport).get_position().y;
				rect = m_TextViewport.get_rect();
				float num9 = y2 + ((Rect)(ref rect)).get_yMax();
				height = ((num9 > num8 + height) ? height : (num9 - num8));
				RectTransform rectTransform = m_TextComponent.rectTransform;
				rectTransform.set_anchoredPosition(rectTransform.get_anchoredPosition() + new Vector2(0f, height));
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
			}
		}

		private void MovePageDown(bool shift)
		{
			MovePageDown(shift, goToLastChar: true);
		}

		private void MovePageDown(bool shift, bool goToLastChar)
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			if (hasSelection && !shift)
			{
				int num3 = (caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal)));
			}
			int num4 = (multiLine ? PageDownCharacterPosition(caretSelectPositionInternal, goToLastChar) : (m_TextComponent.textInfo.characterCount - 1));
			if (!shift)
			{
				int num3 = (caretSelectPositionInternal = (caretPositionInternal = num4));
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
			else
			{
				caretSelectPositionInternal = num4;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			if (m_LineType != 0)
			{
				Rect rect = m_TextViewport.get_rect();
				float height = ((Rect)(ref rect)).get_height();
				float y = ((Transform)m_TextComponent.rectTransform).get_position().y;
				Bounds textBounds = m_TextComponent.textBounds;
				float num8 = y + ((Bounds)(ref textBounds)).get_min().y;
				float y2 = ((Transform)m_TextViewport).get_position().y;
				rect = m_TextViewport.get_rect();
				float num9 = y2 + ((Rect)(ref rect)).get_yMin();
				height = ((num9 > num8 + height) ? height : (num9 - num8));
				RectTransform rectTransform = m_TextComponent.rectTransform;
				rectTransform.set_anchoredPosition(rectTransform.get_anchoredPosition() + new Vector2(0f, height));
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
			}
		}

		private void Delete()
		{
			if (m_ReadOnly || stringPositionInternal == stringSelectPositionInternal)
			{
				return;
			}
			if (m_isRichTextEditingAllowed || m_isSelectAll)
			{
				if (stringPositionInternal < stringSelectPositionInternal)
				{
					m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
					stringSelectPositionInternal = stringPositionInternal;
				}
				else
				{
					m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
					stringPositionInternal = stringSelectPositionInternal;
				}
				m_isSelectAll = false;
				return;
			}
			stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			if (caretPositionInternal < caretSelectPositionInternal)
			{
				m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
				stringSelectPositionInternal = stringPositionInternal;
				caretSelectPositionInternal = caretPositionInternal;
			}
			else
			{
				m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
				stringPositionInternal = stringSelectPositionInternal;
				stringPositionInternal = stringSelectPositionInternal;
				caretPositionInternal = caretSelectPositionInternal;
			}
		}

		private void ForwardSpace()
		{
			if (m_ReadOnly)
			{
				return;
			}
			if (hasSelection)
			{
				Delete();
				SendOnValueChangedAndUpdateLabel();
			}
			else if (m_isRichTextEditingAllowed)
			{
				if (stringPositionInternal < text.Length)
				{
					m_Text = text.Remove(stringPositionInternal, 1);
					SendOnValueChangedAndUpdateLabel();
				}
			}
			else if (caretPositionInternal < m_TextComponent.textInfo.characterCount - 1)
			{
				int num2 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal)));
				m_Text = text.Remove(stringPositionInternal, 1);
				SendOnValueChangedAndUpdateLabel();
			}
		}

		private void Backspace()
		{
			if (m_ReadOnly)
			{
				return;
			}
			if (hasSelection)
			{
				Delete();
				SendOnValueChangedAndUpdateLabel();
				return;
			}
			if (m_isRichTextEditingAllowed)
			{
				if (stringPositionInternal > 0)
				{
					m_Text = text.Remove(stringPositionInternal - 1, 1);
					int num2 = (stringSelectPositionInternal = --stringPositionInternal);
					m_isLastKeyBackspace = true;
					SendOnValueChangedAndUpdateLabel();
				}
				return;
			}
			if (caretPositionInternal > 0)
			{
				m_Text = text.Remove(GetStringIndexFromCaretPosition(caretPositionInternal - 1), 1);
				int num2 = (caretSelectPositionInternal = --caretPositionInternal);
				num2 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal)));
			}
			m_isLastKeyBackspace = true;
			SendOnValueChangedAndUpdateLabel();
		}

		protected virtual void Append(string input)
		{
			if (m_ReadOnly || !InPlaceEditing())
			{
				return;
			}
			int i = 0;
			for (int length = input.Length; i < length; i++)
			{
				char c = input[i];
				if (c >= ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\n')
				{
					Append(c);
				}
			}
		}

		protected virtual void Append(char input)
		{
			if (m_ReadOnly || !InPlaceEditing())
			{
				return;
			}
			if (onValidateInput != null)
			{
				input = onValidateInput(text, stringPositionInternal, input);
			}
			else
			{
				if (characterValidation == CharacterValidation.CustomValidator)
				{
					input = Validate(text, stringPositionInternal, input);
					if (input != 0)
					{
						SendOnValueChanged();
						UpdateLabel();
					}
					return;
				}
				if (characterValidation != 0)
				{
					input = Validate(text, stringPositionInternal, input);
				}
			}
			if (input != 0)
			{
				Insert(input);
			}
		}

		private void Insert(char c)
		{
			if (!m_ReadOnly)
			{
				string text = c.ToString();
				Delete();
				if (characterLimit <= 0 || this.text.Length < characterLimit)
				{
					m_Text = this.text.Insert(m_StringPosition, text);
					int num2 = (stringSelectPositionInternal = (stringPositionInternal += text.Length));
					SendOnValueChanged();
				}
			}
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			SendOnValueChanged();
			UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (onValueChanged != null)
			{
				((UnityEvent<string>)onValueChanged).Invoke(text);
			}
		}

		protected void SendOnEndEdit()
		{
			if (onEndEdit != null)
			{
				((UnityEvent<string>)onEndEdit).Invoke(m_Text);
			}
		}

		protected void SendOnSubmit()
		{
			if (onSubmit != null)
			{
				((UnityEvent<string>)onSubmit).Invoke(m_Text);
			}
		}

		protected void SendOnFocus()
		{
			if (onSelect != null)
			{
				((UnityEvent<string>)onSelect).Invoke(m_Text);
			}
		}

		protected void SendOnFocusLost()
		{
			if (onDeselect != null)
			{
				((UnityEvent<string>)onDeselect).Invoke(m_Text);
			}
		}

		protected void SendOnTextSelection()
		{
			m_isSelected = true;
			if (onTextSelection != null)
			{
				((UnityEvent<string, int, int>)onTextSelection).Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
			}
		}

		protected void SendOnEndTextSelection()
		{
			if (m_isSelected)
			{
				if (onEndTextSelection != null)
				{
					((UnityEvent<string, int, int>)onEndTextSelection).Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
				}
				m_isSelected = false;
			}
		}

		protected void UpdateLabel()
		{
			if ((Object)(object)m_TextComponent != (Object)null && (Object)(object)m_TextComponent.font != (Object)null)
			{
				string text = ((Input.get_compositionString().Length <= 0) ? this.text : (this.text.Substring(0, m_StringPosition) + Input.get_compositionString() + this.text.Substring(m_StringPosition)));
				string text2 = ((inputType != InputType.Password) ? text : new string(asteriskChar, text.Length));
				bool flag = string.IsNullOrEmpty(text);
				if ((Object)(object)m_Placeholder != (Object)null)
				{
					((Behaviour)m_Placeholder).set_enabled(flag);
				}
				if (!flag)
				{
					SetCaretVisible();
				}
				m_TextComponent.text = text2 + "\u200b";
				MarkGeometryAsDirty();
				m_IsScrollbarUpdateRequired = true;
			}
		}

		private void UpdateScrollbar()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (Object.op_Implicit((Object)(object)m_VerticalScrollbar))
			{
				Rect rect = m_TextViewport.get_rect();
				float size = ((Rect)(ref rect)).get_height() / m_TextComponent.preferredHeight;
				m_IsUpdatingScrollbarValues = true;
				m_VerticalScrollbar.set_size(size);
				Scrollbar obj = m_VerticalScrollbar;
				float y = m_TextComponent.rectTransform.get_anchoredPosition().y;
				float preferredHeight = m_TextComponent.preferredHeight;
				rect = m_TextViewport.get_rect();
				float scrollPosition;
				obj.set_value(scrollPosition = y / (preferredHeight - ((Rect)(ref rect)).get_height()));
				m_ScrollPosition = scrollPosition;
			}
		}

		private void OnScrollbarValueChange(float value)
		{
			if (m_IsUpdatingScrollbarValues)
			{
				m_IsUpdatingScrollbarValues = false;
			}
			else if (!(value < 0f) && !(value > 1f))
			{
				AdjustTextPositionRelativeToViewport(value);
				m_ScrollPosition = value;
			}
		}

		private void AdjustTextPositionRelativeToViewport(float relativePosition)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			if (textInfo != null && textInfo.lineInfo != null && textInfo.lineCount != 0 && textInfo.lineCount <= textInfo.lineInfo.Length)
			{
				RectTransform rectTransform = m_TextComponent.rectTransform;
				float x = m_TextComponent.rectTransform.get_anchoredPosition().x;
				float preferredHeight = m_TextComponent.preferredHeight;
				Rect rect = m_TextViewport.get_rect();
				rectTransform.set_anchoredPosition(new Vector2(x, (preferredHeight - ((Rect)(ref rect)).get_height()) * relativePosition));
				AssignPositioningIfNeeded();
			}
		}

		private int GetCaretPositionFromStringIndex(int stringIndex)
		{
			int characterCount = m_TextComponent.textInfo.characterCount;
			for (int i = 0; i < characterCount; i++)
			{
				if (m_TextComponent.textInfo.characterInfo[i].index >= stringIndex)
				{
					return i;
				}
			}
			return characterCount;
		}

		private int GetStringIndexFromCaretPosition(int caretPosition)
		{
			ClampCaretPos(ref caretPosition);
			return m_TextComponent.textInfo.characterInfo[caretPosition].index;
		}

		public void ForceLabelUpdate()
		{
			UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			if (Application.get_isPlaying() && !(PrefabUtility.GetPrefabObject((Object)(object)((Component)this).get_gameObject()) != (Object)null))
			{
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild((ICanvasElement)(object)this);
			}
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			if ((int)update == 4)
			{
				UpdateGeometry();
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (Application.get_isPlaying() && shouldHideMobileInput && !((Object)(object)m_CachedInputRenderer == (Object)null))
			{
				OnFillVBO(mesh);
				m_CachedInputRenderer.SetMesh(mesh);
			}
		}

		private void AssignPositioningIfNeeded()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_TextComponent != (Object)null && (Object)(object)caretRectTrans != (Object)null && (((Transform)caretRectTrans).get_localPosition() != ((Transform)m_TextComponent.rectTransform).get_localPosition() || ((Transform)caretRectTrans).get_localRotation() != ((Transform)m_TextComponent.rectTransform).get_localRotation() || ((Transform)caretRectTrans).get_localScale() != ((Transform)m_TextComponent.rectTransform).get_localScale() || caretRectTrans.get_anchorMin() != m_TextComponent.rectTransform.get_anchorMin() || caretRectTrans.get_anchorMax() != m_TextComponent.rectTransform.get_anchorMax() || caretRectTrans.get_anchoredPosition() != m_TextComponent.rectTransform.get_anchoredPosition() || caretRectTrans.get_sizeDelta() != m_TextComponent.rectTransform.get_sizeDelta() || caretRectTrans.get_pivot() != m_TextComponent.rectTransform.get_pivot()))
			{
				((Transform)caretRectTrans).set_localPosition(((Transform)m_TextComponent.rectTransform).get_localPosition());
				((Transform)caretRectTrans).set_localRotation(((Transform)m_TextComponent.rectTransform).get_localRotation());
				((Transform)caretRectTrans).set_localScale(((Transform)m_TextComponent.rectTransform).get_localScale());
				caretRectTrans.set_anchorMin(m_TextComponent.rectTransform.get_anchorMin());
				caretRectTrans.set_anchorMax(m_TextComponent.rectTransform.get_anchorMax());
				caretRectTrans.set_anchoredPosition(m_TextComponent.rectTransform.get_anchoredPosition());
				caretRectTrans.set_sizeDelta(m_TextComponent.rectTransform.get_sizeDelta());
				caretRectTrans.set_pivot(m_TextComponent.rectTransform.get_pivot());
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			VertexHelper val = new VertexHelper();
			try
			{
				if (!isFocused && m_ResetOnDeActivation)
				{
					val.FillMesh(vbo);
					return;
				}
				if (isStringPositionDirty)
				{
					stringPositionInternal = GetStringIndexFromCaretPosition(m_CaretPosition);
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(m_CaretSelectPosition);
					isStringPositionDirty = false;
				}
				if (!hasSelection)
				{
					GenerateCaret(val, Vector2.get_zero());
					SendOnEndTextSelection();
				}
				else
				{
					GenerateHightlight(val, Vector2.get_zero());
					SendOnTextSelection();
				}
				val.FillMesh(vbo);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			if (m_CaretVisible)
			{
				if (m_CursorVerts == null)
				{
					CreateCursorVerts();
				}
				float num = m_CaretWidth;
				int characterCount = m_TextComponent.textInfo.characterCount;
				Vector2 zero = Vector2.get_zero();
				float num2 = 0f;
				caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
				TMP_CharacterInfo tMP_CharacterInfo;
				if (caretPositionInternal == 0)
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[0];
					((Vector2)(ref zero))._002Ector(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				else if (caretPositionInternal < characterCount)
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[caretPositionInternal];
					((Vector2)(ref zero))._002Ector(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				else
				{
					tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[characterCount - 1];
					((Vector2)(ref zero))._002Ector(tMP_CharacterInfo.xAdvance, tMP_CharacterInfo.descender);
					num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
				}
				if ((isFocused && zero != m_LastPosition) || m_forceRectTransformAdjustment)
				{
					AdjustRectTransformRelativeToViewport(zero, num2, tMP_CharacterInfo.isVisible);
				}
				m_LastPosition = zero;
				float num3 = zero.y + num2;
				float num4 = num3 - num2;
				m_CursorVerts[0].position = new Vector3(zero.x, num4, 0f);
				m_CursorVerts[1].position = new Vector3(zero.x, num3, 0f);
				m_CursorVerts[2].position = new Vector3(zero.x + num, num3, 0f);
				m_CursorVerts[3].position = new Vector3(zero.x + num, num4, 0f);
				m_CursorVerts[0].color = Color32.op_Implicit(caretColor);
				m_CursorVerts[1].color = Color32.op_Implicit(caretColor);
				m_CursorVerts[2].color = Color32.op_Implicit(caretColor);
				m_CursorVerts[3].color = Color32.op_Implicit(caretColor);
				vbo.AddUIVertexQuad(m_CursorVerts);
				int height = Screen.get_height();
				zero.y = (float)height - zero.y;
				Input.set_compositionCursorPos(zero);
			}
		}

		private void CreateCursorVerts()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			m_CursorVerts = (UIVertex[])(object)new UIVertex[4];
			for (int i = 0; i < m_CursorVerts.Length; i++)
			{
				m_CursorVerts[i] = UIVertex.simpleVert;
				m_CursorVerts[i].uv0 = Vector2.get_zero();
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			caretPositionInternal = (m_CaretPosition = GetCaretPositionFromStringIndex(stringPositionInternal));
			caretSelectPositionInternal = (m_CaretSelectPosition = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
			float num = 0f;
			Vector2 startPosition = default(Vector2);
			if (caretSelectPositionInternal < textInfo.characterCount)
			{
				((Vector2)(ref startPosition))._002Ector(textInfo.characterInfo[caretSelectPositionInternal].origin, textInfo.characterInfo[caretSelectPositionInternal].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal].ascender - textInfo.characterInfo[caretSelectPositionInternal].descender;
			}
			else
			{
				((Vector2)(ref startPosition))._002Ector(textInfo.characterInfo[caretSelectPositionInternal - 1].xAdvance, textInfo.characterInfo[caretSelectPositionInternal - 1].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal - 1].ascender - textInfo.characterInfo[caretSelectPositionInternal - 1].descender;
			}
			AdjustRectTransformRelativeToViewport(startPosition, num, isCharVisible: true);
			int num2 = Mathf.Max(0, caretPositionInternal);
			int num3 = Mathf.Max(0, caretSelectPositionInternal);
			if (num2 > num3)
			{
				int num4 = num2;
				num2 = num3;
				num3 = num4;
			}
			num3--;
			int num5 = textInfo.characterInfo[num2].lineNumber;
			int lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.get_zero();
			simpleVert.color = Color32.op_Implicit(selectionColor);
			Vector2 val = default(Vector2);
			Vector2 val2 = default(Vector2);
			for (int i = num2; i <= num3 && i < textInfo.characterCount; i++)
			{
				if (i == lastCharacterIndex || i == num3)
				{
					TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[num2];
					TMP_CharacterInfo tMP_CharacterInfo2 = textInfo.characterInfo[i];
					if (i > 0 && tMP_CharacterInfo2.character == '\n' && textInfo.characterInfo[i - 1].character == '\r')
					{
						tMP_CharacterInfo2 = textInfo.characterInfo[i - 1];
					}
					((Vector2)(ref val))._002Ector(tMP_CharacterInfo.origin, textInfo.lineInfo[num5].ascender);
					((Vector2)(ref val2))._002Ector(tMP_CharacterInfo2.xAdvance, textInfo.lineInfo[num5].descender);
					int currentVertCount = vbo.get_currentVertCount();
					simpleVert.position = new Vector3(val.x, val2.y, 0f);
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(val2.x, val2.y, 0f);
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(val2.x, val.y, 0f);
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(val.x, val.y, 0f);
					vbo.AddVert(simpleVert);
					vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
					vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
					num2 = i + 1;
					num5++;
					if (num5 < textInfo.lineCount)
					{
						lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
					}
				}
			}
			m_IsScrollbarUpdateRequired = true;
		}

		private void AdjustRectTransformRelativeToViewport(Vector2 startPosition, float height, bool isCharVisible)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = m_TextViewport.get_rect();
			float xMin = ((Rect)(ref rect)).get_xMin();
			rect = m_TextViewport.get_rect();
			float xMax = ((Rect)(ref rect)).get_xMax();
			float num = xMax - (m_TextComponent.rectTransform.get_anchoredPosition().x + startPosition.x + m_TextComponent.margin.z + (float)m_CaretWidth);
			if (num < 0f && (!multiLine || (multiLine && isCharVisible)))
			{
				RectTransform rectTransform = m_TextComponent.rectTransform;
				rectTransform.set_anchoredPosition(rectTransform.get_anchoredPosition() + new Vector2(num, 0f));
				AssignPositioningIfNeeded();
			}
			float num2 = m_TextComponent.rectTransform.get_anchoredPosition().x + startPosition.x - m_TextComponent.margin.x - xMin;
			if (num2 < 0f)
			{
				RectTransform rectTransform2 = m_TextComponent.rectTransform;
				rectTransform2.set_anchoredPosition(rectTransform2.get_anchoredPosition() + new Vector2(0f - num2, 0f));
				AssignPositioningIfNeeded();
			}
			if (m_LineType != 0)
			{
				rect = m_TextViewport.get_rect();
				float num3 = ((Rect)(ref rect)).get_yMax() - (m_TextComponent.rectTransform.get_anchoredPosition().y + startPosition.y + height);
				if (num3 < -0.0001f)
				{
					RectTransform rectTransform3 = m_TextComponent.rectTransform;
					rectTransform3.set_anchoredPosition(rectTransform3.get_anchoredPosition() + new Vector2(0f, num3));
					AssignPositioningIfNeeded();
					m_IsScrollbarUpdateRequired = true;
				}
				float num4 = m_TextComponent.rectTransform.get_anchoredPosition().y + startPosition.y;
				rect = m_TextViewport.get_rect();
				float num5 = num4 - ((Rect)(ref rect)).get_yMin();
				if (num5 < 0f)
				{
					RectTransform rectTransform4 = m_TextComponent.rectTransform;
					rectTransform4.set_anchoredPosition(rectTransform4.get_anchoredPosition() - new Vector2(0f, num5));
					AssignPositioningIfNeeded();
					m_IsScrollbarUpdateRequired = true;
				}
			}
			if (m_isLastKeyBackspace)
			{
				float num6 = m_TextComponent.rectTransform.get_anchoredPosition().x + m_TextComponent.textInfo.characterInfo[0].origin - m_TextComponent.margin.x;
				float num7 = m_TextComponent.rectTransform.get_anchoredPosition().x + m_TextComponent.textInfo.characterInfo[m_TextComponent.textInfo.characterCount - 1].origin + m_TextComponent.margin.z;
				if (m_TextComponent.rectTransform.get_anchoredPosition().x + startPosition.x <= xMin + 0.0001f)
				{
					if (num6 < xMin)
					{
						float num8 = Mathf.Min((xMax - xMin) / 2f, xMin - num6);
						RectTransform rectTransform5 = m_TextComponent.rectTransform;
						rectTransform5.set_anchoredPosition(rectTransform5.get_anchoredPosition() + new Vector2(num8, 0f));
						AssignPositioningIfNeeded();
					}
				}
				else if (num7 < xMax && num6 < xMin)
				{
					float num9 = Mathf.Min(xMax - num7, xMin - num6);
					RectTransform rectTransform6 = m_TextComponent.rectTransform;
					rectTransform6.set_anchoredPosition(rectTransform6.get_anchoredPosition() + new Vector2(num9, 0f));
					AssignPositioningIfNeeded();
				}
				m_isLastKeyBackspace = false;
			}
			m_forceRectTransformAdjustment = false;
		}

		protected char Validate(string text, int pos, char ch)
		{
			if (characterValidation == CharacterValidation.None || !((Behaviour)this).get_enabled())
			{
				return ch;
			}
			if (characterValidation == CharacterValidation.Integer || characterValidation == CharacterValidation.Decimal)
			{
				bool num = pos == 0 && text.Length > 0 && text[0] == '-';
				bool flag = stringPositionInternal == 0 || stringSelectPositionInternal == 0;
				if (!num)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '-' && (pos == 0 || flag))
					{
						return ch;
					}
					if (ch == '.' && characterValidation == CharacterValidation.Decimal && !text.Contains("."))
					{
						return ch;
					}
				}
			}
			else if (characterValidation == CharacterValidation.Digit)
			{
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.Alphanumeric)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.Name)
			{
				char c = ((text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ');
				char c2 = ((text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n');
				if (char.IsLetter(ch))
				{
					if (char.IsLower(ch) && c == ' ')
					{
						return char.ToUpper(ch);
					}
					if (char.IsUpper(ch) && c != ' ' && c != '\'')
					{
						return char.ToLower(ch);
					}
					return ch;
				}
				switch (ch)
				{
				case '\'':
					if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
					{
						return ch;
					}
					break;
				case ' ':
					if (c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
					{
						return ch;
					}
					break;
				}
			}
			else if (characterValidation == CharacterValidation.EmailAddress)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
				if (ch == '@' && text.IndexOf('@') == -1)
				{
					return ch;
				}
				if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
				{
					return ch;
				}
				if (ch == '.')
				{
					char num2 = ((text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ');
					char c3 = ((text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n');
					if (num2 != '.' && c3 != '.')
					{
						return ch;
					}
				}
			}
			else if (characterValidation == CharacterValidation.Regex)
			{
				if (Regex.IsMatch(ch.ToString(), m_RegexValue))
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.CustomValidator && (Object)(object)m_InputValidator != (Object)null)
			{
				char result = m_InputValidator.Validate(ref text, ref pos, ch);
				m_Text = text;
				int num5 = (stringSelectPositionInternal = (stringPositionInternal = pos));
				return result;
			}
			return '\0';
		}

		public void ActivateInputField()
		{
			if (!((Object)(object)m_TextComponent == (Object)null) && !((Object)(object)m_TextComponent.font == (Object)null) && ((UIBehaviour)this).IsActive() && ((Selectable)this).IsInteractable())
			{
				if (isFocused && m_Keyboard != null && !m_Keyboard.get_active())
				{
					m_Keyboard.set_active(true);
					m_Keyboard.set_text(m_Text);
				}
				m_ShouldActivateNextUpdate = true;
			}
		}

		private void ActivateInputFieldInternal()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)EventSystem.get_current() == (Object)null)
			{
				return;
			}
			if ((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() != (Object)(object)((Component)this).get_gameObject())
			{
				EventSystem.get_current().SetSelectedGameObject(((Component)this).get_gameObject());
			}
			if (TouchScreenKeyboard.get_isSupported())
			{
				if (Input.get_touchSupported())
				{
					TouchScreenKeyboard.set_hideInput(shouldHideMobileInput);
				}
				m_Keyboard = ((inputType == InputType.Password) ? TouchScreenKeyboard.Open(m_Text, keyboardType, false, multiLine, true) : TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine));
				MoveTextEnd(shift: false);
			}
			else
			{
				Input.set_imeCompositionMode((IMECompositionMode)1);
				OnFocus();
			}
			m_AllowInput = true;
			m_OriginalText = text;
			m_WasCanceled = false;
			SetCaretVisible();
			UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			((Selectable)this).OnSelect(eventData);
			SendOnFocus();
			ActivateInputField();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if ((int)eventData.get_button() == 0)
			{
				ActivateInputField();
			}
		}

		public void OnControlClick()
		{
		}

		public void DeactivateInputField()
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			if (!m_AllowInput)
			{
				return;
			}
			m_HasDoneFocusTransition = false;
			m_AllowInput = false;
			if ((Object)(object)m_Placeholder != (Object)null)
			{
				((Behaviour)m_Placeholder).set_enabled(string.IsNullOrEmpty(m_Text));
			}
			if ((Object)(object)m_TextComponent != (Object)null && ((Selectable)this).IsInteractable())
			{
				if (m_WasCanceled && m_RestoreOriginalTextOnEscape)
				{
					text = m_OriginalText;
				}
				if (m_Keyboard != null)
				{
					m_Keyboard.set_active(false);
					m_Keyboard = null;
				}
				if (m_ResetOnDeActivation)
				{
					m_StringPosition = (m_StringSelectPosition = 0);
					m_CaretPosition = (m_CaretSelectPosition = 0);
					((Transform)m_TextComponent.rectTransform).set_localPosition(Vector2.op_Implicit(m_DefaultTransformPosition));
					if ((Object)(object)caretRectTrans != (Object)null)
					{
						((Transform)caretRectTrans).set_localPosition(Vector3.get_zero());
					}
				}
				SendOnEndEdit();
				SendOnEndTextSelection();
				Input.set_imeCompositionMode((IMECompositionMode)0);
			}
			MarkGeometryAsDirty();
			m_IsScrollbarUpdateRequired = true;
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			DeactivateInputField();
			((Selectable)this).OnDeselect(eventData);
			SendOnFocusLost();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (((UIBehaviour)this).IsActive() && ((Selectable)this).IsInteractable())
			{
				if (!isFocused)
				{
					m_ShouldActivateNextUpdate = true;
				}
				SendOnSubmit();
			}
		}

		private void EnforceContentType()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			switch (contentType)
			{
			case ContentType.Standard:
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)0;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Autocorrected:
				m_InputType = InputType.AutoCorrect;
				m_KeyboardType = (TouchScreenKeyboardType)0;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.IntegerNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)4;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			case ContentType.DecimalNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)2;
				m_CharacterValidation = CharacterValidation.Decimal;
				break;
			case ContentType.Alphanumeric:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)1;
				m_CharacterValidation = CharacterValidation.Alphanumeric;
				break;
			case ContentType.Name:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)0;
				m_CharacterValidation = CharacterValidation.Name;
				break;
			case ContentType.EmailAddress:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = (TouchScreenKeyboardType)7;
				m_CharacterValidation = CharacterValidation.EmailAddress;
				break;
			case ContentType.Password:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = (TouchScreenKeyboardType)0;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Pin:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = (TouchScreenKeyboardType)4;
				m_CharacterValidation = CharacterValidation.Digit;
				break;
			}
		}

		private void SetTextComponentWrapMode()
		{
			if (!((Object)(object)m_TextComponent == (Object)null))
			{
				if (m_LineType == LineType.SingleLine)
				{
					m_TextComponent.enableWordWrapping = false;
				}
				else
				{
					m_TextComponent.enableWordWrapping = true;
				}
			}
		}

		private void SetTextComponentRichTextMode()
		{
			if (!((Object)(object)m_TextComponent == (Object)null))
			{
				m_TextComponent.richText = m_RichText;
			}
		}

		private void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
		{
			if (contentType == ContentType.Custom)
			{
				return;
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (contentType == allowedContentTypes[i])
				{
					return;
				}
			}
			contentType = ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (contentType != ContentType.Custom)
			{
				contentType = ContentType.Custom;
			}
		}

		private void SetToCustom(CharacterValidation characterValidation)
		{
			if (contentType == ContentType.Custom)
			{
				characterValidation = CharacterValidation.CustomValidator;
				return;
			}
			contentType = ContentType.Custom;
			characterValidation = CharacterValidation.CustomValidator;
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_HasDoneFocusTransition)
			{
				state = (SelectionState)1;
			}
			else if ((int)state == 2)
			{
				m_HasDoneFocusTransition = true;
			}
			((Selectable)this).DoStateTransition(state, instant);
		}

		public void SetGlobalPointSize(float pointSize)
		{
			TMP_Text tMP_Text = m_Placeholder as TMP_Text;
			if ((Object)(object)tMP_Text != (Object)null)
			{
				tMP_Text.fontSize = pointSize;
			}
			textComponent.fontSize = pointSize;
		}

		public void SetGlobalFontAsset(TMP_FontAsset fontAsset)
		{
			TMP_Text tMP_Text = m_Placeholder as TMP_Text;
			if ((Object)(object)tMP_Text != (Object)null)
			{
				tMP_Text.font = fontAsset;
			}
			textComponent.font = fontAsset;
		}

		[SpecialName]
		Transform ICanvasElement.get_transform()
		{
			return ((Component)this).get_transform();
		}
	}
}
