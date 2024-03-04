using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;
using UnityEngine.Profiling;

public class FloatingMesh1 : MonoBehaviour
{
	private struct FloatingVertex
	{
		public Vector3 pos;

		public Vector3 velocity;

		public float depth;
	}

	public Vector3 offset = Vector3.get_zero();

	[Tooltip("Index for all the verts we will apply the movement to")]
	public static int counter;

	private int index;

	[Tooltip("The collision surfaces that connect to the water surface")]
	public Collider hull;

	[Tooltip("The mesh connected to the hull , mesh that needs to float ")]
	public Mesh mesh;

	[Tooltip("A particular water sensor for this floating mesh")]
	public WaterSensor sensor;

	[Tooltip("The density of the object , effects the reaction away from the water surface")]
	public float density = 1000f;

	[Tooltip("Vector for the direction to ignore when appplying force to the hull ")]
	public Vector3 ignoreHydrodynamicForce;

	[Tooltip("The pressure in a particular direction")]
	public float pressureLinear = 20f;

	[Tooltip("The square of the pressure")]
	public float pressureSquare = 10f;

	[Tooltip("The amount of suction in a particular direction ")]
	public float suctionLinear = 20f;

	[Tooltip("The square of the suction")]
	public float suctionSquare = 10f;

	[Tooltip("How fast the power should fall off over the hull")]
	public float falloffPower = 1f;

	[Tooltip("How much to be effected by the wind")]
	public float bendWind = 0.1f;

	private Rigidbody body;

	private Vector3[] meshVertices;

	private int[] triangles;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private FloatingVertex[] vertices;

	private Vector3 center;

	private Vector3 resultantMoment;

	private Vector3 resultantForce;

	private Vector3 resultantStaticForce;

	public float horizontalHydrostaticTreshold;

