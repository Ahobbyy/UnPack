namespace HumanAPI
{
	public class NodeReroute : Node
	{
		public NodeInput input;

		public NodeOutput output;

		public override string Title => "";

		public override void Process()
		{
			output.SetValue(input.value);
		}
	}
}
