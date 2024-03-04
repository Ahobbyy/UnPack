using UnityEngine;
using UnityEngine.UI;

public class NavigationItem
{
	public Selectable selectable;

	public AutoNavigation subNavigation;

	public Selectable GetSelectable(NavigationItemDirection direction)
	{
		if ((Object)(object)selectable != (Object)null)
		{
			return selectable;
		}
		return subNavigation.GetSelectableItem(direction);
	}

	public void Bind(NavigationItemDirection direction, Selectable next)
	{
		if ((Object)(object)selectable != (Object)null)
		{
			Bind(direction, selectable, next);
		}
		else if ((Object)(object)subNavigation != (Object)null)
		{
			subNavigation.Bind(direction, next);
		}
	}

	private void Bind(NavigationItemDirection direction, Selectable selectable, Selectable next)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Navigation navigation = selectable.get_navigation();
		((Navigation)(ref navigation)).set_mode((Mode)4);
		switch (direction)
		{
		case NavigationItemDirection.Up:
			((Navigation)(ref navigation)).set_selectOnUp(next);
			break;
		case NavigationItemDirection.Down:
			((Navigation)(ref navigation)).set_selectOnDown(next);
			break;
		case NavigationItemDirection.Left:
			((Navigation)(ref navigation)).set_selectOnLeft(next);
			break;
		case NavigationItemDirection.Right:
			((Navigation)(ref navigation)).set_selectOnRight(next);
			break;
		}
		selectable.set_navigation(navigation);
	}
}
