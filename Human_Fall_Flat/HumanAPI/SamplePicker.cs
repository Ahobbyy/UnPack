using UnityEngine;

namespace HumanAPI
{
	public class SamplePicker
	{
		public static SoundManager.SoundMaster pickSampleSound;

		public static string pickCategoryName;

		public static SoundLibrary pickLibrary;

		private Rect pickerRect = new Rect(100f, 100f, 500f, 400f);

		private Vector2 categoryScrollPos;

		private Vector2 sampleScrollPos;

		public void OnGUI()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (pickSampleSound != null)
			{
				pickerRect = GUILayout.Window(15, pickerRect, new WindowFunction(Window), "Pick Sample", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
		}

		private void Window(int id)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			categoryScrollPos = GUILayout.BeginScrollView(categoryScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(240f) });
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Level Library", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			for (int i = 0; i < SoundLibrary.level.library.categories.Count; i++)
			{
				SoundLibrary.SampleCategory sampleCategory = SoundLibrary.level.library.categories[i];
				Rect rect = GUILayoutUtility.GetRect(200f, 24f);
				AudioUI.DrawRect(rect, (sampleCategory.name == pickCategoryName) ? new Color(0f, 0.3f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				if (GUI.Toggle(rect, sampleCategory.name == pickCategoryName, sampleCategory.name))
				{
					pickCategoryName = sampleCategory.name;
					pickLibrary = SoundLibrary.level;
				}
			}
			GUILayout.Label("Main Library", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			for (int j = 0; j < SoundLibrary.main.library.categories.Count; j++)
			{
				SoundLibrary.SampleCategory sampleCategory2 = SoundLibrary.main.library.categories[j];
				Rect rect2 = GUILayoutUtility.GetRect(200f, 24f);
				AudioUI.DrawRect(rect2, (sampleCategory2.name == pickCategoryName) ? new Color(0f, 0.3f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				if (GUI.Toggle(rect2, sampleCategory2.name == pickCategoryName, sampleCategory2.name))
				{
					pickCategoryName = sampleCategory2.name;
					pickLibrary = SoundLibrary.main;
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			sampleScrollPos = GUILayout.BeginScrollView(categoryScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(240f) });
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			SoundLibrary.SerializedSample serializedSample = null;
			if ((Object)(object)pickLibrary != (Object)null)
			{
				for (int k = 0; k < pickLibrary.library.samples.Count; k++)
				{
					SoundLibrary.SerializedSample serializedSample2 = pickLibrary.library.samples[k];
					if (!(serializedSample2.category != pickCategoryName))
					{
						bool flag = pickSampleSound.master.sample == serializedSample2.name;
						Rect rect3 = GUILayoutUtility.GetRect(200f, 24f);
						AudioUI.DrawRect(rect3, flag ? new Color(0f, 0.3f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
						if (flag != GUI.Toggle(rect3, flag, serializedSample2.name))
						{
							serializedSample = serializedSample2;
						}
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			GUILayout.EndHorizontal();
			GUI.DragWindow();
			if (serializedSample != null)
			{
				if (pickSampleSound.master.sample != serializedSample.name)
				{
					pickSampleSound.SetSample(serializedSample.name);
				}
				pickSampleSound = null;
			}
		}
	}
}
