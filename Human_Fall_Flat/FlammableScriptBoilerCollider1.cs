using UnityEngine;

public class FlammableScriptBoilerCollider1 : MonoBehaviour
{
	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private FlammableSourceBoiler1 boiler;

	private void OnEnable()
	{
		boiler = ((Component)this).GetComponentInParent<FlammableSourceBoiler1>();
	}

	public void OnTriggerEnter(Collider boilerFuel)
	{
		Flammable component = ((Component)boilerFuel).get_gameObject().GetComponent<Flammable>();
		if ((Object)(object)component != (Object)null)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Flammable thing hit boiler collision "));
			}
			boiler.AddFuel(component);
		}
		Flame component2 = ((Component)boilerFuel).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component2 != (Object)null)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Flame thing hit boiler collision "));
			}
			boiler.AddFlame(component2);
		}
	}

	public void OnTriggerExit(Collider fuel)
	{
		Flammable component = ((Component)fuel).get_gameObject().GetComponent<Flammable>();
		if ((Object)(object)component != (Object)null)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Removing fuel "));
			}
			boiler.RemoveFuel(component);
		}
		Flame component2 = ((Component)fuel).get_gameObject().GetComponent<Flame>();
		if ((Object)(object)component2 != (Object)null)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Removing flame "));
			}
			boiler.RemoveFlame(component2);
		}
	}

	public FlammableScriptBoilerCollider1()
		: this()
	{
	}
}
