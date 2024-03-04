using UnityEngine;

public class ClothMesh : MonoBehaviour
{
	public AudioSource sailAudio;

	public float springForce = 10f;

	public float damperForce = 1f;

	public float bendSpringForce = 10f;

	public float bendDamperForce = 1f;

	public Transform c1;

	public Transform c2;

	public Transform c3;

	public int subdiv = 3;

	public Rigidbody c1fix;

	public Rigidbody c2fix;

	public Vector3 wind;

	private Vector3[] vertices;

	private Vector3[] bodyPositions;

	private Vector3[] bodyPositionsLocal;

	private float[] weights;

	private Rigidbody[] bodies;

	private SpringJoint[] springs;

	private Mesh mesh;

	public static float[] lift = new float[11]
	{
		0f, 0.5f, 1.3f, 1.6f, 1.5f, 1.4f, 1f, 0.7f, 0.4f, 0.3f,
		0f
	};

	public static float[] drag = new float[10] { 0.01f, 0.1f, 0.15f, 0.2f, 0.3f, 0.4f, 0.5f, 0.7f, 1f, 1.3f };

	public float pressureLinear = 20f;

	public float pressureSquare = 10f;

	public float falloffPower = 1f;

	public float refSpeed = 1f;

	public float bendWind = 0.5f;

	public float transferToBoat = 0.75f;

	public Vector3 forwardDirection = Vector3.get_forward();

	public float bendClose = 0.5f;

	public float bendBeam = 0.4f;

	public float bendRun;

