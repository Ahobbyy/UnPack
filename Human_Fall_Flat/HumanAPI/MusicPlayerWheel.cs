using UnityEngine;

namespace HumanAPI
{
	public class MusicPlayerWheel : Music, IReset
	{
		[SerializeField]
		private Rigidbody rigidbody;

		private MusicManager musicManager;

		private void Awake()
		{
			musicManager = Object.FindObjectOfType<MusicManager>();
		}

		protected override void Update()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 angularVelocity = rigidbody.get_angularVelocity();
			if (((Vector3)(ref angularVelocity)).get_magnitude() > 1f)
			{
				if ((Object)(object)mainPlayer != (Object)null)
				{
					mainPlayer.Trigger();
				}
				else
				{
					Trigger();
				}
				((Behaviour)this).set_enabled(false);
			}
			base.Update();
		}

		void IReset.ResetState(int checkpoint, int subObjectives)
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
