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
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	[XmlRoot("instance_camera", Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class InstanceWithExtra
	{
		private extra[] extraField;

		private string nameField;

		private string sidField;

		private string urlField;

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

		[XmlAttribute(DataType = "anyURI")]
		public string url
		{
			get
			{
				return urlField;
			}
			set
			{
				urlField = value;
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
