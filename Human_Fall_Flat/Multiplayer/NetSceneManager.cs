using UnityEngine;

namespace Multiplayer
{
	[RequireComponent(typeof(NetIdentity))]
	public class NetSceneManager : MonoBehaviour
	{
		public static NetSceneManager instance;

		private NetIdentity identity;

		private uint evtCheckpoint;

		private uint evtResetLevel;

		private void Start()
		{
			identity = ((Component)this).GetComponent<NetIdentity>();
			if (Object.op_Implicit((Object)(object)identity))
			{
				instance = this;
				evtCheckpoint = identity.RegisterEvent(OnEnterCheckpoint);
				evtResetLevel = identity.RegisterEvent(OnResetLevel);
			}
		}

		private void OnDestroy()
		{
			if ((Object)(object)instance == (Object)(object)this)
			{
				instance = null;
			}
		}

		public static void EnterCheckpoint(int number, int subObjectives)
		{
			if (Object.op_Implicit((Object)(object)instance))
			{
				NetStream netStream = instance.identity.BeginEvent(instance.evtCheckpoint);
				netStream.Write((uint)number, 6);
				netStream.Write((uint)subObjectives, 6);
				instance.identity.EndEvent();
				Debug.Log((object)"Send Enter Checkpoint Message");
			}
		}

		public static void ResetLevel(int checkpoint, int subObjectives)
		{
			if (Object.op_Implicit((Object)(object)instance))
			{
				NetStream netStream = instance.identity.BeginEvent(instance.evtResetLevel);
				netStream.Write((uint)checkpoint, 6);
				netStream.Write((uint)subObjectives, 6);
				instance.identity.EndEvent();
				Debug.Log((object)"Send Reset Level Message");
			}
		}

		public static void OnEnterCheckpoint(NetStream stream)
		{
			int checkpoint = (int)stream.ReadUInt32(6);
			int subObjectives = (int)stream.ReadUInt32(6);
			Game.instance.EnterCheckpoint(checkpoint, subObjectives);
			Debug.Log((object)"Received Checkpoint Message");
		}

		public static void OnResetLevel(NetStream stream)
		{
			int checkpoint = (int)stream.ReadUInt32(6);
			int subObjectives = (int)stream.ReadUInt32(6);
			Game.currentLevel.Reset(checkpoint, subObjectives);
			Game.currentLevel.PostEndReset(checkpoint);
			Game.instance.currentCheckpointNumber = 0;
			Game.instance.currentCheckpointSubObjectives = 0;
			Debug.Log((object)"Received Reset Level Message");
		}

		public NetSceneManager()
			: this()
		{
		}
	}
}
