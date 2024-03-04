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
	public class rigid_constraintTechnique_commonLimits
	{
		private rigid_constraintTechnique_commonLimitsLinear linearField;

		private rigid_constraintTechnique_commonLimitsSwing_cone_and_twist swing_cone_and_twistField;

		public rigid_constraintTechnique_commonLimitsSwing_cone_and_twist swing_cone_and_twist
		{
			get
			{
				return swing_cone_and_twistField;
			}
			set
			{
				swing_cone_and_twistField = value;
			}
		}

		public rigid_constraintTechnique_commonLimitsLinear linear
		{
			get
			{
				return linearField;
			}
			set
			{
				linearField = value;
			}
		}
	}
}
