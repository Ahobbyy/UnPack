using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.Rendering.PostProcessing
{
	[ExecuteInEditMode]
	[AddComponentMenu("Rendering/Post-process Volume", 1001)]
	public sealed class PostProcessVolume : MonoBehaviour
	{
		public PostProcessProfile sharedProfile;

		[Tooltip("A global volume is applied to the whole scene.")]
		public bool isGlobal;

		[Min(0f)]
		[Tooltip("Outer distance to start blending from. A value of 0 means no blending and the volume overrides will be applied immediatly upon entry.")]
		public float blendDistance;

		[Range(0f, 1f)]
		[Tooltip("Total weight of this volume in the scene. 0 means it won't do anything, 1 means full effect.")]
		public float weight = 1f;

		[Tooltip("Volume priority in the stack. Higher number means higher priority. Negative values are supported.")]
		public float priority;

		private int m_PreviousLayer;

		private float m_PreviousPriority;

		private List<Collider> m_TempColliders;

		private PostProcessProfile m_InternalProfile;

		public PostProcessProfile profile
		{
			get
			{
				if ((Object)(object)m_InternalProfile == (Object)null)
				{
					m_InternalProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
					if ((Object)(object)sharedProfile != (Object)null)
					{
						foreach (PostProcessEffectSettings setting in sharedProfile.settings)
						{
							PostProcessEffectSettings item = Object.Instantiate<PostProcessEffectSettings>(setting);
							m_InternalProfile.settings.Add(item);
						}
					}
				}
				return m_InternalProfile;
			}
			set
			{
				m_InternalProfile = value;
			}
		}

		internal PostProcessProfile profileRef
		{
			get
			{
				if (!((Object)(object)m_InternalProfile == (Object)null))
				{
					return m_InternalProfile;
				}
				return sharedProfile;
			}
		}

		public bool HasInstantiatedProfile()
		{
			return (Object)(object)m_InternalProfile != (Object)null;
		}

		private void OnEnable()
		{
			PostProcessManager.instance.Register(this);
			m_PreviousLayer = ((Component)this).get_gameObject().get_layer();
			m_TempColliders = new List<Collider>();
		}

		private void OnDisable()
		{
			PostProcessManager.instance.Unregister(this);
		}

		private void Update()
		{
			int layer = ((Component)this).get_gameObject().get_layer();
			if (layer != m_PreviousLayer)
			{
				PostProcessManager.instance.UpdateVolumeLayer(this, m_PreviousLayer, layer);
				m_PreviousLayer = layer;
			}
			if (priority != m_PreviousPriority)
			{
				PostProcessManager.instance.SetLayerDirty(layer);
				m_PreviousPriority = priority;
			}
		}

		private void OnDrawGizmos()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			List<Collider> tempColliders = m_TempColliders;
			((Component)this).GetComponents<Collider>(tempColliders);
			if (isGlobal || tempColliders == null)
			{
				return;
			}
			Gizmos.set_color(ColorUtilities.ToRGBA((uint)EditorPrefs.GetInt("PostProcessing.Volume.GizmoColor", -2144089062)));
			Vector3 localScale = ((Component)this).get_transform().get_localScale();
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(1f / localScale.x, 1f / localScale.y, 1f / localScale.z);
			Gizmos.set_matrix(Matrix4x4.TRS(((Component)this).get_transform().get_position(), ((Component)this).get_transform().get_rotation(), localScale));
			foreach (Collider item in tempColliders)
			{
				if (!item.get_enabled())
				{
					continue;
				}
				Type type = ((object)item).GetType();
				if (type == typeof(BoxCollider))
				{
					BoxCollider val2 = (BoxCollider)item;
					Gizmos.DrawCube(val2.get_center(), val2.get_size());
					Gizmos.DrawWireCube(val2.get_center(), val2.get_size() + val * blendDistance * 4f);
				}
				else if (type == typeof(SphereCollider))
				{
					SphereCollider val3 = (SphereCollider)item;
					Gizmos.DrawSphere(val3.get_center(), val3.get_radius());
					Gizmos.DrawWireSphere(val3.get_center(), val3.get_radius() + val.x * blendDistance * 2f);
				}
				else if (type == typeof(MeshCollider))
				{
					MeshCollider val4 = (MeshCollider)item;
					if (!val4.get_convex())
					{
						val4.set_convex(true);
					}
					Gizmos.DrawMesh(val4.get_sharedMesh());
					Gizmos.DrawWireMesh(val4.get_sharedMesh(), Vector3.get_zero(), Quaternion.get_identity(), Vector3.get_one() + val * blendDistance * 4f);
				}
			}
			tempColliders.Clear();
		}

		public PostProcessVolume()
			: this()
		{
		}
	}
}
