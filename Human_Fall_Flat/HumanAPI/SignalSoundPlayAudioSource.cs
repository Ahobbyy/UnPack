using UnityEngine;

namespace HumanAPI
{
	public class SignalSoundPlayAudioSource : Node
	{
		public NodeInput input;

		[SerializeField]
		private AudioSource target;

		[SerializeField]
		private bool overlayPlaying;

		private bool knownState;

		protected void Awake()
		{
			priority = NodePriority.Update;
		}

		public override void Process()
		{
			base.Process();
			bool flag = input.value >= 0.5f;
			if (flag != knownState)
			{
				knownState = flag;
				if (flag && !SignalManager.skipTransitions && (Object)(object)target != (Object)null && (overlayPlaying || (!overlayPlaying && !target.get_isPlaying())))
				{
					target.Play();
				}
			}
		}
	}
}
