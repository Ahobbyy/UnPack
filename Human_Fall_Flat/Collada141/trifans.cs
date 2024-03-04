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
	[XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class trifans
	{
		private ulong countField;

		private extra[] extraField;

		private InputLocalOffset[] inputField;

		private string materialField;

		private string nameField;

		private string[] pField;

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

		[XmlElement("p")]
		public string[] p
		{
			get
			{
				return pField;
			}
			set
			{
				pField = value;
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

		[XmlAttribute(DataType = "NCName")]
		public string name
		{
			get
			{
				return nameField;
			}
			set
			{
				nameField = value;
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

		[XmlAttribute(DataType = "NCName")]
		public string material
		{
			get
			{
				return materialField;
			}
			set
			{
				materialField = value;
			}
		}
	}
}
