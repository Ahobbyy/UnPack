using UnityEngine;

namespace HumanAPI
{
	public class SignalSetScale : Node
	{
		public NodeInput input;

		public Vector3 targetScale = new Vector3(2f, 2f, 2f);

		public SignalBase triggerSignal;

		private Transform targetObject;

		private Vector3 initialScale;

		private void Start()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			targetObject = ((Component)this).GetComponent<Transform>();
			initialScale = targetObject.get_localScale();
			if ((Object)(object)triggerSignal != (Object)null)
			{
				triggerSignal.onValueChanged += SignalChanged;
				SignalChanged(triggerSignal.value);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if ((Object)(object)triggerSignal != (Object)null)
			{
				triggerSignal.onValueChanged -= SignalChanged;
			}
		}

		public override void Process()
		{
			base.Process();
			SignalChanged(input.value);
		}

		private void SignalChanged(float val)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			targetObject.set_localScale(Vector3.Lerp(initialScale, targetScale, val));
		}
	}
}
