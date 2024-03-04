using UnityEngine;

namespace HumanAPI
{
	public class Bendable : Rope, IReset
	{
		public float bendMultiplier = 1E-05f;

		public float treshold = 500f;

		private bool meshDirty = true;

		public bool isBent;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebugBendable;

		public override void OnEnable()
		{
			if (showDebugBendable)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enabled "));
			}
			base.OnEnable();
			for (int i = 0; i < bones.Length; i++)
			{
				((Component)bones[i]).GetComponent<Rigidbody>().set_isKinematic(true);
				BendableSegment bendableSegment = ((Component)bones[i]).get_gameObject().AddComponent<BendableSegment>();
				bendableSegment.bandable = this;
				bendableSegment.index = i;
				bendableSegment.treshold = treshold;
			}
		}

		public void ReportBend(int index, Vector3 maxImpactVector)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			if (showDebugBendable)
			{
				Debug.Log((object)(((Object)this).get_name() + " Report Bend "));
			}
			isDirty = true;
			for (int i = 0; i < bones.Length; i++)
			{
				float num = Mathf.Abs((float)bones.Length - (float)i * 2f) / (float)bones.Length;
				float num2 = 1f * (float)Mathf.Abs(index - i) / (float)bones.Length;
				num = 2f * num * num * num - 3f * num * num + 1f;
				num2 = 2f * num2 * num2 * num2 - 3f * num2 * num2 + 1f;
				if (!(num2 <= 0f) && !(num <= 0f))
				{
					Rigidbody component = ((Component)bones[i]).GetComponent<Rigidbody>();
					Vector3 val = maxImpactVector * bendMultiplier * num * num2;
					val = Vector3.ClampMagnitude(val, radius / 4f);
					Vector3 val2 = component.get_position() + val;
					Vector3 val3 = originalPositions[i] + Vector3.ClampMagnitude(val2 - originalPositions[i], num * 0.5f);
					component.set_position(val3);
					Vector3 val4 = val3 - originalPositions[i];
					if (((Vector3)(ref val4)).get_magnitude() > 0.1f)
					{
						isBent = true;
					}
				}
			}
			SyncRotation();
		}

		private void SyncRotation()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (showDebugBendable)
			{
				Debug.Log((object)(((Object)this).get_name() + " Sync Rotation "));
			}
			for (int i = 1; i < bones.Length - 1; i++)
			{
				Quaternion rotation = Quaternion.LookRotation(bones[i + 1].get_position() - bones[i - 1].get_position(), bones[i - 1].get_up());
				((Component)bones[i]).GetComponent<Rigidbody>().set_rotation(rotation);
			}
		}

		private void BendBone(int index, Vector3 maxImpactVector)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (showDebugBendable)
			{
				Debug.Log((object)(((Object)this).get_name() + " Bend Bone "));
			}
			if (index >= 0 && index < bones.Length)
			{
				float num = 1f - Mathf.Abs((float)bones.Length - (float)index * 2f) / (float)bones.Length;
				num *= num;
				Rigidbody component = ((Component)bones[index]).GetComponent<Rigidbody>();
				component.MovePosition(component.get_position() + maxImpactVector * bendMultiplier * num);
			}
		}

		public override void CheckDirty()
		{
			base.CheckDirty();
			if (meshDirty)
			{
				isDirty = true;
				meshDirty = false;
			}
		}

		public override void ResetState(int checkpoint, int subObjectives)
		{
			if (showDebugBendable)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			base.ResetState(checkpoint, subObjectives);
			isBent = false;
			isDirty = true;
			meshDirty = true;
		}
	}
}
