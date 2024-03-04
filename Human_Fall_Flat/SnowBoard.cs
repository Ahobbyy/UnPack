using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class SnowBoard : MonoBehaviour, IGrabbable, IPreReset, IRespawnable, IPreRespawn
{
	[Serializable]
	public class Binding
	{
		public static List<Binding> all = new List<Binding>();

		public Collider col;

		[ReadOnly]
		public Joint joint;

		[HideInInspector]
		public SnowBoard body;

		public Human boundTo;

		public AudioSource SFXConnect;

		public AudioSource SFXDisconnect;

		private bool isFixed;

		private const float accumulatedRelease = 2f;

		private float currentAccumulation;

		public bool occupied => (Object)(object)joint != (Object)null;

		public void Init(SnowBoard parent)
		{
			body = parent;
			all.Add(this);
		}

		public void FixedUpdate(float forceRelief)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)joint == (Object)null)
			{
				return;
			}
			if (isFixed)
			{
				Vector3.Dot(((Component)body).get_transform().get_forward(), -((Component)joint).get_transform().get_up());
				bool flag = body.grabbedCount >= 1;
				currentAccumulation = Mathf.MoveTowards(currentAccumulation, flag ? 2f : 0f, Time.get_fixedDeltaTime() + 1f * forceRelief);
				if (currentAccumulation >= 2f)
				{
					Disconnect();
					SFXDisconnect.PlayOneShot(SFXDisconnect.get_clip());
				}
			}
			else
			{
				Vector3 val = ((Component)joint).get_transform().get_position() - ((Component)col).get_transform().get_position();
				if (((Vector3)(ref val)).get_magnitude() < 0.15f)
				{
					AttachFixed();
					SFXConnect.PlayOneShot(SFXConnect.get_clip());
				}
			}
		}

		public void AttachSpring(GameObject foot, Human human)
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			foreach (Binding item in all)
			{
				if ((Object)(object)item.joint != (Object)null && (Object)(object)((Component)item.joint).get_gameObject() == (Object)(object)foot)
				{
					return;
				}
			}
			SpringJoint val = foot.AddComponent<SpringJoint>();
			val.set_spring(10000f);
			val.set_damper(141.42f);
			((Joint)val).set_breakForce(6000f);
			((Joint)val).set_breakTorque(300f);
			((Joint)val).set_connectedBody(body.body);
			((Joint)val).set_autoConfigureConnectedAnchor(false);
			((Joint)val).set_anchor(Vector3.get_zero());
			((Joint)val).set_connectedAnchor(((Component)body).get_transform().InverseTransformPoint(((Component)col).get_transform().get_position()));
			isFixed = false;
			boundTo = human;
			joint = (Joint)(object)val;
		}

		public void AttachFixed()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			GameObject gameObject = ((Component)joint).get_gameObject();
			Object.Destroy((Object)(object)joint);
			ConfigurableJoint val = gameObject.AddComponent<ConfigurableJoint>();
			ConfigurableJointMotion val2 = (ConfigurableJointMotion)0;
			val.set_zMotion((ConfigurableJointMotion)0);
			ConfigurableJointMotion xMotion;
			val.set_yMotion(xMotion = val2);
			val.set_xMotion(xMotion);
			val2 = (ConfigurableJointMotion)2;
			val.set_angularZMotion((ConfigurableJointMotion)2);
			val.set_angularYMotion(xMotion = val2);
			val.set_angularXMotion(xMotion);
			((Joint)val).set_anchor(Vector3.get_zero());
			((Joint)val).set_connectedBody(body.body);
			((Joint)val).set_autoConfigureConnectedAnchor(false);
			((Joint)val).set_connectedAnchor(((Component)body).get_transform().InverseTransformPoint(((Component)col).get_transform().get_position()));
			((Joint)val).set_breakForce(15000f);
			((Joint)val).set_breakTorque(10000f);
			isFixed = true;
			joint = (Joint)(object)val;
		}

		public void Disconnect()
		{
			boundTo = null;
			if ((Object)(object)joint != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)joint);
				joint = null;
			}
		}
	}

	public Binding[] bindings;

	public Rigidbody body;

	public Vector3 breakNormal = Vector3.get_down();

	[HideInInspector]
	public int grabbedCount;

	private RaycastHit[] info = (RaycastHit[])(object)new RaycastHit[2];

	private bool grabbed;

	private void Start()
	{
		if (!Object.op_Implicit((Object)(object)body))
		{
			body = ((Component)this).GetComponent<Rigidbody>();
		}
		Binding[] array = bindings;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Init(this);
		}
	}

	private void FixedUpdate()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		grabbedCount = 0;
		float num = 0f;
		if (grabbed)
		{
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				human.grabManager.grabbedObjects.ForEach(delegate(GameObject g)
				{
					if ((Object)(object)((Component)this).get_gameObject() == (Object)(object)g)
					{
						grabbedCount++;
					}
				});
				num += GetForceRelief(human, human.ragdoll.partLeftHand) + GetForceRelief(human, human.ragdoll.partRightHand);
			}
		}
		Binding[] array = bindings;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].occupied)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		array = bindings;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].FixedUpdate(num);
		}
		Vector3 velocity;
		Vector3 val = (velocity = body.get_velocity());
		((Vector3)(ref velocity)).Scale(((Component)this).get_transform().get_up());
		Vector3 val2 = val;
		((Vector3)(ref val2)).Scale(((Component)this).get_transform().get_right());
		if (Physics.RaycastNonAlloc(new Ray(((Component)this).get_transform().get_position(), Vector3.get_down()), info, 1f) <= 0)
		{
			return;
		}
		Vector3 val3 = default(Vector3);
		for (int k = 0; k < info.Length; k++)
		{
			if (!((Object)(object)((RaycastHit)(ref info[k])).get_transform() == (Object)(object)((Component)body).get_transform()) && !((Object)(object)((RaycastHit)(ref info[k])).get_transform() == (Object)null) && ((Component)((RaycastHit)(ref info[k])).get_transform()).get_gameObject().get_layer() != 9)
			{
				if (((Vector3)(ref val2)).get_magnitude() < ((Vector3)(ref velocity)).get_magnitude())
				{
					float num2 = Vector3.Dot(((RaycastHit)(ref info[k])).get_normal(), -((Component)this).get_transform().get_forward());
					num2 += 1f;
					Debug.DrawLine(((RaycastHit)(ref info[k])).get_point(), ((RaycastHit)(ref info[k])).get_point() - body.get_velocity() * num2 * 1000f * Time.get_fixedDeltaTime(), Color.get_green());
					body.AddForceAtPosition(-body.get_velocity() * num2 * 200f, ((RaycastHit)(ref info[k])).get_point());
				}
				Vector3 right = ((Component)body).get_transform().get_right();
				Vector3 velocity2 = body.get_velocity();
				float num3 = Vector3.Dot(right, ((Vector3)(ref velocity2)).get_normalized());
				float num4 = Mathf.Lerp(1f, 0f, Mathf.Abs(num3));
				((Vector3)(ref val3))._002Ector(body.get_velocity().x, 0f, body.get_velocity().z);
				((Vector3)(ref val3)).Scale(body.get_velocity());
				num4 *= ((Vector3)(ref val3)).get_magnitude() / 5f;
				float num5 = 0f;
				num5 = ((!(num3 < 0f)) ? Vector3.SignedAngle(((Component)body).get_transform().get_right(), body.get_velocity(), -((Component)body).get_transform().get_forward()) : Vector3.SignedAngle(((Component)body).get_transform().get_right(), body.get_velocity(), ((Component)body).get_transform().get_forward()));
				body.SafeAddTorque(-((Component)body).get_transform().get_forward() * num4 * num5, (ForceMode)0);
			}
		}
	}

	private float GetForceRelief(Human human, HumanSegment hand)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (!hand.sensor.IsGrabbed(((Component)this).get_gameObject()))
		{
			return 0f;
		}
		Vector3 val = ((Component)this).get_transform().TransformVector(breakNormal);
		Vector3 normalized = ((Vector3)(ref val)).get_normalized();
		float num = Mathf.InverseLerp(0.5f, 0.9f, Vector3.Dot(normalized, human.controls.walkDirection));
		return Mathf.Max(0f, num);
	}

	private void OnDrowning(Human human)
	{
		Binding[] array = bindings;
		foreach (Binding binding in array)
		{
			if ((Object)(object)binding.boundTo != (Object)null && ((object)binding.boundTo).Equals((object)human))
			{
				Debug.Log((object)"Disconnect");
				Disconnect();
			}
		}
	}

	private void OnEnable()
	{
		Game.OnDrowning = (Action<Human>)Delegate.Remove(Game.OnDrowning, new Action<Human>(OnDrowning));
		Game.OnDrowning = (Action<Human>)Delegate.Combine(Game.OnDrowning, new Action<Human>(OnDrowning));
	}

	private void OnDisable()
	{
		Game.OnDrowning = (Action<Human>)Delegate.Remove(Game.OnDrowning, new Action<Human>(OnDrowning));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!((Object)(object)((Component)other).get_gameObject().GetComponent<FootCollisionAudioSensor>() != (Object)null))
		{
			return;
		}
		Binding binding = FindClosest(other);
		if (binding != null && !binding.occupied && grabbedCount < 1)
		{
			Human componentInParent = ((Component)other).GetComponentInParent<Human>();
			binding.AttachSpring(((Component)other).get_gameObject(), componentInParent);
			if ((Object)(object)componentInParent != (Object)null && (Object)(object)SnowboardAchievement.instance != (Object)null)
			{
				SnowboardAchievement.instance.RegisterAttach(this, componentInParent);
			}
			allBindingsUsed();
		}
	}

	private bool allBindingsUsed()
	{
		bool result = true;
		Binding[] array = bindings;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].occupied)
			{
				result = false;
			}
		}
		return result;
	}

	public void CreateHipsSpring(GameObject ball)
	{
	}

	private Binding FindClosest(Collider feet)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Binding[] array = bindings;
		foreach (Binding binding in array)
		{
			if (Vector3.Distance(((Component)binding.col).get_transform().get_position(), ((Component)feet).get_transform().get_position()) < 0.2f)
			{
				return binding;
			}
		}
		return null;
	}

	public void OnGrab()
	{
		grabbed = true;
	}

	public void OnRelease()
	{
		grabbed = false;
	}

	public void Disconnect()
	{
		Binding[] array = bindings;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disconnect();
		}
	}

	public void PreResetState(int checkpoint)
	{
		Disconnect();
	}

	public void Respawn(Vector3 offset)
	{
		Disconnect();
	}

	private void OnDrawGizmos()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.DrawLine(((Component)this).get_transform().get_position(), ((Component)this).get_transform().get_position() + Vector3.get_down());
	}

	void IPreRespawn.PreRespawn()
	{
		Disconnect();
	}

	public SnowBoard()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
