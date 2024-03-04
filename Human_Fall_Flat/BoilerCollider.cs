using UnityEngine;

public class BoilerCollider : MonoBehaviour
{
	private Boiler boiler;

	private void OnEnable()
	{
		boiler = ((Component)this).GetComponentInParent<Boiler>();
	}

	public void OnTriggerEnter(Collider other)
	{
		Coal component = ((Component)other).get_gameObject().GetComponent<Coal>();
		if ((Object)(object)component != (Object)null)
		{
			boiler.AddCoal(component);
		}
		Flame component2 = ((Component)other).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component2 != (Object)null)
		{
			boiler.AddFlame(component2);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		Coal component = ((Component)other).get_gameObject().GetComponent<Coal>();
		if ((Object)(object)component != (Object)null)
		{
			boiler.RemoveCoal(component);
		}
		Flame component2 = ((Component)other).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component2 != (Object)null)
		{
			boiler.RemoveFlame(component2);
		}
	}

	public BoilerCollider()
		: this()
	{
	}
}
