using UnityEngine;

public class HintTreeChop : Hint
{
	public TreeCutting theTree;

	protected override bool StillValid()
	{
		if ((Object)(object)theTree != (Object)null)
		{
			return !theTree.IsCut;
		}
		return false;
	}
}
