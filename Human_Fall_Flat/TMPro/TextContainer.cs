using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Text Container")]
	public class TextContainer : UIBehaviour
	{
		private bool m_hasChanged;

		[SerializeField]
		private Vector2 m_pivot;

		[SerializeField]
		private TextContainerAnchors m_anchorPosition = TextContainerAnchors.Middle;

		[SerializeField]
		private Rect m_rect;

		private bool m_isDefaultWidth;

		private bool m_isDefaultHeight;

		private bool m_isAutoFitting;

		private Vector3[] m_corners = (Vector3[])(object)new Vector3[4];

		private Vector3[] m_worldCorners = (Vector3[])(object)new Vector3[4];

		[SerializeField]
		private Vector4 m_margins;

		private RectTransform m_rectTransform;

		private static Vector2 k_defaultSize = new Vector2(100f, 100f);

		private TextMeshPro m_textMeshPro;

		public bool hasChanged
		{
			get
			{
				return m_hasChanged;
			}
			set
			{
				m_hasChanged = value;
			}
		}

		public Vector2 pivot
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_pivot;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				if (m_pivot != value)
				{
					m_pivot = value;
					m_anchorPosition = GetAnchorPosition(m_pivot);
					m_hasChanged = true;
					OnContainerChanged();
				}
			}
		}

		public TextContainerAnchors anchorPosition
		{
			get
			{
				return m_anchorPosition;
			}
			set
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				if (m_anchorPosition != value)
				{
					m_anchorPosition = value;
					m_pivot = GetPivot(m_anchorPosition);
					m_hasChanged = true;
					OnContainerChanged();
				}
			}
		}

		public Rect rect
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_rect;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_rect != value)
				{
					m_rect = value;
					m_hasChanged = true;
					OnContainerChanged();
				}
			}
		}

		public Vector2 size
		{
			get
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				return new Vector2(((Rect)(ref m_rect)).get_width(), ((Rect)(ref m_rect)).get_height());
			}
			set
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				if (new Vector2(((Rect)(ref m_rect)).get_width(), ((Rect)(ref m_rect)).get_height()) != value)
				{
					SetRect(value);
					m_hasChanged = true;
					m_isDefaultWidth = false;
					m_isDefaultHeight = false;
					OnContainerChanged();
				}
			}
		}

		public float width
		{
			get
			{
				return ((Rect)(ref m_rect)).get_width();
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				SetRect(new Vector2(value, ((Rect)(ref m_rect)).get_height()));
				m_hasChanged = true;
				m_isDefaultWidth = false;
				OnContainerChanged();
			}
		}

		public float height
		{
			get
			{
				return ((Rect)(ref m_rect)).get_height();
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				SetRect(new Vector2(((Rect)(ref m_rect)).get_width(), value));
				m_hasChanged = true;
				m_isDefaultHeight = false;
				OnContainerChanged();
			}
		}

		public bool isDefaultWidth => m_isDefaultWidth;

		public bool isDefaultHeight => m_isDefaultHeight;

		public bool isAutoFitting
		{
			get
			{
				return m_isAutoFitting;
			}
			set
			{
				m_isAutoFitting = value;
			}
		}

		public Vector3[] corners => m_corners;

		public Vector3[] worldCorners => m_worldCorners;

		public Vector4 margins
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_margins;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_margins != value)
				{
					m_margins = value;
					m_hasChanged = true;
					OnContainerChanged();
				}
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

		public TextMeshPro textMeshPro
		{
			get
			{
				if ((Object)(object)m_textMeshPro == (Object)null)
				{
					m_textMeshPro = ((Component)this).GetComponent<TextMeshPro>();
				}
				return m_textMeshPro;
			}
		}

		protected override void Awake()
		{
			Debug.LogWarning((object)("The Text Container component is now Obsolete and can safely be removed from [" + ((Object)((Component)this).get_gameObject()).get_name() + "]."), (Object)(object)this);
		}

		protected override void OnEnable()
		{
			OnContainerChanged();
		}

		protected override void OnDisable()
		{
		}

		private void OnContainerChanged()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			UpdateCorners();
			if ((Object)(object)m_rectTransform != (Object)null)
			{
				m_rectTransform.set_sizeDelta(size);
				((Transform)m_rectTransform).set_hasChanged(true);
			}
			if ((Object)(object)textMeshPro != (Object)null)
			{
				((Graphic)m_textMeshPro).SetVerticesDirty();
				m_textMeshPro.margin = m_margins;
			}
		}

		protected override void OnValidate()
		{
			m_hasChanged = true;
			OnContainerChanged();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)rectTransform == (Object)null)
			{
				m_rectTransform = ((Component)this).get_gameObject().AddComponent<RectTransform>();
			}
			if (m_rectTransform.get_sizeDelta() != k_defaultSize)
			{
				size = m_rectTransform.get_sizeDelta();
			}
			pivot = m_rectTransform.get_pivot();
			m_hasChanged = true;
			OnContainerChanged();
		}

		private void SetRect(Vector2 size)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			m_rect = new Rect(((Rect)(ref m_rect)).get_x(), ((Rect)(ref m_rect)).get_y(), size.x, size.y);
		}

		private void UpdateCorners()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			m_corners[0] = new Vector3((0f - m_pivot.x) * ((Rect)(ref m_rect)).get_width(), (0f - m_pivot.y) * ((Rect)(ref m_rect)).get_height());
			m_corners[1] = new Vector3((0f - m_pivot.x) * ((Rect)(ref m_rect)).get_width(), (1f - m_pivot.y) * ((Rect)(ref m_rect)).get_height());
			m_corners[2] = new Vector3((1f - m_pivot.x) * ((Rect)(ref m_rect)).get_width(), (1f - m_pivot.y) * ((Rect)(ref m_rect)).get_height());
			m_corners[3] = new Vector3((1f - m_pivot.x) * ((Rect)(ref m_rect)).get_width(), (0f - m_pivot.y) * ((Rect)(ref m_rect)).get_height());
			if ((Object)(object)m_rectTransform != (Object)null)
			{
				m_rectTransform.set_pivot(m_pivot);
			}
		}

		private Vector2 GetPivot(TextContainerAnchors anchor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.get_zero();
			switch (anchor)
			{
			case TextContainerAnchors.TopLeft:
				((Vector2)(ref zero))._002Ector(0f, 1f);
				break;
			case TextContainerAnchors.Top:
				((Vector2)(ref zero))._002Ector(0.5f, 1f);
				break;
			case TextContainerAnchors.TopRight:
				((Vector2)(ref zero))._002Ector(1f, 1f);
				break;
			case TextContainerAnchors.Left:
				((Vector2)(ref zero))._002Ector(0f, 0.5f);
				break;
			case TextContainerAnchors.Middle:
				((Vector2)(ref zero))._002Ector(0.5f, 0.5f);
				break;
			case TextContainerAnchors.Right:
				((Vector2)(ref zero))._002Ector(1f, 0.5f);
				break;
			case TextContainerAnchors.BottomLeft:
				((Vector2)(ref zero))._002Ector(0f, 0f);
				break;
			case TextContainerAnchors.Bottom:
				((Vector2)(ref zero))._002Ector(0.5f, 0f);
				break;
			case TextContainerAnchors.BottomRight:
				((Vector2)(ref zero))._002Ector(1f, 0f);
				break;
			}
			return zero;
		}

		private TextContainerAnchors GetAnchorPosition(Vector2 pivot)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			if (pivot == new Vector2(0f, 1f))
			{
				return TextContainerAnchors.TopLeft;
			}
			if (pivot == new Vector2(0.5f, 1f))
			{
				return TextContainerAnchors.Top;
			}
			if (pivot == new Vector2(1f, 1f))
			{
				return TextContainerAnchors.TopRight;
			}
			if (pivot == new Vector2(0f, 0.5f))
			{
				return TextContainerAnchors.Left;
			}
			if (pivot == new Vector2(0.5f, 0.5f))
			{
				return TextContainerAnchors.Middle;
			}
			if (pivot == new Vector2(1f, 0.5f))
			{
				return TextContainerAnchors.Right;
			}
			if (pivot == new Vector2(0f, 0f))
			{
				return TextContainerAnchors.BottomLeft;
			}
			if (pivot == new Vector2(0.5f, 0f))
			{
				return TextContainerAnchors.Bottom;
			}
			if (pivot == new Vector2(1f, 0f))
			{
				return TextContainerAnchors.BottomRight;
			}
			return TextContainerAnchors.Custom;
		}

		public TextContainer()
			: this()
		{
		}
	}
}
