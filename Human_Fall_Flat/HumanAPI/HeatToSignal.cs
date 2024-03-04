using Multiplayer;
using UnityEngine;
using UnityEngine.Events;

namespace HumanAPI
{
	public class HeatToSignal : Node
	{
		public NodeOutput heat;

		protected override void OnEnable()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			base.OnEnable();
			NetBody componentInParent = ((Component)this).GetComponentInParent<NetBody>();
			if (Object.op_Implicit((Object)(object)componentInParent))
			{
				componentInParent.m_respawnEvent.AddListener(new UnityAction(Reset));
			}
		}

		protected override void OnDisable()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			base.OnDisable();
			NetBody componentInParent = ((Component)this).GetComponentInParent<NetBody>();
			if (Object.op_Implicit((Object)(object)componentInParent))
			{
				componentInParent.m_respawnEvent.RemoveListener(new UnityAction(Reset));
			}
		}

		public void InsideObjectChangedState(GameObject other)
		{
			OnEnter(other);
		}

		private void OnEnter(GameObject other)
		{
			Flame component = other.GetComponent<Flame>();
			if ((Object)(object)component != (Object)null)
			{
				heat.SetValue(heat.value + component.isHot.value);
			}
			Flammable component2 = other.GetComponent<Flammable>();
			if (Object.op_Implicit((Object)(object)component2))
			{
				heat.SetValue(heat.value + component2.output.value);
			}
		}

		public void OnTriggerEnter(Collider other)
		{
			OnEnter(((Component)other).get_gameObject());
		}

		private void Reset()
		{
			heat.SetValue(0f);
		}

		public void OnTriggerExit(Collider other)
		{
			Flame component = ((Component)other).GetComponent<Flame>();
			if ((Object)(object)component != (Object)null)
			{
				float num = heat.value - component.isHot.value;
				if (num >= 0f)
				{
					heat.SetValue(num);
				}
			}
			Flammable component2 = ((Component)other).GetComponent<Flammable>();
			if (Object.op_Implicit((Object)(object)component2))
			{
				float num2 = heat.value - component2.output.value;
				if (num2 >= 0f)
				{
					heat.SetValue(num2);
				}
			}
		}
	}
}
