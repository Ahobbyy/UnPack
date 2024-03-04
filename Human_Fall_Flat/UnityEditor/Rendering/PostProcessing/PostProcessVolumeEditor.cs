using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(PostProcessVolume))]
	public sealed class PostProcessVolumeEditor : BaseEditor<PostProcessVolume>
	{
		private SerializedProperty m_Profile;

		private SerializedProperty m_IsGlobal;

		private SerializedProperty m_BlendRadius;

		private SerializedProperty m_Weight;

		private SerializedProperty m_Priority;

		private EffectListEditor m_EffectList;

		public PostProcessProfile profileRef
		{
			get
			{
				if (!base.m_Target.HasInstantiatedProfile())
				{
					return base.m_Target.sharedProfile;
				}
				return base.m_Target.profile;
			}
		}

		private void OnEnable()
		{
			m_Profile = FindProperty((PostProcessVolume x) => x.sharedProfile);
			m_IsGlobal = FindProperty((PostProcessVolume x) => x.isGlobal);
			m_BlendRadius = FindProperty((PostProcessVolume x) => x.blendDistance);
			m_Weight = FindProperty((PostProcessVolume x) => x.weight);
			m_Priority = FindProperty((PostProcessVolume x) => x.priority);
			m_EffectList = new EffectListEditor((Editor)(object)this);
			RefreshEffectListEditor(base.m_Target.sharedProfile);
		}

		private void OnDisable()
		{
			if (m_EffectList != null)
			{
				m_EffectList.Clear();
			}
		}

		private void RefreshEffectListEditor(PostProcessProfile asset)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			m_EffectList.Clear();
			if ((Object)(object)asset != (Object)null)
			{
				m_EffectList.Init(asset, new SerializedObject((Object)(object)asset));
			}
		}

		public override void OnInspectorGUI()
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			((Editor)this).get_serializedObject().Update();
			EditorGUILayout.PropertyField(m_IsGlobal, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (!m_IsGlobal.get_boolValue())
			{
				EditorGUILayout.PropertyField(m_BlendRadius, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			EditorGUILayout.PropertyField(m_Weight, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_Priority, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag = false;
			bool flag2 = m_Profile.get_objectReferenceValue() != (Object)null;
			bool hasMultipleDifferentValues = m_Profile.get_hasMultipleDifferentValues();
			int num = (flag2 ? 45 : 60);
			float num2 = (float)EditorGUI.get_indentLevel() * 15f;
			Rect rect = GUILayoutUtility.GetRect(1f, EditorGUIUtility.get_singleLineHeight());
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), EditorGUIUtility.get_labelWidth() - num2, ((Rect)(ref rect)).get_height());
			Rect val2 = default(Rect);
			((Rect)(ref val2))._002Ector(((Rect)(ref val)).get_xMax(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width() - ((Rect)(ref val)).get_width() - (float)(num * ((!flag2) ? 1 : 2)), ((Rect)(ref rect)).get_height());
			Rect val3 = default(Rect);
			((Rect)(ref val3))._002Ector(((Rect)(ref val2)).get_xMax(), ((Rect)(ref rect)).get_y(), (float)num, ((Rect)(ref rect)).get_height());
			Rect val4 = default(Rect);
			((Rect)(ref val4))._002Ector(((Rect)(ref val3)).get_xMax(), ((Rect)(ref rect)).get_y(), (float)num, ((Rect)(ref rect)).get_height());
			EditorGUI.PrefixLabel(val, EditorUtilities.GetContent(base.m_Target.HasInstantiatedProfile() ? "Profile (Instance)|A copy of a profile asset." : "Profile|A reference to a profile asset."));
			ChangeCheckScope val5 = new ChangeCheckScope();
			try
			{
				EditorGUI.BeginProperty(val2, GUIContent.none, m_Profile);
				PostProcessProfile postProcessProfile = null;
				postProcessProfile = ((!base.m_Target.HasInstantiatedProfile()) ? ((PostProcessProfile)(object)EditorGUI.ObjectField(val2, m_Profile.get_objectReferenceValue(), typeof(PostProcessProfile), false)) : ((PostProcessProfile)(object)EditorGUI.ObjectField(val2, (Object)(object)base.m_Target.profile, typeof(PostProcessProfile), false)));
				if (val5.get_changed())
				{
					flag = true;
					m_Profile.set_objectReferenceValue((Object)(object)postProcessProfile);
					if (base.m_Target.HasInstantiatedProfile())
					{
						base.m_Target.profile = null;
					}
				}
				EditorGUI.EndProperty();
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			DisabledScope val6 = default(DisabledScope);
			((DisabledScope)(ref val6))._002Ector(hasMultipleDifferentValues);
			try
			{
				if (GUI.Button(val3, EditorUtilities.GetContent("New|Create a new profile."), flag2 ? EditorStyles.get_miniButtonLeft() : EditorStyles.get_miniButton()))
				{
					string name = ((Object)base.m_Target).get_name();
					PostProcessProfile objectReferenceValue = ProfileFactory.CreatePostProcessProfile(((Component)base.m_Target).get_gameObject().get_scene(), name);
					m_Profile.set_objectReferenceValue((Object)(object)objectReferenceValue);
					base.m_Target.profile = null;
					flag = true;
				}
				if (flag2 && GUI.Button(val4, EditorUtilities.GetContent(base.m_Target.HasInstantiatedProfile() ? "Save|Save the instantiated profile" : "Clone|Create a new profile and copy the content of the currently assigned profile."), EditorStyles.get_miniButtonRight()))
				{
					PostProcessProfile postProcessProfile2 = profileRef;
					string assetPath = AssetDatabase.GetAssetPath(m_Profile.get_objectReferenceValue());
					assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
					PostProcessProfile postProcessProfile3 = Object.Instantiate<PostProcessProfile>(postProcessProfile2);
					postProcessProfile3.settings.Clear();
					AssetDatabase.CreateAsset((Object)(object)postProcessProfile3, assetPath);
					foreach (PostProcessEffectSettings setting in postProcessProfile2.settings)
					{
						PostProcessEffectSettings postProcessEffectSettings = Object.Instantiate<PostProcessEffectSettings>(setting);
						((Object)postProcessEffectSettings).set_hideFlags((HideFlags)3);
						((Object)postProcessEffectSettings).set_name(((Object)setting).get_name());
						postProcessProfile3.settings.Add(postProcessEffectSettings);
						AssetDatabase.AddObjectToAsset((Object)(object)postProcessEffectSettings, (Object)(object)postProcessProfile3);
					}
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					m_Profile.set_objectReferenceValue((Object)(object)postProcessProfile3);
					base.m_Target.profile = null;
					flag = true;
				}
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val6)).Dispose();
			}
			EditorGUILayout.Space();
			if (m_Profile.get_objectReferenceValue() == (Object)null && !base.m_Target.HasInstantiatedProfile())
			{
				if (flag)
				{
					m_EffectList.Clear();
				}
				EditorGUILayout.HelpBox("Assign a Post-process Profile to this volume using the \"Asset\" field or create one automatically by clicking the \"New\" button.\nAssets are automatically put in a folder next to your scene file. If you scene hasn't been saved yet they will be created at the root of the Assets folder.", (MessageType)1);
			}
			else
			{
				if (flag || (Object)(object)profileRef != (Object)(object)m_EffectList.asset)
				{
					RefreshEffectListEditor(profileRef);
				}
				if (!hasMultipleDifferentValues)
				{
					m_EffectList.OnGUI();
				}
			}
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}
	}
}
