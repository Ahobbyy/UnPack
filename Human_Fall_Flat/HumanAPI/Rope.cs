using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class Rope : RopeRender, INetBehavior, IRespawnable
	{
		[Tooltip("A list of points to use in connection with the rope")]
		public Transform[] handles;

		[Tooltip("Whether or not the start  of the rope should be a fixed location")]
		public bool fixStart;

		[Tooltip("Whether or not hte end of the rope should be a fixed location")]
		public bool fixEnd;

		[Tooltip("The start joint is locked")]
		public bool fixStartDir;

		[Tooltip("The end joint is fixed")]
		public bool fixEndDir;

		[Tooltip("Start location for the Rope via a connected Rigid body")]
		public Rigidbody startBody;

		[Tooltip("End location for the Rope via a connected Rigid body")]
		public Rigidbody endBody;

		[Tooltip("How much to divide up the rope")]
		public int rigidSegments = 10;

		[Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
		public float segmentMass = 20f;

		[Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
		public float lengthMultiplier = 1f;

		[Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
		public PhysicMaterial ropeMaterial;

		protected Transform[] bones;

		protected NetBodySleep[] boneSleep;

		[Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
		public Vector3[] bonePos;

		[Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
		public Vector3[] boneRot;

		[Tooltip("Print stuff from this script to the Log")]
		public bool showDebug;

		private bool initialized;

		private Vector3[] boneForward;

		private Vector3[] boneRight;

		private Vector3[] boneUp;

		protected Vector3[] originalPositions;

		protected float boneLen;

		private NetStream originalState;

		private NetVector3Encoder posEncoder = new NetVector3Encoder(500f, 18, 4, 8);

		private NetVector3Encoder diffEncoder = new NetVector3Encoder(3.90625f, 11, 3, 7);

		private NetQuaternionEncoder rotEncoder = new NetQuaternionEncoder(9, 4, 6);

		public virtual Vector3[] GetHandlePositions()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Get Handle positions "));
			}
			Vector3[] array = (Vector3[])(object)new Vector3[handles.Length];
			for (int i = 0; i < handles.Length; i++)
			{
				array[i] = handles[i].get_position();
			}
			return array;
		}

		public override void OnEnable()
		{
			EnsureInitialized();
		}

		private void EnsureInitialized()
		{
			if (!initialized)
			{
				initialized = true;
				Initialize();
			}
		}

		private void Initialize()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_0661: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Unknown result type (might be due to invalid IL or missing references)
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0769: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Unknown result type (might be due to invalid IL or missing references)
			//IL_0806: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " OnEnable "));
			}
			if ((Object)(object)((Component)this).GetComponent<NetIdentity>() == (Object)null)
			{
				Debug.LogError((object)"Missing NetIdentity", (Object)(object)this);
			}
			Vector3[] handlePositions = GetHandlePositions();
			if (handlePositions.Length < 2)
			{
				return;
			}
			Vector3 val = handlePositions[0];
			Vector3 val2 = handlePositions[handlePositions.Length - 1];
			float num = 0f;
			for (int i = 0; i < handlePositions.Length - 1; i++)
			{
				float num2 = num;
				Vector3 val3 = handlePositions[i] - handlePositions[i + 1];
				num = num2 + ((Vector3)(ref val3)).get_magnitude();
			}
			num *= lengthMultiplier;
			boneLen = num / (float)rigidSegments;
			bones = (Transform[])(object)new Transform[rigidSegments];
			boneSleep = new NetBodySleep[rigidSegments];
			boneRight = (Vector3[])(object)new Vector3[rigidSegments];
			boneUp = (Vector3[])(object)new Vector3[rigidSegments];
			originalPositions = (Vector3[])(object)new Vector3[bones.Length];
			boneForward = (Vector3[])(object)new Vector3[rigidSegments];
			Vector3 val4 = (val2 - val) / (float)rigidSegments;
			Quaternion rotation = Quaternion.LookRotation(((Vector3)(ref val4)).get_normalized());
			Vector3 zero = Vector3.get_zero();
			for (int j = 0; j < rigidSegments; j++)
			{
				zero = val + val4 * (0.5f + (float)j);
				GameObject gameObject = ((Component)this).get_gameObject();
				gameObject = new GameObject("bone" + j);
				gameObject.get_transform().SetParent(((Component)this).get_transform(), true);
				originalPositions[j] = zero;
				gameObject.get_transform().set_position(zero);
				gameObject.get_transform().set_rotation(rotation);
				bones[j] = gameObject.get_transform();
				gameObject.set_tag("Target");
				gameObject.set_layer(14);
				Rigidbody val5 = gameObject.AddComponent<Rigidbody>();
				val5.set_mass(segmentMass);
				val5.set_drag(0.1f);
				val5.set_angularDrag(0.1f);
				CapsuleCollider obj = gameObject.AddComponent<CapsuleCollider>();
				obj.set_direction(2);
				obj.set_radius(radius);
				obj.set_height(boneLen + radius * 2f);
				((Collider)obj).set_sharedMaterial(ropeMaterial);
				if (j != 0)
				{
					ConfigurableJoint obj2 = gameObject.AddComponent<ConfigurableJoint>();
					((Joint)obj2).set_connectedBody(((Component)bones[j - 1]).GetComponent<Rigidbody>());
					ConfigurableJointMotion val6 = (ConfigurableJointMotion)0;
					obj2.set_zMotion((ConfigurableJointMotion)0);
					ConfigurableJointMotion xMotion;
					obj2.set_yMotion(xMotion = val6);
					obj2.set_xMotion(xMotion);
					val6 = (ConfigurableJointMotion)1;
					obj2.set_angularZMotion((ConfigurableJointMotion)1);
					obj2.set_angularYMotion(xMotion = val6);
					obj2.set_angularXMotion(xMotion);
					SoftJointLimitSpring val7 = default(SoftJointLimitSpring);
					((SoftJointLimitSpring)(ref val7)).set_spring(100f);
					((SoftJointLimitSpring)(ref val7)).set_damper(10f);
					obj2.set_angularXLimitSpring(val7);
					val7 = default(SoftJointLimitSpring);
					((SoftJointLimitSpring)(ref val7)).set_spring(100f);
					((SoftJointLimitSpring)(ref val7)).set_damper(10f);
					obj2.set_angularYZLimitSpring(val7);
					SoftJointLimit val8 = default(SoftJointLimit);
					((SoftJointLimit)(ref val8)).set_limit(-20f);
					obj2.set_lowAngularXLimit(val8);
					val8 = default(SoftJointLimit);
					((SoftJointLimit)(ref val8)).set_limit(20f);
					obj2.set_highAngularXLimit(val8);
					val8 = default(SoftJointLimit);
					((SoftJointLimit)(ref val8)).set_limit(20f);
					obj2.set_angularYLimit(val8);
					val8 = default(SoftJointLimit);
					((SoftJointLimit)(ref val8)).set_limit(20f);
					obj2.set_angularZLimit(val8);
					JointDrive val9 = default(JointDrive);
					((JointDrive)(ref val9)).set_positionSpring(50f);
					obj2.set_angularXDrive(val9);
					val9 = default(JointDrive);
					((JointDrive)(ref val9)).set_positionSpring(50f);
					obj2.set_angularYZDrive(val9);
					((Joint)obj2).set_axis(new Vector3(0f, 0f, 1f));
					obj2.set_secondaryAxis(new Vector3(1f, 0f, 0f));
					((Joint)obj2).set_autoConfigureConnectedAnchor(false);
					((Joint)obj2).set_anchor(new Vector3(0f, 0f, (0f - boneLen) / 2f));
					((Joint)obj2).set_connectedAnchor(new Vector3(0f, 0f, boneLen / 2f));
					obj2.set_projectionMode((JointProjectionMode)1);
					obj2.set_projectionDistance(0.02f);
				}
				boneSleep[j] = new NetBodySleep(val5);
			}
			float num3 = (0f - boneLen) / 2f / lengthMultiplier;
			int num4 = -1;
			zero = Vector3.get_zero();
			Vector3 val10 = Vector3.get_zero();
			for (int k = 0; k < rigidSegments; k++)
			{
				Vector3 val11;
				for (; num3 <= 0f; num3 += ((Vector3)(ref val11)).get_magnitude())
				{
					num4++;
					val11 = handlePositions[num4 + 1] - handlePositions[num4];
					val10 = ((Vector3)(ref val11)).get_normalized();
					rotation = Quaternion.LookRotation(val10);
					zero = handlePositions[num4] - num3 * val10;
				}
				((Component)bones[k]).get_transform().set_position(zero);
				((Component)bones[k]).get_transform().set_rotation(rotation);
				zero += val10 * boneLen / lengthMultiplier;
				num3 -= boneLen / lengthMultiplier;
			}
			if (fixStart && bones != null && bones.Length != 0)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Fix Start "));
				}
				ConfigurableJoint val12 = ((Component)bones[0]).get_gameObject().AddComponent<ConfigurableJoint>();
				ConfigurableJointMotion val6 = (ConfigurableJointMotion)0;
				val12.set_zMotion((ConfigurableJointMotion)0);
				ConfigurableJointMotion xMotion;
				val12.set_yMotion(xMotion = val6);
				val12.set_xMotion(xMotion);
				val12.set_projectionMode((JointProjectionMode)1);
				val12.set_projectionDistance(0.02f);
				if (fixStartDir)
				{
					val6 = (ConfigurableJointMotion)0;
					val12.set_angularZMotion((ConfigurableJointMotion)0);
					val12.set_angularYMotion(xMotion = val6);
					val12.set_angularXMotion(xMotion);
				}
				((Joint)val12).set_anchor(new Vector3(0f, 0f, (0f - boneLen) / 2f));
				((Joint)val12).set_autoConfigureConnectedAnchor(false);
				if ((Object)(object)startBody != (Object)null)
				{
					((Joint)val12).set_connectedBody(startBody);
					((Joint)val12).set_connectedAnchor(((Component)startBody).get_transform().InverseTransformPoint(val));
				}
				else
				{
					((Joint)val12).set_connectedAnchor(val);
				}
			}
			if (fixEnd)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Fix End "));
				}
				ConfigurableJoint val13 = ((Component)bones[bones.Length - 1]).get_gameObject().AddComponent<ConfigurableJoint>();
				ConfigurableJointMotion val6 = (ConfigurableJointMotion)0;
				val13.set_zMotion((ConfigurableJointMotion)0);
				ConfigurableJointMotion xMotion;
				val13.set_yMotion(xMotion = val6);
				val13.set_xMotion(xMotion);
				val13.set_projectionMode((JointProjectionMode)1);
				val13.set_projectionDistance(0.02f);
				if (fixEndDir)
				{
					val6 = (ConfigurableJointMotion)0;
					val13.set_angularZMotion((ConfigurableJointMotion)0);
					val13.set_angularYMotion(xMotion = val6);
					val13.set_angularXMotion(xMotion);
				}
				((Joint)val13).set_anchor(new Vector3(0f, 0f, boneLen / 2f));
				((Joint)val13).set_autoConfigureConnectedAnchor(false);
				if ((Object)(object)endBody != (Object)null)
				{
					((Joint)val13).set_connectedBody(endBody);
					((Joint)val13).set_connectedAnchor(((Component)endBody).get_transform().InverseTransformPoint(val2));
				}
				else
				{
					((Joint)val13).set_connectedAnchor(val2);
				}
			}
			if (bonePos == null || bonePos.Length != rigidSegments)
			{
				bonePos = (Vector3[])(object)new Vector3[rigidSegments];
				boneRot = (Vector3[])(object)new Vector3[rigidSegments];
			}
			else
			{
				for (int l = 0; l < rigidSegments; l++)
				{
					((Component)bones[l]).get_transform().set_position(((Component)this).get_transform().TransformPoint(bonePos[l]));
					((Component)bones[l]).get_transform().set_rotation(((Component)this).get_transform().get_rotation() * Quaternion.Euler(boneRot[l]));
				}
			}
			base.OnEnable();
			originalState = NetStream.AllocStream();
			CollectState(originalState);
		}

		private void OnDestroy()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " OnDestroy "));
			}
			if (originalState != null)
			{
				originalState = originalState.Release();
			}
		}

		public override void CheckDirty()
		{
			base.CheckDirty();
			EnsureInitialized();
			Rigidbody component = ((Component)bones[0]).GetComponent<Rigidbody>();
			if (!component.IsSleeping() && !component.get_isKinematic())
			{
				isDirty = true;
			}
		}

		public override void ReadData()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < rigidSegments; i++)
			{
				Quaternion val = Quaternion.Inverse(((Component)this).get_transform().get_rotation());
				bonePos[i] = ((Component)this).get_transform().InverseTransformPoint(bones[i].get_position());
				Quaternion val2 = val * bones[i].get_rotation();
				boneRot[i] = ((Quaternion)(ref val2)).get_eulerAngles();
				boneForward[i] = val2 * Vector3.get_forward();
				boneUp[i] = val2 * Vector3.get_up();
				boneRight[i] = val2 * Vector3.get_right();
			}
		}

		public override void GetPoint(float dist, out Vector3 pos, out Vector3 normal, out Vector3 binormal)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			int num = Mathf.FloorToInt(dist * (float)rigidSegments - 0.5f);
			float num2 = dist * (float)rigidSegments - 0.5f - (float)num;
			int num3 = Mathf.Clamp(num, 0, rigidSegments - 1);
			int num4 = Mathf.Clamp(num + 1, 0, rigidSegments - 1);
			if (num == -1)
			{
				normal = boneRight[num3];
				binormal = boneUp[num3];
				pos = bonePos[num3] + boneForward[num3] * boneLen * (num2 - 1f);
				return;
			}
			if (num == rigidSegments - 1)
			{
				normal = boneRight[num3];
				binormal = boneUp[num3];
				pos = bonePos[num3] + boneForward[num3] * boneLen * num2;
				return;
			}
			float num5 = num2 * num2;
			float num6 = num5 * num2;
			float num7 = 2f * num6 - 3f * num5 + 1f;
			float num8 = num6 - 2f * num5 + num2;
			float num9 = -2f * num6 + 3f * num5;
			float num10 = num6 - num5;
			Vector3 val = boneForward[num3] * boneLen;
			Vector3 val2 = boneForward[num4] * boneLen;
			pos = num7 * bonePos[num3] + num8 * val + num9 * bonePos[num4] + num10 * val2;
			normal = num7 * boneRight[num3] + num9 * boneRight[num4];
			binormal = num7 * boneUp[num3] + num9 * boneUp[num4];
		}

		public void StartNetwork(NetIdentity identity)
		{
		}

		public virtual void ResetState(int checkpoint, int subObjectives)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			EnsureInitialized();
			if (originalState != null)
			{
				originalState.Seek(0);
				ApplyState(originalState);
				for (int i = 0; i < rigidSegments; i++)
				{
					Rigidbody component = ((Component)bones[i]).GetComponent<Rigidbody>();
					Vector3 zero;
					component.set_angularVelocity(zero = Vector3.get_zero());
					component.set_velocity(zero);
				}
			}
			isDirty = true;
		}

		public void SetMaster(bool isMaster)
		{
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc Max Delta in Bits  "));
			}
			return posEncoder.CalculateMaxDeltaSizeInBits() + rigidSegments * diffEncoder.CalculateMaxDeltaSizeInBits();
		}

		private void FixedUpdate()
		{
			EnsureInitialized();
			if (boneSleep != null)
			{
				for (int i = 0; i < rigidSegments; i++)
				{
					boneSleep[i].HandleSleep();
				}
			}
		}

		public void CollectState(NetStream stream)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Collect State "));
			}
			EnsureInitialized();
			Vector3 value = bones[0].TransformPoint(-Vector3.get_forward() * boneLen / 2f);
			posEncoder.CollectState(stream, value);
			value = posEncoder.Dequantize(posEncoder.Quantize(value));
			for (int i = 0; i < rigidSegments; i++)
			{
				Vector3 value2 = bones[i].TransformPoint(Vector3.get_forward() * boneLen / 2f) - value;
				diffEncoder.CollectState(stream, value2);
				value += diffEncoder.Dequantize(diffEncoder.Quantize(value2));
			}
		}

		public virtual void ApplyState(NetStream state)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply State "));
			}
			EnsureInitialized();
			Vector3 val = posEncoder.ApplyState(state);
			for (int i = 0; i < rigidSegments; i++)
			{
				Transform val2 = bones[i];
				Vector3 val3 = diffEncoder.ApplyState(state);
				Vector3 val4 = val + val3;
				Vector3 val5 = (val + val4) / 2f;
				Quaternion rotation = ((i == 0) ? Quaternion.LookRotation(val4 - val) : Quaternion.LookRotation(val4 - val, bones[i - 1].get_up()));
				if (val2.get_position() != val5)
				{
					val2.set_position(val5);
					isDirty = true;
				}
				Quaternion rotation2 = val2.get_rotation();
				if (((Quaternion)(ref rotation2)).get_eulerAngles() != ((Quaternion)(ref rotation)).get_eulerAngles())
				{
					val2.set_rotation(rotation);
					isDirty = true;
				}
				val = val4;
			}
		}

		public virtual void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply Lerped State "));
			}
			EnsureInitialized();
			Vector3 val = posEncoder.ApplyLerpedState(state0, state1, mix);
			for (int i = 0; i < rigidSegments; i++)
			{
				Transform val2 = bones[i];
				Vector3 val3 = diffEncoder.ApplyLerpedState(state0, state1, mix);
				Vector3 val4 = val + val3;
				Vector3 val5 = (val + val4) / 2f;
				Quaternion rotation = ((i == 0) ? Quaternion.LookRotation(val4 - val) : Quaternion.LookRotation(val4 - val, bones[i - 1].get_up()));
				if (val2.get_position() != val5)
				{
					val2.set_position(val5);
					isDirty = true;
				}
				Quaternion rotation2 = val2.get_rotation();
				if (((Quaternion)(ref rotation2)).get_eulerAngles() != ((Quaternion)(ref rotation)).get_eulerAngles())
				{
					val2.set_rotation(rotation);
					isDirty = true;
				}
				val = val4;
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calculate Delta "));
			}
			posEncoder.CalculateDelta(state0, state1, delta);
			for (int i = 0; i < rigidSegments; i++)
			{
				diffEncoder.CalculateDelta(state0, state1, delta);
			}
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Add Delta "));
			}
			posEncoder.AddDelta(state0, delta, result);
			for (int i = 0; i < rigidSegments; i++)
			{
				diffEncoder.AddDelta(state0, delta, result);
			}
		}

		public void Respawn(Vector3 offset)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Respawn "));
			}
			ResetState(0, 0);
			for (int i = 0; i < rigidSegments; i++)
			{
				Transform obj = bones[i];
				obj.set_position(obj.get_position() + offset);
			}
			ForceUpdate();
		}
	}
}
