using HumanAPI;
using UnityEngine;

public class FlammableScriptHeatSource1 : Node
{
	[Tooltip("Flag whether this heat source is always hot")]
	public bool isAlwaysHot;

	[Tooltip("local store for the state if the heat source")]
	public bool ignited;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	public NodeOutput isHot = new NodeOutput
	{
		initialValue = 1f
	};

	public new void OnEnable()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " OnEnable "));
		}
		if (isAlwaysHot)
		{
			isHot.SetValue(1f);
			ignited = true;
		}
	}

	public void Ignite()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Ignite "));
		}
		isHot.SetValue(1f);
		ignited = true;
	}

	public void Extinguish()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Extinguish "));
		}
		if (isAlwaysHot)
		{
			isHot.SetValue(1f);
			ignited = true;
		}
		else
		{
			isHot.SetValue(0f);
			ignited = false;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Entered Range " + ((Object)other).get_name()));
		}
		FlammableScriptHeatSource1 component = ((Component)other).get_gameObject().GetComponent<FlammableScriptHeatSource1>();
		if (Object.op_Implicit((Object)(object)component))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Pass on the Heat "));
			}
			component.Ignite();
		}
		FlammableScriptHeatColourChange1 component2 = ((Component)other).get_gameObject().GetComponent<FlammableScriptHeatColourChange1>();
		if ((Object)(object)component2 != (Object)null && ignited)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing here is supposed to change colour"));
			}
			component2.Ignite();
		}
		Flammable component3 = ((Component)other).get_gameObject().GetComponent<Flammable>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing is flammable , its supposed to get hot "));
			}
			component3.heat = 1f;
		}
		Flame component4 = ((Component)other).get_gameObject().GetComponent<Flame>();
		if (Object.op_Implicit((Object)(object)component4))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing is flammable , its supposed to get hot "));
			}
			component4.Ignite();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Left Range " + ((Object)other).get_name()));
		}
		FlammableScriptHeatColourChange1 component = ((Component)other).get_gameObject().GetComponent<FlammableScriptHeatColourChange1>();
		if ((Object)(object)component != (Object)null)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing here is a piece of coal "));
			}
			component.Extinguish();
		}
		Flammable component2 = ((Component)other).get_gameObject().GetComponent<Flammable>();
		if (Object.op_Implicit((Object)(object)component2))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing is flammable , its supposed to get hot "));
			}
			component2.heat = 0f;
		}
		Flame component3 = ((Component)other).get_gameObject().GetComponent<Flame>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " The other thing is flame , its supposed to get hot "));
			}
			component3.Extinguish();
		}
	}
}
