using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro
{
	public class TMP_ScrollbarEventHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
	{
		public bool isSelected;

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log((object)"Scrollbar click...");
		}

		public void OnSelect(BaseEventData eventData)
		{
			Debug.Log((object)"Scrollbar selected");
			isSelected = true;
		}

		public void OnDeselect(BaseEventData eventData)
		{
			Debug.Log((object)"Scrollbar De-Selected");
			isSelected = false;
		}

		public TMP_ScrollbarEventHandler()
			: this()
		{
		}
	}
}
