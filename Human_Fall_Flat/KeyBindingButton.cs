using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingButton : MonoBehaviour
{
	public TextMeshProUGUI keysText;

	public TextMeshProUGUI labelText;

	public ConfigureKeysMenu parent;

	public Image listenRect;

	public Image coverRect;

	public KeyBindingButton()
		: this()
	{
	}
}
