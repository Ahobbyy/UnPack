using System.Collections;
using UnityEngine;

public class MacXmasOptimizer : MonoBehaviour
{
	public Light[] problematicLights;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(DisableLights());
	}

	private IEnumerator DisableLights()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Light[] array = problematicLights;
		for (int i = 0; i < array.Length; i++)
		{
			((Behaviour)array[i]).set_enabled(false);
		}
	}

	public MacXmasOptimizer()
		: this()
	{
	}
}
