using System.Text;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
	private TextMeshProUGUI mTextBox;

	private StringBuilder builder = new StringBuilder(16);

	private void Start()
	{
		mTextBox = ((Component)this).GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		builder.Length = 0;
		builder.AppendFormat("{0:F}", 1f / Time.get_smoothDeltaTime());
		mTextBox.SetText(builder.ToString());
	}

	public FPS()
		: this()
	{
	}
}
