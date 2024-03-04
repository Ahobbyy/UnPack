using System;
using I2.Loc;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenuOptions : MonoBehaviour
{
	private enum KickState
	{
		Idle,
		KickingSomeone,
		RecentlyKicked
	}

	public RectTransform MutePlayer;

	public RectTransform ViewProfile;

	public RectTransform KickPlayer;

	public TextMeshProUGUI MutePlayerText;

	public TextMeshProUGUI ViewProfileText;

	public TextMeshProUGUI KickPlayerText2;

	public Text KickPlayerText;

	public TextMeshProUGUI MutePlayerGlyph;

	public TextMeshProUGUI ViewProfileGlyph;

	public TextMeshProUGUI KickPlayerGlyph;

	[SerializeField]
	private const float KickedMsgViewTime = 2f;

	private GameObject m_selectedButton;

	private UpdatePlayerStatus m_UpdateButtonComponenet;

	private float m_fKickDelayDuration = 2f;

	private float m_fKickedTimeStamp;

	private float m_fKickingProcessTimer;

	private float m_fKickedTimer;

	private KickState m_eKickState;

	private bool m_bIsPlayerGettingKicked;

	private bool m_bPlayerRecentlyKicked;

	private bool m_bCanKickThisPlayer;

	private KickState eKickState
	{
		get
		{
			return m_eKickState;
		}
		set
		{
			m_eKickState = value;
			UpdateKickPlayerText();
		}
	}

	public bool ShouldShow { get; private set; }

	private void Awake()
	{
		SetPlayerActionVisibility(flag: false);
	}

	private void Start()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		ChatManager.Instance.ChatListUpdated += EOnChatListUpdated;
		ChatManager.Instance.ChatUserUpdated += EOnChatUserUpdated;
		((Behaviour)KickPlayerText2).set_enabled(false);
		((Component)MutePlayer).get_gameObject().SetActive(false);
		((Component)ViewProfile).get_gameObject().SetActive(false);
		KickPlayerGlyph.fontSize = 48f;
		KickPlayerGlyph.rectTransform.set_anchorMax(new Vector2(1f, 1f));
		KickPlayer.set_anchorMin(new Vector2(0f, 0f));
		KickPlayer.set_anchorMax(new Vector2(1f, 1f));
		KickPlayer.set_pivot(new Vector2(0f, 0.5f));
		((Graphic)KickPlayerText).get_rectTransform().set_anchorMin(new Vector2(0.1f, 0.15f));
	}

	private void EOnChatListUpdated(object sender, EventArgs e)
	{
		UpdatePlayerActionText();
	}

	private void EOnChatUserUpdated(object sender, EventArgs e)
	{
		UpdatePlayerActionText();
	}

	private void OnEnable()
	{
		UpdatePlayerActionText();
	}

	private bool KickButtonDown()
	{
		return false;
	}

	private bool KickButtonUp()
	{
		return false;
	}

	public void OnUpdate()
	{
		if (!Object.op_Implicit((Object)(object)m_UpdateButtonComponenet))
		{
			return;
		}
		if (KickButtonDown() && m_bCanKickThisPlayer)
		{
			eKickState = KickState.KickingSomeone;
			m_fKickedTimer = 0f;
		}
		if (KickButtonUp() && eKickState != KickState.RecentlyKicked)
		{
			eKickState = KickState.Idle;
			m_fKickingProcessTimer = 0f;
		}
		if (eKickState == KickState.KickingSomeone && m_bCanKickThisPlayer)
		{
			m_fKickingProcessTimer += Time.get_deltaTime();
			if (m_fKickingProcessTimer > m_fKickDelayDuration)
			{
				m_fKickingProcessTimer = 0f;
				m_UpdateButtonComponenet.KickUser();
				eKickState = KickState.RecentlyKicked;
			}
		}
		if (eKickState == KickState.RecentlyKicked)
		{
			ChatManager.Instance.Refresh();
			m_fKickedTimer += Time.get_deltaTime();
			if (m_fKickedTimer > 2f)
			{
				m_fKickedTimer = 0f;
				eKickState = KickState.Idle;
			}
		}
	}

	public void SetPlayerActionVisibility(bool flag)
	{
		ShouldShow = flag;
	}

	public void OnButtonUpdate(GameObject FocusedButton)
	{
		if ((Object)(object)m_selectedButton != (Object)(object)FocusedButton)
		{
			if (Object.op_Implicit((Object)(object)m_UpdateButtonComponenet))
			{
				m_UpdateButtonComponenet.SetColor(highlighted: false);
			}
			m_fKickingProcessTimer = 0f;
			m_selectedButton = FocusedButton;
			m_UpdateButtonComponenet = (((Object)(object)m_selectedButton != (Object)null) ? m_selectedButton.GetComponent<UpdatePlayerStatus>() : null);
			if (Object.op_Implicit((Object)(object)m_UpdateButtonComponenet))
			{
				m_UpdateButtonComponenet.SetColor(highlighted: true);
				m_bCanKickThisPlayer = App.isServer && m_UpdateButtonComponenet.GetButtonUserIndex != 0;
			}
			SetPlayerActionVisibility((Object)(object)m_UpdateButtonComponenet != (Object)null);
			UpdatePlayerActionText();
		}
	}

	public void UpdatePlayerActionText()
	{
		UpdateMuteText();
		UpdateKickPlayerText();
		UpdateViewProfileText();
	}

	private void UpdateMuteText()
	{
		if (Object.op_Implicit((Object)(object)m_UpdateButtonComponenet) && ChatManager.Instance.GetChatUserList().Count - 1 >= m_UpdateButtonComponenet.GetButtonUserIndex)
		{
			if (!Object.op_Implicit((Object)(object)MutePlayer))
			{
				Debug.LogWarning((object)"MUTE TEXT OBJECT MISSING !!");
			}
			else
			{
				_ = ChatManager.Instance.GetChatUserList()[m_UpdateButtonComponenet.GetButtonUserIndex].IsMuted;
			}
		}
	}

	private void UpdateKickPlayerText()
	{
		if (!Object.op_Implicit((Object)(object)KickPlayer))
		{
			Debug.LogWarning((object)"KICK TEXT OBJECT MISSING!!");
			return;
		}
		string text = "";
		text = "ℇ/Y";
		string text2 = "";
		KickPlayerGlyph.SetText((eKickState == KickState.RecentlyKicked) ? string.Empty : text);
		switch (eKickState)
		{
		case KickState.Idle:
			text2 = LocalizationManager.GetTermTranslation("MULTIPLAYER/VOICECHAT/KICKPLAYER");
			break;
		case KickState.RecentlyKicked:
			text2 = string.Format(LocalizationManager.GetTermTranslation("MULTIPLAYER/VOICECHAT/PLAYERKICKEDMP"), ChatManager.sRecentlyKickedPlayerName);
			break;
		case KickState.KickingSomeone:
			text2 = LocalizationManager.GetTermTranslation("MULTIPLAYER/VOICECHAT/KICKINGPLAYER");
			break;
		}
		bool active = eKickState == KickState.RecentlyKicked || (m_bCanKickThisPlayer && Object.op_Implicit((Object)(object)m_UpdateButtonComponenet));
		((Component)KickPlayer).get_gameObject().SetActive(active);
		KickPlayerText.set_text(text2);
		KickPlayerText2.text = text2;
	}

	private void UpdateViewProfileText()
	{
		if (!Object.op_Implicit((Object)(object)ViewProfile))
		{
			Debug.LogWarning((object)"PROFILE TEXT OBJECT MISSING!!");
		}
	}

	public MultiplayerMenuOptions()
		: this()
	{
	}
}
