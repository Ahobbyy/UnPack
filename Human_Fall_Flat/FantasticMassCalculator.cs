using UnityEngine;

public class FantasticMassCalculator : MonoBehaviour
{
	private enum Material
	{
		Custom = 0,
		SoftWood_25 = 25,
		HardWood_110 = 110,
		Plastic_90 = 90,
		Tin_200 = 200,
		Iron_785 = 785,
		Ice_930 = 930,
		Water_997 = 997
	}

	[SerializeField]
	private Material material;

	[SerializeField]
	private int customDensity;

	private void Start()
	{
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Rigidbody>()))
		{
			if (material == Material.Custom)
			{
				((Component)this).GetComponent<Rigidbody>().SetDensity((float)customDensity * 0.01f);
			}
			else
			{
				((Component)this).GetComponent<Rigidbody>().SetDensity((float)material * 0.01f);
			}
			((Component)this).GetComponent<Rigidbody>().set_mass(((Component)this).GetComponent<Rigidbody>().get_mass());
		}
		else
		{
			Debug.Log((object)("Can't set material density on " + ((Object)((Component)this).get_gameObject()).get_name()), (Object)(object)((Component)this).get_gameObject());
		}
	}

	private void Update()
	{
	}

	public FantasticMassCalculator()
		: this()
	{
	}
}
