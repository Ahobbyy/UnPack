using UnityEngine;

namespace HumanAPI
{
	[AddNodeMenuItem]
	[AddComponentMenu("Human/Signals/Math/SignalAmbientLight")]
	public class SignalAmbientLight : Node
	{
		public NodeInput r;

		public NodeInput g;

		public NodeInput b;

		public NodeInput a;

		public override string Title => "Ambient Light: (" + r.value + ", " + g.value + ", " + b.value + ", " + a.value + ")";

		public override void Process()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			RenderSettings.set_ambientLight(new Color(r.value, g.value, b.value, a.value));
		}
	}
}
