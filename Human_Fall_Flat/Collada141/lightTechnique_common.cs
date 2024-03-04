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
	public class lightTechnique_common
	{
		private object itemField;

		[XmlElement("ambient", typeof(lightTechnique_commonAmbient))]
		[XmlElement("directional", typeof(lightTechnique_commonDirectional))]
		[XmlElement("point", typeof(lightTechnique_commonPoint))]
		[XmlElement("spot", typeof(lightTechnique_commonSpot))]
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
