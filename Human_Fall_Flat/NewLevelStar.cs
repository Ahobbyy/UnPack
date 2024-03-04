using UnityEngine;
using UnityEngine.UI;

public class NewLevelStar : MonoBehaviour
{
	public Image outlineGlow;

	public bool autoHide = true;

	private bool newLevels;

	private void OnEnable()
	{
		if (autoHide)
		{
			newLevels = !GameSave.HasSeenLatestLevel();
			if (!newLevels)
			{
				((Component)this).get_gameObject().SetActive(false);
			}
		}
	}

	private void Update()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Sin(Time.get_time() * 3f);
		((Graphic)outlineGlow).set_color(new Color(1f, 1f, 1f, (num + 1f) / 2f * 0.75f));
	}

	public NewLevelStar()
		: this()
	{
	}
}
