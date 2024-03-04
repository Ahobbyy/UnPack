using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Sound/Music Player", 10)]
	public class MusicPlayer : Music, IPostEndReset
	{
		private MusicManager musicManager;

		private uint evtCollision;

		private NetIdentity identity;

		private void Awake()
		{
			musicManager = Object.FindObjectOfType<MusicManager>();
		}

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

		public void OnTriggerEnter(Collider other)
		{
			if (!ReplayRecorder.isPlaying && !NetGame.isClient && ((Component)other).get_gameObject().get_tag() == "Player")
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
			}
		}

		void IPostEndReset.PostEndResetState(int checkpoint)
		{
			if (checkpoint == 0)
			{
				((Behaviour)this).set_enabled(true);
				lastPlayTime = float.MinValue;
				Music.currentMusic = null;
				if (Object.op_Implicit((Object)(object)musicManager))
				{
					musicManager.currentSong = null;
				}
			}
		}
	}
}
