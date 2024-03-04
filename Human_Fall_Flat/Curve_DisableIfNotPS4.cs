using UnityEngine;

public class Curve_DisableIfNotPS4 : MonoBehaviour
{
	private void Start()
	{
		((Component)this).get_gameObject().SetActive(false);
	}

	public Curve_DisableIfNotPS4()
		: this()
	{
	}
}
