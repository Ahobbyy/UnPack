using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class skinVertex_weights
	{
		private ulong countField;

		private extra[] extraField;

		private InputLocalOffset[] inputField;

		private string vField;

		private string vcountField;

		[XmlElement("input")]
		public InputLocalOffset[] input
		{
			get
			{
				return inputField;
			}
			set
			{
				inputField = value;
			}
		}

		public string vcount
		{
			get
			{
				return vcountField;
			}
			set
			{
				vcountField = value;
			}
		}

		public string v
		{
			get
			{
				return vField;
			}
			set
			{
				vField = value;
			}
		}

		[XmlElement("extra")]
		public extra[] extra
		{
			get
			{
				return extraField;
			}
			set
			{
				extraField = value;
			}
		}

		[XmlAttribute]
		public ulong count
		{
			get
			{
				return countField;
			}
			set
			{
				countField = value;
			}
		}
	}
}
