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
	public class instance_materialBind_vertex_input
	{
		private string input_semanticField;

		private ulong input_setField;

		private bool input_setFieldSpecified;

		private string semanticField;

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

		[XmlAttribute(DataType = "NCName")]
		public string input_semantic
		{
			get
			{
				return input_semanticField;
			}
			set
			{
				input_semanticField = value;
			}
		}

		[XmlAttribute]
		public ulong input_set
		{
			get
			{
				return input_setField;
			}
			set
			{
				input_setField = value;
			}
		}

		[XmlIgnore]
		public bool input_setSpecified
		{
			get
			{
				return input_setFieldSpecified;
			}
			set
			{
				input_setFieldSpecified = value;
			}
		}
	}
}
