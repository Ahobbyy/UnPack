using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedRope : MonoBehaviour
{
	public Transform start;

	public Transform end;

	public bool fixStart;

	public bool fixEnd;

	public int meshSegments = 20;

	public int rigidSegments = 10;

	public float segmentMass = 20f;

	public int segmentsAround = 6;

	public float radius = 0.1f;

	public float lengthMultiplier = 1f;

	public PhysicMaterial ropeMaterial;

	private void OnEnable()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = start.get_position() - end.get_position();
		float num = ((Vector3)(ref val)).get_magnitude() * lengthMultiplier;
		int num2 = meshSegments + 1;
		float num3 = num / (float)meshSegments;
		float num4 = num / (float)rigidSegments;
		Vector3[] array = (Vector3[])(object)new Vector3[num2 * segmentsAround];
		BoneWeight[] array2 = (BoneWeight[])(object)new BoneWeight[num2 * segmentsAround];
		int[] array3 = new int[meshSegments * segmentsAround * 6];
		int num5 = 0;
		Vector3 val2 = default(Vector3);
		for (int i = 0; i < num2; i++)
		{
			((Vector3)(ref val2))._002Ector(0f, 0f, (float)i * num3);
			BoneWeight val3 = CalculateBoneWeights(i);
			for (int j = 0; j < segmentsAround; j++)
			{
				Vector3 val4 = Vector2.op_Implicit(VectorExtensions.Rotate(new Vector2(radius, 0f), (float)Math.PI * 2f * (float)j / (float)segmentsAround));
				Vector3 val5 = val2 + val4;
				array2[num5] = val3;
				array[num5++] = val5;
			}
		}
		num5 = 0;
		for (int k = 0; k < num2 - 1; k++)
		{
			for (int l = 0; l < segmentsAround; l++)
			{
				int num6 = k * segmentsAround;
				int num7 = num6 + segmentsAround;
				int num8 = (l + 1) % segmentsAround;
				int num9 = num6 + l;
				int num10 = num6 + num8;
				int num11 = num7 + l;
				int num12 = num7 + num8;
				array3[num5++] = num9;
				array3[num5++] = num10;
				array3[num5++] = num11;
				array3[num5++] = num11;
				array3[num5++] = num10;
				array3[num5++] = num12;
			}
		}
		Mesh val6 = new Mesh();
		((Object)val6).set_name("rope " + ((Object)this).get_name());
		val6.set_vertices(array);
		val6.set_triangles(array3);
		val6.set_boneWeights(array2);
		val6.RecalculateNormals();
		Matrix4x4[] array4 = (Matrix4x4[])(object)new Matrix4x4[rigidSegments];
		Transform[] array5 = (Transform[])(object)new Transform[rigidSegments];
		Vector3 position = default(Vector3);
		for (int m = 0; m < rigidSegments; m++)
		{
			((Vector3)(ref position))._002Ector(0f, 0f, num4 / 2f + (float)m * num4);
			GameObject val7 = ((Component)this).get_gameObject();
			if (m > 0)
			{
				val7 = new GameObject("bone" + m);
				val7.get_transform().SetParent(((Component)this).get_transform(), true);
			}
			val7.get_transform().set_position(position);
			array5[m] = val7.get_transform();
			array4[m] = val7.get_transform().get_worldToLocalMatrix();
			val7.set_tag("Target");
			val7.set_layer(10);
			val7.AddComponent<Rigidbody>().set_mass(segmentMass);
			CapsuleCollider obj = val7.AddComponent<CapsuleCollider>();
			obj.set_direction(2);
			obj.set_radius(radius);
			obj.set_height(num4);
			((Collider)obj).set_sharedMaterial(ropeMaterial);
			if (m != 0)
			{
				ConfigurableJoint obj2 = val7.AddComponent<ConfigurableJoint>();
				((Joint)obj2).set_connectedBody(((Component)array5[m - 1]).GetComponent<Rigidbody>());
				ConfigurableJointMotion val8 = (ConfigurableJointMotion)0;
				obj2.set_zMotion((ConfigurableJointMotion)0);
				ConfigurableJointMotion xMotion;
				obj2.set_yMotion(xMotion = val8);
				obj2.set_xMotion(xMotion);
				val8 = (ConfigurableJointMotion)1;
				obj2.set_angularZMotion((ConfigurableJointMotion)1);
				obj2.set_angularYMotion(xMotion = val8);
				obj2.set_angularXMotion(xMotion);
				SoftJointLimitSpring val9 = default(SoftJointLimitSpring);
				((SoftJointLimitSpring)(ref val9)).set_spring(100f);
				((SoftJointLimitSpring)(ref val9)).set_damper(10f);
				obj2.set_angularXLimitSpring(val9);
				val9 = default(SoftJointLimitSpring);
				((SoftJointLimitSpring)(ref val9)).set_spring(100f);
				((SoftJointLimitSpring)(ref val9)).set_damper(10f);
				obj2.set_angularYZLimitSpring(val9);
				SoftJointLimit val10 = default(SoftJointLimit);
				((SoftJointLimit)(ref val10)).set_limit(-20f);
				obj2.set_lowAngularXLimit(val10);
				val10 = default(SoftJointLimit);
				((SoftJointLimit)(ref val10)).set_limit(20f);
				obj2.set_highAngularXLimit(val10);
				val10 = default(SoftJointLimit);
				((SoftJointLimit)(ref val10)).set_limit(20f);
				obj2.set_angularYLimit(val10);
				val10 = default(SoftJointLimit);
				((SoftJointLimit)(ref val10)).set_limit(20f);
				obj2.set_angularZLimit(val10);
				((Joint)obj2).set_axis(new Vector3(0f, 0f, 1f));
				obj2.set_secondaryAxis(new Vector3(1f, 0f, 0f));
				((Joint)obj2).set_anchor(new Vector3(0f, 0f, (0f - num4) / 2f));
				((Joint)obj2).set_connectedAnchor(new Vector3(0f, 0f, num4 / 2f));
				((Joint)obj2).set_autoConfigureConnectedAnchor(false);
			}
		}
		val6.set_bindposes(array4);
		SkinnedMeshRenderer component = ((Component)this).GetComponent<SkinnedMeshRenderer>();
		component.set_bones(array5);
		component.set_sharedMesh(val6);
		((Component)this).get_transform().set_position(Vector3.get_up());
		component.set_rootBone(array5[0]);
		val6.RecalculateBounds();
		component.set_localBounds(new Bounds(Vector3.get_zero(), Vector3.get_one() * num));
	}

	public BoneWeight CalculateBoneWeights(int ring)
	{
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f * (float)ring / (float)meshSegments;
		int num2 = Mathf.FloorToInt(num * (float)rigidSegments - 0.5f);
		float num3 = num * (float)rigidSegments - 0.5f - (float)num2;
		float num4 = num3 * num3;
		float num5 = num4 * num3;
		float num6 = 2f * num5 - 3f * num4 + 1f;
		float num7 = num5 - 2f * num4 + num3;
		float num8 = -2f * num5 + 3f * num4;
		float num9 = num5 - num4;
		float num10 = (0f - num7) / 2f;
		float num11 = num6 - num9 / 2f;
		float num12 = num7 / 2f + num8;
		float num13 = num9 / 2f;
		int item = Mathf.Clamp(num2 - 1, 0, rigidSegments - 1);
		int item2 = Mathf.Clamp(num2, 0, rigidSegments - 1);
		int item3 = Mathf.Clamp(num2 + 1, 0, rigidSegments - 1);
		int item4 = Mathf.Clamp(num2 + 2, 0, rigidSegments - 1);
		num10 = (num13 = 0f);
		num11 = 1f - num3;
		num12 = 1f - num11;
		List<int> list = new List<int>();
		List<float> list2 = new List<float>();
		list.Add(item);
		list2.Add(num10);
		if (num11 > list2[0])
		{
			list.Insert(0, item2);
			list2.Insert(0, num11);
		}
		else
		{
			list.Add(item2);
			list2.Add(num11);
		}
		if (num12 > list2[0])
		{
			list.Insert(0, item3);
			list2.Insert(0, num12);
		}
		else if (num12 > list2[1])
		{
			list.Insert(1, item3);
			list2.Insert(1, num12);
		}
		else
		{
			list.Add(item3);
			list2.Add(num12);
		}
		if (num13 > list2[0])
		{
			list.Insert(0, item4);
			list2.Insert(0, num13);
		}
		else if (num13 > list2[1])
		{
			list.Insert(1, item4);
			list2.Insert(1, num13);
		}
		else if (num13 > list2[2])
		{
			list.Insert(2, item4);
			list2.Insert(2, num13);
		}
		else
		{
			list.Add(item4);
			list2.Add(num13);
		}
		BoneWeight result = default(BoneWeight);
		((BoneWeight)(ref result)).set_boneIndex0(list[0]);
		((BoneWeight)(ref result)).set_boneIndex1(list[1]);
		((BoneWeight)(ref result)).set_boneIndex2(list[2]);
		((BoneWeight)(ref result)).set_boneIndex3(list[3]);
		((BoneWeight)(ref result)).set_weight0(list2[0]);
		((BoneWeight)(ref result)).set_weight1(list2[1]);
		((BoneWeight)(ref result)).set_weight2(list2[2]);
		((BoneWeight)(ref result)).set_weight3(list2[3]);
		return result;
	}

	private void Update()
	{
	}

	public SkinnedRope()
		: this()
	{
	}
}
