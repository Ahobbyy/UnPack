using UnityEngine;

public class HintClockTower : Hint
{
	public Rigidbody theCog;

	protected override bool StillValid()
	{
		if ((Object)(object)theCog != (Object)null)
		{
			return !theCog.get_isKinematic();
		}
		return false;
	}
}
