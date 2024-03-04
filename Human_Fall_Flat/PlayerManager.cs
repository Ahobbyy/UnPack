using Multiplayer;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public enum PlayerStatus
	{
		CanJoin,
		Joining,
		Joined,
		CanLeave,
		Leaving,
		Left,
		PostLeft,
		Online
	}

	public static PlayerManager instance;

	public float maxSplitScreenDelay = 2f;

	public float buttonPressWaitTime = 1f;

	private float currentSplitScreenDelay;

	private float addCoopBlock;

	private const float kSecondPlayerRequestedTimeDelay = 3f;

	private float secondPlayerRequestedTime;

	private bool _secondPlayerRequested;

	public PlayerStatus SecondPlayerStatus { get; set; }

	private bool secondPlayerRequested
	{
		get
		{
			return _secondPlayerRequested;
		}
		set
		{
			_secondPlayerRequested = value;
			secondPlayerRequestedTime = Time.get_unscaledTime() + 3f;
		}
	}

	private void OnEnable()
	{
		instance = this;
	}

	private bool GameStateValidForCoop(bool checkLoadLevel = false)
	{
		if (App.state != AppSate.Menu && App.state != AppSate.PlayLevel && App.state != AppSate.Customize && App.state != AppSate.ClientLobby && App.state != AppSate.ServerLobby && App.state != AppSate.ClientPlayLevel && App.state != AppSate.ServerPlayLevel && App.state != AppSate.ClientJoin && App.state != AppSate.ClientLoadLobby && App.state != AppSate.ServerHost && App.state != AppSate.ServerLoadLobby && App.state != AppSate.ClientWaitServerLoad)
		{
			if (!checkLoadLevel)
			{
				return false;
			}
			if (App.state != AppSate.LoadLevel && App.state != AppSate.ClientLoadLevel && App.state != AppSate.ServerLoadLevel)
			{
				return false;
			}
		}
		return true;
	}

	private void UpdateStandaloneCoop()
	{
	}

	private void Update()
	{
		if (addCoopBlock > 0f)
		{
			addCoopBlock -= Time.get_deltaTime();
		}
		else if (!((Object)(object)Game.instance == (Object)null) && !((Object)(object)NetGame.instance == (Object)null) && NetGame.instance.local != null)
		{
			UpdateStandaloneCoop();
		}
	}

	public static void SetSingle()
	{
	}

	private void RemovePlayer(int index)
	{
	}

	public void ApplyControls()
	{
	}

	public void OnLocalPlayerAdded(NetPlayer player)
	{
	}

	public void OnLocalPlayerRemoved(NetPlayer player)
	{
	}

	private void IncrementSecondPlayerStatus()
	{
		if (SecondPlayerStatus != PlayerStatus.Left)
		{
			SecondPlayerStatus++;
		}
		else
		{
			SecondPlayerStatus = PlayerStatus.CanJoin;
		}
	}

	public PlayerManager()
		: this()
	{
	}
}
