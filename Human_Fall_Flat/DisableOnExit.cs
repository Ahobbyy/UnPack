using HumanAPI;
using Multiplayer;
using UnityEngine;

public class DisableOnExit : MonoBehaviour
{
	public static void ExitingLevel(Level level)
	{
		DisableOnExit[] componentsInChildren = ((Component)level).GetComponentsInChildren<DisableOnExit>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).get_gameObject().SetActive(false);
		}
		NetScope.ExitingLevel(level);
	}

	public DisableOnExit()
		: this()
	{
	}
}
