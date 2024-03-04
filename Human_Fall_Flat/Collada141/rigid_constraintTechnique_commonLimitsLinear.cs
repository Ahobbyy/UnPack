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
	public class rigid_constraintTechnique_commonLimitsLinear
	{
		private TargetableFloat3 maxField;

		private TargetableFloat3 minField;

		public TargetableFloat3 min
		{
			get
			{
				return minField;
			}
			set
			{
				minField = value;
			}
		}

		public TargetableFloat3 max
		{
			get
			{
				return maxField;
			}
			set
			{
				maxField = value;
			}
		}
	}
}
