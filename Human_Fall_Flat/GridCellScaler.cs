using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridCellScaler : MonoBehaviour
{
	private GridLayoutGroup gridLayoutGroup;

	private RectTransform rect;

	private float height;

	private int cellCount = 2;

	private AutoNavigation m_NavigationObject;

	[SerializeField]
	private int Rows = 4;

	[SerializeField]
	private int Columns = 2;

	private void Start()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		gridLayoutGroup = ((Component)this).GetComponent<GridLayoutGroup>();
		rect = ((Component)this).GetComponent<RectTransform>();
		GridLayoutGroup obj = gridLayoutGroup;
		Rect val = rect.get_rect();
		float num = ((Rect)(ref val)).get_height();
		val = rect.get_rect();
		obj.set_cellSize(new Vector2(num, ((Rect)(ref val)).get_height()));
		cellCount = ((Component)this).GetComponentsInChildren<RectTransform>().Length;
	}

	private void OnEnable()
	{
		UpdateCellDimensions();
	}

	private void OnRectTransformDimensionsChange()
	{
		UpdateCellDimensions();
	}

	private void UpdateCellDimensions()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)gridLayoutGroup != (Object)null && (Object)(object)rect != (Object)null)
		{
			Rect val = rect.get_rect();
			float num = (((Rect)(ref val)).get_height() - gridLayoutGroup.get_spacing().y * (float)Rows) / (float)Rows;
			val = rect.get_rect();
			float num2 = (((Rect)(ref val)).get_width() - gridLayoutGroup.get_spacing().x * (float)Columns) / (float)Columns;
			gridLayoutGroup.set_cellSize(new Vector2(num2, num));
		}
	}

	public GridCellScaler()
		: this()
	{
	}
}
