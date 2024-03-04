using UnityEngine;

namespace HumanAPI
{
	public class LibraryOverlay
	{
		private SoundLibrary activeLibrary;

		public bool showMain;

		private SoundLibrary.SampleCategory activeCategory;

		private Vector2 categoryScrollPos;

		private Vector2 sampleScrollPos;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		public void OnGUI()
		{
			activeLibrary = (showMain ? SoundLibrary.main : SoundLibrary.level);
			if (!((Object)(object)activeLibrary == (Object)null))
			{
				if (GUILayout.Button("Save Library", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					activeLibrary.Save();
				}
				if (GUILayout.Button("Reload Library", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					activeLibrary.LoadFilesystem();
				}
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
				ShowCategories();
				ShowSamples();
				GUILayout.EndHorizontal();
			}
		}

		private void ShowCategories()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			categoryScrollPos = GUILayout.BeginScrollView(categoryScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(240f) });
			for (int i = 0; i < activeLibrary.library.categories.Count; i++)
			{
				SoundLibrary.SampleCategory sampleCategory = activeLibrary.library.categories[i];
				Rect rect = GUILayoutUtility.GetRect(200f, 24f);
				AudioUI.DrawRect(rect, (sampleCategory == activeCategory) ? new Color(0f, 0.3f, 0f, 0.7f) : new Color(0.3f, 0.3f, 0.3f, 0.5f));
				if (GUI.Toggle(rect, sampleCategory == activeCategory, sampleCategory.name))
				{
					activeCategory = sampleCategory;
				}
			}
			GUILayout.EndScrollView();
		}

		private void ShowSamples()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			if (activeCategory == null)
			{
				return;
			}
			sampleScrollPos = GUILayout.BeginScrollView(sampleScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(640f) });
			for (int i = 0; i < activeCategory.samples.Count; i++)
			{
				Rect rect = GUILayoutUtility.GetRect(600f, 0f);
				((Rect)(ref rect)).set_height(24f);
				if (i % 2 == 0)
				{
					AudioUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
				}
				else
				{
					AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
				}
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect)).get_height()) });
				SoundLibrary.SerializedSample serializedSample = activeCategory.samples[i];
				string text = serializedSample.name;
				if (serializedSample.isSwitch)
				{
					text = ((!serializedSample.isLoop) ? (text + " S") : (text + " SL"));
				}
				else if (serializedSample.isLoop)
				{
					text += " L";
				}
				GUILayout.Label(text, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("lvl", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				Rect rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num = AudioUI.DrawHorizontalSlider(rect2, 0f, 1f, 1f, serializedSample.vB, AudioSliderType.Volume);
				rect2 = GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				serializedSample.vR = AudioUI.DrawHorizontalSlider(rect2, 0f, 24f, 0f, serializedSample.vR, AudioSliderType.Linear);
				GUILayout.Label("tune", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num2 = AudioUI.DrawHorizontalSlider(rect2, -2400f, 2400f, 1f, serializedSample.pB, AudioSliderType.Pitch);
				rect2 = GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				serializedSample.pR = AudioUI.DrawHorizontalSlider(rect2, 0f, 2400f, 1f, serializedSample.pR, AudioSliderType.Linear);
				GUILayout.Label("xfade", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num3 = AudioUI.DrawHorizontalSlider(rect2, 10f, 5000f, 100f, serializedSample.crossFade * 1000f, AudioSliderType.Log2);
				if (serializedSample.crossFade * 1000f != num3)
				{
					serializedSample.crossFade = num3 / 1000f;
				}
				if (num != serializedSample.vB || num2 != serializedSample.pB)
				{
					serializedSample.vB = num;
					serializedSample.pB = num2;
					if ((Object)(object)SoundManager.level != (Object)null)
					{
						SoundManager.level.RefreshSampleParameters(serializedSample);
					}
					if ((Object)(object)SoundManager.main != (Object)null)
					{
						SoundManager.main.RefreshSampleParameters(serializedSample);
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
	}
}
