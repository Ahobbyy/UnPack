using UnityEngine;

namespace HumanAPI
{
	public class CollisionByLayerSensor : Node
	{
		public NodeOutput output;

		public int LayerToCheck = 9;

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.get_gameObject().get_layer() == LayerToCheck)
			{
				output.SetValue(1f);
			}
		}

		private void OnCollisionStay(Collision collision)
		{
			if (collision.get_gameObject().get_layer() == LayerToCheck)
			{
				output.SetValue(1f);
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (collision.get_gameObject().get_layer() == LayerToCheck)
			{
				output.SetValue(0f);
			}
		}
	}
}
