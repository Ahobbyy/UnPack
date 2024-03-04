namespace nn
{
	public class ErrorRange
	{
		private int _module;

		private int _descriptionBegin;

		private int _descriptionEnd;

		public int Module => _module;

		public int DescriptionBegin => _descriptionBegin;

		public int DescriptionEnd => _descriptionEnd;

		internal ErrorRange(int Module, int DescriptionBegin, int DescriptionEnd)
		{
			_module = Module;
			_descriptionBegin = DescriptionBegin;
			_descriptionEnd = DescriptionEnd;
		}

		public bool Includes(Result result)
		{
			if (result.GetModule() != Module)
			{
				return false;
			}
			int description = result.GetDescription();
			if (DescriptionBegin <= description)
			{
				return description < DescriptionEnd;
			}
			return false;
		}
	}
}
