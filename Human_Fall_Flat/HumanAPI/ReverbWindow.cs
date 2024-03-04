using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	public class ReverbWindow : EditorWindow
	{
		private Reverb activeReverb;

		private ReverbZone[] zones;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private GUIStyle style;

		private Vector2 scrollPos;

		internal static void Init(Reverb reverb)
		{
			ReverbWindow obj = EditorWindow.GetWindow(typeof(ReverbWindow), false, "Reverb") as ReverbWindow;
			obj.activeReverb = reverb;
			obj.OnEnable();
		}

		private void OnEnable()
		{
			if ((Object)(object)activeReverb != (Object)null)
			{
				Rebuild(activeReverb);
			}
		}

		private void Rebuild(Reverb activeReverb)
		{
			zones = ((Component)activeReverb).get_gameObject().GetComponentsInChildren<ReverbZone>();
		}

		private void OnGUI()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)activeReverb == (Object)null)
			{
				return;
			}
			scrollPos = GUILayout.BeginScrollView(scrollPos, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.Label(new Rect(100f, 0f, 100f, 20f), "level");
			int num = 100 + 100;
			GUI.Label(new Rect((float)num, 0f, 100f, 20f), "delay");
			int num2 = num + 100;
			GUI.Label(new Rect((float)num2, 0f, 100f, 20f), "diffusion");
			GUI.Label(new Rect((float)(num2 + 100), 0f, 100f, 20f), "weight");
			for (int i = 0; i < zones.Length; i++)
			{
				int num3 = (i + 1) * 20;
				if (EditorApplication.get_isPlaying() && ReverbManager.instance.zones.Contains(zones[i]))
				{
					EditorGUI.DrawRect(new Rect(0f, (float)num3, 100f, 20f), new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				GUI.Label(new Rect(0f, (float)num3, 100f, 20f), ((Object)zones[i]).get_name());
				Sliders.DrawHorizontalSlider(new Rect(100f, (float)num3, 100f, 20f), -24f, 12f, zones[i].level, SliderType.Level);
				int num4 = 100 + 100;
				Sliders.DrawHorizontalSlider(new Rect((float)num4, (float)num3, 100f, 20f), 0f, 2f, zones[i].delay, SliderType.Linear);
				int num5 = num4 + 100;
				Sliders.DrawHorizontalSlider(new Rect((float)num5, (float)num3, 100f, 20f), 0f, 1f, zones[i].diffusion, SliderType.Linear);
				Sliders.DrawHorizontalSlider(new Rect((float)(num5 + 100), (float)num3, 100f, 20f), 1f, 1000f, zones[i].weight, SliderType.Log2);
			}
			EditorGUILayout.EndScrollView();
			if (EditorApplication.get_isPlaying())
			{
				((EditorWindow)this).Repaint();
			}
		}

		public ReverbWindow()
			: this()
		{
		}
	}
}
