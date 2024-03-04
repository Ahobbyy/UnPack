using System.IO;
using UnityEngine;

public class RestartableRigid : MonoBehaviour, IReset
{
	private struct RigidState
	{
		public Rigidbody rigid;

		public Vector3 position;

		public Quaternion rotation;

		public bool recorded;

		public Vector3 recordedPosition;

		public Quaternion recordedRotation;
	}

	private struct JointState
	{
		private bool valid;

		private bool isHinge;

		public Joint joint;

		public Rigidbody rigid;

		public Vector3 anchor;

		public Vector3 axis;

		public float breakForce;

		public float breakTorque;

		public Vector3 connectedAnchor;

		public Rigidbody connectedBody;

		public bool enableCollision;

		public bool enablePreprocessing;

		public JointLimits limits;

		public JointMotor motor;

		public JointSpring spring;

		public bool useLimits;

		public bool useMotor;

		public bool useSpring;

		public static JointState FromJoint(Joint joint)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			JointState jointState = default(JointState);
			jointState.joint = joint;
			jointState.rigid = ((Component)joint).GetComponent<Rigidbody>();
			jointState.anchor = joint.get_anchor();
			jointState.axis = joint.get_axis();
			jointState.breakForce = joint.get_breakForce();
			jointState.breakTorque = joint.get_breakTorque();
			jointState.connectedAnchor = joint.get_connectedAnchor();
			jointState.connectedBody = joint.get_connectedBody();
			jointState.enableCollision = joint.get_enableCollision();
			jointState.enablePreprocessing = joint.get_enablePreprocessing();
			JointState result = jointState;
			HingeJoint val = (HingeJoint)(object)((joint is HingeJoint) ? joint : null);
			if ((Object)(object)val != (Object)null)
			{
				result.isHinge = true;
				result.limits = val.get_limits();
				result.motor = val.get_motor();
				result.spring = val.get_spring();
				result.useLimits = val.get_useLimits();
				result.useMotor = val.get_useMotor();
				result.useSpring = val.get_useSpring();
				result.valid = true;
			}
			else if (joint is FixedJoint)
			{
				result.isHinge = false;
				result.valid = true;
			}
			return result;
		}

		public Joint RecreateJoint()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			if (!valid)
			{
				return null;
			}
			if ((Object)(object)joint == (Object)null)
			{
				HingeJoint val = null;
				if (isHinge)
				{
					val = (HingeJoint)(object)(joint = (Joint)(object)((Component)rigid).get_gameObject().AddComponent<HingeJoint>());
				}
				else
				{
					joint = (Joint)(object)((Component)rigid).get_gameObject().AddComponent<FixedJoint>();
				}
				joint.set_autoConfigureConnectedAnchor(false);
				joint.set_connectedBody(connectedBody);
				joint.set_anchor(anchor);
				joint.set_axis(axis);
				joint.set_breakForce(breakForce);
				joint.set_breakTorque(breakTorque);
				joint.set_connectedAnchor(connectedAnchor);
				joint.set_enableCollision(enableCollision);
				joint.set_enablePreprocessing(enablePreprocessing);
				if ((Object)(object)val != (Object)null)
				{
					val.set_limits(limits);
					val.set_motor(motor);
					val.set_spring(spring);
					val.set_useLimits(useLimits);
					val.set_useMotor(useMotor);
					val.set_useSpring(useSpring);
				}
			}
			return joint;
		}
	}

	private RigidState[] initialState;

	private JointState[] jointState;

	private void OnEnable()
	{
	}

	public void Reset(Vector3 offset)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Reset(Vector3.get_zero());
	}

	public void RecordRigidBody(StreamWriter writer)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		if (initialState == null)
		{
			return;
		}
		for (int i = 0; i < initialState.Length; i++)
		{
			if (initialState[i].rigid.IsSleeping())
			{
				RigidState rigidState = initialState[i];
				rigidState.recorded = true;
				rigidState.recordedPosition = initialState[i].rigid.get_position();
				rigidState.recordedRotation = initialState[i].rigid.get_rotation();
				initialState[i] = rigidState;
				writer.WriteLine(GetGameObjectPath(((Component)this).get_transform()));
				writer.WriteLine(GetGameObjectPath(((Component)initialState[i].rigid).get_transform()));
				Vector3 position = initialState[i].rigid.get_position();
				writer.WriteLine(((Vector3)(ref position)).ToString("F4"));
				Quaternion rotation = initialState[i].rigid.get_rotation();
				writer.WriteLine(((Quaternion)(ref rotation)).ToString("F4"));
			}
		}
	}

	private string GetGameObjectPath(Transform transform)
	{
		string text = ((Object)transform).get_name();
		while ((Object)(object)transform.get_parent() != (Object)null)
		{
			transform = transform.get_parent();
			text = ((Object)transform).get_name() + "/" + text;
		}
		return text;
	}

	public void SetRecordedInfo(string rigidBodyName, Vector3 pos, Quaternion rot)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (initialState == null)
		{
			return;
		}
		for (int i = 0; i < initialState.Length; i++)
		{
			if (GetGameObjectPath(((Component)initialState[i].rigid).get_transform()) == rigidBodyName)
			{
				RigidState rigidState = initialState[i];
				rigidState.recorded = true;
				rigidState.recordedPosition = pos;
				rigidState.recordedRotation = rot;
				initialState[i] = rigidState;
				rigidState.rigid.MovePosition(pos);
				rigidState.rigid.MoveRotation(rot);
				((Component)rigidState.rigid).get_transform().set_position(pos);
				((Component)rigidState.rigid).get_transform().set_rotation(rot);
				Rigidbody rigid = rigidState.rigid;
				Vector3 zero;
				rigidState.rigid.set_velocity(zero = Vector3.get_zero());
				rigid.set_angularVelocity(zero);
				rigidState.rigid.Sleep();
				return;
			}
		}
		Debug.Log((object)("SetRecordedInfo not found " + rigidBodyName));
	}

	public RestartableRigid()
		: this()
	{
	}
}
