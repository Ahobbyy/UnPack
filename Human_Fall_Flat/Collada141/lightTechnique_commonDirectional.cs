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
	public class lightTechnique_commonDirectional
	{
		private TargetableFloat3 colorField;

		public TargetableFloat3 color
		{
			get
			{
				return colorField;
			}
			set
			{
				colorField = value;
			}
		}
	}
}
