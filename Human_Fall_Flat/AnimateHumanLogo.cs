using System.Collections;
using UnityEngine;

public class AnimateHumanLogo : MonoBehaviour
{
	public float animationTime = 0.1f;

	public IEnumerator DropAnimation()
	{
		float time = 0f;
		while (time < animationTime)
		{
			((Component)this).get_transform().set_localPosition(((Component)this).get_transform().get_localPosition().SetZ(Mathf.Lerp(-1500f, 0f, time / animationTime)));
			time += Time.get_deltaTime();
			yield return null;
		}
		((Component)this).get_transform().set_localPosition(((Component)this).get_transform().get_localPosition().SetZ(Mathf.Lerp(-1500f, 0f, time / animationTime)));
	}

	public AnimateHumanLogo()
		: this()
	{
	}
}
