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
	public class rigid_constraintTechnique_commonSpringAngular
	{
		private TargetableFloat dampingField;

		private TargetableFloat stiffnessField;

		private TargetableFloat target_valueField;

		public TargetableFloat stiffness
		{
			get
			{
				return stiffnessField;
			}
			set
			{
				stiffnessField = value;
			}
		}

		public TargetableFloat damping
		{
			get
			{
				return dampingField;
			}
			set
			{
				dampingField = value;
			}
		}

		public TargetableFloat target_value
		{
			get
			{
				return target_valueField;
			}
			set
			{
				target_valueField = value;
			}
		}
	}
}
