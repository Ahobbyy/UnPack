using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[XmlInclude(typeof(common_transparent_type))]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class common_color_or_texture_type
	{
		private object itemField;

		[XmlElement("color", typeof(common_color_or_texture_typeColor))]
		[XmlElement("param", typeof(common_color_or_texture_typeParam))]
		[XmlElement("texture", typeof(common_color_or_texture_typeTexture))]
		public object Item
		{
			get
			{
				return itemField;
			}
			set
			{
				itemField = value;
			}
		}
	}
}
