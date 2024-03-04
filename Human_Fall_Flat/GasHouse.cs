using Multiplayer;
using UnityEngine;

public class GasHouse : MonoBehaviour
{
	private Rigidbody[] gasHousePieces;

	private void Awake()
	{
		gasHousePieces = ((Component)this).GetComponentsInChildren<Rigidbody>();
	}

	public void PrepareForDestruction()
	{
		if (gasHousePieces != null && !NetGame.isClient)
		{
			Rigidbody[] array = gasHousePieces;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].set_isKinematic(false);
			}
		}
	}

	public GasHouse()
		: this()
	{
	}
}
