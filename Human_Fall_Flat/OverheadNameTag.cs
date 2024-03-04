using Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class OverheadNameTag : MonoBehaviour
{
	public Text textMesh;

	public float maxScale = 0.3f;

	public float minScale = 0.15f;

	public float maxScaleDistance = 10f;

	public float minOffsetFromHead = 0.5f;

	public float maxOffsetFromHead = 5f;

	public float rotateSpeed = 1f;

	public float FadeInDuration = 0.1f;

	public float FadeOutDuration = 0.2f;

	public float minWidth = 337f;

	public GameObject Child;

	public SpriteRenderer SpeakerSprite;

	public Sprite TalkingSprite;

	public Sprite NotTalkingSprite;

	public Sprite MutedSprite;

	public float waitTimeOnForceShow = 5f;

	private NetPlayer player;

	private Image childBackground;

	private static Camera mainCamera;

	private float TransitionTimer;

	private bool TransitionInProgress;

	private bool isTalking;

	public float MinimumBgWidth;

	private bool forceShow;

	private float currentWaitTime;

	private float SpeakerSize;

	private float sizeAddition = 0.075f;

	private float initialBGAlpha;

	private float getChildWidth => textMesh.get_preferredWidth() * ((Transform)((Graphic)textMesh).get_rectTransform()).get_localScale().x + SpeakerSize + sizeAddition;

	private void Start()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		player = ((Component)this).GetComponentInParent<NetPlayer>();
		childBackground = Child.GetComponent<Image>();
		if (Object.op_Implicit((Object)(object)childBackground))
		{
			initialBGAlpha = ((Graphic)childBackground).get_color().a;
			((Graphic)childBackground).set_color(new Color(1f, 1f, 1f, 0f));
		}
		if ((Object)(object)player == (Object)null)
		{
			Child.SetActive(false);
		}
		else
		{
			player.overHeadNameTag = this;
		}
		SpeakerSprite.set_sprite(NotTalkingSprite);
		Child.SetActive(false);
		((Renderer)SpeakerSprite).set_enabled(false);
	}

	private void OnEnable()
	{
		textMesh.set_text("WW");
		MinimumBgWidth = getChildWidth;
		textMesh.set_text("");
		AdjustTagWidth();
	}

	private void FadeTransition(bool FadeIn)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)Child) || !Object.op_Implicit((Object)(object)childBackground) || !Object.op_Implicit((Object)(object)SpeakerSprite) || !Object.op_Implicit((Object)(object)textMesh))
		{
			return;
		}
		if (FadeIn)
		{
			if (!Child.get_activeSelf())
			{
				Child.SetActive(true);
			}
			if (TransitionTimer < 1f)
			{
				TransitionTimer += Time.get_unscaledDeltaTime() / FadeInDuration;
				TransitionTimer = Mathf.Clamp01(TransitionTimer);
				((Graphic)childBackground).set_color(new Color(((Graphic)childBackground).get_color().r, ((Graphic)childBackground).get_color().g, ((Graphic)childBackground).get_color().b, TransitionTimer * initialBGAlpha));
				SpeakerSprite.set_color(new Color(SpeakerSprite.get_color().r, SpeakerSprite.get_color().g, SpeakerSprite.get_color().b, TransitionTimer));
				((Graphic)textMesh).set_color(new Color(((Graphic)textMesh).get_color().r, ((Graphic)textMesh).get_color().g, ((Graphic)textMesh).get_color().b, TransitionTimer));
			}
			else if (TransitionInProgress)
			{
				TransitionInProgress = false;
			}
		}
		else if (TransitionTimer > 0f)
		{
			TransitionTimer -= Time.get_unscaledDeltaTime() / FadeOutDuration;
			TransitionTimer = Mathf.Clamp01(TransitionTimer);
			((Graphic)childBackground).set_color(new Color(((Graphic)childBackground).get_color().r, ((Graphic)childBackground).get_color().g, ((Graphic)childBackground).get_color().b, TransitionTimer * initialBGAlpha));
			SpeakerSprite.set_color(new Color(SpeakerSprite.get_color().r, SpeakerSprite.get_color().g, SpeakerSprite.get_color().b, TransitionTimer));
			((Graphic)textMesh).set_color(new Color(((Graphic)textMesh).get_color().r, ((Graphic)textMesh).get_color().g, ((Graphic)textMesh).get_color().b, TransitionTimer));
		}
		else if (Child.get_activeSelf())
		{
			Child.SetActive(false);
			TransitionInProgress = false;
		}
	}

	private void Update()
	{
	}

	public void UpdateNameTag(ChatUser user)
	{
		textMesh.set_text(user.GamerTag);
		((Renderer)SpeakerSprite).set_enabled(false);
		AdjustTagWidth();
	}

	private void AdjustTagWidth()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(textMesh.get_text()) && textMesh.get_text().Length > 16)
		{
			textMesh.set_text(textMesh.get_text().Substring(0, 16) + "…");
		}
		RectTransform component = Child.GetComponent<RectTransform>();
		float num = ((getChildWidth < MinimumBgWidth) ? MinimumBgWidth : getChildWidth);
		Rect rect = component.get_rect();
		component.set_sizeDelta(new Vector2(num, ((Rect)(ref rect)).get_height()));
	}

	public OverheadNameTag()
		: this()
	{
	}
}
