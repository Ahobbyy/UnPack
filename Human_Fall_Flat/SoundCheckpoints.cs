using System.Collections.Generic;
using UnityEngine;

public class SoundCheckpoints : MonoBehaviour
{
	[SerializeField]
	private Transform root;

	private List<Transform> soundCheckpoints = new List<Transform>();

	private int currentCheckpoint = -1;

	private void Start()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		if ((Object)(object)root == (Object)null)
		{
			return;
		}
		foreach (Transform item2 in ((Component)root).get_gameObject().get_transform())
		{
			Transform item = item2;
			soundCheckpoints.Add(item);
		}
	}

	public void GoPreviousCheckpoint()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (soundCheckpoints.Count < 1)
		{
			return;
		}
		GameObject val = GameObject.Find("Player(Clone)");
		if (!((Object)(object)val == (Object)null))
		{
			Transform val2 = val.get_transform().Find("Ball");
			if (!((Object)(object)val2 == (Object)null) && currentCheckpoint > 0)
			{
				((Component)val2).get_transform().set_position(soundCheckpoints[currentCheckpoint - 1].get_position());
				((Component)val2).get_transform().set_rotation(soundCheckpoints[currentCheckpoint - 1].get_rotation());
				currentCheckpoint--;
			}
		}
	}

	public void GoNextCheckpoint()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (soundCheckpoints.Count < 1)
		{
			return;
		}
		GameObject val = GameObject.Find("Player(Clone)");
		if (!((Object)(object)val == (Object)null))
		{
			Transform val2 = val.get_transform().Find("Ball");
			if (!((Object)(object)val2 == (Object)null) && currentCheckpoint + 1 < soundCheckpoints.Count)
			{
				((Component)val2).get_transform().set_position(soundCheckpoints[currentCheckpoint + 1].get_position());
				((Component)val2).get_transform().set_rotation(soundCheckpoints[currentCheckpoint + 1].get_rotation());
				currentCheckpoint++;
			}
		}
	}

	private void Update()
	{
		if (Game.GetKeyDown((KeyCode)283))
		{
			GoPreviousCheckpoint();
		}
		else if (Game.GetKeyDown((KeyCode)284))
		{
			GoNextCheckpoint();
		}
	}

	public SoundCheckpoints()
		: this()
	{
	}
}
