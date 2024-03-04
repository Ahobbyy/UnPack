using I2.Loc;
using TMPro;
using UnityEngine;

public class LobbySelectMenuOptions : MonoBehaviour
{
	public GameObject JoinGame;

	public TextMeshProUGUI RefreshGlyph;

	public TextMeshProUGUI JoinGameGlyph;

	public TextMeshProUGUI FriendGlyph;

	public TextMeshProUGUI RefreshText;

	public TextMeshProUGUI JoinGameText;

	public TextMeshProUGUI FriendText;

	private GameObject FriendGame;

	private void Start()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		RefreshGlyph.text = "Y/<size=48><voffset=-0.1em>ℇ";
		RefreshGlyph.fontSize = 40f;
		RefreshText.rectTransform.set_anchorMin(new Vector2(0.35f, 0.1f));
		RefreshGlyph.rectTransform.set_anchorMax(new Vector2(0.4f, 1f));
		LocalizationManager.OnLocalisation += Localise;
		Localise();
		JoinGameGlyph.fontSize = 48f;
		JoinGameText.rectTransform.set_anchorMin(new Vector2(0.5f, 0.1f));
		JoinGameGlyph.rectTransform.set_anchorMax(new Vector2(0.5f, 0.7f));
		FriendGlyph.text = "F/<size=48><voffset=-0.1em>ℚ";
		FriendGlyph.fontSize = 40f;
		FriendText.rectTransform.set_anchorMin(new Vector2(0.35f, 0.1f));
		FriendGlyph.rectTransform.set_anchorMax(new Vector2(0.4f, 0.9f));
		GetFriendGame();
	}

	private void Localise()
	{
		JoinGameGlyph.text = "<size=34>" + ScriptLocalization.Get("MULTIPLAYER/ReturnKey") + "<size=40>/<size=48><voffset=-0.15em>\u20fd</voffset>";
	}

	private void GetFriendGame()
	{
		FriendGame = ((Component)FriendText.transform.get_parent()).get_gameObject();
	}

	public void ShowRefreshText(bool show)
	{
		if (Object.op_Implicit((Object)(object)RefreshGlyph))
		{
			((Behaviour)RefreshGlyph).set_enabled(show);
		}
		if (Object.op_Implicit((Object)(object)RefreshText))
		{
			((Behaviour)RefreshText).set_enabled(show);
		}
	}

	public bool JoinTextShown()
	{
		return JoinGame.get_activeInHierarchy();
	}

	public void ShowJoinText(bool value)
	{
		JoinGame.SetActive(value);
	}

	public bool FriendTextShown()
	{
		return FriendGame.get_activeInHierarchy();
	}

	public void ShowFriendText(bool value)
	{
		FriendGame.SetActive(value);
	}

	public void SetFriendText(bool mode)
	{
		string term = ((!mode) ? "MENU/PLAYERS/FRIENDS" : "MULTIPLAYER/LOBBY.ALL");
		FriendText.text = ScriptLocalization.Get(term);
	}

	public LobbySelectMenuOptions()
		: this()
	{
	}
}
