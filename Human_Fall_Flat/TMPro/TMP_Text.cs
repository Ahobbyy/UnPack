using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro
{
	public class TMP_Text : MaskableGraphic
	{
		protected enum TextInputSources
		{
			Text,
			SetText,
			SetCharArray,
			String
		}

		[SerializeField]
		protected string m_text;

		[SerializeField]
		protected bool m_isRightToLeft;

		[SerializeField]
		protected TMP_FontAsset m_fontAsset;

		protected TMP_FontAsset m_currentFontAsset;

		protected bool m_isSDFShader;

		[SerializeField]
		protected Material m_sharedMaterial;

		protected Material m_currentMaterial;

		protected MaterialReference[] m_materialReferences = new MaterialReference[32];

		protected Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[16]);

		protected int m_currentMaterialIndex;

		[SerializeField]
		protected Material[] m_fontSharedMaterials;

		[SerializeField]
		protected Material m_fontMaterial;

		[SerializeField]
		protected Material[] m_fontMaterials;

		protected bool m_isMaterialDirty;

		[SerializeField]
		protected Color32 m_fontColor32 = Color32.op_Implicit(Color.get_white());

		[SerializeField]
		protected Color m_fontColor = Color.get_white();

		protected static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		protected Color32 m_underlineColor = s_colorWhite;

		protected Color32 m_strikethroughColor = s_colorWhite;

		protected Color32 m_highlightColor = s_colorWhite;

		[SerializeField]
		protected bool m_enableVertexGradient;

		[SerializeField]
		protected VertexGradient m_fontColorGradient = new VertexGradient(Color.get_white());

		[SerializeField]
		protected TMP_ColorGradient m_fontColorGradientPreset;

		[SerializeField]
		protected TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		protected bool m_tintAllSprites;

		protected bool m_tintSprite;

		protected Color32 m_spriteColor;

		[SerializeField]
		protected bool m_overrideHtmlColors;

		[SerializeField]
		protected Color32 m_faceColor = Color32.op_Implicit(Color.get_white());

		[SerializeField]
		protected Color32 m_outlineColor = Color32.op_Implicit(Color.get_black());

		protected float m_outlineWidth;

		[SerializeField]
		protected float m_fontSize = 36f;

		protected float m_currentFontSize;

		[SerializeField]
		protected float m_fontSizeBase = 36f;

		protected TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[16]);

		[SerializeField]
		protected int m_fontWeight = 400;

		protected int m_fontWeightInternal;

		protected TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[16]);

		[SerializeField]
		protected bool m_enableAutoSizing;

		protected float m_maxFontSize;

		protected float m_minFontSize;

		[SerializeField]
		protected float m_fontSizeMin;

		[SerializeField]
		protected float m_fontSizeMax;

		[SerializeField]
		protected FontStyles m_fontStyle;

		protected FontStyles m_style;

		protected TMP_BasicXmlTagStack m_fontStyleStack;

		protected bool m_isUsingBold;

		[SerializeField]
		[FormerlySerializedAs("m_lineJustification")]
		protected TextAlignmentOptions m_textAlignment = TextAlignmentOptions.TopLeft;

		protected TextAlignmentOptions m_lineJustification;

		protected TMP_XmlTagStack<TextAlignmentOptions> m_lineJustificationStack = new TMP_XmlTagStack<TextAlignmentOptions>(new TextAlignmentOptions[16]);

		protected Vector3[] m_textContainerLocalCorners = (Vector3[])(object)new Vector3[4];

		[SerializeField]
		protected bool m_isAlignmentEnumConverted;

		[SerializeField]
		protected float m_characterSpacing;

		protected float m_cSpacing;

		protected float m_monoSpacing;

		[SerializeField]
		protected float m_wordSpacing;

		[SerializeField]
		protected float m_lineSpacing;

		protected float m_lineSpacingDelta;

		protected float m_lineHeight = -32767f;

		[SerializeField]
		protected float m_lineSpacingMax;

		[SerializeField]
		protected float m_paragraphSpacing;

		[SerializeField]
		protected float m_charWidthMaxAdj;

		protected float m_charWidthAdjDelta;

		[SerializeField]
		protected bool m_enableWordWrapping;

		protected bool m_isCharacterWrappingEnabled;

		protected bool m_isNonBreakingSpace;

		protected bool m_isIgnoringAlignment;

		[SerializeField]
		protected float m_wordWrappingRatios = 0.4f;

		[SerializeField]
		protected TextOverflowModes m_overflowMode;

		[SerializeField]
		protected int m_firstOverflowCharacterIndex = -1;

		[SerializeField]
		protected TMP_Text m_linkedTextComponent;

		[SerializeField]
		protected bool m_isLinkedTextComponent;

		protected bool m_isTextTruncated;

		[SerializeField]
		protected bool m_enableKerning;

		[SerializeField]
		protected bool m_enableExtraPadding;

		[SerializeField]
		protected bool checkPaddingRequired;

		[SerializeField]
		protected bool m_isRichText = true;

		[SerializeField]
		protected bool m_parseCtrlCharacters = true;

		protected bool m_isOverlay;

		[SerializeField]
		protected bool m_isOrthographic;

		[SerializeField]
		protected bool m_isCullingEnabled;

		[SerializeField]
		protected bool m_ignoreRectMaskCulling;

		[SerializeField]
		protected bool m_ignoreCulling = true;

		[SerializeField]
		protected TextureMappingOptions m_horizontalMapping;

		[SerializeField]
		protected TextureMappingOptions m_verticalMapping;

		[SerializeField]
		protected float m_uvLineOffset;

		protected TextRenderFlags m_renderMode = TextRenderFlags.Render;

		[SerializeField]
		protected VertexSortingOrder m_geometrySortingOrder;

		[SerializeField]
		protected int m_firstVisibleCharacter;

		protected int m_maxVisibleCharacters = 99999;

		protected int m_maxVisibleWords = 99999;

		protected int m_maxVisibleLines = 99999;

		[SerializeField]
		protected bool m_useMaxVisibleDescender = true;

		[SerializeField]
		protected int m_pageToDisplay = 1;

		protected bool m_isNewPage;

		[SerializeField]
		protected Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

		protected float m_marginLeft;

		protected float m_marginRight;

		protected float m_marginWidth;

		protected float m_marginHeight;

		protected float m_width = -1f;

		[SerializeField]
		protected TMP_TextInfo m_textInfo;

		[SerializeField]
		protected bool m_havePropertiesChanged;

		[SerializeField]
		protected bool m_isUsingLegacyAnimationComponent;

		protected Transform m_transform;

		protected RectTransform m_rectTransform;

		protected bool m_autoSizeTextContainer;

		protected Mesh m_mesh;

		[SerializeField]
		protected bool m_isVolumetricText;

		[SerializeField]
		protected TMP_SpriteAnimator m_spriteAnimator;

		protected float m_flexibleHeight = -1f;

		protected float m_flexibleWidth = -1f;

		protected float m_minWidth;

		protected float m_minHeight;

		protected float m_maxWidth;

		protected float m_maxHeight;

		protected LayoutElement m_LayoutElement;

		protected float m_preferredWidth;

		protected float m_renderedWidth;

		protected bool m_isPreferredWidthDirty;

		protected float m_preferredHeight;

		protected float m_renderedHeight;

		protected bool m_isPreferredHeightDirty;

		protected bool m_isCalculatingPreferredValues;

		private int m_recursiveCount;

		protected int m_layoutPriority;

		protected bool m_isCalculateSizeRequired;

		protected bool m_isLayoutDirty;

		protected bool m_verticesAlreadyDirty;

		protected bool m_layoutAlreadyDirty;

		protected bool m_isAwake;

		[SerializeField]
		protected bool m_isInputParsingRequired;

		[SerializeField]
		protected TextInputSources m_inputSource;

		protected string old_text;

		protected float m_fontScale;

		protected float m_fontScaleMultiplier;

		protected char[] m_htmlTag = new char[128];

		protected XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		protected float[] m_attributeParameterValues = new float[16];

		protected float tag_LineIndent;

		protected float tag_Indent;

		protected TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[16]);

		protected bool tag_NoParsing;

		protected bool m_isParsingText;

		protected Matrix4x4 m_FXMatrix;

		protected bool m_isFXMatrixSet;

		protected int[] m_char_buffer;

		private TMP_CharacterInfo[] m_internalCharacterInfo;

		protected char[] m_input_CharArray = new char[256];

		private int m_charArray_Length;

		protected int m_totalCharacterCount;

		protected WordWrapState m_SavedWordWrapState;

		protected WordWrapState m_SavedLineState;

		protected int m_characterCount;

		protected int m_firstCharacterOfLine;

		protected int m_firstVisibleCharacterOfLine;

		protected int m_lastCharacterOfLine;

		protected int m_lastVisibleCharacterOfLine;

		protected int m_lineNumber;

		protected int m_lineVisibleCharacterCount;

		protected int m_pageNumber;

		protected float m_maxAscender;

		protected float m_maxCapHeight;

		protected float m_maxDescender;

		protected float m_maxLineAscender;

		protected float m_maxLineDescender;

		protected float m_startOfLineAscender;

		protected float m_lineOffset;

		protected Extents m_meshExtents;

		protected Color32 m_htmlColor = Color32.op_Implicit(new Color(255f, 255f, 255f, 128f));

		protected TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>((Color32[])(object)new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_underlineColorStack = new TMP_XmlTagStack<Color32>((Color32[])(object)new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_strikethroughColorStack = new TMP_XmlTagStack<Color32>((Color32[])(object)new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_highlightColorStack = new TMP_XmlTagStack<Color32>((Color32[])(object)new Color32[16]);

		protected TMP_ColorGradient m_colorGradientPreset;

		protected TMP_XmlTagStack<TMP_ColorGradient> m_colorGradientStack = new TMP_XmlTagStack<TMP_ColorGradient>(new TMP_ColorGradient[16]);

		protected float m_tabSpacing;

		protected float m_spacing;

		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		protected float m_padding;

		protected float m_baselineOffset;

		protected TMP_XmlTagStack<float> m_baselineOffsetStack = new TMP_XmlTagStack<float>(new float[16]);

		protected float m_xAdvance;

		protected TMP_TextElementType m_textElementType;

		protected TMP_TextElement m_cached_TextElement;

		protected TMP_Glyph m_cached_Underline_GlyphInfo;

		protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		protected TMP_SpriteAsset m_defaultSpriteAsset;

		protected TMP_SpriteAsset m_currentSpriteAsset;

		protected int m_spriteCount;

		protected int m_spriteIndex;

		protected int m_spriteAnimationID;

		protected bool m_ignoreActiveState;

		private readonly float[] k_Power = new float[10] { 0.5f, 0.05f, 0.005f, 0.0005f, 5E-05f, 5E-06f, 5E-07f, 5E-08f, 5E-09f, 5E-10f };

		protected static Vector2 k_LargePositiveVector2 = new Vector2(2.14748365E+09f, 2.14748365E+09f);

		protected static Vector2 k_LargeNegativeVector2 = new Vector2(-2.14748365E+09f, -2.14748365E+09f);

		protected static float k_LargePositiveFloat = 32767f;

		protected static float k_LargeNegativeFloat = -32767f;

		protected static int k_LargePositiveInt = int.MaxValue;

		protected static int k_LargeNegativeInt = -2147483647;

		public string text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (!(m_text == value))
				{
					m_text = (old_text = value);
					m_inputSource = TextInputSources.String;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool isRightToLeftText
		{
			get
			{
				return m_isRightToLeft;
			}
			set
			{
				if (m_isRightToLeft != value)
				{
					m_isRightToLeft = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public TMP_FontAsset font
		{
			get
			{
				return m_fontAsset;
			}
			set
			{
				if (!((Object)(object)m_fontAsset == (Object)(object)value))
				{
					m_fontAsset = value;
					LoadFontAsset();
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public virtual Material fontSharedMaterial
		{
			get
			{
				return m_sharedMaterial;
			}
			set
			{
				if (!((Object)(object)m_sharedMaterial == (Object)(object)value))
				{
					SetSharedMaterial(value);
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetMaterialDirty();
				}
			}
		}

		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return GetSharedMaterials();
			}
			set
			{
				SetSharedMaterials(value);
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetMaterialDirty();
			}
		}

		public Material fontMaterial
		{
			get
			{
				return GetMaterial(m_sharedMaterial);
			}
			set
			{
				if (!((Object)(object)m_sharedMaterial != (Object)null) || ((Object)m_sharedMaterial).GetInstanceID() != ((Object)value).GetInstanceID())
				{
					m_sharedMaterial = value;
					m_padding = GetPaddingForMaterial();
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetMaterialDirty();
				}
			}
		}

		public virtual Material[] fontMaterials
		{
			get
			{
				return GetMaterials(m_fontSharedMaterials);
			}
			set
			{
				SetSharedMaterials(value);
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetMaterialDirty();
			}
		}

		public override Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_fontColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				if (!(m_fontColor == value))
				{
					m_havePropertiesChanged = true;
					m_fontColor = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public float alpha
		{
			get
			{
				return m_fontColor.a;
			}
			set
			{
				if (m_fontColor.a != value)
				{
					m_fontColor.a = value;
					m_havePropertiesChanged = true;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool enableVertexGradient
		{
			get
			{
				return m_enableVertexGradient;
			}
			set
			{
				if (m_enableVertexGradient != value)
				{
					m_havePropertiesChanged = true;
					m_enableVertexGradient = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public VertexGradient colorGradient
		{
			get
			{
				return m_fontColorGradient;
			}
			set
			{
				m_havePropertiesChanged = true;
				m_fontColorGradient = value;
				((Graphic)this).SetVerticesDirty();
			}
		}

		public TMP_ColorGradient colorGradientPreset
		{
			get
			{
				return m_fontColorGradientPreset;
			}
			set
			{
				m_havePropertiesChanged = true;
				m_fontColorGradientPreset = value;
				((Graphic)this).SetVerticesDirty();
			}
		}

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return m_spriteAsset;
			}
			set
			{
				m_spriteAsset = value;
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				m_isCalculateSizeRequired = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetLayoutDirty();
			}
		}

		public bool tintAllSprites
		{
			get
			{
				return m_tintAllSprites;
			}
			set
			{
				if (m_tintAllSprites != value)
				{
					m_tintAllSprites = value;
					m_havePropertiesChanged = true;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool overrideColorTags
		{
			get
			{
				return m_overrideHtmlColors;
			}
			set
			{
				if (m_overrideHtmlColors != value)
				{
					m_havePropertiesChanged = true;
					m_overrideHtmlColors = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public Color32 faceColor
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				if ((Object)(object)m_sharedMaterial == (Object)null)
				{
					return m_faceColor;
				}
				m_faceColor = Color32.op_Implicit(m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor));
				return m_faceColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (!m_faceColor.Compare(value))
				{
					SetFaceColor(value);
					m_havePropertiesChanged = true;
					m_faceColor = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetMaterialDirty();
				}
			}
		}

		public Color32 outlineColor
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				if ((Object)(object)m_sharedMaterial == (Object)null)
				{
					return m_outlineColor;
				}
				m_outlineColor = Color32.op_Implicit(m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor));
				return m_outlineColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (!m_outlineColor.Compare(value))
				{
					SetOutlineColor(value);
					m_havePropertiesChanged = true;
					m_outlineColor = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public float outlineWidth
		{
			get
			{
				if ((Object)(object)m_sharedMaterial == (Object)null)
				{
					return m_outlineWidth;
				}
				m_outlineWidth = m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return m_outlineWidth;
			}
			set
			{
				if (m_outlineWidth != value)
				{
					SetOutlineThickness(value);
					m_havePropertiesChanged = true;
					m_outlineWidth = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public float fontSize
		{
			get
			{
				return m_fontSize;
			}
			set
			{
				if (m_fontSize != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_fontSize = value;
					if (!m_enableAutoSizing)
					{
						m_fontSizeBase = m_fontSize;
					}
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float fontScale => m_fontScale;

		public int fontWeight
		{
			get
			{
				return m_fontWeight;
			}
			set
			{
				if (m_fontWeight != value)
				{
					m_fontWeight = value;
					m_isCalculateSizeRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = ((Graphic)this).get_canvas();
				if (!Object.op_Implicit((Object)(object)canvas))
				{
					return 1f;
				}
				if (!Object.op_Implicit((Object)(object)font))
				{
					return canvas.get_scaleFactor();
				}
				if ((Object)(object)m_currentFontAsset == (Object)null || m_currentFontAsset.fontInfo.PointSize <= 0f || m_fontSize <= 0f)
				{
					return 1f;
				}
				return m_fontSize / m_currentFontAsset.fontInfo.PointSize;
			}
		}

		public bool enableAutoSizing
		{
			get
			{
				return m_enableAutoSizing;
			}
			set
			{
				if (m_enableAutoSizing != value)
				{
					m_enableAutoSizing = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float fontSizeMin
		{
			get
			{
				return m_fontSizeMin;
			}
			set
			{
				if (m_fontSizeMin != value)
				{
					m_fontSizeMin = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float fontSizeMax
		{
			get
			{
				return m_fontSizeMax;
			}
			set
			{
				if (m_fontSizeMax != value)
				{
					m_fontSizeMax = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public FontStyles fontStyle
		{
			get
			{
				return m_fontStyle;
			}
			set
			{
				if (m_fontStyle != value)
				{
					m_fontStyle = value;
					m_havePropertiesChanged = true;
					checkPaddingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool isUsingBold => m_isUsingBold;

		public TextAlignmentOptions alignment
		{
			get
			{
				return m_textAlignment;
			}
			set
			{
				if (m_textAlignment != value)
				{
					m_havePropertiesChanged = true;
					m_textAlignment = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public float characterSpacing
		{
			get
			{
				return m_characterSpacing;
			}
			set
			{
				if (m_characterSpacing != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_characterSpacing = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float wordSpacing
		{
			get
			{
				return m_wordSpacing;
			}
			set
			{
				if (m_wordSpacing != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_wordSpacing = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float lineSpacing
		{
			get
			{
				return m_lineSpacing;
			}
			set
			{
				if (m_lineSpacing != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_lineSpacing = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float lineSpacingAdjustment
		{
			get
			{
				return m_lineSpacingMax;
			}
			set
			{
				if (m_lineSpacingMax != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_lineSpacingMax = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float paragraphSpacing
		{
			get
			{
				return m_paragraphSpacing;
			}
			set
			{
				if (m_paragraphSpacing != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_paragraphSpacing = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float characterWidthAdjustment
		{
			get
			{
				return m_charWidthMaxAdj;
			}
			set
			{
				if (m_charWidthMaxAdj != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_charWidthMaxAdj = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool enableWordWrapping
		{
			get
			{
				return m_enableWordWrapping;
			}
			set
			{
				if (m_enableWordWrapping != value)
				{
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					m_isCalculateSizeRequired = true;
					m_enableWordWrapping = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public float wordWrappingRatios
		{
			get
			{
				return m_wordWrappingRatios;
			}
			set
			{
				if (m_wordWrappingRatios != value)
				{
					m_wordWrappingRatios = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public TextOverflowModes overflowMode
		{
			get
			{
				return m_overflowMode;
			}
			set
			{
				if (m_overflowMode != value)
				{
					m_overflowMode = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool isTextOverflowing
		{
			get
			{
				if (m_firstOverflowCharacterIndex != -1)
				{
					return true;
				}
				return false;
			}
		}

		public int firstOverflowCharacterIndex => m_firstOverflowCharacterIndex;

		public TMP_Text linkedTextComponent
		{
			get
			{
				return m_linkedTextComponent;
			}
			set
			{
				if ((Object)(object)m_linkedTextComponent != (Object)(object)value)
				{
					if ((Object)(object)m_linkedTextComponent != (Object)null)
					{
						m_linkedTextComponent.overflowMode = TextOverflowModes.Overflow;
						m_linkedTextComponent.linkedTextComponent = null;
						m_linkedTextComponent.isLinkedTextComponent = false;
					}
					m_linkedTextComponent = value;
					if ((Object)(object)m_linkedTextComponent != (Object)null)
					{
						m_linkedTextComponent.isLinkedTextComponent = true;
					}
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetLayoutDirty();
			}
		}

		public bool isLinkedTextComponent
		{
			get
			{
				return m_isLinkedTextComponent;
			}
			set
			{
				m_isLinkedTextComponent = value;
				if (!m_isLinkedTextComponent)
				{
					m_firstVisibleCharacter = 0;
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetLayoutDirty();
			}
		}

		public bool isTextTruncated => m_isTextTruncated;

		public bool enableKerning
		{
			get
			{
				return m_enableKerning;
			}
			set
			{
				if (m_enableKerning != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_enableKerning = value;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool extraPadding
		{
			get
			{
				return m_enableExtraPadding;
			}
			set
			{
				if (m_enableExtraPadding != value)
				{
					m_havePropertiesChanged = true;
					m_enableExtraPadding = value;
					UpdateMeshPadding();
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool richText
		{
			get
			{
				return m_isRichText;
			}
			set
			{
				if (m_isRichText != value)
				{
					m_isRichText = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool parseCtrlCharacters
		{
			get
			{
				return m_parseCtrlCharacters;
			}
			set
			{
				if (m_parseCtrlCharacters != value)
				{
					m_parseCtrlCharacters = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public bool isOverlay
		{
			get
			{
				return m_isOverlay;
			}
			set
			{
				if (m_isOverlay != value)
				{
					m_isOverlay = value;
					SetShaderDepth();
					m_havePropertiesChanged = true;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool isOrthographic
		{
			get
			{
				return m_isOrthographic;
			}
			set
			{
				if (m_isOrthographic != value)
				{
					m_havePropertiesChanged = true;
					m_isOrthographic = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool enableCulling
		{
			get
			{
				return m_isCullingEnabled;
			}
			set
			{
				if (m_isCullingEnabled != value)
				{
					m_isCullingEnabled = value;
					SetCulling();
					m_havePropertiesChanged = true;
				}
			}
		}

		public bool ignoreRectMaskCulling
		{
			get
			{
				return m_ignoreRectMaskCulling;
			}
			set
			{
				if (m_ignoreRectMaskCulling != value)
				{
					m_ignoreRectMaskCulling = value;
					m_havePropertiesChanged = true;
				}
			}
		}

		public bool ignoreVisibility
		{
			get
			{
				return m_ignoreCulling;
			}
			set
			{
				if (m_ignoreCulling != value)
				{
					m_havePropertiesChanged = true;
					m_ignoreCulling = value;
				}
			}
		}

		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return m_horizontalMapping;
			}
			set
			{
				if (m_horizontalMapping != value)
				{
					m_havePropertiesChanged = true;
					m_horizontalMapping = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public TextureMappingOptions verticalMapping
		{
			get
			{
				return m_verticalMapping;
			}
			set
			{
				if (m_verticalMapping != value)
				{
					m_havePropertiesChanged = true;
					m_verticalMapping = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public float mappingUvLineOffset
		{
			get
			{
				return m_uvLineOffset;
			}
			set
			{
				if (m_uvLineOffset != value)
				{
					m_havePropertiesChanged = true;
					m_uvLineOffset = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public TextRenderFlags renderMode
		{
			get
			{
				return m_renderMode;
			}
			set
			{
				if (m_renderMode != value)
				{
					m_renderMode = value;
					m_havePropertiesChanged = true;
				}
			}
		}

		public VertexSortingOrder geometrySortingOrder
		{
			get
			{
				return m_geometrySortingOrder;
			}
			set
			{
				m_geometrySortingOrder = value;
				m_havePropertiesChanged = true;
				((Graphic)this).SetVerticesDirty();
			}
		}

		public int firstVisibleCharacter
		{
			get
			{
				return m_firstVisibleCharacter;
			}
			set
			{
				if (m_firstVisibleCharacter != value)
				{
					m_havePropertiesChanged = true;
					m_firstVisibleCharacter = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public int maxVisibleCharacters
		{
			get
			{
				return m_maxVisibleCharacters;
			}
			set
			{
				if (m_maxVisibleCharacters != value)
				{
					m_havePropertiesChanged = true;
					m_maxVisibleCharacters = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public int maxVisibleWords
		{
			get
			{
				return m_maxVisibleWords;
			}
			set
			{
				if (m_maxVisibleWords != value)
				{
					m_havePropertiesChanged = true;
					m_maxVisibleWords = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public int maxVisibleLines
		{
			get
			{
				return m_maxVisibleLines;
			}
			set
			{
				if (m_maxVisibleLines != value)
				{
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					m_maxVisibleLines = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public bool useMaxVisibleDescender
		{
			get
			{
				return m_useMaxVisibleDescender;
			}
			set
			{
				if (m_useMaxVisibleDescender != value)
				{
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public int pageToDisplay
		{
			get
			{
				return m_pageToDisplay;
			}
			set
			{
				if (m_pageToDisplay != value)
				{
					m_havePropertiesChanged = true;
					m_pageToDisplay = value;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public virtual Vector4 margin
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_margin;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				if (!(m_margin == value))
				{
					m_margin = value;
					ComputeMarginSize();
					m_havePropertiesChanged = true;
					((Graphic)this).SetVerticesDirty();
				}
			}
		}

		public TMP_TextInfo textInfo => m_textInfo;

		public bool havePropertiesChanged
		{
			get
			{
				return m_havePropertiesChanged;
			}
			set
			{
				if (m_havePropertiesChanged != value)
				{
					m_havePropertiesChanged = value;
					m_isInputParsingRequired = true;
					((Graphic)this).SetAllDirty();
				}
			}
		}

		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return m_isUsingLegacyAnimationComponent;
			}
			set
			{
				m_isUsingLegacyAnimationComponent = value;
			}
		}

		public Transform transform
		{
			get
			{
				if ((Object)(object)m_transform == (Object)null)
				{
					m_transform = ((Component)this).GetComponent<Transform>();
				}
				return m_transform;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				if ((Object)(object)m_rectTransform == (Object)null)
				{
					m_rectTransform = ((Component)this).GetComponent<RectTransform>();
				}
				return m_rectTransform;
			}
		}

		public virtual bool autoSizeTextContainer { get; set; }

		public virtual Mesh mesh => m_mesh;

		public bool isVolumetricText
		{
			get
			{
				return m_isVolumetricText;
			}
			set
			{
				if (m_isVolumetricText != value)
				{
					m_havePropertiesChanged = value;
					m_textInfo.ResetVertexLayout(value);
					m_isInputParsingRequired = true;
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetLayoutDirty();
				}
			}
		}

		public Bounds bounds
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				if ((Object)(object)m_mesh == (Object)null)
				{
					return default(Bounds);
				}
				return GetCompoundBounds();
			}
		}

		public Bounds textBounds
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (m_textInfo == null)
				{
					return default(Bounds);
				}
				return GetTextBounds();
			}
		}

		protected TMP_SpriteAnimator spriteAnimator
		{
			get
			{
				if ((Object)(object)m_spriteAnimator == (Object)null)
				{
					m_spriteAnimator = ((Component)this).GetComponent<TMP_SpriteAnimator>();
					if ((Object)(object)m_spriteAnimator == (Object)null)
					{
						m_spriteAnimator = ((Component)this).get_gameObject().AddComponent<TMP_SpriteAnimator>();
					}
				}
				return m_spriteAnimator;
			}
		}

		public float flexibleHeight => m_flexibleHeight;

		public float flexibleWidth => m_flexibleWidth;

		public float minWidth => m_minWidth;

		public float minHeight => m_minHeight;

		public float maxWidth => m_maxWidth;

		public float maxHeight => m_maxHeight;

		protected LayoutElement layoutElement
		{
			get
			{
				if ((Object)(object)m_LayoutElement == (Object)null)
				{
					m_LayoutElement = ((Component)this).GetComponent<LayoutElement>();
				}
				return m_LayoutElement;
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				if (!m_isPreferredWidthDirty)
				{
					return m_preferredWidth;
				}
				m_preferredWidth = GetPreferredWidth();
				return m_preferredWidth;
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				if (!m_isPreferredHeightDirty)
				{
					return m_preferredHeight;
				}
				m_preferredHeight = GetPreferredHeight();
				return m_preferredHeight;
			}
		}

		public virtual float renderedWidth => GetRenderedWidth();

		public virtual float renderedHeight => GetRenderedHeight();

		public int layoutPriority => m_layoutPriority;

		protected virtual void LoadFontAsset()
		{
		}

		protected virtual void SetSharedMaterial(Material mat)
		{
		}

		protected virtual Material GetMaterial(Material mat)
		{
			return null;
		}

		protected virtual void SetFontBaseMaterial(Material mat)
		{
		}

		protected virtual Material[] GetSharedMaterials()
		{
			return null;
		}

		protected virtual void SetSharedMaterials(Material[] materials)
		{
		}

		protected virtual Material[] GetMaterials(Material[] mats)
		{
			return null;
		}

		protected virtual Material CreateMaterialInstance(Material source)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Material val = new Material(source);
			val.set_shaderKeywords(source.get_shaderKeywords());
			((Object)val).set_name(((Object)val).get_name() + " (Instance)");
			return val;
		}

		protected void SetVertexColorGradient(TMP_ColorGradient gradient)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)gradient == (Object)null))
			{
				m_fontColorGradient.bottomLeft = gradient.bottomLeft;
				m_fontColorGradient.bottomRight = gradient.bottomRight;
				m_fontColorGradient.topLeft = gradient.topLeft;
				m_fontColorGradient.topRight = gradient.topRight;
				((Graphic)this).SetVerticesDirty();
			}
		}

		protected void SetTextSortingOrder(VertexSortingOrder order)
		{
		}

		protected void SetTextSortingOrder(int[] order)
		{
		}

		protected virtual void SetFaceColor(Color32 color)
		{
		}

		protected virtual void SetOutlineColor(Color32 color)
		{
		}

		protected virtual void SetOutlineThickness(float thickness)
		{
		}

		protected virtual void SetShaderDepth()
		{
		}

		protected virtual void SetCulling()
		{
		}

		protected virtual float GetPaddingForMaterial()
		{
			return 0f;
		}

		protected virtual float GetPaddingForMaterial(Material mat)
		{
			return 0f;
		}

		protected virtual Vector3[] GetTextContainerLocalCorners()
		{
			return null;
		}

		public virtual void ForceMeshUpdate()
		{
		}

		public virtual void ForceMeshUpdate(bool ignoreActiveState)
		{
		}

		internal void SetTextInternal(string text)
		{
			m_text = text;
			m_renderMode = TextRenderFlags.DontRender;
			m_isInputParsingRequired = true;
			ForceMeshUpdate();
			m_renderMode = TextRenderFlags.Render;
		}

		public virtual void UpdateGeometry(Mesh mesh, int index)
		{
		}

		public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
		}

		public virtual void UpdateVertexData()
		{
		}

		public virtual void SetVertices(Vector3[] vertices)
		{
		}

		public virtual void UpdateMeshPadding()
		{
		}

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Graphic)this).CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}

		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			((Graphic)this).CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			InternalCrossFadeAlpha(alpha, duration, ignoreTimeScale);
		}

		protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
		}

		protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
		}

		protected void ParseInputText()
		{
			m_isInputParsingRequired = false;
			switch (m_inputSource)
			{
			case TextInputSources.Text:
			case TextInputSources.String:
				StringToCharArray(m_text, ref m_char_buffer);
				break;
			case TextInputSources.SetText:
				SetTextArrayToCharArray(m_input_CharArray, ref m_char_buffer);
				break;
			}
			SetArraySizes(m_char_buffer);
		}

		public void SetText(string text)
		{
			SetText(text, syncTextInputBox: true);
		}

		public void SetText(string text, bool syncTextInputBox)
		{
			m_inputSource = TextInputSources.SetCharArray;
			StringToCharArray(text, ref m_char_buffer);
			if (syncTextInputBox)
			{
				m_text = text;
			}
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		public void SetText(string text, float arg0)
		{
			SetText(text, arg0, 255f, 255f);
		}

		public void SetText(string text, float arg0, float arg1)
		{
			SetText(text, arg0, arg1, 255f);
		}

		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			int precision = 0;
			int index = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					if (text[i + 2] == ':')
					{
						precision = text[i + 3] - 48;
					}
					switch (text[i + 1])
					{
					case '0':
						AddFloatToCharArray(arg0, ref index, precision);
						break;
					case '1':
						AddFloatToCharArray(arg1, ref index, precision);
						break;
					case '2':
						AddFloatToCharArray(arg2, ref index, precision);
						break;
					}
					i = ((text[i + 2] != ':') ? (i + 2) : (i + 4));
				}
				else
				{
					m_input_CharArray[index] = c;
					index++;
				}
			}
			m_input_CharArray[index] = '\0';
			m_charArray_Length = index;
			m_text = new string(m_input_CharArray, 0, index);
			m_inputSource = TextInputSources.SetText;
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		public void SetText(StringBuilder text)
		{
			m_inputSource = TextInputSources.SetCharArray;
			m_text = text.ToString();
			StringBuilderToIntArray(text, ref m_char_buffer);
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText)
		{
			if (sourceText == null || sourceText.Length == 0)
			{
				return;
			}
			if (m_char_buffer == null)
			{
				m_char_buffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (sourceText[i] == '\\' && i < sourceText.Length - 1)
				{
					switch (sourceText[i + 1])
					{
					case 'n':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						i++;
						writeIndex++;
						continue;
					case 'r':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 13;
						i++;
						writeIndex++;
						continue;
					case 't':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 9;
						i++;
						writeIndex++;
						continue;
					}
				}
				if (sourceText[i] == '<')
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == m_char_buffer.Length)
				{
					ResizeInternalArray(ref m_char_buffer);
				}
				m_char_buffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == m_char_buffer.Length)
			{
				ResizeInternalArray(ref m_char_buffer);
			}
			m_char_buffer[writeIndex] = 0;
			m_inputSource = TextInputSources.SetCharArray;
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText, int start, int length)
		{
			if (sourceText == null || sourceText.Length == 0 || length == 0)
			{
				return;
			}
			if (m_char_buffer == null)
			{
				m_char_buffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			int i = start;
			for (int num = start + length; i < num; i++)
			{
				if (sourceText[i] == '\\' && i < length - 1)
				{
					switch (sourceText[i + 1])
					{
					case 'n':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						i++;
						writeIndex++;
						continue;
					case 'r':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 13;
						i++;
						writeIndex++;
						continue;
					case 't':
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 9;
						i++;
						writeIndex++;
						continue;
					}
				}
				if (sourceText[i] == '<')
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == m_char_buffer.Length)
				{
					ResizeInternalArray(ref m_char_buffer);
				}
				m_char_buffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == m_char_buffer.Length)
			{
				ResizeInternalArray(ref m_char_buffer);
			}
			m_char_buffer[writeIndex] = 0;
			m_inputSource = TextInputSources.SetCharArray;
			m_havePropertiesChanged = true;
			m_isInputParsingRequired = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		public void SetCharArray(int[] sourceText, int start, int length)
		{
			if (sourceText == null || sourceText.Length == 0 || length == 0)
			{
				return;
			}
			if (m_char_buffer == null)
			{
				m_char_buffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			int i = start;
			for (int num = start + length; i < num; i++)
			{
				if (sourceText[i] == 92 && i < length - 1)
				{
					switch (sourceText[i + 1])
					{
					case 110:
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						i++;
						writeIndex++;
						continue;
					case 114:
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 13;
						i++;
						writeIndex++;
						continue;
					case 116:
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 9;
						i++;
						writeIndex++;
						continue;
					}
				}
				if (sourceText[i] == 60)
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == m_char_buffer.Length)
				{
					ResizeInternalArray(ref m_char_buffer);
				}
				m_char_buffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == m_char_buffer.Length)
			{
				ResizeInternalArray(ref m_char_buffer);
			}
			m_char_buffer[writeIndex] = 0;
			m_inputSource = TextInputSources.SetCharArray;
			m_havePropertiesChanged = true;
			m_isInputParsingRequired = true;
			m_isCalculateSizeRequired = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		protected void SetTextArrayToCharArray(char[] sourceText, ref int[] charBuffer)
		{
			if (sourceText == null || m_charArray_Length == 0)
			{
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			for (int i = 0; i < m_charArray_Length; i++)
			{
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					writeIndex++;
					continue;
				}
				if (sourceText[i] == '<')
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = 0;
		}

		protected void StringToCharArray(string sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			m_styleStack.SetDefault(0);
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (m_inputSource == TextInputSources.Text && sourceText[i] == '\\' && sourceText.Length > i + 1)
				{
					switch (sourceText[i + 1])
					{
					case 'U':
						if (sourceText.Length > i + 9)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = GetUTF32(i + 2);
							i += 9;
							writeIndex++;
							continue;
						}
						break;
					case '\\':
						if (m_parseCtrlCharacters && sourceText.Length > i + 2)
						{
							if (writeIndex + 2 > charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = sourceText[i + 1];
							charBuffer[writeIndex + 1] = sourceText[i + 2];
							i += 2;
							writeIndex += 2;
							continue;
						}
						break;
					case 'n':
						if (m_parseCtrlCharacters)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 10;
							i++;
							writeIndex++;
							continue;
						}
						break;
					case 'r':
						if (m_parseCtrlCharacters)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 13;
							i++;
							writeIndex++;
							continue;
						}
						break;
					case 't':
						if (m_parseCtrlCharacters)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 9;
							i++;
							writeIndex++;
							continue;
						}
						break;
					case 'u':
						if (sourceText.Length > i + 5)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = (ushort)GetUTF16(i + 2);
							i += 5;
							writeIndex++;
							continue;
						}
						break;
					}
				}
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					writeIndex++;
					continue;
				}
				if (sourceText[i] == '<' && m_isRichText)
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = 0;
		}

		protected void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			m_styleStack.Clear();
			m_text = sourceText.ToString();
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (m_parseCtrlCharacters && sourceText[i] == '\\' && sourceText.Length > i + 1)
				{
					switch (sourceText[i + 1])
					{
					case 'U':
						if (sourceText.Length > i + 9)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = GetUTF32(i + 2);
							i += 9;
							writeIndex++;
							continue;
						}
						break;
					case '\\':
						if (sourceText.Length > i + 2)
						{
							if (writeIndex + 2 > charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = sourceText[i + 1];
							charBuffer[writeIndex + 1] = sourceText[i + 2];
							i += 2;
							writeIndex += 2;
							continue;
						}
						break;
					case 'n':
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						i++;
						writeIndex++;
						continue;
					case 'r':
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 13;
						i++;
						writeIndex++;
						continue;
					case 't':
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 9;
						i++;
						writeIndex++;
						continue;
					case 'u':
						if (sourceText.Length > i + 5)
						{
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = (ushort)GetUTF16(i + 2);
							i += 5;
							writeIndex++;
							continue;
						}
						break;
					}
				}
				if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
					i++;
					writeIndex++;
					continue;
				}
				if (sourceText[i] == '<')
				{
					if (IsTagName(ref sourceText, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = 0;
		}

		private bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] sourceText2 = style.styleOpeningTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset2 = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset2, ref charBuffer, ref writeIndex))
						{
							i = srcOffset2;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] sourceText2 = style.styleOpeningTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset2 = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset2, ref charBuffer, ref writeIndex))
						{
							i = srcOffset2;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] sourceText2 = style.styleOpeningTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset2 = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset2, ref charBuffer, ref writeIndex))
						{
							i = srcOffset2;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset));
			if (style == null || srcOffset == 0)
			{
				return false;
			}
			m_styleStack.Add(style.hashCode);
			int num = style.styleOpeningTagArray.Length;
			int[] sourceText2 = style.styleOpeningTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset2 = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset2, ref charBuffer, ref writeIndex))
						{
							i = srcOffset2;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(m_styleStack.CurrentItem());
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] sourceText2 = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(m_styleStack.CurrentItem());
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] sourceText2 = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(m_styleStack.CurrentItem());
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] sourceText2 = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			TMP_Style style = TMP_StyleSheet.GetStyle(m_styleStack.CurrentItem());
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] sourceText2 = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = sourceText2[i];
				if (num2 == 60)
				{
					if (IsTagName(ref sourceText2, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText2, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText2, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText2, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool IsTagName(ref string text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref char[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref int[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast((char)text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private bool IsTagName(ref StringBuilder text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private int GetTagHashCode(ref string text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ text[i];
				}
			}
			return num;
		}

		private int GetTagHashCode(ref char[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ text[i];
				}
			}
			return num;
		}

		private int GetTagHashCode(ref int[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != 34)
				{
					if (text[i] == 62)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ text[i];
				}
			}
			return num;
		}

		private int GetTagHashCode(ref StringBuilder text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ text[i];
				}
			}
			return num;
		}

		private void ResizeInternalArray<T>(ref T[] array)
		{
			int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
			Array.Resize(ref array, newSize);
		}

		protected void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				m_input_CharArray[index++] = '-';
				number = 0f - number;
			}
			number += k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			AddIntToCharArray(num, ref index, precision);
			if (precision > 0)
			{
				m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					m_input_CharArray[index++] = (char)(num2 + 48);
					number -= (float)num2;
				}
			}
		}

		protected void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				m_input_CharArray[num++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = m_input_CharArray[index];
				m_input_CharArray[index] = m_input_CharArray[num];
				m_input_CharArray[num] = c;
				index++;
			}
			index = num2;
		}

		protected virtual int SetArraySizes(int[] chars)
		{
			return 0;
		}

		protected virtual void GenerateTextMesh()
		{
		}

		public Vector2 GetPreferredValues()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (m_isInputParsingRequired || m_isTextTruncated)
			{
				m_isCalculatingPreferredValues = true;
				ParseInputText();
			}
			float num = GetPreferredWidth();
			float num2 = GetPreferredHeight();
			return new Vector2(num, num2);
		}

		public Vector2 GetPreferredValues(float width, float height)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (m_isInputParsingRequired || m_isTextTruncated)
			{
				m_isCalculatingPreferredValues = true;
				ParseInputText();
			}
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(width, height);
			float num = GetPreferredWidth(val);
			float num2 = GetPreferredHeight(val);
			return new Vector2(num, num2);
		}

		public Vector2 GetPreferredValues(string text)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			m_isCalculatingPreferredValues = true;
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			Vector2 val = k_LargePositiveVector2;
			float num = GetPreferredWidth(val);
			float num2 = GetPreferredHeight(val);
			return new Vector2(num, num2);
		}

		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			m_isCalculatingPreferredValues = true;
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(width, height);
			float num = GetPreferredWidth(val);
			float num2 = GetPreferredHeight(val);
			return new Vector2(num, num2);
		}

		protected float GetPreferredWidth()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			float defaultFontSize = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			Vector2 marginSize = k_LargePositiveVector2;
			if (m_isInputParsingRequired || m_isTextTruncated)
			{
				m_isCalculatingPreferredValues = true;
				ParseInputText();
			}
			m_recursiveCount = 0;
			float x = CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: true).x;
			m_isPreferredWidthDirty = false;
			return x;
		}

		protected float GetPreferredWidth(Vector2 margin)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			float defaultFontSize = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			m_recursiveCount = 0;
			return CalculatePreferredValues(defaultFontSize, margin, ignoreTextAutoSizing: true).x;
		}

		protected float GetPreferredHeight()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			float defaultFontSize = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			Vector2 marginSize = default(Vector2);
			((Vector2)(ref marginSize))._002Ector((m_marginWidth != 0f) ? m_marginWidth : k_LargePositiveFloat, k_LargePositiveFloat);
			if (m_isInputParsingRequired || m_isTextTruncated)
			{
				m_isCalculatingPreferredValues = true;
				ParseInputText();
			}
			m_recursiveCount = 0;
			float y = CalculatePreferredValues(defaultFontSize, marginSize, !m_enableAutoSizing).y;
			m_isPreferredHeightDirty = false;
			return y;
		}

		protected float GetPreferredHeight(Vector2 margin)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			float defaultFontSize = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			m_recursiveCount = 0;
			return CalculatePreferredValues(defaultFontSize, margin, ignoreTextAutoSizing: true).y;
		}

		public Vector2 GetRenderedValues()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Bounds val = GetTextBounds();
			return Vector2.op_Implicit(((Bounds)(ref val)).get_size());
		}

		public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Bounds val = GetTextBounds(onlyVisibleCharacters);
			return Vector2.op_Implicit(((Bounds)(ref val)).get_size());
		}

		protected float GetRenderedWidth()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return GetRenderedValues().x;
		}

		protected float GetRenderedWidth(bool onlyVisibleCharacters)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return GetRenderedValues(onlyVisibleCharacters).x;
		}

		protected float GetRenderedHeight()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return GetRenderedValues().y;
		}

		protected float GetRenderedHeight(bool onlyVisibleCharacters)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return GetRenderedValues(onlyVisibleCharacters).y;
		}

		protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_13da: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_fontAsset == (Object)null || m_fontAsset.characterDictionary == null)
			{
				Debug.LogWarning((object)("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + ((Object)this).GetInstanceID()));
				return Vector2.get_zero();
			}
			if (m_char_buffer == null || m_char_buffer.Length == 0 || m_char_buffer[0] == 0)
			{
				return Vector2.get_zero();
			}
			m_currentFontAsset = m_fontAsset;
			m_currentMaterial = m_sharedMaterial;
			m_currentMaterialIndex = 0;
			m_materialReferenceStack.SetDefault(new MaterialReference(0, m_currentFontAsset, null, m_currentMaterial, m_padding));
			int totalCharacterCount = m_totalCharacterCount;
			if (m_internalCharacterInfo == null || totalCharacterCount > m_internalCharacterInfo.Length)
			{
				m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount > 1024) ? (totalCharacterCount + 256) : Mathf.NextPowerOfTwo(totalCharacterCount)];
			}
			m_fontScale = defaultFontSize / m_currentFontAsset.fontInfo.PointSize * (m_isOrthographic ? 1f : 0.1f);
			m_fontScaleMultiplier = 1f;
			float num = defaultFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
			float num2 = m_fontScale;
			m_currentFontSize = defaultFontSize;
			m_sizeStack.SetDefault(m_currentFontSize);
			float num3 = 0f;
			int num4 = 0;
			m_style = m_fontStyle;
			m_lineJustification = m_textAlignment;
			m_lineJustificationStack.SetDefault(m_lineJustification);
			float num5 = 1f;
			m_baselineOffset = 0f;
			m_baselineOffsetStack.Clear();
			m_lineOffset = 0f;
			m_lineHeight = -32767f;
			float num6 = m_currentFontAsset.fontInfo.LineHeight - (m_currentFontAsset.fontInfo.Ascender - m_currentFontAsset.fontInfo.Descender);
			m_cSpacing = 0f;
			m_monoSpacing = 0f;
			float num7 = 0f;
			m_xAdvance = 0f;
			float num8 = 0f;
			tag_LineIndent = 0f;
			tag_Indent = 0f;
			m_indentStack.SetDefault(0f);
			tag_NoParsing = false;
			m_characterCount = 0;
			m_firstCharacterOfLine = 0;
			m_maxLineAscender = k_LargeNegativeFloat;
			m_maxLineDescender = k_LargePositiveFloat;
			m_lineNumber = 0;
			float x = marginSize.x;
			m_marginLeft = 0f;
			m_marginRight = 0f;
			m_width = -1f;
			float num9 = 0f;
			float num10 = 0f;
			float num11 = 0f;
			m_isCalculatingPreferredValues = true;
			m_maxAscender = 0f;
			m_maxDescender = 0f;
			bool flag = true;
			bool flag2 = false;
			WordWrapState state = default(WordWrapState);
			SaveWordWrappingState(ref state, 0, 0);
			WordWrapState state2 = default(WordWrapState);
			int num12 = 0;
			m_recursiveCount++;
			int endIndex = 0;
			for (int i = 0; m_char_buffer[i] != 0; i++)
			{
				num4 = m_char_buffer[i];
				m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				m_currentFontAsset = m_materialReferences[m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = m_currentMaterialIndex;
				if (m_isRichText && num4 == 60)
				{
					m_isParsingText = true;
					m_textElementType = TMP_TextElementType.Character;
					if (ValidateHtmlTag(m_char_buffer, i + 1, out endIndex))
					{
						i = endIndex;
						if (m_textElementType == TMP_TextElementType.Character)
						{
							continue;
						}
					}
				}
				m_isParsingText = false;
				bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
				float num13 = 1f;
				if (m_textElementType == TMP_TextElementType.Character)
				{
					if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num4))
						{
							num4 = char.ToUpper((char)num4);
						}
					}
					else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num4))
						{
							num4 = char.ToLower((char)num4);
						}
					}
					else if (((m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num4))
					{
						num13 = 0.8f;
						num4 = char.ToUpper((char)num4);
					}
				}
				if (m_textElementType == TMP_TextElementType.Sprite)
				{
					m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
					m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
					TMP_Sprite tMP_Sprite = m_currentSpriteAsset.spriteInfoList[m_spriteIndex];
					if (tMP_Sprite == null)
					{
						continue;
					}
					if (num4 == 60)
					{
						num4 = 57344 + m_spriteIndex;
					}
					m_currentFontAsset = m_fontAsset;
					float num14 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = m_fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * num14;
					m_cached_TextElement = tMP_Sprite;
					m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
					m_internalCharacterInfo[m_characterCount].scale = num14;
					m_currentMaterialIndex = currentMaterialIndex;
				}
				else if (m_textElementType == TMP_TextElementType.Character)
				{
					m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
					if (m_cached_TextElement == null)
					{
						continue;
					}
					m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
					m_fontScale = m_currentFontSize * num13 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = m_fontScale * m_fontScaleMultiplier * m_cached_TextElement.scale;
					m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
				}
				float num15 = num2;
				if (num4 == 173)
				{
					num2 = 0f;
				}
				m_internalCharacterInfo[m_characterCount].character = (char)num4;
				if (m_enableKerning && m_characterCount >= 1)
				{
					int character = m_internalCharacterInfo[m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num4);
					m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out var value);
					if (value != null)
					{
						m_xAdvance += value.XadvanceOffset * num2;
					}
				}
				float num16 = 0f;
				if (m_monoSpacing != 0f)
				{
					num16 = m_monoSpacing / 2f - (m_cached_TextElement.width / 2f + m_cached_TextElement.xOffset) * num2;
					m_xAdvance += num16;
				}
				num5 = ((m_textElementType != 0 || isUsingAlternateTypeface || ((m_style & FontStyles.Bold) != FontStyles.Bold && (m_fontStyle & FontStyles.Bold) != FontStyles.Bold)) ? 1f : (1f + m_currentFontAsset.boldSpacing * 0.01f));
				m_internalCharacterInfo[m_characterCount].baseLine = 0f - m_lineOffset + m_baselineOffset;
				float num17 = m_currentFontAsset.fontInfo.Ascender * ((m_textElementType == TMP_TextElementType.Character) ? (num2 / num13) : m_internalCharacterInfo[m_characterCount].scale) + m_baselineOffset;
				m_internalCharacterInfo[m_characterCount].ascender = num17 - m_lineOffset;
				m_maxLineAscender = ((num17 > m_maxLineAscender) ? num17 : m_maxLineAscender);
				float num18 = m_currentFontAsset.fontInfo.Descender * ((m_textElementType == TMP_TextElementType.Character) ? (num2 / num13) : m_internalCharacterInfo[m_characterCount].scale) + m_baselineOffset;
				float num19 = (m_internalCharacterInfo[m_characterCount].descender = num18 - m_lineOffset);
				m_maxLineDescender = ((num18 < m_maxLineDescender) ? num18 : m_maxLineDescender);
				if ((m_style & FontStyles.Subscript) == FontStyles.Subscript || (m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num20 = (num17 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
					num17 = m_maxLineAscender;
					m_maxLineAscender = ((num20 > m_maxLineAscender) ? num20 : m_maxLineAscender);
					float num21 = (num18 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
					num18 = m_maxLineDescender;
					m_maxLineDescender = ((num21 < m_maxLineDescender) ? num21 : m_maxLineDescender);
				}
				if (m_lineNumber == 0)
				{
					m_maxAscender = ((m_maxAscender > num17) ? m_maxAscender : num17);
				}
				if (num4 == 9 || (!char.IsWhiteSpace((char)num4) && num4 != 8203) || m_textElementType == TMP_TextElementType.Sprite)
				{
					float num22 = ((m_width != -1f) ? Mathf.Min(x + 0.0001f - m_marginLeft - m_marginRight, m_width) : (x + 0.0001f - m_marginLeft - m_marginRight));
					bool flag3 = (m_lineJustification & (TextAlignmentOptions)16) == (TextAlignmentOptions)16 || (m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8;
					num11 = m_xAdvance + m_cached_TextElement.xAdvance * ((num4 != 173) ? num2 : num15);
					if (num11 > num22 * (flag3 ? 1.05f : 1f))
					{
						if (enableWordWrapping && m_characterCount != m_firstCharacterOfLine)
						{
							if (num12 == state2.previous_WordBreak || flag)
							{
								if (!m_isCharacterWrappingEnabled)
								{
									m_isCharacterWrappingEnabled = true;
								}
								else
								{
									flag2 = true;
								}
							}
							i = RestoreWordWrappingState(ref state2);
							num12 = i;
							if (m_char_buffer[i] == 173)
							{
								m_isTextTruncated = true;
								m_char_buffer[i] = 45;
								CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: true);
								return Vector2.get_zero();
							}
							if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f)
							{
								float num23 = m_maxLineAscender - m_startOfLineAscender;
								m_lineOffset += num23;
								state2.lineOffset = m_lineOffset;
								state2.previousLineAscender = m_maxLineAscender;
							}
							float num24 = m_maxLineAscender - m_lineOffset;
							float num25 = m_maxLineDescender - m_lineOffset;
							m_maxDescender = ((m_maxDescender < num25) ? m_maxDescender : num25);
							m_firstCharacterOfLine = m_characterCount;
							num9 += m_xAdvance;
							num10 = ((!m_enableWordWrapping) ? Mathf.Max(num10, num24 - num25) : (m_maxAscender - m_maxDescender));
							SaveWordWrappingState(ref state, i, m_characterCount - 1);
							m_lineNumber++;
							if (m_lineHeight == -32767f)
							{
								float num26 = m_internalCharacterInfo[m_characterCount].ascender - m_internalCharacterInfo[m_characterCount].baseLine;
								num7 = 0f - m_maxLineDescender + num26 + (num6 + m_lineSpacing + m_lineSpacingDelta) * num;
								m_lineOffset += num7;
								m_startOfLineAscender = num26;
							}
							else
							{
								m_lineOffset += m_lineHeight + m_lineSpacing * num;
							}
							m_maxLineAscender = k_LargeNegativeFloat;
							m_maxLineDescender = k_LargePositiveFloat;
							m_xAdvance = 0f + tag_Indent;
							continue;
						}
						if (!ignoreTextAutoSizing && defaultFontSize > m_fontSizeMin)
						{
							_ = m_charWidthAdjDelta;
							_ = m_charWidthMaxAdj / 100f;
							m_maxFontSize = defaultFontSize;
							defaultFontSize -= Mathf.Max((defaultFontSize - m_minFontSize) / 2f, 0.05f);
							defaultFontSize = (float)(int)(Mathf.Max(defaultFontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
							if (m_recursiveCount > 20)
							{
								return new Vector2(num9, num10);
							}
							return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false);
						}
					}
				}
				if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f && !m_isNewPage)
				{
					float num27 = m_maxLineAscender - m_startOfLineAscender;
					num19 -= num27;
					m_lineOffset += num27;
					m_startOfLineAscender += num27;
					state2.lineOffset = m_lineOffset;
					state2.previousLineAscender = m_startOfLineAscender;
				}
				if (num4 == 9)
				{
					float num28 = m_currentFontAsset.fontInfo.TabWidth * num2;
					float num29 = Mathf.Ceil(m_xAdvance / num28) * num28;
					m_xAdvance = ((num29 > m_xAdvance) ? num29 : (m_xAdvance + num28));
				}
				else if (m_monoSpacing != 0f)
				{
					m_xAdvance += m_monoSpacing - num16 + (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing;
					if (char.IsWhiteSpace((char)num4) || num4 == 8203)
					{
						m_xAdvance += m_wordSpacing * num2;
					}
				}
				else
				{
					m_xAdvance += (m_cached_TextElement.xAdvance * num5 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing;
					if (char.IsWhiteSpace((char)num4) || num4 == 8203)
					{
						m_xAdvance += m_wordSpacing * num2;
					}
				}
				if (num4 == 13)
				{
					num8 = Mathf.Max(num8, num9 + m_xAdvance);
					num9 = 0f;
					m_xAdvance = 0f + tag_Indent;
				}
				if (num4 == 10 || m_characterCount == totalCharacterCount - 1)
				{
					if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f)
					{
						float num30 = m_maxLineAscender - m_startOfLineAscender;
						num19 -= num30;
						m_lineOffset += num30;
					}
					float num31 = m_maxLineDescender - m_lineOffset;
					m_maxDescender = ((m_maxDescender < num31) ? m_maxDescender : num31);
					m_firstCharacterOfLine = m_characterCount + 1;
					if (num4 == 10 && m_characterCount != totalCharacterCount - 1)
					{
						num8 = Mathf.Max(num8, num9 + num11);
						num9 = 0f;
					}
					else
					{
						num9 = Mathf.Max(num8, num9 + num11);
					}
					num10 = m_maxAscender - m_maxDescender;
					if (num4 == 10)
					{
						SaveWordWrappingState(ref state, i, m_characterCount);
						SaveWordWrappingState(ref state2, i, m_characterCount);
						m_lineNumber++;
						if (m_lineHeight == -32767f)
						{
							num7 = 0f - m_maxLineDescender + num17 + (num6 + m_lineSpacing + m_paragraphSpacing + m_lineSpacingDelta) * num;
							m_lineOffset += num7;
						}
						else
						{
							m_lineOffset += m_lineHeight + (m_lineSpacing + m_paragraphSpacing) * num;
						}
						m_maxLineAscender = k_LargeNegativeFloat;
						m_maxLineDescender = k_LargePositiveFloat;
						m_startOfLineAscender = num17;
						m_xAdvance = 0f + tag_LineIndent + tag_Indent;
						m_characterCount++;
						continue;
					}
				}
				if (m_enableWordWrapping || m_overflowMode == TextOverflowModes.Truncate || m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((char.IsWhiteSpace((char)num4) || num4 == 8203 || num4 == 45 || num4 == 173) && !m_isNonBreakingSpace && num4 != 160 && num4 != 8209 && num4 != 8239 && num4 != 8288)
					{
						SaveWordWrappingState(ref state2, i, m_characterCount);
						m_isCharacterWrappingEnabled = false;
						flag = false;
					}
					else if (((num4 > 4352 && num4 < 4607) || (num4 > 11904 && num4 < 40959) || (num4 > 43360 && num4 < 43391) || (num4 > 44032 && num4 < 55295) || (num4 > 63744 && num4 < 64255) || (num4 > 65072 && num4 < 65103) || (num4 > 65280 && num4 < 65519)) && !m_isNonBreakingSpace)
					{
						if (flag || flag2 || (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num4) && m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_internalCharacterInfo[m_characterCount + 1].character)))
						{
							SaveWordWrappingState(ref state2, i, m_characterCount);
							m_isCharacterWrappingEnabled = false;
							flag = false;
						}
					}
					else if (flag || m_isCharacterWrappingEnabled || flag2)
					{
						SaveWordWrappingState(ref state2, i, m_characterCount);
					}
				}
				m_characterCount++;
			}
			num3 = m_maxFontSize - m_minFontSize;
			if (!m_isCharacterWrappingEnabled && !ignoreTextAutoSizing && num3 > 0.051f && defaultFontSize < m_fontSizeMax)
			{
				m_minFontSize = defaultFontSize;
				defaultFontSize += Mathf.Max((m_maxFontSize - defaultFontSize) / 2f, 0.05f);
				defaultFontSize = (float)(int)(Mathf.Min(defaultFontSize, m_fontSizeMax) * 20f + 0.5f) / 20f;
				if (m_recursiveCount > 20)
				{
					return new Vector2(num9, num10);
				}
				return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false);
			}
			m_isCharacterWrappingEnabled = false;
			m_isCalculatingPreferredValues = false;
			num9 += ((m_margin.x > 0f) ? m_margin.x : 0f);
			num9 += ((m_margin.z > 0f) ? m_margin.z : 0f);
			num10 += ((m_margin.y > 0f) ? m_margin.y : 0f);
			num10 += ((m_margin.w > 0f) ? m_margin.w : 0f);
			num9 = (float)(int)(num9 * 100f + 1f) / 100f;
			num10 = (float)(int)(num10 * 100f + 1f) / 100f;
			return new Vector2(num9, num10);
		}

		protected virtual Bounds GetCompoundBounds()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return default(Bounds);
		}

		protected Bounds GetTextBounds()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			if (m_textInfo == null || m_textInfo.characterCount > m_textInfo.characterInfo.Length)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
			for (int i = 0; i < m_textInfo.characterCount && i < m_textInfo.characterInfo.Length; i++)
			{
				if (m_textInfo.characterInfo[i].isVisible)
				{
					extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[i].bottomLeft.x);
					extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[i].descender);
					extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[i].xAdvance);
					extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[i].ascender);
				}
			}
			Vector2 val = default(Vector2);
			val.x = extents.max.x - extents.min.x;
			val.y = extents.max.y - extents.min.y;
			return new Bounds(Vector2.op_Implicit((extents.min + extents.max) / 2f), Vector2.op_Implicit(val));
		}

		protected Bounds GetTextBounds(bool onlyVisibleCharacters)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			if (m_textInfo == null)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
			for (int i = 0; i < m_textInfo.characterCount && !((i > maxVisibleCharacters || m_textInfo.characterInfo[i].lineNumber > m_maxVisibleLines) && onlyVisibleCharacters); i++)
			{
				if (!onlyVisibleCharacters || m_textInfo.characterInfo[i].isVisible)
				{
					extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[i].origin);
					extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[i].descender);
					extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[i].xAdvance);
					extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[i].ascender);
				}
			}
			Vector2 val = default(Vector2);
			val.x = extents.max.x - extents.min.x;
			val.y = extents.max.y - extents.min.y;
			return new Bounds(Vector2.op_Implicit((extents.min + extents.max) / 2f), Vector2.op_Implicit(val));
		}

		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		protected void ResizeLineExtents(int size)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size + 1));
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < m_textInfo.lineInfo.Length)
				{
					array[i] = m_textInfo.lineInfo[i];
					continue;
				}
				array[i].lineExtents.min = k_LargePositiveVector2;
				array[i].lineExtents.max = k_LargeNegativeVector2;
				array[i].ascender = k_LargeNegativeFloat;
				array[i].descender = k_LargePositiveFloat;
			}
			m_textInfo.lineInfo = array;
		}

		public virtual TMP_TextInfo GetTextInfo(string text)
		{
			return null;
		}

		protected virtual void ComputeMarginSize()
		{
		}

		protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
		{
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			state.currentFontAsset = m_currentFontAsset;
			state.currentSpriteAsset = m_currentSpriteAsset;
			state.currentMaterial = m_currentMaterial;
			state.currentMaterialIndex = m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = m_lineVisibleCharacterCount;
			state.visible_LinkCount = m_textInfo.linkCount;
			state.firstCharacterIndex = m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = m_lastVisibleCharacterOfLine;
			state.fontStyle = m_style;
			state.fontScale = m_fontScale;
			state.fontScaleMultiplier = m_fontScaleMultiplier;
			state.currentFontSize = m_currentFontSize;
			state.xAdvance = m_xAdvance;
			state.maxCapHeight = m_maxCapHeight;
			state.maxAscender = m_maxAscender;
			state.maxDescender = m_maxDescender;
			state.maxLineAscender = m_maxLineAscender;
			state.maxLineDescender = m_maxLineDescender;
			state.previousLineAscender = m_startOfLineAscender;
			state.preferredWidth = m_preferredWidth;
			state.preferredHeight = m_preferredHeight;
			state.meshExtents = m_meshExtents;
			state.lineNumber = m_lineNumber;
			state.lineOffset = m_lineOffset;
			state.baselineOffset = m_baselineOffset;
			state.vertexColor = m_htmlColor;
			state.underlineColor = m_underlineColor;
			state.strikethroughColor = m_strikethroughColor;
			state.highlightColor = m_highlightColor;
			state.isNonBreakingSpace = m_isNonBreakingSpace;
			state.tagNoParsing = tag_NoParsing;
			state.basicStyleStack = m_fontStyleStack;
			state.colorStack = m_colorStack;
			state.underlineColorStack = m_underlineColorStack;
			state.strikethroughColorStack = m_strikethroughColorStack;
			state.highlightColorStack = m_highlightColorStack;
			state.colorGradientStack = m_colorGradientStack;
			state.sizeStack = m_sizeStack;
			state.indentStack = m_indentStack;
			state.fontWeightStack = m_fontWeightStack;
			state.styleStack = m_styleStack;
			state.baselineStack = m_baselineOffsetStack;
			state.actionStack = m_actionStack;
			state.materialReferenceStack = m_materialReferenceStack;
			state.lineJustificationStack = m_lineJustificationStack;
			state.spriteAnimationID = m_spriteAnimationID;
			if (m_lineNumber < m_textInfo.lineInfo.Length)
			{
				state.lineInfo = m_textInfo.lineInfo[m_lineNumber];
			}
		}

		protected int RestoreWordWrappingState(ref WordWrapState state)
		{
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			int previous_WordBreak = state.previous_WordBreak;
			m_currentFontAsset = state.currentFontAsset;
			m_currentSpriteAsset = state.currentSpriteAsset;
			m_currentMaterial = state.currentMaterial;
			m_currentMaterialIndex = state.currentMaterialIndex;
			m_characterCount = state.total_CharacterCount + 1;
			m_lineVisibleCharacterCount = state.visible_CharacterCount;
			m_textInfo.linkCount = state.visible_LinkCount;
			m_firstCharacterOfLine = state.firstCharacterIndex;
			m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			m_style = state.fontStyle;
			m_fontScale = state.fontScale;
			m_fontScaleMultiplier = state.fontScaleMultiplier;
			m_currentFontSize = state.currentFontSize;
			m_xAdvance = state.xAdvance;
			m_maxCapHeight = state.maxCapHeight;
			m_maxAscender = state.maxAscender;
			m_maxDescender = state.maxDescender;
			m_maxLineAscender = state.maxLineAscender;
			m_maxLineDescender = state.maxLineDescender;
			m_startOfLineAscender = state.previousLineAscender;
			m_preferredWidth = state.preferredWidth;
			m_preferredHeight = state.preferredHeight;
			m_meshExtents = state.meshExtents;
			m_lineNumber = state.lineNumber;
			m_lineOffset = state.lineOffset;
			m_baselineOffset = state.baselineOffset;
			m_htmlColor = state.vertexColor;
			m_underlineColor = state.underlineColor;
			m_strikethroughColor = state.strikethroughColor;
			m_highlightColor = state.highlightColor;
			m_isNonBreakingSpace = state.isNonBreakingSpace;
			tag_NoParsing = state.tagNoParsing;
			m_fontStyleStack = state.basicStyleStack;
			m_colorStack = state.colorStack;
			m_underlineColorStack = state.underlineColorStack;
			m_strikethroughColorStack = state.strikethroughColorStack;
			m_highlightColorStack = state.highlightColorStack;
			m_colorGradientStack = state.colorGradientStack;
			m_sizeStack = state.sizeStack;
			m_indentStack = state.indentStack;
			m_fontWeightStack = state.fontWeightStack;
			m_styleStack = state.styleStack;
			m_baselineOffsetStack = state.baselineStack;
			m_actionStack = state.actionStack;
			m_materialReferenceStack = state.materialReferenceStack;
			m_lineJustificationStack = state.lineJustificationStack;
			m_spriteAnimationID = state.spriteAnimationID;
			if (m_lineNumber < m_textInfo.lineInfo.Length)
			{
				m_textInfo.lineInfo[m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
			m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
			vertexColor.a = ((m_fontColor32.a < vertexColor.a) ? m_fontColor32.a : vertexColor.a);
			if (!m_enableVertexGradient)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!m_overrideHtmlColors && m_colorStack.index > 1)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
			}
			else if ((Object)(object)m_fontColorGradientPreset != (Object)null)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = Color32.op_Implicit(m_fontColorGradientPreset.bottomLeft * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = Color32.op_Implicit(m_fontColorGradientPreset.topLeft * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = Color32.op_Implicit(m_fontColorGradientPreset.topRight * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = Color32.op_Implicit(m_fontColorGradientPreset.bottomRight * Color32.op_Implicit(vertexColor));
			}
			else
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = Color32.op_Implicit(m_fontColorGradient.bottomLeft * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = Color32.op_Implicit(m_fontColorGradient.topLeft * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = Color32.op_Implicit(m_fontColorGradient.topRight * Color32.op_Implicit(vertexColor));
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = Color32.op_Implicit(m_fontColorGradient.bottomRight * Color32.op_Implicit(vertexColor));
			}
			if ((Object)(object)m_colorGradientPreset != (Object)null)
			{
				ref Color32 reference = ref m_textInfo.characterInfo[m_characterCount].vertex_BL.color;
				reference = Color32.op_Implicit(Color32.op_Implicit(reference) * m_colorGradientPreset.bottomLeft);
				ref Color32 reference2 = ref m_textInfo.characterInfo[m_characterCount].vertex_TL.color;
				reference2 = Color32.op_Implicit(Color32.op_Implicit(reference2) * m_colorGradientPreset.topLeft);
				ref Color32 reference3 = ref m_textInfo.characterInfo[m_characterCount].vertex_TR.color;
				reference3 = Color32.op_Implicit(Color32.op_Implicit(reference3) * m_colorGradientPreset.topRight);
				ref Color32 reference4 = ref m_textInfo.characterInfo[m_characterCount].vertex_BR.color;
				reference4 = Color32.op_Implicit(Color32.op_Implicit(reference4) * m_colorGradientPreset.bottomRight);
			}
			if (!m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = m_currentFontAsset.fontInfo;
			Vector2 val = default(Vector2);
			val.x = (m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			val.y = 1f - (m_cached_TextElement.y + padding + style_padding + m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 val2 = default(Vector2);
			val2.x = val.x;
			val2.y = 1f - (m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 val3 = default(Vector2);
			val3.x = (m_cached_TextElement.x + padding + style_padding + m_cached_TextElement.width) / fontInfo.AtlasWidth;
			val3.y = val2.y;
			Vector2 uv = default(Vector2);
			uv.x = val3.x;
			uv.y = val.y;
			m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = val;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = val2;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = val3;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv;
		}

		protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0573: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
			if (m_tintAllSprites)
			{
				m_tintSprite = true;
			}
			Color32 val = (m_tintSprite ? m_spriteColor.Multiply(vertexColor) : m_spriteColor);
			val.a = ((val.a < m_fontColor32.a) ? (val.a = ((val.a < vertexColor.a) ? val.a : vertexColor.a)) : m_fontColor32.a);
			if (!m_enableVertexGradient)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = val;
			}
			else if (!m_overrideHtmlColors && m_colorStack.index > 1)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = val;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = val;
			}
			else if ((Object)(object)m_fontColorGradientPreset != (Object)null)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradientPreset.bottomLeft)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradientPreset.topLeft)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradientPreset.topRight)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradientPreset.bottomRight)) : val);
			}
			else
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradient.bottomLeft)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradient.topLeft)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradient.topRight)) : val);
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = (m_tintSprite ? val.Multiply(Color32.op_Implicit(m_fontColorGradient.bottomRight)) : val);
			}
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(m_cached_TextElement.x / (float)m_currentSpriteAsset.spriteSheet.get_width(), m_cached_TextElement.y / (float)m_currentSpriteAsset.spriteSheet.get_height());
			Vector2 val3 = default(Vector2);
			((Vector2)(ref val3))._002Ector(val2.x, (m_cached_TextElement.y + m_cached_TextElement.height) / (float)m_currentSpriteAsset.spriteSheet.get_height());
			Vector2 val4 = default(Vector2);
			((Vector2)(ref val4))._002Ector((m_cached_TextElement.x + m_cached_TextElement.width) / (float)m_currentSpriteAsset.spriteSheet.get_width(), val3.y);
			Vector2 uv = default(Vector2);
			((Vector2)(ref uv))._002Ector(val4.x, val2.y);
			m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = val2;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = val3;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = val4;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			if (isVolumetric)
			{
				Vector3 val = default(Vector3);
				((Vector3)(ref val))._002Ector(0f, 0f, m_fontSize * m_fontScale);
				m_textInfo.meshInfo[materialReferenceIndex].vertices[4 + index_X4] = characterInfo[i].vertex_BL.position + val;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[5 + index_X4] = characterInfo[i].vertex_TL.position + val;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[6 + index_X4] = characterInfo[i].vertex_TR.position + val;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[7 + index_X4] = characterInfo[i].vertex_BR.position + val;
			}
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			if (isVolumetric)
			{
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[4 + index_X4] = characterInfo[i].vertex_BL.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[5 + index_X4] = characterInfo[i].vertex_TL.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[6 + index_X4] = characterInfo[i].vertex_TR.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[7 + index_X4] = characterInfo[i].vertex_BR.uv;
			}
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			if (isVolumetric)
			{
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[4 + index_X4] = characterInfo[i].vertex_BL.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[5 + index_X4] = characterInfo[i].vertex_TL.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[6 + index_X4] = characterInfo[i].vertex_TR.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[7 + index_X4] = characterInfo[i].vertex_BR.uv2;
			}
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			if (isVolumetric)
			{
				Color32 val2 = default(Color32);
				((Color32)(ref val2))._002Ector(byte.MaxValue, byte.MaxValue, (byte)128, byte.MaxValue);
				m_textInfo.meshInfo[materialReferenceIndex].colors32[4 + index_X4] = val2;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[5 + index_X4] = val2;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[6 + index_X4] = val2;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[7 + index_X4] = val2;
			}
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + ((!isVolumetric) ? 4 : 8);
		}

		protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_060e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_064a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0651: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_0727: Unknown result type (might be due to invalid IL or missing references)
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_077a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0798: Unknown result type (might be due to invalid IL or missing references)
			//IL_079a: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0805: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_0812: Unknown result type (might be due to invalid IL or missing references)
			//IL_0814: Unknown result type (might be due to invalid IL or missing references)
			//IL_081e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			if (m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning((object)"Unable to add underline since the Font Asset doesn't contain the underline character.", (Object)(object)this);
				}
				return;
			}
			int num = index + 12;
			if (num > m_textInfo.meshInfo[0].vertices.Length)
			{
				m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < m_cached_Underline_GlyphInfo.width * maxScale)
			{
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = m_padding * startScale / maxScale;
			float num4 = m_padding * endScale / maxScale;
			float height = m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(0f - num2, m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(0f - num2, (0f - (height + m_padding)) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 10] = end + new Vector3(0f, m_padding * maxScale, 0f);
			vertices[index + 11] = end + new Vector3(0f, (0f - (height + m_padding)) * maxScale, 0f);
			Vector2[] uvs = m_textInfo.meshInfo[0].uvs0;
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector((m_cached_Underline_GlyphInfo.x - num3) / m_fontAsset.fontInfo.AtlasWidth, 1f - (m_cached_Underline_GlyphInfo.y + m_padding + m_cached_Underline_GlyphInfo.height) / m_fontAsset.fontInfo.AtlasHeight);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(val.x, 1f - (m_cached_Underline_GlyphInfo.y - m_padding) / m_fontAsset.fontInfo.AtlasHeight);
			Vector2 val3 = default(Vector2);
			((Vector2)(ref val3))._002Ector((m_cached_Underline_GlyphInfo.x - num3 + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, val2.y);
			Vector2 val4 = default(Vector2);
			((Vector2)(ref val4))._002Ector(val3.x, val.y);
			Vector2 val5 = default(Vector2);
			((Vector2)(ref val5))._002Ector((m_cached_Underline_GlyphInfo.x + num4 + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, val2.y);
			Vector2 val6 = default(Vector2);
			((Vector2)(ref val6))._002Ector(val5.x, val.y);
			Vector2 val7 = default(Vector2);
			((Vector2)(ref val7))._002Ector((m_cached_Underline_GlyphInfo.x + num4 + m_cached_Underline_GlyphInfo.width) / m_fontAsset.fontInfo.AtlasWidth, val2.y);
			Vector2 val8 = default(Vector2);
			((Vector2)(ref val8))._002Ector(val7.x, val.y);
			uvs[index] = val;
			uvs[1 + index] = val2;
			uvs[2 + index] = val3;
			uvs[3 + index] = val4;
			uvs[4 + index] = new Vector2(val3.x - val3.x * 0.001f, val.y);
			uvs[5 + index] = new Vector2(val3.x - val3.x * 0.001f, val2.y);
			uvs[6 + index] = new Vector2(val3.x + val3.x * 0.001f, val2.y);
			uvs[7 + index] = new Vector2(val3.x + val3.x * 0.001f, val.y);
			uvs[8 + index] = val6;
			uvs[9 + index] = val5;
			uvs[10 + index] = val7;
			uvs[11 + index] = val8;
			float num5 = 0f;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float scale = Mathf.Abs(sdfScale);
			Vector2[] uvs2 = m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = PackUV(0f, 0f, scale);
			uvs2[1 + index] = PackUV(0f, 1f, scale);
			uvs2[2 + index] = PackUV(x, 1f, scale);
			uvs2[3 + index] = PackUV(x, 0f, scale);
			num5 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = PackUV(num5, 0f, scale);
			uvs2[5 + index] = PackUV(num5, 1f, scale);
			uvs2[6 + index] = PackUV(x, 1f, scale);
			uvs2[7 + index] = PackUV(x, 0f, scale);
			num5 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = PackUV(num5, 0f, scale);
			uvs2[9 + index] = PackUV(num5, 1f, scale);
			uvs2[10 + index] = PackUV(1f, 1f, scale);
			uvs2[11 + index] = PackUV(1f, 0f, scale);
			Color32[] colors = m_textInfo.meshInfo[0].colors32;
			colors[index] = underlineColor;
			colors[1 + index] = underlineColor;
			colors[2 + index] = underlineColor;
			colors[3 + index] = underlineColor;
			colors[4 + index] = underlineColor;
			colors[5 + index] = underlineColor;
			colors[6 + index] = underlineColor;
			colors[7 + index] = underlineColor;
			colors[8 + index] = underlineColor;
			colors[9 + index] = underlineColor;
			colors[10 + index] = underlineColor;
			colors[11 + index] = underlineColor;
			index += 12;
		}

		protected virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			if (m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning((object)"Unable to add underline since the Font Asset doesn't contain the underline character.", (Object)(object)this);
				}
				return;
			}
			int num = index + 4;
			if (num > m_textInfo.meshInfo[0].vertices.Length)
			{
				m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			Vector3[] vertices = m_textInfo.meshInfo[0].vertices;
			vertices[index] = start;
			vertices[index + 1] = new Vector3(start.x, end.y, 0f);
			vertices[index + 2] = end;
			vertices[index + 3] = new Vector3(end.x, start.y, 0f);
			Vector2[] uvs = m_textInfo.meshInfo[0].uvs0;
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector((m_cached_Underline_GlyphInfo.x + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, 1f - (m_cached_Underline_GlyphInfo.y + m_cached_Underline_GlyphInfo.height / 2f) / m_fontAsset.fontInfo.AtlasHeight);
			uvs[index] = val;
			uvs[1 + index] = val;
			uvs[2 + index] = val;
			uvs[3 + index] = val;
			Vector2[] uvs2 = m_textInfo.meshInfo[0].uvs2;
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(0f, 1f);
			uvs2[index] = val2;
			uvs2[1 + index] = val2;
			uvs2[2 + index] = val2;
			uvs2[3 + index] = val2;
			Color32[] colors = m_textInfo.meshInfo[0].colors32;
			highlightColor.a = ((m_htmlColor.a < highlightColor.a) ? m_htmlColor.a : highlightColor.a);
			colors[index] = highlightColor;
			colors[1 + index] = highlightColor;
			colors[2 + index] = highlightColor;
			colors[3 + index] = highlightColor;
			index += 4;
		}

		protected void LoadDefaultSettings()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			if (m_text == null)
			{
				if (TMP_Settings.autoSizeTextContainer)
				{
					autoSizeTextContainer = true;
				}
				else
				{
					m_rectTransform = rectTransform;
					if (((object)this).GetType() == typeof(TextMeshPro))
					{
						m_rectTransform.set_sizeDelta(TMP_Settings.defaultTextMeshProTextContainerSize);
					}
					else
					{
						m_rectTransform.set_sizeDelta(TMP_Settings.defaultTextMeshProUITextContainerSize);
					}
				}
				m_enableWordWrapping = TMP_Settings.enableWordWrapping;
				m_enableKerning = TMP_Settings.enableKerning;
				m_enableExtraPadding = TMP_Settings.enableExtraPadding;
				m_tintAllSprites = TMP_Settings.enableTintAllSprites;
				m_parseCtrlCharacters = TMP_Settings.enableParseEscapeCharacters;
				m_fontSize = (m_fontSizeBase = TMP_Settings.defaultFontSize);
				m_fontSizeMin = m_fontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
				m_fontSizeMax = m_fontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
				m_isAlignmentEnumConverted = true;
			}
			else if (!m_isAlignmentEnumConverted)
			{
				m_isAlignmentEnumConverted = true;
				m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(m_textAlignment);
			}
		}

		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			fontAsset.characterDictionary.TryGetValue(95, out m_cached_Underline_GlyphInfo);
			fontAsset.characterDictionary.TryGetValue(8230, out m_cached_Ellipsis_GlyphInfo);
		}

		protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
		{
			chars[insertionIndex] = c;
			for (int i = insertionIndex + tagLength; i < chars.Length; i++)
			{
				chars[i - 3] = chars[i];
			}
		}

		protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
		{
			bool num = (m_style & FontStyles.Italic) == FontStyles.Italic || (m_fontStyle & FontStyles.Italic) == FontStyles.Italic;
			TMP_FontAsset tMP_FontAsset = null;
			int num2 = fontWeight / 100;
			if (num)
			{
				return m_currentFontAsset.fontWeights[num2].italicTypeface;
			}
			return m_currentFontAsset.fontWeights[num2].regularTypeface;
		}

		protected virtual void SetActiveSubMeshes(bool state)
		{
		}

		protected virtual void ClearSubMeshObjects()
		{
		}

		public virtual void ClearMesh()
		{
		}

		public virtual void ClearMesh(bool uploadGeometry)
		{
		}

		public virtual string GetParsedText()
		{
			if (m_textInfo == null)
			{
				return string.Empty;
			}
			int characterCount = m_textInfo.characterCount;
			char[] array = new char[characterCount];
			for (int i = 0; i < characterCount && i < m_textInfo.characterInfo.Length; i++)
			{
				array[i] = m_textInfo.characterInfo[i].character;
			}
			return new string(array);
		}

		protected Vector2 PackUV(float x, float y, float scale)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = default(Vector2);
			val.x = (int)(x * 511f);
			val.y = (int)(y * 511f);
			val.x = val.x * 4096f + val.y;
			val.y = scale;
			return val;
		}

		protected float PackUV(float x, float y)
		{
			double num = (int)(x * 511f);
			double num2 = (int)(y * 511f);
			return (float)(num * 4096.0 + num2);
		}

		protected int HexToInt(char hex)
		{
			return hex switch
			{
				'0' => 0, 
				'1' => 1, 
				'2' => 2, 
				'3' => 3, 
				'4' => 4, 
				'5' => 5, 
				'6' => 6, 
				'7' => 7, 
				'8' => 8, 
				'9' => 9, 
				'A' => 10, 
				'B' => 11, 
				'C' => 12, 
				'D' => 13, 
				'E' => 14, 
				'F' => 15, 
				'a' => 10, 
				'b' => 11, 
				'c' => 12, 
				'd' => 13, 
				'e' => 14, 
				'f' => 15, 
				_ => 15, 
			};
		}

		protected int GetUTF16(int i)
		{
			return (HexToInt(m_text[i]) << 12) + (HexToInt(m_text[i + 1]) << 8) + (HexToInt(m_text[i + 2]) << 4) + HexToInt(m_text[i + 3]);
		}

		protected int GetUTF32(int i)
		{
			return 0 + (HexToInt(m_text[i]) << 30) + (HexToInt(m_text[i + 1]) << 24) + (HexToInt(m_text[i + 2]) << 20) + (HexToInt(m_text[i + 3]) << 16) + (HexToInt(m_text[i + 4]) << 12) + (HexToInt(m_text[i + 5]) << 8) + (HexToInt(m_text[i + 6]) << 4) + HexToInt(m_text[i + 7]);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			switch (tagCount)
			{
			case 4:
			{
				byte num8 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
				byte b19 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
				byte b20 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
				return new Color32(num8, b19, b20, byte.MaxValue);
			}
			case 5:
			{
				byte num7 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
				byte b16 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
				byte b17 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
				byte b18 = (byte)(HexToInt(hexChars[4]) * 16 + HexToInt(hexChars[4]));
				return new Color32(num7, b16, b17, b18);
			}
			case 7:
			{
				byte num6 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
				byte b14 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
				byte b15 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
				return new Color32(num6, b14, b15, byte.MaxValue);
			}
			case 9:
			{
				byte num5 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
				byte b11 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
				byte b12 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
				byte b13 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
				return new Color32(num5, b11, b12, b13);
			}
			case 10:
			{
				byte num4 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
				byte b9 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
				byte b10 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
				return new Color32(num4, b9, b10, byte.MaxValue);
			}
			case 11:
			{
				byte num3 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
				byte b6 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
				byte b7 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
				byte b8 = (byte)(HexToInt(hexChars[10]) * 16 + HexToInt(hexChars[10]));
				return new Color32(num3, b6, b7, b8);
			}
			case 13:
			{
				byte num2 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
				byte b4 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
				byte b5 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
				return new Color32(num2, b4, b5, byte.MaxValue);
			}
			case 15:
			{
				byte num = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
				byte b = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
				byte b2 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
				byte b3 = (byte)(HexToInt(hexChars[13]) * 16 + HexToInt(hexChars[14]));
				return new Color32(num, b, b2, b3);
			}
			default:
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			switch (length)
			{
			case 7:
			{
				byte num2 = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
				byte b4 = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
				byte b5 = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
				return new Color32(num2, b4, b5, byte.MaxValue);
			}
			case 9:
			{
				byte num = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
				byte b = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
				byte b2 = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
				byte b3 = (byte)(HexToInt(hexChars[startIndex + 7]) * 16 + HexToInt(hexChars[startIndex + 8]));
				return new Color32(num, b, b2, b3);
			}
			default:
				return s_colorWhite;
			}
		}

		private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
		{
			int lastIndex = startIndex;
			int num = 0;
			while (lastIndex < startIndex + length)
			{
				parameters[num] = ConvertToFloat(chars, startIndex, length, out lastIndex);
				length -= lastIndex - startIndex + 1;
				startIndex = lastIndex + 1;
				num++;
			}
			return num;
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length)
		{
			int lastIndex = 0;
			return ConvertToFloat(chars, startIndex, length, out lastIndex);
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
		{
			if (startIndex == 0)
			{
				lastIndex = 0;
				return -9999f;
			}
			int num = startIndex + length;
			bool flag = true;
			float num2 = 0f;
			int num3 = 1;
			if (chars[startIndex] == '+')
			{
				num3 = 1;
				startIndex++;
			}
			else if (chars[startIndex] == '-')
			{
				num3 = -1;
				startIndex++;
			}
			float num4 = 0f;
			for (int i = startIndex; i < num; i++)
			{
				uint num5 = chars[i];
				if (num5 < 48 || num5 > 57)
				{
					switch (num5)
					{
					case 46u:
						break;
					case 44u:
						if (i + 1 < num && chars[i + 1] == ' ')
						{
							lastIndex = i + 1;
						}
						else
						{
							lastIndex = i;
						}
						return num4;
					default:
						continue;
					}
				}
				if (num5 == 46)
				{
					flag = false;
					num2 = 0.1f;
				}
				else if (flag)
				{
					num4 = num4 * 10f + (float)((num5 - 48) * num3);
				}
				else
				{
					num4 += (float)(num5 - 48) * num2 * (float)num3;
					num2 *= 0.1f;
				}
			}
			lastIndex = num;
			return num4;
		}

		protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0648: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Unknown result type (might be due to invalid IL or missing references)
			//IL_067e: Unknown result type (might be due to invalid IL or missing references)
			//IL_068a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1352: Unknown result type (might be due to invalid IL or missing references)
			//IL_1357: Unknown result type (might be due to invalid IL or missing references)
			//IL_1360: Unknown result type (might be due to invalid IL or missing references)
			//IL_1365: Unknown result type (might be due to invalid IL or missing references)
			//IL_1371: Unknown result type (might be due to invalid IL or missing references)
			//IL_141f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1424: Unknown result type (might be due to invalid IL or missing references)
			//IL_142d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1432: Unknown result type (might be due to invalid IL or missing references)
			//IL_143e: Unknown result type (might be due to invalid IL or missing references)
			//IL_145c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1461: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1509: Unknown result type (might be due to invalid IL or missing references)
			//IL_150e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2711: Unknown result type (might be due to invalid IL or missing references)
			//IL_2716: Unknown result type (might be due to invalid IL or missing references)
			//IL_2722: Unknown result type (might be due to invalid IL or missing references)
			//IL_2748: Unknown result type (might be due to invalid IL or missing references)
			//IL_274d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2759: Unknown result type (might be due to invalid IL or missing references)
			//IL_277f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2784: Unknown result type (might be due to invalid IL or missing references)
			//IL_2790: Unknown result type (might be due to invalid IL or missing references)
			//IL_27b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_27bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_27c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_286d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2872: Unknown result type (might be due to invalid IL or missing references)
			//IL_2877: Unknown result type (might be due to invalid IL or missing references)
			//IL_2883: Unknown result type (might be due to invalid IL or missing references)
			//IL_2890: Unknown result type (might be due to invalid IL or missing references)
			//IL_2895: Unknown result type (might be due to invalid IL or missing references)
			//IL_289a: Unknown result type (might be due to invalid IL or missing references)
			//IL_28a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_28b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_28b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_28c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_28d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_28db: Unknown result type (might be due to invalid IL or missing references)
			//IL_28e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_28ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_28f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_28fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_2903: Unknown result type (might be due to invalid IL or missing references)
			//IL_290f: Unknown result type (might be due to invalid IL or missing references)
			//IL_292c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2931: Unknown result type (might be due to invalid IL or missing references)
			//IL_293d: Unknown result type (might be due to invalid IL or missing references)
			//IL_295b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2960: Unknown result type (might be due to invalid IL or missing references)
			//IL_296c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2979: Unknown result type (might be due to invalid IL or missing references)
			//IL_297e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2983: Unknown result type (might be due to invalid IL or missing references)
			//IL_298f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bec: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bf1: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f59: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3129: Unknown result type (might be due to invalid IL or missing references)
			//IL_312e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3755: Unknown result type (might be due to invalid IL or missing references)
			//IL_375a: Unknown result type (might be due to invalid IL or missing references)
			//IL_376b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3770: Unknown result type (might be due to invalid IL or missing references)
			//IL_3775: Unknown result type (might be due to invalid IL or missing references)
			//IL_37c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_37d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_37de: Unknown result type (might be due to invalid IL or missing references)
			//IL_37e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_37e8: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			m_xmlAttribute[num2].nameHashCode = 0;
			m_xmlAttribute[num2].valueType = TagType.None;
			m_xmlAttribute[num2].valueHashCode = 0;
			m_xmlAttribute[num2].valueStartIndex = 0;
			m_xmlAttribute[num2].valueLength = 0;
			m_xmlAttribute[1].nameHashCode = 0;
			m_xmlAttribute[2].nameHashCode = 0;
			m_xmlAttribute[3].nameHashCode = 0;
			m_xmlAttribute[4].nameHashCode = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			for (int i = startIndex; i < chars.Length && chars[i] != 0; i++)
			{
				if (num >= m_htmlTag.Length)
				{
					break;
				}
				if (chars[i] == 60)
				{
					break;
				}
				if (chars[i] == 62)
				{
					flag2 = true;
					endIndex = i;
					m_htmlTag[num] = '\0';
					break;
				}
				m_htmlTag[num] = (char)chars[i];
				num++;
				if (b == 1)
				{
					switch (tagType)
					{
					case TagType.None:
						if (chars[i] == 43 || chars[i] == 45 || chars[i] == 46 || (chars[i] >= 48 && chars[i] <= 57))
						{
							tagType = TagType.NumericalValue;
							m_xmlAttribute[num2].valueType = TagType.NumericalValue;
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueLength++;
						}
						else if (chars[i] == 35)
						{
							tagType = TagType.ColorValue;
							m_xmlAttribute[num2].valueType = TagType.ColorValue;
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueLength++;
						}
						else if (chars[i] == 34)
						{
							tagType = TagType.StringValue;
							m_xmlAttribute[num2].valueType = TagType.StringValue;
							m_xmlAttribute[num2].valueStartIndex = num;
						}
						else
						{
							tagType = TagType.StringValue;
							m_xmlAttribute[num2].valueType = TagType.StringValue;
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ chars[i];
							m_xmlAttribute[num2].valueLength++;
						}
						break;
					case TagType.NumericalValue:
						if (chars[i] == 112 || chars[i] == 101 || chars[i] == 37 || chars[i] == 32)
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							m_xmlAttribute[num2].nameHashCode = 0;
							m_xmlAttribute[num2].valueType = TagType.None;
							m_xmlAttribute[num2].valueHashCode = 0;
							m_xmlAttribute[num2].valueStartIndex = 0;
							m_xmlAttribute[num2].valueLength = 0;
							if (chars[i] == 101)
							{
								tagUnits = TagUnits.FontUnits;
							}
							else if (chars[i] == 37)
							{
								tagUnits = TagUnits.Percentage;
							}
						}
						else if (b != 2)
						{
							m_xmlAttribute[num2].valueLength++;
						}
						break;
					case TagType.ColorValue:
						if (chars[i] != 32)
						{
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						b = 2;
						tagType = TagType.None;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagType.None;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
						break;
					case TagType.StringValue:
						if (chars[i] != 34)
						{
							m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ chars[i];
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						b = 2;
						tagType = TagType.None;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagType.None;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
						break;
					}
				}
				if (chars[i] == 61)
				{
					b = 1;
				}
				if (b == 0 && chars[i] == 32)
				{
					if (flag)
					{
						return false;
					}
					flag = true;
					b = 2;
					tagType = TagType.None;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagType.None;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
				}
				if (b == 0)
				{
					m_xmlAttribute[num2].nameHashCode = (m_xmlAttribute[num2].nameHashCode << 3) - m_xmlAttribute[num2].nameHashCode + chars[i];
				}
				if (b == 2 && chars[i] == 32)
				{
					b = 0;
				}
			}
			if (!flag2)
			{
				return false;
			}
			if (tag_NoParsing && m_xmlAttribute[0].nameHashCode != 53822163 && m_xmlAttribute[0].nameHashCode != 49429939)
			{
				return false;
			}
			if (m_xmlAttribute[0].nameHashCode == 53822163 || m_xmlAttribute[0].nameHashCode == 49429939)
			{
				tag_NoParsing = false;
				return true;
			}
			if (m_htmlTag[0] == '#' && num == 4)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[0] == '#' && num == 5)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[0] == '#' && num == 7)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[0] == '#' && num == 9)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			float num3 = 0f;
			Material material;
			switch (m_xmlAttribute[0].nameHashCode)
			{
			case 66:
			case 98:
				m_style |= FontStyles.Bold;
				m_fontStyleStack.Add(FontStyles.Bold);
				m_fontWeightInternal = 700;
				m_fontWeightStack.Add(700);
				return true;
			case 395:
			case 427:
				if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
				{
					m_fontWeightInternal = m_fontWeightStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Bold) == 0)
					{
						m_style &= (FontStyles)(-2);
					}
				}
				return true;
			case 73:
			case 105:
				m_style |= FontStyles.Italic;
				m_fontStyleStack.Add(FontStyles.Italic);
				return true;
			case 402:
			case 434:
				if (m_fontStyleStack.Remove(FontStyles.Italic) == 0)
				{
					m_style &= (FontStyles)(-3);
				}
				return true;
			case 83:
			case 115:
				m_style |= FontStyles.Strikethrough;
				m_fontStyleStack.Add(FontStyles.Strikethrough);
				if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
				{
					m_strikethroughColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				}
				else
				{
					m_strikethroughColor = m_htmlColor;
				}
				m_strikethroughColorStack.Add(m_strikethroughColor);
				return true;
			case 412:
			case 444:
				if ((m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
				{
					m_style &= (FontStyles)(-65);
				}
				return true;
			case 85:
			case 117:
				m_style |= FontStyles.Underline;
				m_fontStyleStack.Add(FontStyles.Underline);
				if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
				{
					m_underlineColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				}
				else
				{
					m_underlineColor = m_htmlColor;
				}
				m_underlineColorStack.Add(m_underlineColor);
				return true;
			case 414:
			case 446:
				if ((m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
				{
					m_underlineColor = m_underlineColorStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Underline) == 0)
					{
						m_style &= (FontStyles)(-5);
					}
				}
				return true;
			case 30245:
			case 43045:
				m_style |= FontStyles.Highlight;
				m_fontStyleStack.Add(FontStyles.Highlight);
				m_highlightColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				m_highlightColorStack.Add(m_highlightColor);
				return true;
			case 143092:
			case 155892:
				if ((m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
				{
					m_highlightColor = m_highlightColorStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
					{
						m_style &= (FontStyles)(-513);
					}
				}
				return true;
			case 4728:
			case 6552:
				m_fontScaleMultiplier *= ((m_currentFontAsset.fontInfo.SubSize > 0f) ? m_currentFontAsset.fontInfo.SubSize : 1f);
				m_baselineOffsetStack.Push(m_baselineOffset);
				m_baselineOffset += m_currentFontAsset.fontInfo.SubscriptOffset * m_fontScale * m_fontScaleMultiplier;
				m_fontStyleStack.Add(FontStyles.Subscript);
				m_style |= FontStyles.Subscript;
				return true;
			case 20849:
			case 22673:
				if ((m_style & FontStyles.Subscript) == FontStyles.Subscript)
				{
					if (m_fontScaleMultiplier < 1f)
					{
						m_baselineOffset = m_baselineOffsetStack.Pop();
						m_fontScaleMultiplier /= ((m_currentFontAsset.fontInfo.SubSize > 0f) ? m_currentFontAsset.fontInfo.SubSize : 1f);
					}
					if (m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
					{
						m_style &= (FontStyles)(-257);
					}
				}
				return true;
			case 4742:
			case 6566:
				m_fontScaleMultiplier *= ((m_currentFontAsset.fontInfo.SubSize > 0f) ? m_currentFontAsset.fontInfo.SubSize : 1f);
				m_baselineOffsetStack.Push(m_baselineOffset);
				m_baselineOffset += m_currentFontAsset.fontInfo.SuperscriptOffset * m_fontScale * m_fontScaleMultiplier;
				m_fontStyleStack.Add(FontStyles.Superscript);
				m_style |= FontStyles.Superscript;
				return true;
			case 20863:
			case 22687:
				if ((m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					if (m_fontScaleMultiplier < 1f)
					{
						m_baselineOffset = m_baselineOffsetStack.Pop();
						m_fontScaleMultiplier /= ((m_currentFontAsset.fontInfo.SubSize > 0f) ? m_currentFontAsset.fontInfo.SubSize : 1f);
					}
					if (m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
					{
						m_style &= (FontStyles)(-129);
					}
				}
				return true;
			case -330774850:
			case 2012149182:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				if ((m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
				{
					return true;
				}
				m_style &= (FontStyles)(-2);
				switch ((int)num3)
				{
				case 100:
					m_fontWeightInternal = 100;
					break;
				case 200:
					m_fontWeightInternal = 200;
					break;
				case 300:
					m_fontWeightInternal = 300;
					break;
				case 400:
					m_fontWeightInternal = 400;
					break;
				case 500:
					m_fontWeightInternal = 500;
					break;
				case 600:
					m_fontWeightInternal = 600;
					break;
				case 700:
					m_fontWeightInternal = 700;
					m_style |= FontStyles.Bold;
					break;
				case 800:
					m_fontWeightInternal = 800;
					break;
				case 900:
					m_fontWeightInternal = 900;
					break;
				}
				m_fontWeightStack.Add(m_fontWeightInternal);
				return true;
			case -1885698441:
			case 457225591:
				m_fontWeightInternal = m_fontWeightStack.Remove();
				if (m_fontWeightInternal == 400)
				{
					m_style &= (FontStyles)(-2);
				}
				return true;
			case 4556:
			case 6380:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_xAdvance = num3;
					return true;
				case TagUnits.FontUnits:
					m_xAdvance = num3 * m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					return true;
				case TagUnits.Percentage:
					m_xAdvance = m_marginWidth * num3 / 100f;
					return true;
				default:
					return false;
				}
			case 20677:
			case 22501:
				m_isIgnoringAlignment = false;
				return true;
			case 11642281:
			case 16034505:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_baselineOffset = num3;
					return true;
				case TagUnits.FontUnits:
					m_baselineOffset = num3 * m_fontScale * m_fontAsset.fontInfo.Ascender;
					return true;
				case TagUnits.Percentage:
					return false;
				default:
					return false;
				}
			case 50348802:
			case 54741026:
				m_baselineOffset = 0f;
				return true;
			case 31191:
			case 43991:
				if (m_overflowMode == TextOverflowModes.Page)
				{
					m_xAdvance = 0f + tag_LineIndent + tag_Indent;
					m_lineOffset = 0f;
					m_pageNumber++;
					m_isNewPage = true;
				}
				return true;
			case 31169:
			case 43969:
				m_isNonBreakingSpace = true;
				return true;
			case 144016:
			case 156816:
				m_isNonBreakingSpace = false;
				return true;
			case 32745:
			case 45545:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					if (m_htmlTag[5] == '+')
					{
						m_currentFontSize = m_fontSize + num3;
						m_sizeStack.Add(m_currentFontSize);
						m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
						return true;
					}
					if (m_htmlTag[5] == '-')
					{
						m_currentFontSize = m_fontSize + num3;
						m_sizeStack.Add(m_currentFontSize);
						m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
						return true;
					}
					m_currentFontSize = num3;
					m_sizeStack.Add(m_currentFontSize);
					m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					return true;
				case TagUnits.FontUnits:
					m_currentFontSize = m_fontSize * num3;
					m_sizeStack.Add(m_currentFontSize);
					m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					return true;
				case TagUnits.Percentage:
					m_currentFontSize = m_fontSize * num3 / 100f;
					m_sizeStack.Add(m_currentFontSize);
					m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					return true;
				default:
					return false;
				}
			case 145592:
			case 158392:
				m_currentFontSize = m_sizeStack.Remove();
				m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
				return true;
			case 28511:
			case 41311:
			{
				int valueHashCode3 = m_xmlAttribute[0].valueHashCode;
				int nameHashCode = m_xmlAttribute[1].nameHashCode;
				int valueHashCode = m_xmlAttribute[1].valueHashCode;
				if (valueHashCode3 == 764638571 || valueHashCode3 == 523367755)
				{
					m_currentFontAsset = m_materialReferences[0].fontAsset;
					m_currentMaterial = m_materialReferences[0].material;
					m_currentMaterialIndex = 0;
					m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
					m_materialReferenceStack.Add(m_materialReferences[0]);
					return true;
				}
				if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode3, out var fontAsset))
				{
					fontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					if ((Object)(object)fontAsset == (Object)null)
					{
						return false;
					}
					MaterialReferenceManager.AddFontAsset(fontAsset);
				}
				if (nameHashCode == 0 && valueHashCode == 0)
				{
					m_currentMaterial = fontAsset.material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				else
				{
					if (nameHashCode != 103415287 && nameHashCode != 72669687)
					{
						return false;
					}
					if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
					{
						m_currentMaterial = material;
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
						m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
					}
					else
					{
						material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength));
						if ((Object)(object)material == (Object)null)
						{
							return false;
						}
						MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
						m_currentMaterial = material;
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
						m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
					}
				}
				m_currentFontAsset = fontAsset;
				m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
				return true;
			}
			case 141358:
			case 154158:
			{
				MaterialReference materialReference2 = m_materialReferenceStack.Remove();
				m_currentFontAsset = materialReference2.fontAsset;
				m_currentMaterial = materialReference2.material;
				m_currentMaterialIndex = materialReference2.index;
				m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
				return true;
			}
			case 72669687:
			case 103415287:
			{
				int valueHashCode = m_xmlAttribute[0].valueHashCode;
				if (valueHashCode == 764638571 || valueHashCode == 523367755)
				{
					if (((Object)m_currentFontAsset.atlas).GetInstanceID() != ((Object)m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
					{
						return false;
					}
					m_currentMaterial = m_materialReferences[0].material;
					m_currentMaterialIndex = 0;
					m_materialReferenceStack.Add(m_materialReferences[0]);
					return true;
				}
				if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
				{
					if (((Object)m_currentFontAsset.atlas).GetInstanceID() != ((Object)material.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
					{
						return false;
					}
					m_currentMaterial = material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				else
				{
					material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					if ((Object)(object)material == (Object)null)
					{
						return false;
					}
					if (((Object)m_currentFontAsset.atlas).GetInstanceID() != ((Object)material.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
					{
						return false;
					}
					MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
					m_currentMaterial = material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				return true;
			}
			case 343615334:
			case 374360934:
			{
				if (((Object)m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() != ((Object)m_materialReferenceStack.PreviousItem().material.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
				{
					return false;
				}
				MaterialReference materialReference = m_materialReferenceStack.Remove();
				m_currentMaterial = materialReference.material;
				m_currentMaterialIndex = materialReference.index;
				return true;
			}
			case 230446:
			case 320078:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_xAdvance += num3;
					return true;
				case TagUnits.FontUnits:
					m_xAdvance += num3 * m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					return true;
				case TagUnits.Percentage:
					return false;
				default:
					return false;
				}
			case 186622:
			case 276254:
				if (m_xmlAttribute[0].valueLength != 3)
				{
					return false;
				}
				m_htmlColor.a = (byte)(HexToInt(m_htmlTag[7]) * 16 + HexToInt(m_htmlTag[8]));
				return true;
			case 1750458:
				return false;
			case 426:
				return true;
			case 30266:
			case 43066:
				if (m_isParsingText && !m_isCalculatingPreferredValues)
				{
					int linkCount = m_textInfo.linkCount;
					if (linkCount + 1 > m_textInfo.linkInfo.Length)
					{
						TMP_TextInfo.Resize(ref m_textInfo.linkInfo, linkCount + 1);
					}
					m_textInfo.linkInfo[linkCount].textComponent = this;
					m_textInfo.linkInfo[linkCount].hashCode = m_xmlAttribute[0].valueHashCode;
					m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = m_characterCount;
					m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + m_xmlAttribute[0].valueStartIndex;
					m_textInfo.linkInfo[linkCount].linkIdLength = m_xmlAttribute[0].valueLength;
					m_textInfo.linkInfo[linkCount].SetLinkID(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				}
				return true;
			case 143113:
			case 155913:
				if (m_isParsingText && !m_isCalculatingPreferredValues)
				{
					m_textInfo.linkInfo[m_textInfo.linkCount].linkTextLength = m_characterCount - m_textInfo.linkInfo[m_textInfo.linkCount].linkTextfirstCharacterIndex;
					m_textInfo.linkCount++;
				}
				return true;
			case 186285:
			case 275917:
				switch (m_xmlAttribute[0].valueHashCode)
				{
				case 3774683:
					m_lineJustification = TextAlignmentOptions.Left;
					m_lineJustificationStack.Add(m_lineJustification);
					return true;
				case 136703040:
					m_lineJustification = TextAlignmentOptions.Right;
					m_lineJustificationStack.Add(m_lineJustification);
					return true;
				case -458210101:
					m_lineJustification = TextAlignmentOptions.Center;
					m_lineJustificationStack.Add(m_lineJustification);
					return true;
				case -523808257:
					m_lineJustification = TextAlignmentOptions.Justified;
					m_lineJustificationStack.Add(m_lineJustification);
					return true;
				case 122383428:
					m_lineJustification = TextAlignmentOptions.Flush;
					m_lineJustificationStack.Add(m_lineJustification);
					return true;
				default:
					return false;
				}
			case 976214:
			case 1065846:
				m_lineJustification = m_lineJustificationStack.Remove();
				return true;
			case 237918:
			case 327550:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_width = num3;
					break;
				case TagUnits.FontUnits:
					return false;
				case TagUnits.Percentage:
					m_width = m_marginWidth * num3 / 100f;
					break;
				}
				return true;
			case 1027847:
			case 1117479:
				m_width = -1f;
				return true;
			case 192323:
			case 281955:
				if (m_htmlTag[6] == '#' && num == 10)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					return true;
				}
				if (m_htmlTag[6] == '#' && num == 11)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					return true;
				}
				if (m_htmlTag[6] == '#' && num == 13)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					return true;
				}
				if (m_htmlTag[6] == '#' && num == 15)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					return true;
				}
				switch (m_xmlAttribute[0].valueHashCode)
				{
				case 125395:
					m_htmlColor = Color32.op_Implicit(Color.get_red());
					m_colorStack.Add(m_htmlColor);
					return true;
				case 3573310:
					m_htmlColor = Color32.op_Implicit(Color.get_blue());
					m_colorStack.Add(m_htmlColor);
					return true;
				case 117905991:
					m_htmlColor = Color32.op_Implicit(Color.get_black());
					m_colorStack.Add(m_htmlColor);
					return true;
				case 121463835:
					m_htmlColor = Color32.op_Implicit(Color.get_green());
					m_colorStack.Add(m_htmlColor);
					return true;
				case 140357351:
					m_htmlColor = Color32.op_Implicit(Color.get_white());
					m_colorStack.Add(m_htmlColor);
					return true;
				case 26556144:
					m_htmlColor = new Color32(byte.MaxValue, (byte)128, (byte)0, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					return true;
				case -36881330:
					m_htmlColor = new Color32((byte)160, (byte)32, (byte)240, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					return true;
				case 554054276:
					m_htmlColor = Color32.op_Implicit(Color.get_yellow());
					m_colorStack.Add(m_htmlColor);
					return true;
				default:
					return false;
				}
			case 69403544:
			case 100149144:
			{
				int valueHashCode5 = m_xmlAttribute[0].valueHashCode;
				if (MaterialReferenceManager.TryGetColorGradientPreset(valueHashCode5, out var gradientPreset))
				{
					m_colorGradientPreset = gradientPreset;
				}
				else
				{
					if ((Object)(object)gradientPreset == (Object)null)
					{
						gradientPreset = Resources.Load<TMP_ColorGradient>(TMP_Settings.defaultColorGradientPresetsPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					}
					if ((Object)(object)gradientPreset == (Object)null)
					{
						return false;
					}
					MaterialReferenceManager.AddColorGradientPreset(valueHashCode5, gradientPreset);
					m_colorGradientPreset = gradientPreset;
				}
				m_colorGradientStack.Add(m_colorGradientPreset);
				return true;
			}
			case 340349191:
			case 371094791:
				m_colorGradientPreset = m_colorGradientStack.Remove();
				return true;
			case 1356515:
			case 1983971:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_cSpacing = num3;
					break;
				case TagUnits.FontUnits:
					m_cSpacing = num3;
					m_cSpacing *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					return false;
				}
				return true;
			case 6886018:
			case 7513474:
				if (!m_isParsingText)
				{
					return true;
				}
				if (m_characterCount > 0)
				{
					m_xAdvance -= m_cSpacing;
					m_textInfo.characterInfo[m_characterCount - 1].xAdvance = m_xAdvance;
				}
				m_cSpacing = 0f;
				return true;
			case 1524585:
			case 2152041:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					m_monoSpacing = num3;
					break;
				case TagUnits.FontUnits:
					m_monoSpacing = num3;
					m_monoSpacing *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					return false;
				}
				return true;
			case 7054088:
			case 7681544:
				m_monoSpacing = 0f;
				return true;
			case 280416:
				return false;
			case 982252:
			case 1071884:
				m_htmlColor = m_colorStack.Remove();
				return true;
			case 1441524:
			case 2068980:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					tag_Indent = num3;
					break;
				case TagUnits.FontUnits:
					tag_Indent = num3;
					tag_Indent *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					tag_Indent = m_marginWidth * num3 / 100f;
					break;
				}
				m_indentStack.Add(tag_Indent);
				m_xAdvance = tag_Indent;
				return true;
			case 6971027:
			case 7598483:
				tag_Indent = m_indentStack.Remove();
				return true;
			case -842656867:
			case 1109386397:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				switch (tagUnits)
				{
				case TagUnits.Pixels:
					tag_LineIndent = num3;
					break;
				case TagUnits.FontUnits:
					tag_LineIndent = num3;
					tag_LineIndent *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					tag_LineIndent = m_marginWidth * num3 / 100f;
					break;
				}
				m_xAdvance += tag_LineIndent;
				return true;
			case -445537194:
			case 1897386838:
				tag_LineIndent = 0f;
				return true;
			case 1619421:
			case 2246877:
			{
				int valueHashCode4 = m_xmlAttribute[0].valueHashCode;
				m_spriteIndex = -1;
				TMP_SpriteAsset tMP_SpriteAsset;
				if (m_xmlAttribute[0].valueType == TagType.None || m_xmlAttribute[0].valueType == TagType.NumericalValue)
				{
					if ((Object)(object)m_spriteAsset != (Object)null)
					{
						m_currentSpriteAsset = m_spriteAsset;
					}
					else if ((Object)(object)m_defaultSpriteAsset != (Object)null)
					{
						m_currentSpriteAsset = m_defaultSpriteAsset;
					}
					else if ((Object)(object)m_defaultSpriteAsset == (Object)null)
					{
						if ((Object)(object)TMP_Settings.defaultSpriteAsset != (Object)null)
						{
							m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
						}
						else
						{
							m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
						}
						m_currentSpriteAsset = m_defaultSpriteAsset;
					}
					if ((Object)(object)m_currentSpriteAsset == (Object)null)
					{
						return false;
					}
				}
				else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode4, out tMP_SpriteAsset))
				{
					m_currentSpriteAsset = tMP_SpriteAsset;
				}
				else
				{
					if ((Object)(object)tMP_SpriteAsset == (Object)null)
					{
						tMP_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					}
					if ((Object)(object)tMP_SpriteAsset == (Object)null)
					{
						return false;
					}
					MaterialReferenceManager.AddSpriteAsset(valueHashCode4, tMP_SpriteAsset);
					m_currentSpriteAsset = tMP_SpriteAsset;
				}
				if (m_xmlAttribute[0].valueType == TagType.NumericalValue)
				{
					int num6 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
					if (num6 == -9999)
					{
						return false;
					}
					if (num6 > m_currentSpriteAsset.spriteInfoList.Count - 1)
					{
						return false;
					}
					m_spriteIndex = num6;
				}
				m_spriteColor = s_colorWhite;
				m_tintSprite = false;
				for (int k = 0; k < m_xmlAttribute.Length && m_xmlAttribute[k].nameHashCode != 0; k++)
				{
					int nameHashCode2 = m_xmlAttribute[k].nameHashCode;
					int num7 = 0;
					switch (nameHashCode2)
					{
					case 30547:
					case 43347:
						num7 = m_currentSpriteAsset.GetSpriteIndexFromHashcode(m_xmlAttribute[k].valueHashCode);
						if (num7 == -1)
						{
							return false;
						}
						m_spriteIndex = num7;
						break;
					case 205930:
					case 295562:
						num7 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
						if (num7 == -9999)
						{
							return false;
						}
						if (num7 > m_currentSpriteAsset.spriteInfoList.Count - 1)
						{
							return false;
						}
						m_spriteIndex = num7;
						break;
					case 33019:
					case 45819:
						m_tintSprite = ConvertToFloat(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength) != 0f;
						break;
					case 192323:
					case 281955:
						m_spriteColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength);
						break;
					case 26705:
					case 39505:
						if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength, ref m_attributeParameterValues) != 3)
						{
							return false;
						}
						m_spriteIndex = (int)m_attributeParameterValues[0];
						if (m_isParsingText)
						{
							spriteAnimator.DoSpriteAnimation(m_characterCount, m_currentSpriteAsset, m_spriteIndex, (int)m_attributeParameterValues[1], (int)m_attributeParameterValues[2]);
						}
						break;
					default:
						if (nameHashCode2 != 2246877 && nameHashCode2 != 1619421)
						{
							return false;
						}
						break;
					}
				}
				if (m_spriteIndex == -1)
				{
					return false;
				}
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
				m_textElementType = TMP_TextElementType.Sprite;
				return true;
			}
			case 514803617:
			case 730022849:
				m_style |= FontStyles.LowerCase;
				m_fontStyleStack.Add(FontStyles.LowerCase);
				return true;
			case -1883544150:
			case -1668324918:
				if (m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
				{
					m_style &= (FontStyles)(-9);
				}
				return true;
			case 9133802:
			case 13526026:
			case 566686826:
			case 781906058:
				m_style |= FontStyles.UpperCase;
				m_fontStyleStack.Add(FontStyles.UpperCase);
				return true;
			case -1831660941:
			case -1616441709:
			case 47840323:
			case 52232547:
				if (m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
				{
					m_style &= (FontStyles)(-17);
				}
				return true;
			case 551025096:
			case 766244328:
				m_style |= FontStyles.SmallCaps;
				m_fontStyleStack.Add(FontStyles.SmallCaps);
				return true;
			case -1847322671:
			case -1632103439:
				if (m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
				{
					m_style &= (FontStyles)(-33);
				}
				return true;
			case 1482398:
			case 2109854:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				m_marginLeft = num3;
				switch (tagUnits)
				{
				case TagUnits.FontUnits:
					m_marginLeft *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * m_marginLeft / 100f;
					break;
				}
				m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
				m_marginRight = m_marginLeft;
				return true;
			case 7011901:
			case 7639357:
				m_marginLeft = 0f;
				m_marginRight = 0f;
				return true;
			case -855002522:
			case 1100728678:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				m_marginLeft = num3;
				switch (tagUnits)
				{
				case TagUnits.FontUnits:
					m_marginLeft *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * m_marginLeft / 100f;
					break;
				}
				m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
				return true;
			case -1690034531:
			case -884817987:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				m_marginRight = num3;
				switch (tagUnits)
				{
				case TagUnits.FontUnits:
					m_marginRight *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
					break;
				case TagUnits.Percentage:
					m_marginRight = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * m_marginRight / 100f;
					break;
				}
				m_marginRight = ((m_marginRight >= 0f) ? m_marginRight : 0f);
				return true;
			case -842693512:
			case 1109349752:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f || num3 == 0f)
				{
					return false;
				}
				m_lineHeight = num3;
				switch (tagUnits)
				{
				case TagUnits.FontUnits:
					m_lineHeight *= m_fontAsset.fontInfo.LineHeight * m_fontScale;
					break;
				case TagUnits.Percentage:
					m_lineHeight = m_fontAsset.fontInfo.LineHeight * m_lineHeight / 100f * m_fontScale;
					break;
				}
				return true;
			case -445573839:
			case 1897350193:
				m_lineHeight = -32767f;
				return true;
			case 10723418:
			case 15115642:
				tag_NoParsing = true;
				return true;
			case 1286342:
			case 1913798:
			{
				int valueHashCode2 = m_xmlAttribute[0].valueHashCode;
				if (m_isParsingText)
				{
					m_actionStack.Add(valueHashCode2);
					Debug.Log((object)("Action ID: [" + valueHashCode2 + "] First character index: " + m_characterCount));
				}
				return true;
			}
			case 6815845:
			case 7443301:
				if (m_isParsingText)
				{
					Debug.Log((object)("Action ID: [" + m_actionStack.CurrentItem() + "] Last character index: " + (m_characterCount - 1)));
				}
				m_actionStack.Remove();
				return true;
			case 226050:
			case 315682:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				m_FXMatrix = Matrix4x4.TRS(Vector3.get_zero(), Quaternion.get_identity(), new Vector3(num3, 1f, 1f));
				m_isFXMatrixSet = true;
				return true;
			case 1015979:
			case 1105611:
				m_isFXMatrixSet = false;
				return true;
			case 1600507:
			case 2227963:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -9999f)
				{
					return false;
				}
				m_FXMatrix = Matrix4x4.TRS(Vector3.get_zero(), Quaternion.Euler(0f, 0f, num3), Vector3.get_one());
				m_isFXMatrixSet = true;
				return true;
			case 7130010:
			case 7757466:
				m_isFXMatrixSet = false;
				return true;
			case 227814:
			case 317446:
				if (m_xmlAttribute[1].nameHashCode == 327550)
				{
					float num5 = ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
					switch (tagUnits)
					{
					case TagUnits.Pixels:
						Debug.Log((object)("Table width = " + num5 + "px."));
						break;
					case TagUnits.FontUnits:
						Debug.Log((object)("Table width = " + num5 + "em."));
						break;
					case TagUnits.Percentage:
						Debug.Log((object)("Table width = " + num5 + "%."));
						break;
					}
				}
				return true;
			case 1017743:
			case 1107375:
				return true;
			case 670:
			case 926:
				return true;
			case 2973:
			case 3229:
				return true;
			case 660:
			case 916:
				return true;
			case 2963:
			case 3219:
				return true;
			case 656:
			case 912:
			{
				for (int j = 1; j < m_xmlAttribute.Length && m_xmlAttribute[j].nameHashCode != 0; j++)
				{
					switch (m_xmlAttribute[j].nameHashCode)
					{
					case 327550:
					{
						float num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength);
						switch (tagUnits)
						{
						case TagUnits.Pixels:
							Debug.Log((object)("Table width = " + num4 + "px."));
							break;
						case TagUnits.FontUnits:
							Debug.Log((object)("Table width = " + num4 + "em."));
							break;
						case TagUnits.Percentage:
							Debug.Log((object)("Table width = " + num4 + "%."));
							break;
						}
						break;
					}
					case 275917:
						switch (m_xmlAttribute[j].valueHashCode)
						{
						case 3774683:
							Debug.Log((object)"TD align=\"left\".");
							break;
						case 136703040:
							Debug.Log((object)"TD align=\"right\".");
							break;
						case -458210101:
							Debug.Log((object)"TD align=\"center\".");
							break;
						case -523808257:
							Debug.Log((object)"TD align=\"justified\".");
							break;
						}
						break;
					}
				}
				return true;
			}
			case 2959:
			case 3215:
				return true;
			default:
				return false;
			}
		}

		public TMP_Text()
			: this()
		{
		}//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)

	}
}
