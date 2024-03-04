using System;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shell : MonoBehaviour
{
	public TMP_InputField input;

	public TextMeshProUGUI text;

	public GameObject ui;

	public static bool visible = false;

	public static Shell instance;

	private bool lostDevice;

	private bool reloadSkins;

	private static CommandRegistry commands = new CommandRegistry(Print);

	private static string contents = "";

	private void Awake()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		instance = this;
		Application.add_logMessageReceived(new LogCallback(Application_logMessageReceived));
		RegisterCommand("?", commands.OnHelp);
		RegisterCommand("help", commands.OnHelp, "help <command>\r\nExplains command.");
	}

	private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected I4, but got Unknown
		if (condition.Contains("device lost"))
		{
			lostDevice = true;
			return;
		}
		switch ((int)type)
		{
		case 0:
		case 1:
		case 4:
			Print("<#FF0000>" + condition + "</color>");
			Print("<#FF7F7F>" + stackTrace + "</color>");
			break;
		case 2:
			Print("<#FFFF00>" + condition + "</color>");
			break;
		default:
			Print(condition);
			break;
		}
	}

	private void Update()
	{
		if (lostDevice)
		{
			reloadSkins = true;
			lostDevice = false;
		}
		else if (reloadSkins)
		{
			reloadSkins = (lostDevice = false);
			for (int i = 0; i < NetGame.instance.players.Count; i++)
			{
				NetGame.instance.players[i].ReapplySkin();
			}
		}
		if (MenuSystem.keyboardState != 0 && MenuSystem.keyboardState != KeyboardState.Shell)
		{
			return;
		}
		if (Game.GetKeyDown((KeyCode)96) || Game.GetKeyDown((KeyCode)282))
		{
			visible = !visible;
			ui.SetActive(visible);
			MenuSystem.keyboardState = (visible ? KeyboardState.Shell : KeyboardState.None);
			if (visible)
			{
				MenuSystem.instance.FocusOnMouseOver(enable: false);
				input.ActivateInputField();
			}
			else
			{
				input.DeactivateInputField();
				if (input.text.EndsWith("`"))
				{
					input.text = input.text.Substring(0, input.text.Length - 1);
				}
			}
		}
		if (!visible || !((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() == (Object)(object)((Component)input).get_gameObject()))
		{
			return;
		}
		if (Game.GetKeyDown((KeyCode)27))
		{
			if (!string.IsNullOrEmpty(input.text))
			{
				input.text = "";
			}
			else
			{
				visible = false;
				ui.SetActive(false);
				MenuSystem.keyboardState = KeyboardState.None;
				input.DeactivateInputField();
			}
		}
		if (Game.GetKeyDown((KeyCode)13))
		{
			string text = input.text.Trim();
			Print("> " + text);
			commands.Execute(text.ToLowerInvariant());
			input.text = "";
			input.ActivateInputField();
		}
	}

	public static void RawInvoke(string cmd)
	{
		commands.Execute(cmd.ToLowerInvariant());
	}

	public static void RegisterCommand(string command, Action onCommand, string help = null)
	{
		commands.RegisterCommand(command, onCommand, help);
	}

	public static void RegisterCommand(string command, Action<string> onCommand, string help = null)
	{
		commands.RegisterCommand(command, onCommand, help);
	}

	public static void Print(string str)
	{
		contents = contents + "\r\n" + str;
		int num = 0;
		int num2 = -1;
		while ((num2 = contents.IndexOf('\n', num2 + 1)) >= 0)
		{
			num++;
		}
		num2 = -1;
		while (num >= 40)
		{
			num2 = contents.IndexOf('\n', num2 + 1);
			num--;
		}
		if (num2 > 0)
		{
			contents = contents.Substring(num2 + 1);
		}
		instance.text.text = contents;
	}

	public Shell()
		: this()
	{
	}
}
