using UnityEngine;

namespace HumanAPI
{
	public class ReleaseGrabSignal : Node
	{
		public float threshold = 1f;

		public NodeInput input;

		public GameObject item;

		public float blockTime = 1f;

		private void Awake()
		{
			if ((Object)(object)item == (Object)null)
			{
				item = ((Component)this).get_gameObject();
			}
		}

		private void Update()
		{
			if (input.value >= threshold && GrabManager.IsGrabbedAny(item))
			{
				GrabManager.Release(item, blockTime);
			}
		}
	}
}
