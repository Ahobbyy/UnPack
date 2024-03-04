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
	public class lightTechnique_commonPoint
	{
		private TargetableFloat3 colorField;

		private TargetableFloat constant_attenuationField;

		private TargetableFloat linear_attenuationField;

		private TargetableFloat quadratic_attenuationField;

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

		public TargetableFloat constant_attenuation
		{
			get
			{
				return constant_attenuationField;
			}
			set
			{
				constant_attenuationField = value;
			}
		}

		public TargetableFloat linear_attenuation
		{
			get
			{
				return linear_attenuationField;
			}
			set
			{
				linear_attenuationField = value;
			}
		}

		public TargetableFloat quadratic_attenuation
		{
			get
			{
				return quadratic_attenuationField;
			}
			set
			{
				quadratic_attenuationField = value;
			}
		}
	}
}
