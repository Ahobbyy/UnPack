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
	public class instance_rigid_body
	{
		private string bodyField;

		private extra[] extraField;

		private string nameField;

		private string sidField;

		private string targetField;

		private technique[] techniqueField;

		private instance_rigid_bodyTechnique_common technique_commonField;

		public instance_rigid_bodyTechnique_common technique_common
		{
			get
			{
				return technique_commonField;
			}
			set
			{
				technique_commonField = value;
			}
		}

		[XmlElement("technique")]
		public technique[] technique
		{
			get
			{
				return techniqueField;
			}
			set
			{
				techniqueField = value;
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
		public string body
		{
			get
			{
				return bodyField;
			}
			set
			{
				bodyField = value;
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
	}
}
