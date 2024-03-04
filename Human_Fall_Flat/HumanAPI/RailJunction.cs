using UnityEngine;

namespace HumanAPI
{
	public class RailJunction : Node
	{
		public bool showDebug;

		public NodeInput select;

		public RailEnd forkA;

		public RailEnd forkB;

		public RailSnap junctionLock;

		public GameObject trackA;

		public GameObject trackB;

		public override void Process()
		{
			base.Process();
			SyncFork();
		}

		private void SyncFork()
		{
			if ((Object)(object)junctionLock != (Object)null && (Object)(object)junctionLock.currentRail == (Object)(object)forkA.rail)
			{
				forkA.connectedTo.connectedTo = forkA;
			}
			else if ((Object)(object)junctionLock != (Object)null && (Object)(object)junctionLock.currentRail == (Object)(object)forkB.rail)
			{
				forkB.connectedTo.connectedTo = forkB;
			}
			else if (select.value < -0.5f)
			{
				forkA.connectedTo.connectedTo = forkA;
				if ((Object)(object)trackA != (Object)null)
				{
					trackA.SetActive(false);
				}
				if ((Object)(object)trackB != (Object)null)
				{
					trackB.SetActive(true);
				}
			}
			else if (select.value > 0.5f)
			{
				forkB.connectedTo.connectedTo = forkB;
				if ((Object)(object)trackB != (Object)null)
				{
					trackB.SetActive(false);
				}
				if ((Object)(object)trackA != (Object)null)
				{
					trackA.SetActive(true);
				}
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if ((Object)(object)junctionLock != (Object)null && (Object)(object)((Component)junctionLock).get_transform().get_parent() == (Object)(object)((Component)other).get_transform().get_parent())
			{
				SyncFork();
			}
		}
	}
}
