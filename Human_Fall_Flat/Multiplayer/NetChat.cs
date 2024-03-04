using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
	public class NetChat : MonoBehaviour
	{
		public Color[] colors;

		public InputField input;

		public Text text;

		public GameObject receivedUI;

		public GameObject typeUI;

		public static bool visible = false;

		public static bool typing = false;

		public static NetChat instance;

		public static CommandRegistry serverCommands = new CommandRegistry(Print, expand: true);

		public static CommandRegistry clientCommands = new CommandRegistry(Print, expand: true);

		public static bool allowChat = true;

		private float phase;

		private float speed;

		private float dismissIn;

		private static string contents = "";

		private const float kDismissChatWindowTime = 3f;

		public static void RegisterCommand(bool server, bool client, string command, Action onCommand, string help = null)
		{
			if (server)
			{
				serverCommands.RegisterCommand(command, onCommand, help);
			}
			if (client)
			{
				clientCommands.RegisterCommand(command, onCommand, help);
			}
		}

		public static void RegisterCommand(bool server, bool client, string command, Action<string> onCommand, string help = null)
		{
			if (server)
			{
				serverCommands.RegisterCommand(command, onCommand, help);
			}
			if (client)
			{
				clientCommands.RegisterCommand(command, onCommand, help);
			}
		}

		public static void UnRegisterCommand(bool server, bool client, string command, Action onCommand, string help = null)
		{
			if (server)
			{
				serverCommands.UnRegisterCommand(command, onCommand, help);
			}
			if (client)
			{
				clientCommands.UnRegisterCommand(command, onCommand, help);
			}
		}

		public static void UnRegisterCommand(bool server, bool client, string command, Action<string> onCommand, string help = null)
		{
			if (server)
			{
				serverCommands.UnRegisterCommand(command, onCommand, help);
			}
			if (client)
			{
				clientCommands.UnRegisterCommand(command, onCommand, help);
			}
		}

		private void OnEnable()
		{
			instance = this;
			Show(showMessages: false, showType: false);
			serverCommands.helpColor = (clientCommands.helpColor = "<#FFFF7F>");
			serverCommands.RegisterCommand("?", serverCommands.OnHelp);
			serverCommands.RegisterCommand("help", serverCommands.OnHelp);
			clientCommands.RegisterCommand("?", clientCommands.OnHelp);
			clientCommands.RegisterCommand("help", clientCommands.OnHelp);
			RegisterCommand(server: true, client: true, "list", OnList, "#MULTIPLAYER/CHAT.HelpList");
			RegisterCommand(server: true, client: true, "mute", OnMute, "#MULTIPLAYER/CHAT.HelpMute");
			RegisterCommand(server: true, client: false, "kick", OnKick, "#MULTIPLAYER/CHAT.HelpKick");
		}

		public static void PrintHelp()
		{
			if (NetGame.isServer)
			{
				serverCommands.OnHelp("");
			}
			else
			{
				clientCommands.OnHelp("");
			}
		}

		private NetHost GetClient(string txt)
		{
			if (string.IsNullOrEmpty(txt))
			{
				return null;
			}
			string[] array = txt.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 1)
			{
				return null;
			}
			int result = 0;
			if (int.TryParse(array[0], out result))
			{
				if (result == 1)
				{
					return NetGame.instance.server;
				}
				if (result - 2 < NetGame.instance.readyclients.Count && result > 1)
				{
					return NetGame.instance.readyclients[result - 2];
				}
			}
			string arg = ScriptLocalization.Get("XTRA/NetChat_NoPlayer");
			Print($"{arg} {result}");
			OnList();
			return null;
		}

		private void OnList()
		{
			if (!NetGame.isNetStarted)
			{
				Print(ScriptLocalization.Get("XTRA/NetChat_OnlyMP"));
				return;
			}
			int num = 1;
			Print(string.Format("{0} {1} {2}", num++, NetGame.instance.server.name, NetGame.instance.server.mute ? ScriptLocalization.Get("XTRA/NetChat_Muted") : ""));
			foreach (NetHost readyclient in NetGame.instance.readyclients)
			{
				Print(string.Format("{0} {1} {2}", num++, readyclient.name, readyclient.mute ? ScriptLocalization.Get("XTRA/NetChat_Muted") : ""));
			}
		}

		private void OnMute(string txt)
		{
			if (string.IsNullOrEmpty(txt))
			{
				CommandRegistry.ShowCurrentHelp();
				return;
			}
			if (!NetGame.isNetStarted)
			{
				Print(ScriptLocalization.Get("XTRA/NetChat_OnlyMP"));
				return;
			}
			NetHost client = GetClient(txt);
			if (client == null)
			{
				return;
			}
			if (client == NetGame.instance.local)
			{
				Print(ScriptLocalization.Get("XTRA/NetChat_NoMuteMe"));
				return;
			}
			client.mute = !client.mute;
			if (client.mute)
			{
				string text = ScriptLocalization.Get("XTRA/NetChat_Muted");
				Print(text);
				Print($"{text} {txt} {client.name}");
			}
			else
			{
				string text2 = ScriptLocalization.Get("XTRA/NetChat_UnMuted");
				Print(text2);
				Print($"{text2} {txt} {client.name}");
			}
		}

		private void OnKick(string txt)
		{
			if (string.IsNullOrEmpty(txt))
			{
				CommandRegistry.ShowCurrentHelp();
				return;
			}
			if (!NetGame.isServer)
			{
				Print(ScriptLocalization.Get("XTRA/NetChat_OnlyHost"));
				return;
			}
			NetHost client = GetClient(txt);
			if (client != null)
			{
				if (client == NetGame.instance.local)
				{
					Print(ScriptLocalization.Get("XTRA/NetChat_NoKickMe"));
				}
				else
				{
					NetGame.instance.Kick(client);
				}
			}
		}

		private void Show(bool showMessages, bool showType)
		{
			if (showMessages || showType)
			{
				speed = 10f;
			}
			else
			{
				speed = -2f;
			}
			if (showType)
			{
				showMessages = true;
			}
			visible = showMessages;
			typing = showType;
			typeUI.SetActive(typing);
			if (typing)
			{
				input.ActivateInputField();
			}
			if (typing)
			{
				MenuSystem.keyboardState = KeyboardState.NetChat;
				if ((Object)(object)MenuSystem.instance != (Object)null)
				{
					MenuSystem.instance.FocusOnMouseOver(enable: false);
				}
			}
			else
			{
				if ((Object)(object)MenuSystem.instance != (Object)null)
				{
					MenuSystem.instance.FocusOnMouseOver(enable: true);
				}
				((MonoBehaviour)this).StartCoroutine(UnblockMenu());
			}
		}

		private IEnumerator UnblockMenu()
		{
			yield return (object)new WaitForSeconds(0.1f);
			MenuSystem.keyboardState = KeyboardState.None;
		}

		private void Update()
		{
			phase = Mathf.Clamp01(phase + speed * Time.get_deltaTime());
			if (((Component)this).GetComponent<CanvasGroup>().get_alpha() != phase)
			{
				((Component)this).GetComponent<CanvasGroup>().set_alpha(phase);
				if (phase > 0f != receivedUI.get_activeSelf())
				{
					receivedUI.SetActive(phase > 0f);
				}
			}
			allowChat = (Object)(object)MenuSystem.instance.activeMenu == (Object)null || MenuSystem.instance.activeMenu is MultiplayerLobbyMenu;
			if (!NetGame.isNetStarted && !string.IsNullOrEmpty(contents))
			{
				contents = "";
				CropContents();
			}
			if (!NetGame.isNetStarted || !allowChat)
			{
				if (visible)
				{
					Show(showMessages: false, showType: false);
				}
				return;
			}
			if (!typing && visible && dismissIn > 0f)
			{
				dismissIn -= Time.get_deltaTime();
				if (dismissIn <= 0f)
				{
					Show(showMessages: false, showType: false);
				}
			}
			if (MenuSystem.keyboardState != 0 && MenuSystem.keyboardState != KeyboardState.NetChat)
			{
				return;
			}
			if (Input.GetKeyDown((KeyCode)116) && !typing)
			{
				Show(showMessages: true, showType: true);
			}
			else if (Input.GetKeyDown((KeyCode)13) && typing)
			{
				string text = input.get_text().Trim();
				if (string.IsNullOrEmpty(text))
				{
					input.set_text("");
					input.DeactivateInputField();
					Show(showMessages: false, showType: false);
					return;
				}
				if (text.StartsWith("/"))
				{
					if (NetGame.isServer)
					{
						serverCommands.Execute(text.Substring(1));
					}
					else
					{
						clientCommands.Execute(text.Substring(1));
					}
				}
				else
				{
					Send(text);
				}
				input.set_text("");
				Show(showMessages: true, showType: false);
			}
			else if (Input.GetKeyDown((KeyCode)27) && typing)
			{
				input.set_text("");
				input.DeactivateInputField();
				Show(showMessages: false, showType: false);
			}
		}

		public static void Print(string str)
		{
			if (!visible)
			{
				instance.Show(showMessages: true, typing);
			}
			instance.dismissIn = 3f;
			str = str.Replace("<#", "<color=#");
			if (string.IsNullOrEmpty(contents))
			{
				contents = str;
			}
			else
			{
				contents = contents + "\r\n" + str;
			}
			CropContents();
		}

		public static void CropContents()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			instance.text.set_text(contents);
			List<UILineInfo> list = new List<UILineInfo>();
			float preferredHeight = instance.text.get_preferredHeight();
			while (true)
			{
				float num = preferredHeight;
				Rect rect = ((Graphic)instance.text).get_rectTransform().get_rect();
				if (!(num > ((Rect)(ref rect)).get_height()))
				{
					break;
				}
				instance.text.get_cachedTextGeneratorForLayout().GetLines(list);
				if (list.Count == 0)
				{
					break;
				}
				if (list.Count == 1)
				{
					contents = string.Empty;
				}
				else if (list[1].startCharIdx > 0)
				{
					contents = contents.Substring(list[1].startCharIdx);
				}
				else
				{
					int num2 = contents.IndexOf('\n', 0);
					if (num2 >= 0)
					{
						contents = contents.Substring(num2 + 1);
					}
					else
					{
						contents = string.Empty;
					}
				}
				instance.text.set_text(contents);
				preferredHeight = instance.text.get_preferredHeight();
			}
		}

		public static void OnReceive(uint clientId, string nick, string msg)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			NetHost netHost = NetGame.instance.FindReadyHost(clientId);
			if (netHost != null && !netHost.mute)
			{
				msg = msg.Replace('<', '〈');
				msg = msg.Replace('>', '〉');
				Print($"<#{HexConverter.ColorToHex(Color32.op_Implicit(instance.colors[(long)clientId % (long)instance.colors.Length]))}>{nick}</color> {msg}");
			}
		}

		public void Send(string msg)
		{
			if (NetGame.isNetStarted)
			{
				NetGame.instance.SendChatMessage(msg);
			}
		}

		public NetChat()
			: this()
		{
		}
	}
}
