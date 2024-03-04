using TMPro;
using UnityEngine;

public class ScrollableTextArea : MonoBehaviour
{
	public TextMeshProUGUI textArea;

	private float scrollPos;

	private float totalHeight;

	private float viewPortHeight;

	private Vector3 startPos;

	private float tweenFrom;

	private float tweenTo;

	private float tweenPhase = 1f;

	private float tweenDuration = 0.3f;

	private void OnEnable()
	{
		textArea = ((Component)this).GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)textArea == (Object)null)
		{
			return;
		}
		if (totalHeight != textArea.preferredHeight)
		{
			totalHeight = textArea.preferredHeight;
			Rect rect = textArea.rectTransform.get_rect();
			viewPortHeight = ((Rect)(ref rect)).get_height();
			startPos = ((Transform)textArea.rectTransform).get_localPosition();
		}
		if (!(totalHeight > viewPortHeight))
		{
			return;
		}
		float num = Input.get_mouseScrollDelta().y * 100f;
		if (num != 0f)
		{
			float num2 = tweenTo - num;
			num2 = Mathf.Clamp(num2, 0f, totalHeight - viewPortHeight);
			if (num2 != tweenTo)
			{
				tweenTo = num2;
				tweenFrom = scrollPos;
				tweenPhase = 0f;
			}
		}
		if (tweenPhase != 1f)
		{
			tweenPhase += Time.get_unscaledDeltaTime() / tweenDuration;
			tweenPhase = Mathf.Clamp01(tweenPhase);
			float num3 = Ease.easeOutQuad(0f, 1f, tweenPhase);
			scrollPos = Mathf.Lerp(tweenFrom, tweenTo, num3);
			((Transform)textArea.rectTransform).set_localPosition(startPos + new Vector3(0f, scrollPos, 0f));
		}
	}

	public ScrollableTextArea()
		: this()
	{
	}
}
