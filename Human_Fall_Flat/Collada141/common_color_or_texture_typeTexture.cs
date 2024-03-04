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
	public class common_color_or_texture_typeTexture
	{
		private extra extraField;

		private string texcoordField;

		private string textureField;

		public extra extra
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
		public string texture
		{
			get
			{
				return textureField;
			}
			set
			{
				textureField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string texcoord
		{
			get
			{
				return texcoordField;
			}
			set
			{
				texcoordField = value;
			}
		}
	}
}
