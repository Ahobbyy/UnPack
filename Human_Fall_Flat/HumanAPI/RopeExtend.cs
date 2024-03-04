using UnityEngine;

namespace HumanAPI
{
	public class RopeExtend : Rope, IControllable, IReset
	{
		public float targetExtend = 0.5f;

		public float extend = 1.5f;

		public float compress = 0.5f;

		private float minBoneLen;

		private float maxBoneLen;

		private float originalTargetExtend;

		private ConfigurableJoint[] joints;

		private CapsuleCollider[] colliders;

		public override void OnEnable()
		{
			base.OnEnable();
			originalTargetExtend = targetExtend;
			minBoneLen = boneLen * compress;
			maxBoneLen = boneLen * extend;
			joints = (ConfigurableJoint[])(object)new ConfigurableJoint[bones.Length + 1];
			colliders = (CapsuleCollider[])(object)new CapsuleCollider[bones.Length];
			for (int i = 0; i < bones.Length; i++)
			{
				joints[i] = ((Component)bones[i]).GetComponent<ConfigurableJoint>();
				colliders[i] = ((Component)bones[i]).GetComponent<CapsuleCollider>();
			}
			if ((Object)(object)endBody != (Object)null)
			{
				ConfigurableJoint[] components = ((Component)bones[bones.Length - 1]).GetComponents<ConfigurableJoint>();
				joints[bones.Length] = components[1];
			}
		}

		private void FixedUpdate()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Lerp(minBoneLen, maxBoneLen, targetExtend);
			if (num == boneLen)
			{
				return;
			}
			boneLen = num;
			for (int i = 0; i < bones.Length; i++)
			{
				ConfigurableJoint val = joints[i];
				colliders[i].set_height(boneLen + radius * 2f);
				if (i != bones.Length - 1)
				{
					((Joint)val).set_anchor(new Vector3(0f, 0f, (0f - boneLen) / 2f));
				}
				if (i != 0)
				{
					((Joint)val).set_connectedAnchor(new Vector3(0f, 0f, boneLen / 2f));
				}
			}
			if ((Object)(object)endBody != (Object)null)
			{
				((Joint)joints[bones.Length]).set_anchor(new Vector3(0f, 0f, boneLen / 2f));
				endBody.WakeUp();
			}
		}

		public void SetControlValue(float v)
		{
			targetExtend = v;
		}

		public new void ResetState(int checkpoint, int subObjectives)
		{
			targetExtend = originalTargetExtend;
		}
	}
}
