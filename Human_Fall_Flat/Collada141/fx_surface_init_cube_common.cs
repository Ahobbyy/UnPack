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
	public class fx_surface_init_cube_common
	{
		private object[] itemsField;

		[XmlElement("all", typeof(fx_surface_init_cube_commonAll))]
		[XmlElement("face", typeof(fx_surface_init_cube_commonFace))]
		[XmlElement("primary", typeof(fx_surface_init_cube_commonPrimary))]
		public object[] Items
		{
			get
			{
				return itemsField;
			}
			set
			{
				itemsField = value;
			}
		}
	}
}
