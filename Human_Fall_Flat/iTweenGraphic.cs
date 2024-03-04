using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class iTweenGraphic : MonoBehaviour
{
	private Graphic graphic;

	private void OnEnable()
	{
		graphic = ((Component)this).GetComponent<Graphic>();
	}

	public void iTweenOnUpdateColor(Color value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		graphic.set_color(value);
	}

	public static void ColorTo(GameObject go, Color color, float duration)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", go.GetComponent<Graphic>().get_color());
		hashtable.Add("to", color);
		hashtable.Add("time", duration);
		hashtable.Add("onupdate", "iTweenOnUpdateColor");
		iTween.ValueTo(go, hashtable);
	}

	public iTweenGraphic()
		: this()
	{
	}
}