	private void Start()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		bodies = (Rigidbody[])(object)new Rigidbody[subdiv * subdiv];
		bodyPositions = (Vector3[])(object)new Vector3[subdiv * subdiv];
		bodyPositionsLocal = (Vector3[])(object)new Vector3[subdiv * subdiv];
		weights = new float[subdiv * subdiv];
		for (int i = 0; i < subdiv; i++)
		{
			Vector3 val = Vector3.Lerp(c1.get_position(), c2.get_position(), 1f * (float)i / (float)(subdiv - 1));
			Vector3 val2 = Vector3.Lerp(c1.get_position(), c3.get_position(), 1f * (float)i / (float)(subdiv - 1));
			float num = 1f * (float)i / (float)(subdiv - 1);
			for (int j = 0; j <= i; j++)
			{
				float num2 = ((i == 0) ? 0f : (1f * (float)j / (float)i));
				float num3 = 0.25f + num * (1f - num2);
				weights[i * subdiv + j] = num3;
				Vector3 pos = ((i == 0) ? val : Vector3.Lerp(val, val2, 1f * (float)j / (float)i));
				CreateMass(i * subdiv + j, pos);
				if (i > 0 && j < i)
				{
					CreateSpring(i, j, i - 1, j, springForce, damperForce);
				}
				if (j > 0)
				{
					CreateSpring(i, j, i, j - 1, springForce, damperForce);
				}
				if (i > 0 && j > 0)
				{
					CreateSpring(i, j, i - 1, j - 1, springForce, damperForce);
				}
				if (i > 1 && j < i - 1)
				{
					CreateSpring(i, j, i - 2, j, bendSpringForce, bendDamperForce);
				}
				if (j > 1)
				{
					CreateSpring(i, j, i, j - 2, bendSpringForce, bendDamperForce);
				}
				if (i > 1 && j > 1)
				{
					CreateSpring(i, j, i - 2, j - 2, bendSpringForce, bendDamperForce);
				}
				if (j == 0)
				{
					((Joint)((Component)bodies[i * subdiv + j]).get_gameObject().AddComponent<FixedJoint>()).set_connectedBody(c1fix);
				}
				else if ((Object)(object)c2fix != (Object)null && i == subdiv - 1)
				{
					((Joint)((Component)bodies[i * subdiv + j]).get_gameObject().AddComponent<FixedJoint>()).set_connectedBody(c2fix);
				}
			}
		}
		int[] array = new int[subdiv * (subdiv - 1) / 2 * 6 * 2 - (subdiv - 1) * 6];
		vertices = (Vector3[])(object)new Vector3[array.Length];
		int num4 = 0;
		for (int k = 0; k < subdiv - 1; k++)
		{
			for (int l = 0; l <= k; l++)
			{
				array[num4] = num4++;
				array[num4] = num4++;
				array[num4] = num4++;
				array[num4] = num4++;
				array[num4] = num4++;
				array[num4] = num4++;
				if (l < k)
				{
					array[num4] = num4++;
					array[num4] = num4++;
					array[num4] = num4++;
					array[num4] = num4++;
					array[num4] = num4++;
					array[num4] = num4++;
				}
			}
		}
		UpdateVertices();
		mesh = new Mesh();
		((Object)mesh).set_name("Sail");
		mesh.set_vertices(vertices);
		mesh.set_triangles(array);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
		((Component)sailAudio).get_transform().set_parent(((Component)bodies[subdiv / 2 * subdiv + subdiv / 3]).get_transform());
		((Component)sailAudio).get_transform().set_localPosition(Vector3.get_zero());
	}

	private void UpdateVertices()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < subdiv; i++)
		{
			for (int j = 0; j <= i; j++)
			{
				Vector3 position = bodies[i * subdiv + j].get_position();
				bodyPositions[i * subdiv + j] = position;
				bodyPositionsLocal[i * subdiv + j] = ((Component)this).get_transform().InverseTransformPoint(position);
			}
		}
		int num = 0;
		for (int k = 0; k < subdiv - 1; k++)
		{
			for (int l = 0; l <= k; l++)
			{
				vertices[num] = (vertices[num + 3] = bodyPositionsLocal[k * subdiv + l]);
				vertices[num + 1] = (vertices[num + 5] = bodyPositionsLocal[(k + 1) * subdiv + l]);
				vertices[num + 2] = (vertices[num + 4] = bodyPositionsLocal[(k + 1) * subdiv + l + 1]);
				num += 6;
				if (l < k)
				{
					vertices[num] = (vertices[num + 3] = bodyPositionsLocal[k * subdiv + l]);
					vertices[num + 1] = (vertices[num + 5] = bodyPositionsLocal[k * subdiv + l + 1]);
					vertices[num + 2] = (vertices[num + 4] = bodyPositionsLocal[(k + 1) * subdiv + l + 1]);
					num += 6;
				}
			}
		}
	}

	private void FixedUpdate()
	{
		UpdateVertices();
		mesh.set_vertices(vertices);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
		ApplyAeroDynamic(apply: true);
	}

	public static float Sample(float[] values, float deg)
	{
		deg /= 10f;
		int num = Mathf.FloorToInt(deg);
		if (num >= values.Length - 1)
		{
			return values[values.Length - 1];
		}
		return Mathf.Lerp(values[num], values[num + 1], deg - (float)num);
	}

	private void ApplyAeroDynamic(bool apply)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		if (!apply)
		{
			Gizmos.set_color(Color.get_red());
		}
		float num = Vector3.Dot(((Vector3)(ref wind)).get_normalized(), ((Component)this).get_transform().TransformDirection(forwardDirection));
		if (num > 0f)
		{
			bendWind = Mathf.Lerp(bendBeam, bendRun, num);
		}
		else
		{
			bendWind = Mathf.Lerp(bendBeam, bendClose, 0f - num);
		}
		for (int i = 1; i < subdiv; i++)
		{
			for (int j = 1; j <= i; j++)
			{
				Vector3 val = bodyPositions[(i - 1) * subdiv + j - 1];
				Vector3 val2 = bodyPositions[i * subdiv + j - 1];
				Vector3 val3 = bodyPositions[i * subdiv + j];
				Vector3 val4 = Vector3.Cross(val - val2, val - val3);
				float magnitude = ((Vector3)(ref val4)).get_magnitude();
				val4 = ((Vector3)(ref val4)).get_normalized();
				Vector3 val5 = wind - bodies[i * subdiv + j - 1].get_velocity();
				float num2 = Vector3.Dot(((Vector3)(ref val5)).get_normalized(), val4);
				if (num2 < 0f)
				{
					num2 *= -1f;
					val4 *= -1f;
				}
				Vector3 val6 = val4 - ((Vector3)(ref val5)).get_normalized() * bendWind;
				val4 = ((Vector3)(ref val6)).get_normalized();
				float num3 = ((Vector3)(ref val5)).get_magnitude() / refSpeed;
				Vector3 val7 = (pressureLinear * num3 + pressureSquare * num3 * num3) * magnitude * Mathf.Pow(num2, falloffPower) * val4;
				if (apply)
				{
					bodies[i * subdiv + j].AddForce(val7 * (1f - transferToBoat));
					c1fix.AddForceAtPosition(transferToBoat * val7 * weights[i * subdiv + j], val3);
				}
				else
				{
					Gizmos.DrawRay(val3, val7);
				}
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (bodyPositions != null)
		{
			ApplyAeroDynamic(apply: false);
		}
	}

	private void CreateMass(int idx, Vector3 pos)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(idx.ToString());
		val.AddComponent<SphereCollider>().set_radius(0.1f);
		Rigidbody val2 = val.AddComponent<Rigidbody>();
		val.set_tag("Target");
		val.get_transform().set_position(pos);
		val.get_transform().SetParent(((Component)this).get_transform(), true);
		bodies[idx] = val2;
	}

	private void CreateSpring(int r1, int c1, int r2, int c2, float springForce, float damperForce)
	{
		CreateSpring(r1 * subdiv + c1, r2 * subdiv + c2, springForce, damperForce);
	}

	private void CreateSpring(int idx1, int idx2, float springForce, float damperForce)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody val = bodies[idx1];
		Rigidbody val2 = bodies[idx2];
		SpringJoint obj = ((Component)val).get_gameObject().AddComponent<SpringJoint>();
		((Joint)obj).set_autoConfigureConnectedAnchor(false);
		((Joint)obj).set_connectedBody(val2);
		Vector3 anchor;
		((Joint)obj).set_connectedAnchor(anchor = Vector3.get_zero());
		((Joint)obj).set_anchor(anchor);
		anchor = val.get_position() - val2.get_position();
		float magnitude;
		obj.set_maxDistance(magnitude = ((Vector3)(ref anchor)).get_magnitude());
		obj.set_minDistance(magnitude);
		obj.set_spring(springForce);
		obj.set_damper(damperForce);
	}

	public ClothMesh()
		: this()
	{
	}//IL_0076: Unknown result type (might be due to invalid IL or missing references)
	//IL_007b: Unknown result type (might be due to invalid IL or missing references)

}
