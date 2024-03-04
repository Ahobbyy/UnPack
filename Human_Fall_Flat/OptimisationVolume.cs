using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class OptimisationVolume : MonoBehaviour
{
	[Tooltip("Array of game objects to deactivate the renderers of when no local player is in this zone")]
	public GameObject[] deactivateRenderers;

	public Light[] deactivateLights;

	public List<GameObject> exemptObjects;

	private List<GameObject> playersinVolume;

	private List<Renderer> exemptRenderers;

	private bool isActive;

	private BoxCollider boxCollider;

	private bool setupDone;

	private void Awake()
	{
		playersinVolume = new List<GameObject>();
		exemptRenderers = new List<Renderer>();
		boxCollider = ((Component)this).GetComponent<BoxCollider>();
		if ((Object)(object)boxCollider == (Object)null)
		{
			Debug.LogError((object)"OptimisationVolume error, no BoxCollider, disabling");
			((Behaviour)this).set_enabled(false);
		}
		foreach (GameObject exemptObject in exemptObjects)
		{
			Renderer[] componentsInChildren = exemptObject.GetComponentsInChildren<Renderer>();
			exemptRenderers.AddRange(componentsInChildren);
		}
		SetExitObjectState();
		setupDone = true;
	}

	private void FixedUpdate()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!setupDone)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject val in array)
		{
			Bounds bounds = ((Collider)boxCollider).get_bounds();
			if (((Bounds)(ref bounds)).Contains(val.get_transform().get_position()))
			{
				PlayerEnter(val);
			}
			else
			{
				PlayerExit(val);
			}
		}
	}

	private void PlayerEnter(GameObject player)
	{
		if (IsLocalPlayer(player) && !playersinVolume.Contains(player))
		{
			playersinVolume.Add(player);
			if (playersinVolume.Count <= 1)
			{
				SetEnterObjectState();
			}
		}
	}

	private void PlayerExit(GameObject player)
	{
		if (IsLocalPlayer(player) && playersinVolume.Contains(player))
		{
			playersinVolume.Remove(player);
			if (playersinVolume.Count == 0)
			{
				SetExitObjectState();
			}
		}
	}

	private bool IsLocalPlayer(GameObject player)
	{
		if (Object.op_Implicit((Object)(object)((Component)player.get_transform().get_parent()).GetComponent<NetPlayer>()))
		{
			return ((Component)player.get_transform().get_parent()).GetComponent<NetPlayer>().isLocalPlayer;
		}
		return false;
	}

	private void SetEnterObjectState()
	{
		isActive = true;
		SetRenderersVisible(visible: true);
	}

	private void SetExitObjectState()
	{
		isActive = false;
		SetRenderersVisible(visible: false);
	}

	private void SetRenderersVisible(bool visible)
	{
		GameObject[] array = deactivateRenderers;
		foreach (GameObject val in array)
		{
			if (!((Object)(object)val != (Object)null))
			{
				continue;
			}
			Renderer[] componentsInChildren = val.GetComponentsInChildren<Renderer>();
			foreach (Renderer val2 in componentsInChildren)
			{
				if (((Object)((Component)val2).get_gameObject()).get_name() == "Stick")
				{
					_ = 0;
				}
				if (!exemptRenderers.Contains(val2))
				{
					val2.set_enabled(visible);
				}
			}
		}
		Light[] array2 = deactivateLights;
		foreach (Light val3 in array2)
		{
			if ((Object)(object)val3 != (Object)null)
			{
				((Component)val3).get_gameObject().SetActive(visible);
			}
		}
	}

	public OptimisationVolume()
		: this()
	{
	}
}
