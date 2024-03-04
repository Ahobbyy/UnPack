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
	public class instance_effect
	{
		private extra[] extraField;

		private string nameField;

		private instance_effectSetparam[] setparamField;

		private string sidField;

		private instance_effectTechnique_hint[] technique_hintField;

		private string urlField;

		[XmlElement("technique_hint")]
		public instance_effectTechnique_hint[] technique_hint
		{
			get
			{
				return technique_hintField;
			}
			set
			{
				technique_hintField = value;
			}
		}

		[XmlElement("setparam")]
		public instance_effectSetparam[] setparam
		{
			get
			{
				return setparamField;
			}
			set
			{
				setparamField = value;
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
