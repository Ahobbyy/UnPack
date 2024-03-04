using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerStatus : MonoBehaviour
{
	public Sprite IdleIcon;

	public Sprite TalkingIcon;

	public Sprite MutedIcon;

	private int m_iUserIndex = -1;

	public Text PlayerSlotText;

	public Image ChatStatusImage;

	public string test = "DUMMY TEXT";

	[NonSerialized]
	public bool m_DarkBackground;

	public int GetButtonUserIndex => m_iUserIndex;

	public void SetColor(bool highlighted)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Color color = (highlighted ? Color.get_white() : (m_DarkBackground ? Color.get_grey() : Color.get_black()));
		((Graphic)PlayerSlotText).set_color(color);
		((Graphic)ChatStatusImage).set_color(color);
	}

	private void Start()
	{
		if ((Object)(object)ChatStatusImage != (Object)null)
		{
			((Behaviour)ChatStatusImage).set_enabled(false);
		}
		SetColor(highlighted: false);
	}

	private void UpdateButtonText(string NewText)
	{
		if (Object.op_Implicit((Object)(object)PlayerSlotText))
		{
			PlayerSlotText.set_text(NewText);
		}
	}

	private void UpdateIcon(bool IsTalking, bool IsMuted)
	{
		if (Object.op_Implicit((Object)(object)ChatStatusImage))
		{
			if (IsMuted)
			{
				ChatStatusImage.set_sprite(MutedIcon);
			}
			else if (IsTalking)
			{
				ChatStatusImage.set_sprite(TalkingIcon);
			}
			else
			{
				ChatStatusImage.set_sprite(IdleIcon);
			}
		}
		else
		{
			Debug.LogWarning((object)"TextMesh Not found !!");
		}
	}

	public void UpdateStatus(int UserIndex, string GamerTag, bool IsTalking, bool IsMuted)
	{
		m_iUserIndex = UserIndex;
		UpdateButtonText(GamerTag);
		UpdateIcon(IsTalking, IsMuted);
	}

	public void KickUser()
	{
		if ((m_iUserIndex >= 0) & (m_iUserIndex < ChatManager.Instance.GetChatUserCount()))
		{
			ChatManager.Instance.KickPlayer(m_iUserIndex);
		}
	}

	public void ViewPlayerProfile()
	{
		Debug.Log((object)("View profile : " + m_iUserIndex));
		if ((m_iUserIndex >= 0) & (m_iUserIndex < ChatManager.Instance.GetChatUserCount()))
		{
			ChatManager.Instance.ViewPlayerProfile(m_iUserIndex);
		}
	}

	public void TogglePlayerMute()
	{
		if ((m_iUserIndex >= 0) & (m_iUserIndex < ChatManager.Instance.GetChatUserCount()))
		{
			ChatManager.Instance.ToggleChatUserMuteState(m_iUserIndex, !ChatManager.Instance.GetChatUserList()[m_iUserIndex].IsMuted);
		}
	}

	public UpdatePlayerStatus()
		: this()
	{
	}
}
