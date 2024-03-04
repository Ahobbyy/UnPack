using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
	internal struct ColorTween : ITweenValue
	{
		public enum ColorTweenMode
		{
			All,
			RGB,
			Alpha
		}

		public class ColorTweenCallback : UnityEvent<Color>
		{
		}

		private ColorTweenCallback m_Target;

		private Color m_StartColor;

		private Color m_TargetColor;

		private ColorTweenMode m_TweenMode;

		private float m_Duration;

		private bool m_IgnoreTimeScale;

		public Color startColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_StartColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_StartColor = value;
			}
		}

		public Color targetColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_TargetColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_TargetColor = value;
			}
		}

		public ColorTweenMode tweenMode
		{
			get
			{
				return m_TweenMode;
			}
			set
			{
				m_TweenMode = value;
			}
		}

		public float duration
		{
			get
			{
				return m_Duration;
			}
			set
			{
				m_Duration = value;
			}
		}

		public bool ignoreTimeScale
		{
			get
			{
				return m_IgnoreTimeScale;
			}
			set
			{
				m_IgnoreTimeScale = value;
			}
		}

		public void TweenValue(float floatPercentage)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (ValidTarget())
			{
				Color val = Color.Lerp(m_StartColor, m_TargetColor, floatPercentage);
				if (m_TweenMode == ColorTweenMode.Alpha)
				{
					val.r = m_StartColor.r;
					val.g = m_StartColor.g;
					val.b = m_StartColor.b;
				}
				else if (m_TweenMode == ColorTweenMode.RGB)
				{
					val.a = m_StartColor.a;
				}
				((UnityEvent<Color>)m_Target).Invoke(val);
			}
		}

		public void AddOnChangedCallback(UnityAction<Color> callback)
		{
			if (m_Target == null)
			{
				m_Target = new ColorTweenCallback();
			}
			((UnityEvent<Color>)m_Target).AddListener(callback);
		}

		public bool GetIgnoreTimescale()
		{
			return m_IgnoreTimeScale;
		}

		public float GetDuration()
		{
			return m_Duration;
		}

		public bool ValidTarget()
		{
			return m_Target != null;
		}
	}
}
