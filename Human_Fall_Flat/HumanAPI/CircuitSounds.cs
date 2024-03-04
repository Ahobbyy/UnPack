using UnityEngine;

namespace HumanAPI
{
	public class CircuitSounds : MonoBehaviour
	{
		public static CircuitSounds instance;

		public Sound2 plugWireNoCurrent;

		public Sound2 plugWireCurrent;

		public Sound2 unplugWireNoCurrent;

		public Sound2 uplugWireCurrent;

		public Sound2 plugWireShortCircuit;

		private void OnEnable()
		{
			instance = this;
		}

		public static void PlugWireNoCurrent(Vector3 pos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			instance.plugWireNoCurrent.PlayOneShot(pos);
		}

		public static void PlugWireCurrent(Vector3 pos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			instance.plugWireCurrent.PlayOneShot(pos);
		}

		public static void UnplugWireNoCurrent(Vector3 pos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			instance.unplugWireNoCurrent.PlayOneShot(pos);
		}

		public static void UplugWireCurrent(Vector3 pos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			instance.uplugWireCurrent.PlayOneShot(pos);
		}

		public static void PlugWireShortCircuit(Vector3 pos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			instance.plugWireShortCircuit.PlayOneShot(pos);
		}

		public CircuitSounds()
			: this()
		{
		}
	}
}
