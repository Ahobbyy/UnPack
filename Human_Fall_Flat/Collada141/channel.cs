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
	public class channel
	{
		private string sourceField;

		private string targetField;

		[XmlAttribute]
		public string source
		{
			get
			{
				return sourceField;
			}
			set
			{
				sourceField = value;
			}
		}

		[XmlAttribute(DataType = "token")]
		public string target
		{
			get
			{
				return targetField;
			}
			set
			{
				targetField = value;
			}
		}
	}
}
