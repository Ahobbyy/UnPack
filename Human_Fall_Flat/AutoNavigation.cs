using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoNavigation : MonoBehaviour
{
	private List<NavigationItem> items = new List<NavigationItem>();

	public NavigationDirection direction;

	public Selectable defaultItem;

	public bool selectLastFocused = true;

	public int groupCount = 1;

	public int itemsPerGroup;

	public bool fixedItemsPerGroup;

	public List<Selectable> ignoreObjects;

	private bool invalid = true;

	private Selectable current;

	private AutoNavigation currentChild;

	public Selectable GetSelectableItem(NavigationItemDirection direction)
	{
		if (groupCount > 1 && ((Object)(object)current != (Object)null || (Object)(object)currentChild != (Object)null))
		{
			int num = -1;
			for (int i = 0; i < items.Count; i++)
			{
				if (((Object)(object)current != (Object)null && (Object)(object)items[i].selectable == (Object)(object)current) || ((Object)(object)currentChild != (Object)null && (Object)(object)items[i].subNavigation == (Object)(object)currentChild))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return null;
			}
			NavigationItem navigationItem = (((this.direction != NavigationDirection.Vertical || direction != NavigationItemDirection.Right) && (this.direction != 0 || direction != NavigationItemDirection.Down)) ? (((this.direction != NavigationDirection.Vertical || direction != NavigationItemDirection.Left) && (this.direction != 0 || direction != 0)) ? (((this.direction != NavigationDirection.Vertical || direction != NavigationItemDirection.Down) && (this.direction != 0 || direction != NavigationItemDirection.Right)) ? items[Mathf.Min(items.Count - 1, (num / itemsPerGroup + 1) * itemsPerGroup - 1)] : items[num / itemsPerGroup * itemsPerGroup]) : items[items.Count - 1 - (items.Count - 1 - num) % itemsPerGroup]) : items[num % itemsPerGroup]);
			if ((Object)(object)navigationItem.selectable != (Object)null)
			{
				return navigationItem.selectable;
			}
			return navigationItem.subNavigation.GetSelectableItem(direction);
		}
		if ((Object)(object)currentChild != (Object)null)
		{
			return currentChild.GetSelectableItem(direction);
		}
		return current ?? defaultItem ?? ((Component)this).GetComponentInChildren<Selectable>();
	}

	private void Rebuild()
	{
		if ((Object)(object)current != (Object)null && !((Behaviour)current).get_isActiveAndEnabled())
		{
			current = null;
		}
		items.Clear();
		CollectChildrenRecursive(((Component)this).get_transform());
		itemsPerGroup = (fixedItemsPerGroup ? itemsPerGroup : ((items.Count + groupCount - 1) / groupCount));
		if (items.Count != 0)
		{
			for (int i = 0; i < groupCount; i++)
			{
				int num = i * itemsPerGroup;
				int num2 = Mathf.Min((i + 1) * itemsPerGroup, items.Count);
				for (int j = num + 1; j < num2; j++)
				{
					Link(items[j - 1], items[j]);
				}
				if (num2 - 1 > 0 && num2 - 1 < items.Count)
				{
					Link(items[num2 - 1], items[num]);
				}
			}
			for (int k = 1; k < groupCount; k++)
			{
				int num3 = k * itemsPerGroup;
				for (int num4 = (k + 1) * itemsPerGroup - 1; num4 >= num3; num4--)
				{
					int index = Mathf.Min(num4, items.Count - 1);
					Link(items[num4 - itemsPerGroup], items[index], (direction == NavigationDirection.Horizontal) ? NavigationDirection.Vertical : NavigationDirection.Horizontal);
				}
			}
		}
		AutoNavigation componentInParent = ((Component)((Component)this).get_transform().get_parent()).GetComponentInParent<AutoNavigation>();
		if ((Object)(object)componentInParent != (Object)null)
		{
			componentInParent.Rebuild();
		}
	}

	protected void Link(NavigationItem prev, NavigationItem next)
	{
		Link(prev, next, direction);
	}

	protected void Link(NavigationItem prev, NavigationItem next, NavigationDirection direction)
	{
		if (direction == NavigationDirection.Vertical)
		{
			prev.Bind(NavigationItemDirection.Down, next.GetSelectable(NavigationItemDirection.Down));
			next.Bind(NavigationItemDirection.Up, prev.GetSelectable(NavigationItemDirection.Up));
		}
		else
		{
			prev.Bind(NavigationItemDirection.Right, next.GetSelectable(NavigationItemDirection.Right));
			next.Bind(NavigationItemDirection.Left, prev.GetSelectable(NavigationItemDirection.Left));
		}
	}

	private void CollectChildrenRecursive(Transform root)
	{
		for (int i = 0; i < root.get_childCount(); i++)
		{
			Transform child = root.GetChild(i);
			if (!((Component)child).get_gameObject().get_activeInHierarchy())
			{
				continue;
			}
			Selectable val = ((Component)child).GetComponent<Selectable>();
			if (ignoreObjects != null && ignoreObjects.Contains(val))
			{
				val = null;
			}
			if ((Object)(object)val != (Object)null)
			{
				items.Add(new NavigationItem
				{
					selectable = val
				});
				continue;
			}
			AutoNavigation component = ((Component)child).GetComponent<AutoNavigation>();
			if ((Object)(object)component != (Object)null && (Object)(object)((Component)component).GetComponentInChildren<Selectable>() != (Object)null)
			{
				items.Add(new NavigationItem
				{
					subNavigation = component
				});
			}
			else
			{
				CollectChildrenRecursive(child);
			}
		}
	}

	public void OnTransformChildrenChanged()
	{
		invalid = true;
	}

	public void Invalidate()
	{
		invalid = true;
	}

	private void OnEnable()
	{
		invalid = true;
	}

	public void ClearCurrent()
	{
		current = null;
		currentChild = null;
	}

	private void LateUpdate()
	{
		if (selectLastFocused && (Object)(object)EventSystem.get_current().get_currentSelectedGameObject() != (Object)null)
		{
			Selectable component = EventSystem.get_current().get_currentSelectedGameObject().GetComponent<Selectable>();
			if ((Object)(object)component != (Object)null && (Object)(object)current != (Object)(object)component)
			{
				for (int i = 0; i < items.Count; i++)
				{
					if ((Object)(object)items[i].selectable == (Object)(object)component)
					{
						current = component;
						currentChild = null;
						invalid = true;
						AutoNavigation componentInParent = ((Component)((Component)this).get_transform().get_parent()).GetComponentInParent<AutoNavigation>();
						if ((Object)(object)componentInParent != (Object)null)
						{
							componentInParent.ChildSelected(this);
						}
						break;
					}
				}
			}
		}
		if (invalid)
		{
			Rebuild();
			invalid = false;
		}
	}

	private void ChildSelected(AutoNavigation child)
	{
		currentChild = child;
		current = null;
		invalid = true;
	}

	public void Bind(NavigationItemDirection direction, Selectable next)
	{
		if (items.Count == 0)
		{
			return;
		}
		if ((this.direction == NavigationDirection.Vertical && direction == NavigationItemDirection.Left) || (this.direction == NavigationDirection.Horizontal && direction == NavigationItemDirection.Up))
		{
			for (int i = 0; i < itemsPerGroup; i++)
			{
				if (i < items.Count)
				{
					items[i].Bind(direction, next);
				}
			}
		}
		else if ((this.direction == NavigationDirection.Vertical && direction == NavigationItemDirection.Right) || (this.direction == NavigationDirection.Horizontal && direction == NavigationItemDirection.Down))
		{
			for (int j = items.Count - itemsPerGroup; j < items.Count; j++)
			{
				if (j >= 0)
				{
					items[j].Bind(direction, next);
				}
			}
		}
		else if ((this.direction == NavigationDirection.Vertical && direction == NavigationItemDirection.Up) || (this.direction == NavigationDirection.Horizontal && direction == NavigationItemDirection.Left))
		{
			for (int k = 0; k < groupCount; k++)
			{
				if (k * itemsPerGroup < items.Count)
				{
					items[k * itemsPerGroup].Bind(direction, next);
				}
			}
		}
		else
		{
			for (int l = 0; l < groupCount; l++)
			{
				items[Mathf.Min(items.Count - 1, (l + 1) * itemsPerGroup - 1)].Bind(direction, next);
			}
		}
	}

	public AutoNavigation()
		: this()
	{
	}
}
