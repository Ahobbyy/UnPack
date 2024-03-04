using System;
using Multiplayer;
using UnityEngine;

public class NameTag : MonoBehaviour
{
	public delegate void NameTagActivatedCallBack(bool active);

	public TextMesh textMesh;

	public Shader textShader;

	public GameObject textBG;

	public float maxScale = 0.7f;

	public float minScale = 0.5f;

	public float maxScaleDistance = 30f;

	private const int kMaxNameChars = 16;

	private NetPlayer player;

	private MeshFilter meshFilter;

	private Renderer rendererBG;

	private Renderer rendererText;

	private int[] kIndices = new int[6] { 0, 1, 2, 2, 1, 3 };

	private Vector3[] vertices = (Vector3[])(object)new Vector3[4];

	private const float kPixelToPolyWidth = 266.6f;

	private const float kPixelToPolyHeight = 200f;

	private float kInitialZ;

	private const float kWaitTimeOnForceShow = 5f;

	private bool forceShow;

	private float currentWaitTime;

	private static NameTagActivatedCallBack nameTagCallback;

	private static bool previousRenderState;

	public static void RegisterForNameTagCallbacks(NameTagActivatedCallBack callback)
	{
		nameTagCallback = (NameTagActivatedCallBack)Delegate.Combine(nameTagCallback, callback);
	}

	public static void UnRegisterForNameTagCallbacks(NameTagActivatedCallBack callback)
	{
		nameTagCallback = (NameTagActivatedCallBack)Delegate.Remove(nameTagCallback, callback);
	}

	private int CalculateStringWidth()
	{
		string text = textMesh.get_text();
		int length = text.Length;
		int num = 0;
		textMesh.get_font().RequestCharactersInTexture(text, textMesh.get_font().get_fontSize());
		CharacterInfo val = default(CharacterInfo);
		for (int i = 0; i < length; i++)
		{
			textMesh.get_font().GetCharacterInfo(text[i], ref val, textMesh.get_font().get_fontSize());
			num += ((CharacterInfo)(ref val)).get_advance();
		}
		return num;
	}

	private void TruncateName()
	{
		player.nametag = this;
		string text = player.host.name;
		if (text.Length > 16)
		{
			text = text.Substring(0, 16);
			text += "...";
		}
		textMesh.set_text(text);
	}

	private void BuildMesh(int stringWidth, int stringHeight)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)stringWidth / 266.6f;
		float num2 = (float)stringHeight / 200f;
		vertices[0] = new Vector3(0f - num, num2, kInitialZ);
		vertices[1] = new Vector3(num, num2, kInitialZ);
		vertices[2] = new Vector3(0f - num, 0f - num2, kInitialZ);
		vertices[3] = new Vector3(num, 0f - num2, kInitialZ);
	}

	private void Start()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		player = ((Component)this).GetComponentInParent<NetPlayer>();
		if ((Object)(object)player == (Object)null)
		{
			((Component)this).get_gameObject().SetActive(false);
			return;
		}
		TruncateName();
		int stringWidth = CalculateStringWidth();
		int lineHeight = textMesh.get_font().get_lineHeight();
		rendererText = ((Component)textMesh).GetComponent<Renderer>();
		rendererText.get_sharedMaterial().set_shader(textShader);
		rendererBG = textBG.GetComponent<Renderer>();
		meshFilter = ((Component)textBG.get_transform()).GetComponent<MeshFilter>();
		Mesh val = new Mesh();
		BuildMesh(stringWidth, lineHeight);
		val.set_vertices(vertices);
		val.SetIndices(kIndices, (MeshTopology)0, 0);
		val.MarkDynamic();
		val.UploadMeshData(false);
		meshFilter.set_sharedMesh(val);
		EnableRenderers(enable: false);
	}

	private void EnableRenderers(bool enable)
	{
		if (previousRenderState != enable)
		{
			nameTagCallback(enable);
			previousRenderState = enable;
		}
		rendererBG.set_enabled(enable);
		rendererText.set_enabled(enable);
	}

	private bool ShouldDisplay()
	{
		if (!player.isLocalPlayer)
		{
			return (Object)(object)MenuSystem.instance.activeMenu == (Object)null;
		}
		return false;
	}

	private void Update()
	{
		if (currentWaitTime > 0f)
		{
			currentWaitTime -= Time.get_deltaTime();
			if (currentWaitTime < 0f)
			{
				EnableRenderers(enable: false);
			}
		}
	}

	public void Align(Transform transform)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		if (forceShow && ShouldDisplay())
		{
			((Component)this).get_transform().set_rotation(transform.get_rotation());
			float num = Vector3.Distance(((Component)player.human.ragdoll).get_transform().get_position(), ((Component)Camera.get_current()).get_transform().get_position());
			float num2 = Mathf.Tan(((Component)transform).GetComponent<Camera>().get_fieldOfView() * 0.5f * ((float)Math.PI / 180f));
			((Component)this).get_transform().set_localScale(Vector3.get_one() * num * num2 * Mathf.Lerp(maxScale, minScale, num / maxScaleDistance));
		}
	}

	public NameTag()
		: this()
	{
	}
}
