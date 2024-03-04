using System;
using UnityEditor;
using UnityEngine;

namespace HashcodeDigital.Editor
{
	internal class ProgressBarScope : IDisposable
	{
		private const float refreshRate = 0.5f;

		protected readonly string title;

		private int index;

		private readonly int count;

		private DateTime nextShowTime;

		public ProgressBarScope(string title, int count)
		{
			this.title = title;
			this.count = count;
			nextShowTime = DateTime.UtcNow + TimeSpan.FromSeconds(0.5);
		}

		public bool Tick()
		{
			if (index++ == count)
			{
				Debug.LogWarning((object)("Progress updates exceeded initial count of " + count + "!"));
			}
			if (DateTime.UtcNow < nextShowTime)
			{
				return false;
			}
			nextShowTime = DateTime.UtcNow + TimeSpan.FromSeconds(0.5);
			float num = (float)index / (float)count;
			num = Mathf.Clamp01(num);
			if (float.IsNaN(num))
			{
				num = 0f;
			}
			return Show(num);
		}

		protected virtual bool Show(float progress)
		{
			EditorUtility.DisplayProgressBar(title, string.Empty, progress);
			return false;
		}

		public void Dispose()
		{
			EditorUtility.ClearProgressBar();
		}
	}
}
