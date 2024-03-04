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
	public class instance_materialBind
	{
		private string semanticField;

		private string targetField;

		[XmlAttribute(DataType = "NCName")]
		public string semantic
		{
			get
			{
				return semanticField;
			}
			set
			{
				semanticField = value;
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
