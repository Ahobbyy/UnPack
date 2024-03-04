using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[CustomEditor(typeof(PostProcessProfile))]
	internal sealed class PostProcessProfileEditor : Editor
	{
		private EffectListEditor m_EffectList;

		private void OnEnable()
		{
			m_EffectList = new EffectListEditor((Editor)(object)this);
			m_EffectList.Init(((Editor)this).get_target() as PostProcessProfile, ((Editor)this).get_serializedObject());
		}

		private void OnDisable()
		{
			if (m_EffectList != null)
			{
				m_EffectList.Clear();
			}
		}

		public override void OnInspectorGUI()
		{
			((Editor)this).get_serializedObject().Update();
			m_EffectList.OnGUI();
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		public PostProcessProfileEditor()
			: this()
		{
		}
	}
}
