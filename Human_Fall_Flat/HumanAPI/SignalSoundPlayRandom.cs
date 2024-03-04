using UnityEngine;

namespace HumanAPI
{
	public class SignalSoundPlayRandom : Node
	{
		public NodeInput input;

		[SerializeField]
		private AudioSource target;

		[SerializeField]
		private AudioClip[] randomSounds;

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
				if (flag && !SignalManager.skipTransitions && (Object)(object)target != (Object)null && !target.get_isPlaying())
				{
					target.set_clip(randomSounds[Random.Range(0, randomSounds.Length)]);
					target.Play();
				}
			}
		}
	}
}
