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
	public class instance_material
	{
		private instance_materialBind[] bindField;

		private instance_materialBind_vertex_input[] bind_vertex_inputField;

		private extra[] extraField;

		private string nameField;

		private string sidField;

		private string symbolField;

		private string targetField;

		[XmlElement("bind")]
		public instance_materialBind[] bind
		{
			get
			{
				return bindField;
			}
			set
			{
				bindField = value;
			}
		}

		[XmlElement("bind_vertex_input")]
		public instance_materialBind_vertex_input[] bind_vertex_input
		{
			get
			{
				return bind_vertex_inputField;
			}
			set
			{
				bind_vertex_inputField = value;
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
		public string symbol
		{
			get
			{
				return symbolField;
			}
			set
			{
				symbolField = value;
			}
		}

		[XmlAttribute(DataType = "anyURI")]
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

		[XmlAttribute(DataType = "NCName")]
		public string sid
		{
			get
			{
				return sidField;
			}
			set
			{
				sidField = value;
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
	}
}
