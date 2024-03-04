using UnityEngine;

namespace Multiplayer
{
	public class NetObject : MonoBehaviour
	{
		public Transform reftransform;

		public int id;

		private Vector3 startPos;

		private Quaternion startRot;

		private Vector3 netPos;

		private Quaternion netRot;

		public const float posRange = 500f;

		public const int possmall = 5;

		public const int poslarge = 9;

		public const int posfull = 18;

		public const int rotsmall = 5;

		public const int rotlarge = 8;

		public const int rotfull = 9;

		private void Awake()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			startPos = ((Component)this).get_transform().get_localPosition();
			startRot = ((Component)this).get_transform().get_localRotation();
		}

		private void Update()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (Game.GetKeyDown((KeyCode)32))
			{
				((Component)this).get_transform().set_localPosition(startPos);
				((Component)this).get_transform().set_localRotation(startRot);
			}
		}

		public void WriteDelta(NetStream stream, NetStream reference, NetStream fullStream)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			netPos = ((Component)this).get_transform().get_localPosition();
			NetVector3 netVector = NetVector3.Quantize(netPos, 500f, 18);
			NetVector3 netVector2 = ((reference == null) ? default(NetVector3) : NetVector3.Read(reference, 18));
			netRot = ((Component)this).get_transform().get_localRotation();
			NetQuaternion netQuaternion = NetQuaternion.Quantize(netRot, 9);
			NetQuaternion netQuaternion2 = ((reference == null) ? default(NetQuaternion) : NetQuaternion.Read(reference, 9));
			if (netVector != netVector2 || netQuaternion != netQuaternion2)
			{
				stream.Write(v: true);
				NetVector3.Delta(netVector2, netVector, 9).Write(stream, 5, 9, 18);
				NetQuaternion.Delta(netQuaternion2, netQuaternion, 8).Write(stream, 5, 8, 9);
			}
			else
			{
				stream.Write(v: false);
			}
			netVector.Write(fullStream);
			netQuaternion.Write(fullStream);
		}

		public void ReadDelta(NetStream stream, NetStream reference, NetStream fullStream)
		{
			bool num = stream.ReadBool();
			NetVector3 netVector = ((reference == null) ? default(NetVector3) : NetVector3.Read(reference, 18));
			NetQuaternion netQuaternion = ((reference == null) ? default(NetQuaternion) : NetQuaternion.Read(reference, 9));
			NetVector3 netVector2;
			NetQuaternion netQuaternion2;
			if (num)
			{
				NetVector3Delta delta = NetVector3Delta.Read(stream, 5, 9, 18);
				netVector2 = NetVector3.AddDelta(netVector, delta);
				NetQuaternionDelta delta2 = NetQuaternionDelta.Read(stream, 5, 8, 9);
				netQuaternion2 = NetQuaternion.AddDelta(netQuaternion, delta2);
			}
			else
			{
				netVector2 = netVector;
				netQuaternion2 = netQuaternion;
			}
			netVector2.Write(fullStream);
			netQuaternion2.Write(fullStream);
		}

		public void ApplySnapshot(NetStream stream)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			NetVector3 netVector = NetVector3.Read(stream, 18);
			NetQuaternion netQuaternion = NetQuaternion.Read(stream, 9);
			netPos = netVector.Dequantize(500f);
			netRot = netQuaternion.Dequantize();
			((Component)this).get_transform().set_localPosition(netPos);
			((Component)this).get_transform().set_localRotation(netRot);
		}

		public NetObject()
			: this()
		{
		}
	}
}
