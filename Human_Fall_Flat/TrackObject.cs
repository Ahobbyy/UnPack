using Multiplayer;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
	[SerializeField]
	private GameObject objectToTrack;

	private void FixedUpdate()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient && (Object)(object)objectToTrack != (Object)null)
		{
			((Component)this).get_transform().set_position(objectToTrack.get_transform().get_position());
			((Component)this).get_transform().set_rotation(objectToTrack.get_transform().get_rotation());
		}
	}

	public TrackObject()
		: this()
	{
	}
}
