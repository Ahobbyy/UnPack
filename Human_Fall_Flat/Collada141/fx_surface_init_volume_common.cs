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
	public class fx_surface_init_volume_common
	{
		private object itemField;

		[XmlElement("all", typeof(fx_surface_init_volume_commonAll))]
		[XmlElement("primary", typeof(fx_surface_init_volume_commonPrimary))]
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
