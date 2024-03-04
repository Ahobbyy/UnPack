using UnityEngine;

public class UICanvas : MonoBehaviour
{
	public static UICanvas instance;

	public static RectTransform rectTransform;

	public static Camera camera;

	private void OnEnable()
	{
		instance = this;
		Transform transform = ((Component)this).get_transform();
		rectTransform = (RectTransform)(object)((transform is RectTransform) ? transform : null);
		camera = ((Component)this).GetComponent<Canvas>().get_worldCamera();
	}

	public static Vector2 ScreenPointToLocal(Vector2 pos)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = default(Vector2);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, camera, ref result);
		return result;
	}

	public static Vector2 ScreenPointToLocal(RectTransform rect, Vector2 pos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = default(Vector2);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pos, camera, ref result);
		return result;
	}

	public UICanvas()
		: this()
	{
	}
}
