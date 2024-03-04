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
	public class instance_effectTechnique_hint
	{
		private string platformField;

		private string profileField;

		private string refField;

		[XmlAttribute(DataType = "NCName")]
		public string platform
		{
			get
			{
				return platformField;
			}
			set
			{
				platformField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string profile
		{
			get
			{
				return profileField;
			}
			set
			{
				profileField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string @ref
		{
			get
			{
				return refField;
			}
			set
			{
				refField = value;
			}
		}
	}
}
