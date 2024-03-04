using HumanAPI;
using Multiplayer;
using UnityEngine;

public class RespawnRigidsCheckpoint : Checkpoint
{
	public RestartableRigid[] restartList;

	public NetBody[] bodies;

	public override void RespawnHere()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		base.RespawnHere();
		for (int i = 0; i < restartList.Length; i++)
		{
			if ((Object)(object)restartList[i] != (Object)null)
			{
				restartList[i].Reset(Vector3.get_zero());
			}
		}
		for (int j = 0; j < bodies.Length; j++)
		{
			RespawnRoot componentInParent = ((Component)bodies[j]).GetComponentInParent<RespawnRoot>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				componentInParent.Respawn(Vector3.get_zero());
			}
			else
			{
				bodies[j].Respawn(Vector3.get_zero());
			}
		}
	}
}
