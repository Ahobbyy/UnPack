using UnityEngine;

namespace HumanAPI
{
	public class SignalSoundLoopExternal : Node
	{
		public NodeInput input;

		[SerializeField]
		private string soundGameObjectName = "";

		private Sound2 sound2;

		private bool knownState;

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((Object)(object)sound2 == (Object)null)
			{
				GameObject val = GameObject.Find(soundGameObjectName);
				if ((Object)(object)val != (Object)null)
				{
					sound2 = val.GetComponent<Sound2>();
				}
			}
		}

		public override void Process()
		{
			base.Process();
			bool flag = Mathf.Abs(input.value) >= 0.1f;
			if (flag != knownState && (Object)(object)sound2 != (Object)null)
			{
				knownState = flag;
				if (flag)
				{
					sound2.Play();
				}
				else
				{
					sound2.Stop();
				}
			}
		}
	}
}
