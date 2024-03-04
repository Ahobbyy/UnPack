using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class MusicPlayerOnBreak : Music
	{
		public VoronoiShatter wall;

		private uint evtCollision;

		private NetIdentity identity;

		public void Start()
		{
			identity = ((Component)this).GetComponent<NetIdentity>();
			if ((Object)(object)identity != (Object)null)
			{
				evtCollision = identity.RegisterEvent(OnTriggerMusic);
			}
		}

		private void OnTriggerMusic(NetStream stream)
		{
			if ((Object)(object)mainPlayer != (Object)null)
			{
				mainPlayer.Trigger();
			}
			else
			{
				Trigger();
			}
		}

		protected override void Update()
		{
			if (ReplayRecorder.isPlaying || NetGame.isClient)
			{
				return;
			}
			if (wall.shattered)
			{
				if ((Object)(object)mainPlayer != (Object)null)
				{
					mainPlayer.Trigger();
				}
				else
				{
					Trigger();
				}
				if (Object.op_Implicit((Object)(object)identity))
				{
					identity.BeginEvent(evtCollision);
					identity.EndEvent();
				}
				((Behaviour)this).set_enabled(false);
			}
			base.Update();
		}
	}
}