	private void Start()
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Started "));
		}
		index = counter++;
		body = ((Component)this).GetComponent<Rigidbody>();
		if ((Object)(object)body == (Object)null)
		{
			throw new InvalidOperationException("Floater needs a Rigidbody");
		}
		if ((Object)(object)hull == (Object)null)
		{
			hull = ((Component)this).GetComponentInChildren<Collider>();
		}
		if ((Object)(object)hull == (Object)null)
		{
			throw new InvalidOperationException("MeshCollider");
		}
		sensor = ((Component)hull).GetComponent<WaterSensor>();
		if ((Object)(object)sensor == (Object)null)
		{
			sensor = ((Component)hull).get_gameObject().AddComponent<WaterSensor>();
		}
		if ((Object)(object)mesh == (Object)null && hull is MeshCollider)
		{
			ref Mesh val = ref mesh;
			Collider obj = hull;
			val = ((MeshCollider)((obj is MeshCollider) ? obj : null)).get_sharedMesh();
		}
		triangles = mesh.get_triangles();
		meshVertices = mesh.get_vertices();
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < triangles.Length; i++)
		{
			Vector3 item = meshVertices[triangles[i]];
			int num = list.IndexOf(item);
			if (num < 0)
			{
				num = list.Count;
				list.Add(item);
			}
			triangles[i] = num;
		}
		meshVertices = list.ToArray();
		for (int j = 0; j < meshVertices.Length; j++)
		{
			meshVertices[j] = ((Component)this).get_transform().InverseTransformPoint(((Component)hull).get_transform().TransformPoint(meshVertices[j]) + offset);
		}
		vertices = new FloatingVertex[meshVertices.Length];
	}

	private void ApplyForces()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		if (triangles == null || triangles.Length == 0)
		{
			Start();
		}
		if (body.IsSleeping())
		{
			return;
		}
		Profiler.BeginSample("TransfromVerts", (Object)(object)this);
		TransformVertices(((Component)this).get_transform().get_localToWorldMatrix());
		Profiler.EndSample();
		Profiler.BeginSample("ApplyTriangleForces", (Object)(object)this);
		center = body.get_worldCenterOfMass();
		resultantMoment = (resultantForce = (resultantStaticForce = Vector3.get_zero()));
		for (int i = 0; i < triangles.Length; i += 3)
		{
			ApplyTriangleForces(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
		}
		if (horizontalHydrostaticTreshold > 0f)
		{
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(resultantStaticForce.x, 0f, resultantStaticForce.z);
			if (((Vector3)(ref val)).get_magnitude() < horizontalHydrostaticTreshold)
			{
				resultantForce -= val;
			}
		}
		if (ignoreHydrodynamicForce != Vector3.get_zero())
		{
			Vector3 val2 = ((Component)this).get_transform().TransformVector(ignoreHydrodynamicForce);
			float num = Vector3.Dot(resultantForce, ((Vector3)(ref val2)).get_normalized());
			if (num < 0f)
			{
				resultantForce -= num * val2;
			}
		}
		resultantForce = Vector3.ClampMagnitude(resultantForce, body.get_mass() * 100f);
		resultantMoment = Vector3.ClampMagnitude(resultantMoment, body.get_mass() * 100f);
		if (float.IsNaN(resultantForce.x) || float.IsNaN(resultantForce.y) || float.IsNaN(resultantForce.z))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Force NaN "));
			}
		}
		else
		{
			body.AddForce(resultantForce * (float)FloatingMeshSync.skipFrames);
		}
		if (float.IsNaN(resultantForce.x) || float.IsNaN(resultantForce.y) || float.IsNaN(resultantForce.z))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Torque NaN "));
			}
		}
		else
		{
			body.AddTorque(resultantMoment * (float)FloatingMeshSync.skipFrames);
		}
		Profiler.EndSample();
	}

	private void TransformVertices(Matrix4x4 localToWorldMatrix)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < meshVertices.Length; i++)
		{
			Vector3 val = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint3x4(meshVertices[i]);
			Vector3 val2 = Vector3.get_zero();
			if ((Object)(object)body != (Object)null)
			{
				val2 = body.GetPointVelocity(val);
			}
			Vector3 velocity = Vector3.get_zero();
			float depth = -1f;
			if ((Object)(object)sensor != (Object)null && (Object)(object)sensor.waterBody != (Object)null)
			{
				depth = sensor.waterBody.SampleDepth(val, out velocity);
			}
			vertices[i] = new FloatingVertex
			{
				pos = val,
				velocity = val2 - velocity,
				depth = depth
			};
		}
	}

	private void ApplyTriangleForces(FloatingVertex a, FloatingVertex b, FloatingVertex c)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		if (a.depth <= 0f && b.depth <= 0f && c.depth <= 0f)
		{
			return;
		}
		Vector3 areaNormal = -Vector3.Cross(a.pos - b.pos, c.pos - b.pos) * 0.5f;
		FloatingVertex t;
		FloatingVertex m;
		FloatingVertex l;
		if (a.depth <= b.depth && a.depth <= c.depth)
		{
			t = a;
			if (b.depth < c.depth)
			{
				m = b;
				l = c;
			}
			else
			{
				m = c;
				l = b;
			}
		}
		else if (b.depth <= a.depth && b.depth <= c.depth)
		{
			t = b;
			if (a.depth < c.depth)
			{
				m = a;
				l = c;
			}
			else
			{
				m = c;
				l = a;
			}
		}
		else
		{
			t = c;
			if (a.depth < b.depth)
			{
				m = a;
				l = b;
			}
			else
			{
				m = b;
				l = a;
			}
		}
		FloatingVertex floatingVertex;
		if (a.depth >= 0f && b.depth >= 0f && c.depth >= 0f)
		{
			ApplySumbergedTriangleForces(t, m, l, areaNormal);
		}
		else if (m.depth <= 0f)
		{
			float num = (0f - l.depth) / (m.depth - l.depth);
			float num2 = (0f - l.depth) / (t.depth - l.depth);
			floatingVertex = default(FloatingVertex);
			floatingVertex.pos = l.pos + num2 * (t.pos - l.pos);
			floatingVertex.velocity = l.velocity + num2 * (t.velocity - l.velocity);
			floatingVertex.depth = 0f;
			FloatingVertex t2 = floatingVertex;
			floatingVertex = default(FloatingVertex);
			floatingVertex.pos = l.pos + num * (m.pos - l.pos);
			floatingVertex.velocity = l.velocity + num * (m.velocity - l.velocity);
			floatingVertex.depth = 0f;
			FloatingVertex m2 = floatingVertex;
			ApplySumbergedTriangleForces(t2, m2, l, areaNormal);
		}
		else
		{
			float num3 = (0f - t.depth) / (m.depth - t.depth);
			float num4 = (0f - t.depth) / (l.depth - t.depth);
			floatingVertex = default(FloatingVertex);
			floatingVertex.pos = t.pos + num4 * (l.pos - t.pos);
			floatingVertex.velocity = t.velocity + num4 * (l.velocity - t.velocity);
			floatingVertex.depth = 0f;
			FloatingVertex t3 = floatingVertex;
			floatingVertex = default(FloatingVertex);
			floatingVertex.pos = t.pos + num3 * (m.pos - t.pos);
			floatingVertex.velocity = t.velocity + num3 * (m.velocity - t.velocity);
			floatingVertex.depth = 0f;
			FloatingVertex floatingVertex2 = floatingVertex;
			ApplySumbergedTriangleForces(floatingVertex2, m, l, areaNormal);
			ApplySumbergedTriangleForces(t3, floatingVertex2, l, areaNormal);
		}
	}

	private void ApplySumbergedTriangleForces(FloatingVertex t, FloatingVertex m, FloatingVertex l, Vector3 areaNormal)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (m.depth == t.depth || t.depth == l.depth)
		{
			TrianglePointingDown(t, m, l, areaNormal);
			return;
		}
		if (m.depth == l.depth)
		{
			TrianglePointingUp(t, m, l, areaNormal);
			return;
		}
		float num = (m.depth - l.depth) / (t.depth - l.depth);
		FloatingVertex floatingVertex = default(FloatingVertex);
		floatingVertex.pos = l.pos + num * (t.pos - l.pos);
		floatingVertex.velocity = l.velocity + num * (t.velocity - l.velocity);
		floatingVertex.depth = m.depth;
		FloatingVertex floatingVertex2 = floatingVertex;
		TrianglePointingUp(t, m, floatingVertex2, areaNormal);
		TrianglePointingDown(floatingVertex2, m, l, areaNormal);
	}

	private void DrawTriangleGizmos(FloatingVertex t, FloatingVertex m, FloatingVertex l)
	{
	}

	private void TrianglePointingDown(FloatingVertex t, FloatingVertex m, FloatingVertex l, Vector3 areaNormal)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Hydrostatic(t, m, l, areaNormal);
		Hydrodynamic(m, l, t, areaNormal);
	}

	private void TrianglePointingUp(FloatingVertex t, FloatingVertex m, FloatingVertex l, Vector3 areaNormal)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Hydrostatic(m, l, t, areaNormal);
		Hydrodynamic(m, l, t, areaNormal);
	}

	private void Hydrostatic(FloatingVertex a, FloatingVertex b, FloatingVertex c, Vector3 areaNormal)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		Vector3 pos = c.pos;
		Vector3 val = (a.pos + b.pos) / 2f;
		Vector3 pos2 = a.pos;
		Vector3 val2 = b.pos - a.pos;
		val2 = Math3d.ProjectPointOnLine(pos2, ((Vector3)(ref val2)).get_normalized(), c.pos) - pos;
		float magnitude = ((Vector3)(ref val2)).get_magnitude();
		val2 = a.pos - b.pos;
		float magnitude2 = ((Vector3)(ref val2)).get_magnitude();
		float depth = c.depth;
		float depth2 = a.depth;
		float num = density * 9.81f * magnitude2 * magnitude * (depth / 2f + (depth2 - depth) / 3f);
		float num2 = density * 9.81f * magnitude2 * magnitude * magnitude * (depth / 3f + (depth2 - depth) / 4f) / num;
		if (!(magnitude2 < 0.001f) && !(magnitude < 0.001f) && !(num < 0.0001f))
		{
			Vector3 force = (0f - num) * ((Vector3)(ref areaNormal)).get_normalized();
			Vector3 pos3 = pos + (val - pos) * num2 / magnitude;
			AddForceAtPosition(force, pos3, isDynamic: false);
		}
	}

	private void Hydrodynamic(FloatingVertex a, FloatingVertex b, FloatingVertex c, Vector3 areaNormal)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)body == (Object)null))
		{
			float magnitude = ((Vector3)(ref areaNormal)).get_magnitude();
			Vector3 val = areaNormal / magnitude;
			magnitude /= 3f;
			FloatingVertex floatingVertex = a;
			Vector3 val2 = floatingVertex.velocity;
			float magnitude2 = ((Vector3)(ref val2)).get_magnitude();
			if (magnitude2 != 0f)
			{
				val2 /= magnitude2;
			}
			float num = val2.x * val.x + val2.y * val.y + val2.z * val.z;
			float num2 = 0f;
			Vector3 val3;
			if (bendWind > 0f)
			{
				val3 = val - val2 * bendWind;
				val = ((Vector3)(ref val3)).get_normalized();
			}
			num2 = ((!(num > 0f)) ? ((suctionLinear * magnitude2 + suctionSquare * magnitude2 * magnitude2) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(0f - num, falloffPower) : (0f - num))) : ((0f - (pressureLinear * magnitude2 + pressureSquare * magnitude2 * magnitude2)) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(num, falloffPower) : num)));
			AddForceAtPosition(num2 * val, floatingVertex.pos, isDynamic: true);
			FloatingVertex floatingVertex2 = b;
			Vector3 val4 = floatingVertex2.velocity;
			float magnitude3 = ((Vector3)(ref val4)).get_magnitude();
			if (magnitude3 != 0f)
			{
				val4 /= magnitude3;
			}
			float num3 = val4.x * val.x + val4.y * val.y + val4.z * val.z;
			float num4 = 0f;
			if (bendWind > 0f)
			{
				val3 = val - val4 * bendWind;
				val = ((Vector3)(ref val3)).get_normalized();
			}
			num4 = ((!(num3 > 0f)) ? ((suctionLinear * magnitude3 + suctionSquare * magnitude3 * magnitude3) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(0f - num3, falloffPower) : (0f - num3))) : ((0f - (pressureLinear * magnitude3 + pressureSquare * magnitude3 * magnitude3)) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(num3, falloffPower) : num3)));
			AddForceAtPosition(num4 * val, floatingVertex2.pos, isDynamic: true);
			FloatingVertex floatingVertex3 = c;
			Vector3 val5 = floatingVertex3.velocity;
			float magnitude4 = ((Vector3)(ref val5)).get_magnitude();
			if (magnitude4 != 0f)
			{
				val5 /= magnitude4;
			}
			float num5 = val5.x * val.x + val5.y * val.y + val5.z * val.z;
			float num6 = 0f;
			if (bendWind > 0f)
			{
				val3 = val - val5 * bendWind;
				val = ((Vector3)(ref val3)).get_normalized();
			}
			num6 = ((!(num5 > 0f)) ? ((suctionLinear * magnitude4 + suctionSquare * magnitude4 * magnitude4) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(0f - num5, falloffPower) : (0f - num5))) : ((0f - (pressureLinear * magnitude4 + pressureSquare * magnitude4 * magnitude4)) * magnitude * ((falloffPower != 1f) ? Mathf.Pow(num5, falloffPower) : num5)));
			AddForceAtPosition(num6 * val, floatingVertex3.pos, isDynamic: true);
		}
	}

	private void Hydrodynamic(FloatingVertex v, float surface, Vector3 normal)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Private Hydro dynamic "));
		}
		Vector3 val = v.velocity;
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		if (magnitude != 0f)
		{
			val /= magnitude;
		}
		float num = val.x * normal.x + val.y * normal.y + val.z * normal.z;
		float num2 = 0f;
		if (bendWind > 0f)
		{
			Vector3 val2 = normal - val * bendWind;
			normal = ((Vector3)(ref val2)).get_normalized();
		}
		num2 = ((!(num > 0f)) ? ((suctionLinear * magnitude + suctionSquare * magnitude * magnitude) * surface * ((falloffPower != 1f) ? Mathf.Pow(0f - num, falloffPower) : (0f - num))) : ((0f - (pressureLinear * magnitude + pressureSquare * magnitude * magnitude)) * surface * ((falloffPower != 1f) ? Mathf.Pow(num, falloffPower) : num)));
		AddForceAtPosition(num2 * normal, v.pos, isDynamic: true);
	}

	private void AddForceAtPosition(Vector3 force, Vector3 pos, bool isDynamic)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		resultantMoment += Vector3.Cross(pos - center, force);
		resultantForce += force;
		if (float.IsNaN(resultantMoment.x) || float.IsNaN(resultantMoment.y) || float.IsNaN(resultantMoment.z))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " resultantMoment is NaN "));
			}
			resultantMoment = Vector3.get_zero();
		}
		else if (!isDynamic)
		{
			resultantStaticForce += force;
		}
	}

	private void FixedUpdate()
	{
		if (!ReplayRecorder.isPlaying && !NetGame.isClient && index % FloatingMeshSync.skipFrames == FloatingMeshSync.frame % FloatingMeshSync.skipFrames)
		{
			ApplyForces();
		}
	}

	public FloatingMesh1()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
