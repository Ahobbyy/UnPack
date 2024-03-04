using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Sound/Signal Trigger", 10)]
	public class SignalSoundTrigger : Node
	{
		public NodeInput input;

		private bool knownState;

		private Sound2 sound2;

		protected void Awake()
		{
			priority = NodePriority.Update;
			sound2 = ((Component)this).GetComponent<Sound2>();
			if ((Object)(object)sound2 == (Object)null)
			{
				Debug.LogError((object)"SignalSoundTrigger requires a sound", (Object)(object)this);
			}
		}

		public override void Process()
		{
			base.Process();
			bool flag = input.value >= 0.5f;
			if (flag != knownState)
			{
				knownState = flag;
				if (flag && !SignalManager.skipTransitions)
				{
					sound2.PlayOneShot();
				}
			}
		}
	}
}
