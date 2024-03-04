using System;
using System.Collections;
using UnityEngine;

public class GiftService : MonoBehaviour, IDependency
{
	public TextAsset jsonStatus;

	public TextAsset jsonDeliver;

	public static XmasStatus status;

	public static GiftService instance;

	private int activeCalls;

	public static event Action<XmasStatus> statusUpdated;

	private void Awake()
	{
		instance = this;
	}

	private void SetStatus(string json)
	{
		try
		{
			XmasStatus xmasStatus = JsonUtility.FromJson<XmasStatus>(json);
			if (xmasStatus != null)
			{
				status = xmasStatus;
				if (GiftService.statusUpdated != null)
				{
					GiftService.statusUpdated(status);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	public void RefreshStatus()
	{
		if (activeCalls <= 0)
		{
			((MonoBehaviour)this).StartCoroutine(GetStatusCoroutine());
		}
	}

	private IEnumerator GetStatusCoroutine()
	{
		activeCalls++;
		WWW www = new WWW("https://hff.terahard.org/api/get-status.php");
		yield return www;
		if (!string.IsNullOrEmpty(www.get_error()))
		{
			Debug.Log((object)www.get_error());
		}
		else
		{
			SetStatus(www.get_text());
		}
		www.Dispose();
		activeCalls--;
	}

	public void DeliverGift(ulong user, uint gift)
	{
		((MonoBehaviour)this).StartCoroutine(DeliverGiftCoroutine(user, gift));
	}

	private IEnumerator DeliverGiftCoroutine(ulong user, uint gift)
	{
		activeCalls++;
		WWW www = new WWW($"https://hff.terahard.org/api/deliver-gift.php?userId={user}&giftId={gift}");
		yield return www;
		if (!string.IsNullOrEmpty(www.get_error()))
		{
			Debug.Log((object)www.get_error());
		}
		else
		{
			SetStatus(www.get_text());
		}
		www.Dispose();
		activeCalls--;
	}

	public void Initialize()
	{
		RefreshStatus();
	}

	public GiftService()
		: this()
	{
	}
}
