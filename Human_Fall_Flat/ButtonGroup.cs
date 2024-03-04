using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
	private void OnEnable()
	{
		RebuildLinks();
	}

	public void RebuildLinks(bool makeExplicit = false)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Selectable[] componentsInChildren = ((Component)this).GetComponentsInChildren<Selectable>();
		if (componentsInChildren.Length > 1)
		{
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				Link(componentsInChildren[i - 1], componentsInChildren[i]);
			}
			Link(componentsInChildren[componentsInChildren.Length - 1], componentsInChildren[0]);
		}
		if (makeExplicit)
		{
			Selectable[] array = componentsInChildren;
			foreach (Selectable obj in array)
			{
				Navigation navigation = obj.get_navigation();
				((Navigation)(ref navigation)).set_mode((Mode)4);
				obj.set_navigation(navigation);
			}
		}
	}

	private void Link(Selectable above, Selectable below)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Navigation navigation = above.get_navigation();
		((Navigation)(ref navigation)).set_selectOnDown(below);
		above.set_navigation(navigation);
		navigation = below.get_navigation();
		((Navigation)(ref navigation)).set_selectOnUp(above);
		below.set_navigation(navigation);
	}

	public ButtonGroup()
		: this()
	{
	}
}
