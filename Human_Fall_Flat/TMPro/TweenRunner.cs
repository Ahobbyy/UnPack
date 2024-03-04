using System.Collections;
using UnityEngine;

namespace TMPro
{
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		protected MonoBehaviour m_CoroutineContainer;

		protected IEnumerator m_Tween;

		private static IEnumerator Start(T tweenInfo)
		{
			if (tweenInfo.ValidTarget())
			{
				float elapsedTime = 0f;
				while (elapsedTime < tweenInfo.duration)
				{
					elapsedTime += (tweenInfo.ignoreTimeScale ? Time.get_unscaledDeltaTime() : Time.get_deltaTime());
					float floatPercentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
					tweenInfo.TweenValue(floatPercentage);
					yield return null;
				}
				tweenInfo.TweenValue(1f);
			}
		}

		public void Init(MonoBehaviour coroutineContainer)
		{
			m_CoroutineContainer = coroutineContainer;
		}

		public void StartTween(T info)
		{
			if ((Object)(object)m_CoroutineContainer == (Object)null)
			{
				Debug.LogWarning((object)"Coroutine container not configured... did you forget to call Init?");
				return;
			}
			StopTween();
			if (!((Component)m_CoroutineContainer).get_gameObject().get_activeInHierarchy())
			{
				info.TweenValue(1f);
				return;
			}
			m_Tween = Start(info);
			m_CoroutineContainer.StartCoroutine(m_Tween);
		}

		public void StopTween()
		{
			if (m_Tween != null)
			{
				m_CoroutineContainer.StopCoroutine(m_Tween);
				m_Tween = null;
			}
		}
	}
}
