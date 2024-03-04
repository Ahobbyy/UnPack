using I2.Loc;
using TMPro;
using UnityEngine;

public class SplitScreenNotification : MonoBehaviour
{
	public TextMeshProUGUI ButtonGlyphText;

	public TextMeshProUGUI SecondPlayerStatusText;

	public TextMeshProUGUI SplitRefreshGlyph;

	public TextMeshProUGUI SplitRefreshText;

	public TextMeshProUGUI DLCAvailableGlyph;

	public TextMeshProUGUI DLCAvailableText;

	private PlayerManager.PlayerStatus currentSecondPlayerStatus;

	private string joinButtonGlyph = "";

	private string leaveButtonGlyph = "";

	private const string kYButton = "Y/<size=48><voffset=-.08em>ℇ";

	private void Awake()
	{
		LocalizationManager.OnLocalisation += OnLocalisation;
	}

	private void OnLocalisation()
	{
		UpdateSecondPlayerStatus();
	}

	public void SplitRefreshEnable(bool enable)
	{
	}

	public void DLCAvailableEnable(bool enable)
	{
		((Component)((Component)DLCAvailableGlyph.transform.get_parent()).get_transform()).get_gameObject().SetActive(false);
	}

	private void SetupSplitRefresh()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		SplitRefreshGlyph.text = "Y/<size=48><voffset=-.08em>ℇ";
		SplitRefreshGlyph.fontSize = 40f;
		SplitRefreshText.rectTransform.set_anchorMin(new Vector2(0.35f, 0.1f));
		SplitRefreshGlyph.rectTransform.set_anchorMax(new Vector2(0.4f, 1f));
	}

	private void SetupDLCAvailable()
	{
		DLCAvailableGlyph.text = "Y/<size=48><voffset=-.08em>ℇ";
	}

	private void Start()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		joinButtonGlyph = "℆";
		leaveButtonGlyph = "℆";
		ButtonGlyphText.fontSize = 48f;
		ButtonGlyphText.rectTransform.set_anchorMax(new Vector2(0.15f, 0.75f));
		SecondPlayerStatusText.rectTransform.set_anchorMin(new Vector2(0.15f, 0f));
		SecondPlayerStatusText.rectTransform.set_anchorMax(new Vector2(1f, 1f));
		SetupSplitRefresh();
		SetupDLCAvailable();
		UpdateSecondPlayerStatus();
	}

	private void Update()
	{
		if (currentSecondPlayerStatus != PlayerManager.instance.SecondPlayerStatus)
		{
			currentSecondPlayerStatus = PlayerManager.instance.SecondPlayerStatus;
			UpdateSecondPlayerStatus();
		}
	}

	public void UpdateSecondPlayerStatus()
	{
		ButtonGlyphText.text = GetGlyph();
		switch (currentSecondPlayerStatus)
		{
		case PlayerManager.PlayerStatus.CanJoin:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2JOIN");
			break;
		case PlayerManager.PlayerStatus.Joining:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2JOINING");
			break;
		case PlayerManager.PlayerStatus.Joined:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2JOINED");
			break;
		case PlayerManager.PlayerStatus.CanLeave:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2LEAVE");
			break;
		case PlayerManager.PlayerStatus.Leaving:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2LEAVING");
			break;
		case PlayerManager.PlayerStatus.Left:
		case PlayerManager.PlayerStatus.PostLeft:
			SecondPlayerStatusText.text = " " + LocalizationManager.GetTermTranslation("MENU.MAIN/PLAYER2LEFT");
			break;
		case PlayerManager.PlayerStatus.Online:
			SecondPlayerStatusText.text = string.Empty;
			break;
		}
	}

	private string GetGlyph()
	{
		string text = string.Empty;
		switch (currentSecondPlayerStatus)
		{
		case PlayerManager.PlayerStatus.CanJoin:
		case PlayerManager.PlayerStatus.Joining:
			text += joinButtonGlyph;
			break;
		case PlayerManager.PlayerStatus.CanLeave:
		case PlayerManager.PlayerStatus.Leaving:
			text += leaveButtonGlyph;
			break;
		}
		return text;
	}

	public SplitScreenNotification()
		: this()
	{
	}
}
