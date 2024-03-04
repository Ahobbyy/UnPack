using System;
using UnityEngine;

public class ConveyorBelt : SignalTweenBase
{
	private abstract class Part
	{
		protected readonly ConveyorBelt belt;

		public readonly Rigidbody body;

		public readonly Transform transform;

		private readonly float lowBound;

		private readonly float upBound;

		public Part(ConveyorBelt belt, Rigidbody body, float lowBound, float upBound)
		{
			this.belt = belt;
			this.body = body;
			this.lowBound = lowBound;
			this.upBound = upBound;
			transform = ((Component)body).GetComponent<Transform>();
		}

		public bool IsOutside(Vector3 localPos, bool checkLowerBound)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			float num = ToOffset(localPos);
			if (checkLowerBound)
			{
				return num < lowBound;
			}
			return num > upBound;
		}

		protected abstract float ToOffset(Vector3 localPos);

		public abstract Vector3 Move(Vector3 localPos, float delta);
	}

	private class LinearPart : Part
	{
		public LinearPart(ConveyorBelt belt, Rigidbody body, float lowBound, float upBound)
			: base(belt, body, lowBound, upBound)
		{
		}

		protected override float ToOffset(Vector3 localPos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return Vector3.Dot(localPos, belt.forward);
		}

		public override Vector3 Move(Vector3 localPos, float delta)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return localPos + belt.forward * delta;
		}
	}

	private class RadialPart : Part
	{
		public RadialPart(ConveyorBelt belt, Rigidbody body, float lowBound, float upBound)
			: base(belt, body, lowBound, upBound)
		{
		}

		protected override float ToOffset(Vector3 localPos)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			return (90f - Math2d.SignedAngle(Vector2.op_Implicit(-belt.forward), new Vector2(Vector3.Dot(localPos, belt.forward), Vector3.Dot(localPos, belt.up)))) / 180f * belt.radialLength;
		}

		public override Vector3 Move(Vector3 localPos, float delta)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return Quaternion.AngleAxis(delta / belt.radialLength * 180f, belt.right) * localPos;
		}
	}

	public Vector3 forward = Vector3.get_forward();

	public Vector3 right = Vector3.get_right();

	public Vector3 up;

	private Vector3 axis;

	private Vector3 axisForward;

	public GameObject itemPrefab;

	public int segmentCount = 10;

	private float segmentSpacing;

	public float length = 5f;

	public float radius = 0.5f;

	public float speed = 1f;

	private float radialLength;

	private float totalLength;

	private float power = 1f;

	private bool visible;

	public Rigidbody topTrack;

	public Rigidbody bottomTrack;

	public Rigidbody startTrack;

	public Rigidbody endTrack;

	private float frac;

	private Part topPart;

	private Part bottomPart;

	private Part startPart;

	private Part endPart;

	private bool linearIsLong = true;

	private float currentOffset;

	private Mesh linearLong;

	private Mesh linearShort;

	private Mesh radialShort;

	private Mesh radialLong;

	protected override void OnEnable()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		up = Vector3.Cross(forward, right);
		axis = ((Component)this).get_transform().TransformDirection(right);
		axisForward = ((Component)this).get_transform().TransformDirection(forward);
		radialLength = (float)Math.PI * radius;
		totalLength = 2f * (length + radialLength);
		segmentSpacing = totalLength / (float)segmentCount;
		InitializeArrays();
		itemPrefab.SetActive(false);
		base.OnEnable();
	}

	public override void OnValueChanged(float value)
	{
		base.OnValueChanged(value);
		power = value;
	}

	public void OnBecameVisible()
	{
		visible = true;
	}

	public void OnBecameInvisible()
	{
		visible = false;
	}

	private void FixedUpdate()
	{
		if (power != 0f && !((Object)(object)topTrack == (Object)null))
		{
			AdvanceArrays(power * speed * Time.get_fixedDeltaTime());
		}
	}

	private void InitializeArrays()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		topTrack = CreateBody("top");
		bottomTrack = CreateBody("bottom");
		startTrack = CreateBody("start");
		endTrack = CreateBody("end");
		Mesh sharedMesh = itemPrefab.GetComponentInChildren<MeshFilter>().get_sharedMesh();
		Material sharedMaterial = ((Renderer)itemPrefab.GetComponentInChildren<MeshRenderer>()).get_sharedMaterial();
		int num = Mathf.FloorToInt(length / segmentSpacing) + 1;
		int num2 = segmentCount / 2 - num;
		frac = segmentSpacing * (float)num - length;
		linearShort = CreateMeshArray(sharedMesh, num - 1, Matrix4x4.TRS(up * radius, Quaternion.get_identity(), Vector3.get_one()), Matrix4x4.TRS(forward * segmentSpacing, Quaternion.get_identity(), Vector3.get_one()));
		linearLong = CreateMeshArray(sharedMesh, num, Matrix4x4.TRS(up * radius, Quaternion.get_identity(), Vector3.get_one()), Matrix4x4.TRS(forward * segmentSpacing, Quaternion.get_identity(), Vector3.get_one()));
		radialShort = CreateMeshArray(sharedMesh, num2, Matrix4x4.TRS(up * radius, Quaternion.get_identity(), Vector3.get_one()), Matrix4x4.TRS(Vector3.get_zero(), Quaternion.AngleAxis(segmentSpacing / radialLength * 180f, right), Vector3.get_one()));
		radialLong = CreateMeshArray(sharedMesh, num2 + 1, Matrix4x4.TRS(up * radius, Quaternion.get_identity(), Vector3.get_one()), Matrix4x4.TRS(Vector3.get_zero(), Quaternion.AngleAxis(segmentSpacing / radialLength * 180f, right), Vector3.get_one()));
		((Component)topTrack).get_transform().set_localPosition(Vector3.get_zero());
		((Component)topTrack).get_transform().set_localRotation(Quaternion.get_identity());
		((Component)topTrack).get_gameObject().AddComponent<MeshCollider>().set_sharedMesh(linearLong);
		((Component)topTrack).get_gameObject().AddComponent<MeshFilter>().set_sharedMesh(linearLong);
		((Renderer)((Component)topTrack).get_gameObject().AddComponent<MeshRenderer>()).set_sharedMaterial(sharedMaterial);
		((Renderer)((Component)topTrack).get_gameObject().GetComponent<MeshRenderer>()).set_probeAnchor(((Component)this).get_transform());
		((Component)endTrack).get_transform().set_localPosition(forward * length);
		((Component)endTrack).get_transform().set_localRotation(Quaternion.AngleAxis(frac / radialLength * 180f, right));
		((Component)endTrack).get_gameObject().AddComponent<MeshCollider>().set_sharedMesh(radialShort);
		((Component)endTrack).get_gameObject().AddComponent<MeshFilter>().set_sharedMesh(radialShort);
		((Renderer)((Component)endTrack).get_gameObject().AddComponent<MeshRenderer>()).set_sharedMaterial(sharedMaterial);
		((Renderer)((Component)endTrack).get_gameObject().GetComponent<MeshRenderer>()).set_probeAnchor(((Component)this).get_transform());
		((Component)bottomTrack).get_transform().set_localPosition(forward * length);
		((Component)bottomTrack).get_transform().set_localRotation(Quaternion.AngleAxis(180f, right));
		((Component)bottomTrack).get_gameObject().AddComponent<MeshCollider>().set_sharedMesh(linearLong);
		((Component)bottomTrack).get_gameObject().AddComponent<MeshFilter>().set_sharedMesh(linearLong);
		((Renderer)((Component)bottomTrack).get_gameObject().AddComponent<MeshRenderer>()).set_sharedMaterial(sharedMaterial);
		((Renderer)((Component)bottomTrack).get_gameObject().GetComponent<MeshRenderer>()).set_probeAnchor(((Component)this).get_transform());
		((Component)startTrack).get_transform().set_localPosition(Vector3.get_zero());
		((Component)startTrack).get_transform().set_localRotation(Quaternion.AngleAxis(frac / radialLength * 180f + 180f, right));
		((Component)startTrack).get_gameObject().AddComponent<MeshCollider>().set_sharedMesh(radialShort);
		((Component)startTrack).get_gameObject().AddComponent<MeshFilter>().set_sharedMesh(radialShort);
		((Renderer)((Component)startTrack).get_gameObject().AddComponent<MeshRenderer>()).set_sharedMaterial(sharedMaterial);
		((Renderer)((Component)startTrack).get_gameObject().GetComponent<MeshRenderer>()).set_probeAnchor(((Component)this).get_transform());
		topPart = new LinearPart(this, topTrack, 0f - segmentSpacing / 2f, length + segmentSpacing / 2f);
		bottomPart = new LinearPart(this, bottomTrack, 0f - segmentSpacing / 2f, length + segmentSpacing / 2f);
		endPart = new RadialPart(this, endTrack, 0f - segmentSpacing / 2f, radialLength + segmentSpacing / 2f);
		startPart = new RadialPart(this, startTrack, 0f - segmentSpacing / 2f, radialLength + segmentSpacing / 2f);
	}

	private Rigidbody CreateBody(string name)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(name);
		val.get_transform().SetParent(((Component)this).get_transform(), false);
		Rigidbody obj = val.AddComponent<Rigidbody>();
		obj.set_mass(1000f);
		obj.set_isKinematic(true);
		return obj;
	}

	private void AdvanceArrays(float delta)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			Vector3 val = ((Component)human.ragdoll).get_transform().get_position() - ((Component)this).get_transform().get_position();
			num = Mathf.Min(((Vector3)(ref val)).get_magnitude(), num);
			DebugGrabs(human.ragdoll.partLeftHand.sensor.grabJoint);
			DebugGrabs(human.ragdoll.partRightHand.sensor.grabJoint);
		}
		if (!(num > 40f))
		{
			Wrap(delta);
			topTrack.MovePosition(((Component)this).get_transform().get_position() + axisForward * currentOffset);
			bottomTrack.MovePosition(((Component)this).get_transform().get_position() + axisForward * (length - currentOffset));
			float num2 = currentOffset + frac;
			if (!linearIsLong)
			{
				num2 -= segmentSpacing;
			}
			startTrack.MoveRotation(((Component)this).get_transform().get_rotation() * Quaternion.AngleAxis(num2 / radialLength * 180f + 180f, right));
			endTrack.MoveRotation(((Component)this).get_transform().get_rotation() * Quaternion.AngleAxis(num2 / radialLength * 180f, right));
		}
	}

	private void DebugGrabs(ConfigurableJoint joint)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		DebugGrabs(joint, topTrack, Color.get_red());
		DebugGrabs(joint, startTrack, Color.get_green());
		DebugGrabs(joint, endTrack, Color.get_black());
		DebugGrabs(joint, bottomTrack, Color.get_blue());
	}

	private void DebugGrabs(ConfigurableJoint joint, Rigidbody body, Color color)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)joint != (Object)null && (Object)(object)((Joint)joint).get_connectedBody() == (Object)(object)body)
		{
			Vector3 val = ((Component)body).get_transform().TransformPoint(((Joint)joint).get_connectedAnchor());
			Debug.DrawLine(val - Vector3.get_up(), val + Vector3.get_up(), color);
			Debug.DrawLine(val - Vector3.get_right(), val + Vector3.get_right(), color);
			Debug.DrawLine(val - Vector3.get_forward(), val + Vector3.get_forward(), color);
		}
	}

	private void Wrap(float delta)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		currentOffset += delta;
		if (linearIsLong)
		{
			if (currentOffset < 0f)
			{
				float num = delta - currentOffset;
				float post = 0f - segmentSpacing - num;
				WrapParts(topPart, startPart, num, post, lowBound: true);
				WrapParts(bottomPart, endPart, num, post, lowBound: true);
				currentOffset += segmentSpacing;
				topTrack.set_position(topTrack.get_position() + axisForward * segmentSpacing);
				bottomTrack.set_position(bottomTrack.get_position() - axisForward * segmentSpacing);
			}
			else
			{
				if (!(currentOffset > segmentSpacing - frac))
				{
					return;
				}
				float num2 = delta - (currentOffset - (segmentSpacing - frac));
				float post2 = segmentSpacing - num2;
				WrapParts(startPart, topPart, num2, post2, lowBound: false);
				WrapParts(endPart, bottomPart, num2, post2, lowBound: false);
				startTrack.set_rotation(Quaternion.AngleAxis((0f - segmentSpacing) / radialLength * 180f, axis) * startTrack.get_rotation());
				endTrack.set_rotation(Quaternion.AngleAxis((0f - segmentSpacing) / radialLength * 180f, axis) * endTrack.get_rotation());
			}
			linearIsLong = false;
		}
		else
		{
			if (currentOffset > segmentSpacing)
			{
				float num3 = delta - (currentOffset - segmentSpacing);
				float post3 = segmentSpacing - num3;
				WrapParts(topPart, endPart, num3, post3, lowBound: false);
				WrapParts(bottomPart, startPart, num3, post3, lowBound: false);
				currentOffset -= segmentSpacing;
				topTrack.set_position(topTrack.get_position() - axisForward * segmentSpacing);
				bottomTrack.set_position(bottomTrack.get_position() + axisForward * segmentSpacing);
			}
			else
			{
				if (!(currentOffset < segmentSpacing - frac))
				{
					return;
				}
				float num4 = delta - (currentOffset - (segmentSpacing - frac));
				float post4 = 0f - segmentSpacing - num4;
				WrapParts(startPart, bottomPart, num4, post4, lowBound: true);
				WrapParts(endPart, topPart, num4, post4, lowBound: true);
				startTrack.set_rotation(Quaternion.AngleAxis(segmentSpacing / radialLength * 180f, axis) * startTrack.get_rotation());
				endTrack.set_rotation(Quaternion.AngleAxis(segmentSpacing / radialLength * 180f, axis) * endTrack.get_rotation());
			}
			linearIsLong = true;
		}
		MeshCollider component = ((Component)topTrack).get_gameObject().GetComponent<MeshCollider>();
		MeshFilter component2 = ((Component)topTrack).get_gameObject().GetComponent<MeshFilter>();
		MeshCollider component3 = ((Component)bottomTrack).get_gameObject().GetComponent<MeshCollider>();
		Mesh val;
		((Component)bottomTrack).get_gameObject().GetComponent<MeshFilter>().set_sharedMesh(val = (linearIsLong ? linearLong : linearShort));
		Mesh val2;
		component3.set_sharedMesh(val2 = val);
		Mesh sharedMesh;
		component2.set_sharedMesh(sharedMesh = val2);
		component.set_sharedMesh(sharedMesh);
		MeshCollider component4 = ((Component)startTrack).get_gameObject().GetComponent<MeshCollider>();
		MeshFilter component5 = ((Component)startTrack).get_gameObject().GetComponent<MeshFilter>();
		MeshCollider component6 = ((Component)endTrack).get_gameObject().GetComponent<MeshCollider>();
		((Component)endTrack).get_gameObject().GetComponent<MeshFilter>().set_sharedMesh(val = (linearIsLong ? radialShort : radialLong));
		component6.set_sharedMesh(val2 = val);
		component5.set_sharedMesh(sharedMesh = val2);
		component4.set_sharedMesh(sharedMesh);
	}

	private void WrapParts(Part current, Part next, float pre, float post, bool lowBound)
	{
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			WrapParts(human.ragdoll.partLeftHand.sensor.grabJoint, current, next, pre, post, lowBound);
			WrapParts(human.ragdoll.partRightHand.sensor.grabJoint, current, next, pre, post, lowBound);
		}
	}

	private void WrapParts(ConfigurableJoint joint, Part current, Part next, float pre, float post, bool lowBound)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)joint == (Object)null) && !((Object)(object)((Joint)joint).get_connectedBody() != (Object)(object)current.body))
		{
			((Joint)joint).set_autoConfigureConnectedAnchor(false);
			Vector3 connectedAnchor = ((Joint)joint).get_connectedAnchor();
			Vector3 val = current.Move(connectedAnchor, pre + post);
			if (current.IsOutside(val, lowBound))
			{
				val = current.Move(connectedAnchor, pre);
				val = next.transform.InverseTransformPoint(current.transform.TransformPoint(val));
				val = next.Move(val, 0f - pre);
				((Joint)joint).set_connectedBody(next.body);
				((Joint)joint).set_connectedAnchor(val);
			}
			else
			{
				((Joint)joint).set_connectedAnchor(val);
			}
		}
	}

	private Mesh CreateMeshArray(Mesh source, int count, Matrix4x4 initial, Matrix4x4 delta)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		Vector3[] vertices = source.get_vertices();
		Vector2[] uv = source.get_uv();
		Vector3[] normals = source.get_normals();
		int[] triangles = source.get_triangles();
		Vector3[] array = (Vector3[])(object)new Vector3[vertices.Length * count];
		Vector2[] array2 = (Vector2[])(object)new Vector2[uv.Length * count];
		Vector3[] array3 = (Vector3[])(object)new Vector3[normals.Length * count];
		int[] array4 = new int[triangles.Length * count];
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			for (int j = 0; j < triangles.Length; j++)
			{
				array4[num2] = triangles[j] + num;
				num2++;
			}
			for (int k = 0; k < vertices.Length; k++)
			{
				array[num] = ((Matrix4x4)(ref initial)).MultiplyPoint3x4(vertices[k]);
				array3[num] = ((Matrix4x4)(ref initial)).MultiplyVector(normals[k]);
				array2[num] = uv[k];
				num++;
			}
			initial = delta * initial;
		}
		Mesh val = new Mesh();
		((Object)val).set_name(((Object)source).get_name() + "(Array)");
		val.set_vertices(array);
		val.set_uv(array2);
		val.set_normals(array3);
		val.set_triangles(array4);
		val.RecalculateBounds();
		return val;
	}
}
