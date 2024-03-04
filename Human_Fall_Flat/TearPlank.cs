using UnityEngine;

public class TearPlank : MonoBehaviour
{
	public Flame TearPlankEnd1;

	public Flame TearPlankEnd2;

	private GameObject particles1;

	private GameObject particles2;

	public void RespawnExtra()
	{
		TearPlankEnd1.Extinguish();
		TearPlankEnd2.Extinguish();
		DisableParticles();
		EnablePlanks();
	}

	private void EnablePlanks()
	{
		((Component)TearPlankEnd1).get_gameObject().SetActive(true);
		((Component)TearPlankEnd2).get_gameObject().SetActive(true);
	}

	private void DisableParticles()
	{
		if ((Object)(object)particles1 != (Object)null)
		{
			particles1.SetActive(false);
		}
		if ((Object)(object)particles2 != (Object)null)
		{
			particles2.SetActive(false);
		}
	}

	public void OnColdChange()
	{
		RespawnExtra();
	}

	public void OnHotChange(Flame endPlank)
	{
		if (((object)endPlank).Equals((object)TearPlankEnd1))
		{
			particles1.SetActive(true);
			((Component)TearPlankEnd2).get_gameObject().SetActive(false);
		}
		else
		{
			particles2.SetActive(true);
			((Component)TearPlankEnd1).get_gameObject().SetActive(false);
		}
	}

	private void Start()
	{
		if ((Object)(object)TearPlankEnd1 != (Object)null)
		{
			particles1 = ((Component)((Component)((Component)TearPlankEnd1).get_gameObject().get_transform().GetChild(0)).get_transform()).get_gameObject();
			particles2 = ((Component)((Component)((Component)TearPlankEnd2).get_gameObject().get_transform().GetChild(0)).get_transform()).get_gameObject();
		}
	}

	public TearPlank()
		: this()
	{
	}
}
