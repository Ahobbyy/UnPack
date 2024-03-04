using UnityEngine;

public class FlammableCollider : MonoBehaviour
{
	private Flammable flammable;

	private void OnEnable()
	{
		flammable = ((Component)this).GetComponentInParent<Flammable>();
	}

	public void OnTriggerEnter(Collider other)
	{
		Flame component = ((Component)other).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component != (Object)null)
		{
			flammable.AddFlame(component);
		}
		Flammable component2 = ((Component)other).get_gameObject().GetComponent<Flammable>();
		if ((Object)(object)component2 != (Object)null)
		{
			component2.AddFlammable(flammable);
		}
		FlammableExtinguisher component3 = ((Component)other).get_gameObject().GetComponent<FlammableExtinguisher>();
		if ((Object)(object)component3 != (Object)null)
		{
			flammable.AddExtinguisher(component3);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		Flame component = ((Component)other).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component != (Object)null)
		{
			flammable.RemoveFlame(component);
		}
		Flammable component2 = ((Component)other).get_gameObject().GetComponent<Flammable>();
		if ((Object)(object)component2 != (Object)null)
		{
			component2.RemoveFlammable(flammable);
		}
		FlammableExtinguisher component3 = ((Component)other).get_gameObject().GetComponent<FlammableExtinguisher>();
		if ((Object)(object)component3 != (Object)null)
		{
			flammable.RemoveExtinguisher(component3);
		}
	}

	public FlammableCollider()
		: this()
	{
	}
}
