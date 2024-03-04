using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopulatePlayer : MonoBehaviour
{
	[SerializeField]
	private int MaxPlayerCount = 8;

	private UpdatePlayerStatus[] m_arButtonList;

	private AutoNavigation m_NavigationObject;

	private bool m_bIsGridList;

	public GameObject m_PlayerSlotButton;

	public bool m_DarkBackground;

	private void Awake()
	{
		m_arButtonList = new UpdatePlayerStatus[MaxPlayerCount];
		for (int i = 0; i < MaxPlayerCount; i++)
		{
			AddButton();
		}
		m_arButtonList = ((Component)((Component)this).get_gameObject().get_transform()).GetComponentsInChildren<UpdatePlayerStatus>(true);
		UpdatePlayerStatus[] arButtonList = m_arButtonList;
		for (int j = 0; j < arButtonList.Length; j++)
		{
			arButtonList[j].m_DarkBackground = m_DarkBackground;
		}
		m_NavigationObject = ((Component)this).GetComponent<AutoNavigation>();
		m_bIsGridList = Object.op_Implicit((Object)(object)((Component)this).GetComponent<GridCellScaler>());
		m_NavigationObject.fixedItemsPerGroup = m_bIsGridList;
		m_NavigationObject.itemsPerGroup = (m_bIsGridList ? 4 : 0);
	}

	private void Start()
	{
		if ((Object)(object)ChatManager.Instance == (Object)null)
		{
			Debug.LogWarning((object)"Chat Manager Null");
			return;
		}
		ChatManager.Instance.ChatListUpdated += EOnChatListUpdated;
		ChatManager.Instance.ChatUserUpdated += EOnChatUserUpdated;
		UpdateAllPlayerSlots();
	}

	private void UpdateNavigation()
	{
		if (m_bIsGridList && Object.op_Implicit((Object)(object)m_NavigationObject))
		{
			m_NavigationObject.groupCount = ((ChatManager.Instance.GetChatUserCount() <= 4) ? 1 : 2);
		}
		m_NavigationObject.Invalidate();
		if ((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() != (Object)null)
		{
			UpdatePlayerStatus component = EventSystem.get_current().get_currentSelectedGameObject().GetComponent<UpdatePlayerStatus>();
			Selectable component2 = EventSystem.get_current().get_currentSelectedGameObject().GetComponent<Selectable>();
			if (Object.op_Implicit((Object)(object)component) && Object.op_Implicit((Object)(object)component2) && !((Behaviour)component2).get_isActiveAndEnabled() && component.GetButtonUserIndex >= 1 && component.GetButtonUserIndex <= m_arButtonList.Length)
			{
				EventSystem.get_current().SetSelectedGameObject(((Component)m_arButtonList[component.GetButtonUserIndex - 1]).get_gameObject());
			}
		}
	}

	private void UpdateAllPlayerSlots()
	{
		for (int i = 0; i < MaxPlayerCount; i++)
		{
			if (i < ChatManager.Instance.GetChatUserCount())
			{
				((Component)m_arButtonList[i]).get_gameObject().SetActive(true);
				UpdatePlayer(i);
			}
			else
			{
				((Component)m_arButtonList[i]).get_gameObject().SetActive(false);
			}
		}
		UpdateNavigation();
	}

	private void UpdatePlayer(int PlayerIndex)
	{
		if (PlayerIndex < m_arButtonList.Length && (Object)(object)m_arButtonList[PlayerIndex] != (Object)null)
		{
			string gamerTag = ChatManager.Instance.GetChatUserList()[PlayerIndex].GamerTag;
			bool isTalking = ChatManager.Instance.GetChatUserList()[PlayerIndex].IsTalking;
			bool isMuted = ChatManager.Instance.GetChatUserList()[PlayerIndex].IsMuted;
			m_arButtonList[PlayerIndex].UpdateStatus(PlayerIndex, string.IsNullOrEmpty(gamerTag) ? LocalizationManager.GetTermTranslation("MULTIPLAYER/VOICECHAT/JOINING") : gamerTag, isTalking, isMuted);
		}
		else
		{
			Debug.LogWarning((object)"PlayerIndex not pointing to a player.");
		}
	}

	private void EOnChatListUpdated(object sender, EventArgs e)
	{
		UpdateAllPlayerSlots();
	}

	private void EOnChatUserUpdated(object sender, EventArgs e)
	{
		ChatUserEventArgs chatUserEventArgs = (ChatUserEventArgs)e;
		if (chatUserEventArgs != null)
		{
			UpdatePlayer(chatUserEventArgs.Index);
		}
		UpdateAllPlayerSlots();
	}

	private void AddButton()
	{
		if (!Object.op_Implicit((Object)(object)m_PlayerSlotButton))
		{
			Debug.LogWarning((object)"No Button prefab specified, Dang it !!");
		}
		else
		{
			Object.Instantiate<GameObject>(m_PlayerSlotButton, ((Component)this).get_transform(), false).SetActive(true);
		}
	}

	public PopulatePlayer()
		: this()
	{
	}
}
