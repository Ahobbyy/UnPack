using I2.Loc;
using Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSelectLobbyMenuItem : ListViewItem
{
	public Text label;

	public Text labelPlayers;

	private const string kClosedTextID = "MULTIPLAYER/JOIN.Closed";

	private static string closedText;

	public NetTransport.ILobbyEntry boundData;

	private void Start()
	{
		if (closedText == null)
		{
			Localise();
			LocalizationManager.OnLocalisation += Localise;
		}
	}

	private void Localise()
	{
		closedText = ScriptLocalization.Get("MULTIPLAYER/JOIN.Closed");
	}

	public override void Bind(int index, object data)
	{
		base.Bind(index, data);
		DataRefreshed(data);
	}

	public bool IsActive()
	{
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<MenuButton>()))
		{
			return ((Component)this).GetComponent<MenuButton>().isOn;
		}
		return false;
	}

	public void SetActive(bool active)
	{
		((Component)this).GetComponent<MenuButton>().isOn = active;
	}

	public void DataInvalid()
	{
		labelPlayers.set_text(closedText);
	}

	public void DataRefreshed(object data)
	{
		boundData = (NetTransport.ILobbyEntry)data;
		label.set_text(boundData.name());
		boundData.getDisplayInfo(out var info);
		if (info.ShouldDisplayAllAttr(3221225472u) && info.MaxPlayers != 0)
		{
			labelPlayers.set_text($"{info.NumPlayersForDisplay}/{info.MaxPlayers}");
		}
		else
		{
			labelPlayers.set_text("");
		}
	}
}
