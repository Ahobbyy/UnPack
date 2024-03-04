using Multiplayer;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	[RequireComponent(typeof(LightConsume), typeof(NetBody))]
	public class MeltingObject : Node, IReset
	{
		public float meltingSpeed = 1f;

		public float RespawnDelay;

		private NetBody netbody;

		public NodeInput lightIntensity;

		public NodeOutput output;

		public NodeOutput meltedOutput;

		[SerializeField]
		private GameObject translateObject;

		[SerializeField]
		private float translateDistance;

		private LightConsume consume;

		private Vector3 startingHeight;

		private float startingMass;

		private Rigidbody rb;

		private float timeMelting;

		private void Awake()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			startingHeight = ((Component)this).get_transform().get_localScale();
			netbody = ((Component)this).GetComponent<NetBody>();
			consume = ((Component)this).GetComponent<LightConsume>();
			rb = ((Component)this).GetComponent<Rigidbody>();
			startingMass = rb.get_mass();
		}

		private void DoTranslate()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			Vector3 localPosition = translateObject.get_transform().get_localPosition();
			localPosition.y = (0f - (1f - ((Component)this).get_transform().get_localScale().z)) * translateDistance;
			translateObject.get_transform().set_localPosition(localPosition);
		}

		private void FixedUpdate()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			if (!NetGame.isClient && !(lightIntensity.value < 0.5f))
			{
				Vector3 val = ((Component)this).get_transform().InverseTransformDirection(Vector3.get_up());
				Vector3 val2 = default(Vector3);
				((Vector3)(ref val2))._002Ector((0f - lightIntensity.value) * meltingSpeed * Time.get_fixedDeltaTime() * Mathf.Abs(val.x), (0f - lightIntensity.value) * meltingSpeed * Time.get_fixedDeltaTime() * Mathf.Abs(val.y), (0f - lightIntensity.value) * meltingSpeed * Time.get_fixedDeltaTime() * Mathf.Abs(val.z));
				Vector3 val3 = ((Component)this).get_transform().get_localScale() + val2;
				AdjustHandAnchors(((Component)this).get_transform().TransformVector(val2));
				((Component)this).get_transform().set_localScale(val3);
				if ((Object)(object)translateObject != (Object)null)
				{
					DoTranslate();
				}
				Vector3 val4 = default(Vector3);
				((Vector3)(ref val4))._002Ector(val3.x / startingHeight.x, val3.y / startingHeight.y, val3.z / startingHeight.z);
				rb.set_mass((val4.x + val4.y + val4.z) / 3f * startingMass);
				float num = Mathf.Min(new float[3] { val4.x, val4.y, val4.z });
				output.SetValue(1f - num);
				if (output.value >= 0.99f)
				{
					Melted();
				}
				consume.RecalculateAll();
			}
		}

		private void Melted()
		{
			meltedOutput.SetValue(1f);
			((Component)this).get_gameObject().SetActive(false);
			if (netbody.respawn)
			{
				((MonoBehaviour)this).Invoke("Respawn", RespawnDelay);
			}
		}

		public void ResetScale()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((Component)this).get_transform().set_localScale(startingHeight);
			rb.set_mass(startingMass);
			output.SetValue(0f);
		}

		private void Respawn()
		{
			ResetScale();
			((Component)this).get_gameObject().SetActive(true);
			netbody.Respawn();
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			ResetScale();
		}

		public void AdjustHandAnchors(Vector3 scaleDif)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				AdjustHandAnchors(human.ragdoll.partLeftHand.sensor.grabJoint, scaleDif);
				AdjustHandAnchors(human.ragdoll.partRightHand.sensor.grabJoint, scaleDif);
			}
		}

		public void AdjustHandAnchors(ConfigurableJoint joint, Vector3 scaleDif)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)joint == (Object)null) && !((Object)(object)((Joint)joint).get_connectedBody() != (Object)(object)rb))
			{
				((Joint)joint).set_autoConfigureConnectedAnchor(false);
				((Joint)joint).set_connectedAnchor(((Joint)joint).get_connectedAnchor() - ((Component)joint).get_transform().InverseTransformVector(scaleDif));
			}
		}
	}
}
