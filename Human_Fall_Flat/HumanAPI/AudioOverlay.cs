using System;
using UnityEngine;

namespace HumanAPI
{
	public class AudioOverlay : MonoBehaviour
	{
		private enum Tab
		{
			Ambience,
			Reverb,
			MainLibrary,
			LevelLibrary,
			Sounds,
			MenuSounds,
			CharSounds,
			Grain
		}

		private Tab currentTab = Tab.Sounds;

		private AmbienceOverlay ambience = new AmbienceOverlay();

		private ReverbOverlay reverb = new ReverbOverlay();

		private LibraryOverlay library = new LibraryOverlay();

		private LibraryOverlay mainlibrary = new LibraryOverlay
		{
			showMain = true
		};

		private SoundOverlay sounds = new SoundOverlay();

		private GrainOverlay grain = new GrainOverlay();

		private SamplePicker picker = new SamplePicker();

		public void ToggleVisibility()
		{
			((Behaviour)this).set_enabled(!((Behaviour)this).get_enabled());
			HumanControls.freezeMouse = ((Behaviour)this).get_enabled();
			if (((Behaviour)this).get_enabled())
			{
				MenuSystem.instance.EnterMenuInputMode();
			}
			else
			{
				MenuSystem.instance.ExitMenuInputMode();
			}
		}

		private void OnGUI()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			AudioUI.EnsureStyle();
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
			Rect rect = GUILayoutUtility.GetRect((float)Screen.get_width(), 0f);
			((Rect)(ref rect)).set_height(32f);
			AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect)).get_height()) });
			Rect rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.Sounds)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.Sounds, "Sounds"))
			{
				currentTab = Tab.Sounds;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.MenuSounds)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.MenuSounds, "Menu"))
			{
				currentTab = Tab.MenuSounds;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.CharSounds)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.CharSounds, "Character"))
			{
				currentTab = Tab.CharSounds;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.Ambience)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.Ambience, "Ambience"))
			{
				currentTab = Tab.Ambience;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.Reverb)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.Reverb, "Reverb"))
			{
				currentTab = Tab.Reverb;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.MainLibrary)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.MainLibrary, "MainSamples"))
			{
				currentTab = Tab.MainLibrary;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.LevelLibrary)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.LevelLibrary, "Samples"))
			{
				currentTab = Tab.LevelLibrary;
			}
			rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			if (currentTab == Tab.Grain)
			{
				AudioUI.DrawRect(rect2, new Color(0f, 0.3f, 0f, 0.7f));
			}
			if (GUI.Toggle(rect2, currentTab == Tab.Grain, "Grain"))
			{
				currentTab = Tab.Grain;
			}
			GUILayout.EndHorizontal();
			switch (currentTab)
			{
			case Tab.Ambience:
				ambience.OnGUI();
				break;
			case Tab.Reverb:
				reverb.OnGUI();
				break;
			case Tab.LevelLibrary:
				library.OnGUI();
				break;
			case Tab.MainLibrary:
				mainlibrary.OnGUI();
				break;
			case Tab.Sounds:
				sounds.type = SoundManagerType.Level;
				sounds.OnGUI();
				break;
			case Tab.MenuSounds:
				sounds.type = SoundManagerType.Menu;
				sounds.OnGUI();
				break;
			case Tab.CharSounds:
				sounds.type = SoundManagerType.Character;
				sounds.OnGUI();
				break;
			case Tab.Grain:
				grain.OnGUI();
				break;
			default:
				throw new NotImplementedException();
			}
			GUILayout.EndVertical();
			picker.OnGUI();
		}

		public AudioOverlay()
			: this()
		{
		}
	}
}
