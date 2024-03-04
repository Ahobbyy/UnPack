public class WalkTutorial : TutorialBlock
{
	public override bool IsPlayerActivityMakingSense()
	{
		for (int i = 0; i < Human.all.Count; i++)
		{
			if (Human.all[i].state == HumanState.Walk)
			{
				return true;
			}
		}
		return false;
	}
}
