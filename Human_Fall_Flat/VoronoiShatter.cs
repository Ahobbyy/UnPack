using System;
using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;
using Voronoi2;

public class VoronoiShatter : ShatterBase
{
	public GameObject shardPrefab;

	public PhysicMaterial physicsMaterial;

	public ShatterAxis thicknessLocalAxis;

	public float densityPerSqMeter;

	public float totalMass = 100f;

	public int shardLayer = 10;

	public Vector3 adjustColliderSize;

	public float minExplodeImpulse;

	public float maxExplodeImpulse = float.PositiveInfinity;

	public float perShardImpulseFraction = 0.25f;

	public float maxShardVelocity = float.PositiveInfinity;

	public float cellInset;

	private List<GameObject> cells = new List<GameObject>();

	private float scale = 1f;

	private Rigidbody body;

	private Material material;

	[Tooltip("Optional - set the parent object for the shards")]
	public Transform parentObject;

	private NetScope shardParent;

	protected override void OnEnable()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		body = ((Component)this).GetComponent<Rigidbody>();
		material = ((Renderer)renderer).get_sharedMaterial();
		collider = (Collider)(object)((Component)this).GetComponent<BoxCollider>();
		scale = ((Component)this).get_transform().get_lossyScale().x;
	}

	private Vector3 To3D(Vector3 v)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(thicknessLocalAxis switch
		{
			ShatterAxis.X => new Vector3(v.z, v.x, v.y) / scale, 
			ShatterAxis.Y => new Vector3(v.y, v.z, v.x) / scale, 
			ShatterAxis.Z => new Vector3(v.x, v.y, v.z) / scale, 
			_ => throw new InvalidOperationException(), 
		});
	}

	private Vector3 To2D(Vector3 v)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(thicknessLocalAxis switch
		{
			ShatterAxis.X => new Vector3(v.y, v.z, v.x) * scale, 
			ShatterAxis.Y => new Vector3(v.z, v.x, v.y) * scale, 
			ShatterAxis.Z => new Vector3(v.x, v.y, v.z) * scale, 
			_ => throw new InvalidOperationException(), 
		});
	}

	protected override void Shatter(Vector3 contactPoint, Vector3 adjustedImpulse, float impactMagnitude, uint seed, uint netId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Expected O, but got Unknown
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		base.Shatter(contactPoint, adjustedImpulse, impactMagnitude, seed, netId);
		GameObject val = new GameObject(((Object)this).get_name() + "shards");
		val.get_transform().SetParent(((Component)this).get_transform(), false);
		shardParent = val.AddComponent<NetScope>();
		shardParent.AssignNetId(netId);
		shardParent.suppressThrottling = 3f;
		Collider obj = collider;
		BoxCollider val2 = (BoxCollider)(object)((obj is BoxCollider) ? obj : null);
		Vector2 val3 = Vector2.op_Implicit(To2D(((Component)this).get_transform().InverseTransformPoint(contactPoint) - val2.get_center()));
		To2D(val2.get_center());
		Vector3 val4 = To2D(val2.get_size() + adjustColliderSize);
		float x = val4.x;
		float y = val4.y;
		float num = (0f - val4.x) / 2f;
		float num2 = val4.x / 2f;
		float num3 = (0f - val4.y) / 2f;
		float num4 = val4.y / 2f;
		float num5 = (0f - val4.z) / 2f;
		float num6 = val4.z / 2f;
		float num7 = x * y;
		if (densityPerSqMeter == 0f)
		{
			densityPerSqMeter = totalMass / num7;
		}
		float num8 = Mathf.Min(x, y) / 4f;
		Time.get_realtimeSinceStartup();
		State state = Random.get_state();
		Random.InitState((int)seed);
		int num9 = (int)Mathf.Clamp(num7 * 10f, 5f, 50f);
		if (NetGame.isNetStarted)
		{
			num9 /= 2;
		}
		int num10 = num9 / 2;
		Voronoi voronoi = new Voronoi(0.1f);
		float[] array = new float[num9];
		float[] array2 = new float[num9];
		for (int i = 0; i < num10; i++)
		{
			array[i] = Random.Range(num, num2);
			array2[i] = Random.Range(num3, num4);
		}
		for (int j = num10; j < num9; j++)
		{
			int num11 = 0;
			Vector2 val5;
			do
			{
				if (num11++ > 1000)
				{
					return;
				}
				val5 = Random.get_insideUnitCircle() * num8 + val3;
			}
			while (val5.x < num || val5.y < num3 || val5.x > num2 || val5.y > num4);
			array[j] = val5.x;
			array2[j] = val5.y;
		}
		Random.set_state(state);
		List<GraphEdge> list = voronoi.generateVoronoi(array, array2, num, num2, num3, num4);
		List<Vector2>[] array3 = new List<Vector2>[num9];
		for (int k = 0; k < num9; k++)
		{
			array3[k] = new List<Vector2>();
		}
		int count = list.Count;
		Vector2 val6 = default(Vector2);
		Vector2 val7 = default(Vector2);
		for (int l = 0; l < count; l++)
		{
			GraphEdge graphEdge = list[l];
			((Vector2)(ref val6))._002Ector(graphEdge.x1, graphEdge.y1);
			((Vector2)(ref val7))._002Ector(graphEdge.x2, graphEdge.y2);
			if (!(val6 == val7))
			{
				if (!array3[graphEdge.site1].Contains(val6))
				{
					array3[graphEdge.site1].Add(val6);
				}
				if (!array3[graphEdge.site2].Contains(val6))
				{
					array3[graphEdge.site2].Add(val6);
				}
				if (!array3[graphEdge.site1].Contains(val7))
				{
					array3[graphEdge.site1].Add(val7);
				}
				if (!array3[graphEdge.site2].Contains(val7))
				{
					array3[graphEdge.site2].Add(val7);
				}
			}
		}
		float num12 = float.MaxValue;
		int num13 = 0;
		Vector2 val8 = default(Vector2);
		((Vector2)(ref val8))._002Ector(num, num3);
		float num14 = float.MaxValue;
		int num15 = 0;
		Vector2 val9 = default(Vector2);
		((Vector2)(ref val9))._002Ector(num, num4);
		float num16 = float.MaxValue;
		int num17 = 0;
		Vector2 val10 = default(Vector2);
		((Vector2)(ref val10))._002Ector(num2, num3);
		float num18 = float.MaxValue;
		int num19 = 0;
		Vector2 val11 = default(Vector2);
		((Vector2)(ref val11))._002Ector(num2, num4);
		Vector2 val12 = default(Vector2);
		Vector2 val13;
		for (int m = 0; m < num9; m++)
		{
			((Vector2)(ref val12))._002Ector(array[m], array2[m]);
			val13 = val8 - val12;
			float sqrMagnitude = ((Vector2)(ref val13)).get_sqrMagnitude();
			if (sqrMagnitude < num12)
			{
				num12 = sqrMagnitude;
				num13 = m;
			}
			val13 = val9 - val12;
			float sqrMagnitude2 = ((Vector2)(ref val13)).get_sqrMagnitude();
			if (sqrMagnitude2 < num14)
			{
				num14 = sqrMagnitude2;
				num15 = m;
			}
			val13 = val10 - val12;
			float sqrMagnitude3 = ((Vector2)(ref val13)).get_sqrMagnitude();
			if (sqrMagnitude3 < num16)
			{
				num16 = sqrMagnitude3;
				num17 = m;
			}
			val13 = val11 - val12;
			float sqrMagnitude4 = ((Vector2)(ref val13)).get_sqrMagnitude();
			if (sqrMagnitude4 < num18)
			{
				num18 = sqrMagnitude4;
				num19 = m;
			}
		}
		array3[num13].Add(val8);
		array3[num15].Add(val9);
		array3[num17].Add(val10);
		array3[num19].Add(val11);
		Vector3 normalized = ((Vector3)(ref adjustedImpulse)).get_normalized();
		float num20 = Mathf.Clamp(((Vector3)(ref adjustedImpulse)).get_magnitude(), minExplodeImpulse, maxExplodeImpulse) * perShardImpulseFraction;
		List<Vector2> list2 = new List<Vector2>();
		List<Vector2> list3 = new List<Vector2>();
		List<float> list4 = new List<float>();
		Vector2 val14 = default(Vector2);
		for (int n = 0; n < num9; n++)
		{
			((Vector2)(ref val14))._002Ector(array[n], array2[n]);
			List<Vector2> list5 = array3[n];
			if (list5.Count < 3)
			{
				continue;
			}
			list4.Clear();
			list3.Clear();
			list2.Clear();
			int count2 = list5.Count;
			Vector2 val15 = Vector2.get_zero();
			for (int num21 = 0; num21 < count2; num21++)
			{
				val15 += list5[num21];
			}
			val15 /= (float)list5.Count;
			for (int num22 = 0; num22 < count2; num22++)
			{
				Vector2 val16 = list5[num22] - val15;
				float num23 = Mathf.Atan2(val16.x, val16.y);
				int num24;
				for (num24 = 0; num24 < list4.Count && num23 < list4[num24]; num24++)
				{
				}
				list3.Insert(num24, val16);
				list4.Insert(num24, num23);
			}
			if (cellInset > 0f)
			{
				for (int num25 = 0; num25 < count2; num25++)
				{
					Vector2 val17 = list3[num25];
					Vector2 val18 = list3[(num25 + count2 - 1) % count2];
					Vector2 val19 = list3[(num25 + count2 - 1) % count2];
					val13 = val18 - val17 + val19 - val17;
					Vector2 normalized2 = ((Vector2)(ref val13)).get_normalized();
					list2.Add(normalized2);
				}
				for (int num26 = 0; num26 < count2; num26++)
				{
					List<Vector2> list6 = list3;
					int index = num26;
					list6[index] += list2[num26] * cellInset;
				}
			}
			Vector3[] array4 = (Vector3[])(object)new Vector3[count2 * 6];
			int[] array5 = new int[(count2 * 2 + (count2 - 2) * 2) * 3];
			int num27 = count2 * 2 * 3;
			int num28 = num27 + (count2 - 2) * 3;
			Vector3 zero = Vector3.get_zero();
			for (int num29 = 0; num29 < count2; num29++)
			{
				Vector2 val20 = list3[num29];
				array4[num29 * 6] = (array4[num29 * 6 + 1] = (array4[num29 * 6 + 2] = To3D(new Vector3(val20.x, val20.y, num5) - zero)));
				array4[num29 * 6 + 3] = (array4[num29 * 6 + 4] = (array4[num29 * 6 + 5] = To3D(new Vector3(val20.x, val20.y, num6) - zero)));
				int num30 = (num29 + 1) % count2;
				int num31 = num29 * 6 + 3;
				int num32 = num29 * 6;
				int num33 = num30 * 6 + 4;
				int num34 = num30 * 6 + 1;
				array5[num29 * 6] = num31;
				array5[num29 * 6 + 1] = num32;
				array5[num29 * 6 + 2] = num33;
				array5[num29 * 6 + 3] = num33;
				array5[num29 * 6 + 4] = num32;
				array5[num29 * 6 + 5] = num34;
				if (num29 >= 2)
				{
					array5[num27 + (num29 - 2) * 3] = 2;
					array5[num27 + (num29 - 2) * 3 + 1] = num29 * 6 + 2;
					array5[num27 + (num29 - 2) * 3 + 2] = (num29 - 1) * 6 + 2;
					array5[num28 + (num29 - 2) * 3] = 5;
					array5[num28 + (num29 - 2) * 3 + 1] = (num29 - 1) * 6 + 5;
					array5[num28 + (num29 - 2) * 3 + 2] = num29 * 6 + 5;
				}
			}
			Mesh val21 = new Mesh();
			((Object)val21).set_name("cell" + n);
			val21.set_vertices(array4);
			val21.set_triangles(array5);
			val21.RecalculateNormals();
			float num35 = 0f;
			for (int num36 = 0; num36 < count2; num36++)
			{
				Vector2 val22 = list3[num36];
				Vector2 val23 = list3[(num36 + 1) % count2];
				num35 += val22.x * val23.y - val22.y * val23.x;
			}
			num35 /= 2f;
			MeshFilter val24 = null;
			MeshRenderer val25 = null;
			MeshCollider val26 = null;
			CollisionAudioSensor collisionAudioSensor = null;
			GameObject val27;
			if ((Object)(object)shardPrefab == (Object)null)
			{
				val27 = new GameObject();
				val27.set_layer(shardLayer);
			}
			else
			{
				val27 = Object.Instantiate<GameObject>(shardPrefab);
				val24 = val27.GetComponent<MeshFilter>();
				val25 = val27.GetComponent<MeshRenderer>();
				val26 = val27.GetComponent<MeshCollider>();
				collisionAudioSensor = val27.GetComponent<CollisionAudioSensor>();
			}
			val27.SetActive(false);
			((Object)val27).set_name("cell" + n);
			if ((Object)(object)val24 == (Object)null)
			{
				val24 = val27.AddComponent<MeshFilter>();
			}
			val24.set_mesh(val21);
			if ((Object)(object)val25 == (Object)null)
			{
				val25 = val27.AddComponent<MeshRenderer>();
				((Renderer)val25).set_sharedMaterial(material);
			}
			if ((Object)(object)val26 == (Object)null)
			{
				val26 = val27.AddComponent<MeshCollider>();
			}
			Rigidbody val28 = val27.AddComponent<Rigidbody>();
			val28.set_mass(Mathf.Max(4f, (NetGame.isNetStarted ? 0.6f : 1f) * num35 * densityPerSqMeter));
			val26.set_convex(true);
			val26.set_sharedMesh(val21);
			((Collider)val26).set_sharedMaterial(physicsMaterial);
			if ((Object)(object)collisionAudioSensor == (Object)null)
			{
				collisionAudioSensor = val27.AddComponent<CollisionAudioSensor>();
			}
			collisionAudioSensor.pitch = Mathf.Clamp(10f / val28.get_mass(), 0.9f, 1.1f);
			Vector3 val29 = Vector2.op_Implicit((val14 - val3) * 10f);
			val13 = val14 - val3;
			val29.z = Mathf.Lerp(((Vector2)(ref val13)).get_sqrMagnitude(), 100f, 0f);
			float num37 = Mathf.Clamp(num20, 0f, maxShardVelocity * val28.get_mass());
			val27.get_transform().SetParent(((Component)shardParent).get_transform(), false);
			val27.get_transform().set_localPosition(To3D(Vector2.op_Implicit(val15)) + val2.get_center());
			val27.SetActive(true);
			val27.GetComponent<Rigidbody>().SafeAddForceAtPosition(-normalized * num37, (3f * contactPoint + To3D(Vector2.op_Implicit(val14))) / 4f, (ForceMode)1);
			NetIdentity netIdentity = val27.AddComponent<NetIdentity>();
			netIdentity.sceneId = (uint)n;
			val27.AddComponent<NetBody>().Start();
			shardParent.AddIdentity(netIdentity);
			cells.Add(val27);
		}
		shardParent.StartNetwork(repopulate: false);
		if ((Object)(object)parentObject != (Object)null)
		{
			val.get_transform().SetParent(parentObject);
		}
	}

	public override void ResetState(int checkpoint, int subObjectives)
	{
		base.ResetState(checkpoint, subObjectives);
		for (int i = 0; i < cells.Count; i++)
		{
			shardParent.RemoveIdentity(cells[i].GetComponent<NetIdentity>());
			Object.Destroy((Object)(object)cells[i]);
		}
		cells.Clear();
		if ((Object)(object)shardParent != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)shardParent).get_gameObject());
		}
	}
}
