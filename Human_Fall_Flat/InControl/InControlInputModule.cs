using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace InControl
{
	public class InControlInputModule : PointerInputModule
	{
		public enum InputControlType
		{
			None = 0,
			Action1 = 19,
			Action2 = 20,
			Action3 = 21,
			Action4 = 22
		}

		public enum Button
		{
			Action1 = 19,
			Action2,
			Action3,
			Action4
		}

		public Button submitButton = Button.Action1;

		public Button cancelButton = Button.Action2;

		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		public float moveRepeatFirstDuration = 0.8f;

		public float moveRepeatDelayDuration = 0.1f;

		[FormerlySerializedAs("allowMobileDevice")]
		public bool forceModuleActive;

		public bool allowMouseInput = true;

		public bool focusOnMouseHover;

		public override void Process()
		{
		}

		public InControlInputModule()
			: this()
		{
		}
	}
}
