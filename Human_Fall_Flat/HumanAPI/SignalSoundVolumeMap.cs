using UnityEngine;

namespace HumanAPI
{
	public class SignalSoundVolumeMap : Node
	{
		public NodeInput input;

		[SerializeField]
		private string soundGameObjectName = "";

		[SerializeField]
		private float multiplyFactor = 1f;

		private bool knownState;

		private Sound2 sound2;

		protected void Awake()
		{
			priority = NodePriority.Update;
		}

		public override void Process()
		{
			base.Process();
			_ = input.value;
			if ((Object)(object)sound2 != (Object)null)
			{
				sound2.SetBaseVolume(input.value * multiplyFactor);
			}
			else
			{
				FindSound();
			}
		}

		private void FindSound()
		{
			GameObject val = GameObject.Find(soundGameObjectName);
			if ((Object)(object)val != (Object)null)
			{
				sound2 = val.GetComponent<Sound2>();
			}
		}
	}
}
