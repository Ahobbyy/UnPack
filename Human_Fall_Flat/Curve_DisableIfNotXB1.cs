using UnityEngine;

public class Curve_DisableIfNotXB1 : MonoBehaviour
{
	private void Start()
	{
		((Component)this).get_gameObject().SetActive(false);
	}

	public Curve_DisableIfNotXB1()
		: this()
	{
	}
}
