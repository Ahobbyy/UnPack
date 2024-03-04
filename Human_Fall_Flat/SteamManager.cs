using System;
using System.Text;
using I2.Loc;
using Steamworks;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	private static SteamManager s_instance;

	private static bool s_EverInialized;

	private bool m_bInitialized;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	private static SteamManager Instance
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)s_instance == (Object)null)
			{
				GameObject val = new GameObject("SteamManager");
				SteamManager result = val.AddComponent<SteamManager>();
				val.AddComponent<SteamRichPresence>();
				return result;
			}
			return s_instance;
		}
	}

	public static bool Initialized => Instance.m_bInitialized;

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning((object)pchDebugText);
	}

	private void Awake()
	{
		if ((Object)(object)s_instance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).get_gameObject());
			return;
		}
		s_instance = this;
		if (s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad((Object)(object)((Component)this).get_gameObject());
		if (!Packsize.Test())
		{
			Debug.LogError((object)"[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", (Object)(object)this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError((object)"[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", (Object)(object)this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)477160u))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError((object)("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex), (Object)(object)this);
			Application.Quit();
			return;
		}
		m_bInitialized = SteamAPI.Init();
		if (!m_bInitialized)
		{
			Debug.LogError((object)"[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", (Object)(object)this);
		}
		else
		{
			s_EverInialized = true;
		}
	}

	private void OnEnable()
	{
		if ((Object)(object)s_instance == (Object)null)
		{
			s_instance = this;
		}
		if (!m_bInitialized)
		{
			return;
		}
		string @string = PlayerPrefs.GetString("Language", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			LocalizationManager.CurrentLanguage = @string;
			if (LocalizationManager.HasLanguage(@string))
			{
				return;
			}
		}
		if (m_SteamAPIWarningMessageHook == null)
		{
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
		string text = SteamApps.GetCurrentGameLanguage();
		if (text.Equals("schinese"))
		{
			text = "Chinese Simplified";
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "English";
		}
		LocalizationManager.CurrentLanguage = text;
	}

	private void OnDestroy()
	{
		if (!((Object)(object)s_instance != (Object)(object)this))
		{
			s_instance = null;
			if (m_bInitialized)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	private void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}

	public SteamManager()
		: this()
	{
	}
}
