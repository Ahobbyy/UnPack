using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Sound/Signal Trigger", 10)]
	public class SignalSoundVolumeUp : Node
	{
		public NodeInput input;

		[SerializeField]
		private string soundGameObjectName = "";

		[SerializeField]
		private float targetVolume;

		private bool knownState;

		private Sound2 sound2;

		protected void Awake()
		{
			priority = NodePriority.Update;
		}

		public override void Process()
		{
			base.Process();
			bool flag = input.value >= 0.5f;
			if (flag == knownState)
			{
				return;
			}
			knownState = flag;
			if (!flag || SignalManager.skipTransitions)
			{
				return;
			}
			GameObject val = GameObject.Find(soundGameObjectName);
			if ((Object)(object)val != (Object)null)
			{
				sound2 = val.GetComponent<Sound2>();
				if ((Object)(object)sound2 != (Object)null)
				{
					sound2.SetBaseVolume(0.35f);
				}
			}
		}
	}
}
