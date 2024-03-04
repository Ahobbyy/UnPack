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
	public class rigid_constraintRef_attachment
	{
		private object[] itemsField;

		private string rigid_bodyField;

		[XmlElement("extra", typeof(extra))]
		[XmlElement("rotate", typeof(rotate))]
		[XmlElement("translate", typeof(TargetableFloat3))]
		public object[] Items
		{
			get
			{
				return itemsField;
			}
			set
			{
				itemsField = value;
			}
		}

		[XmlAttribute(DataType = "anyURI")]
		public string rigid_body
		{
			get
			{
				return rigid_bodyField;
			}
			set
			{
				rigid_bodyField = value;
			}
		}
	}
}
