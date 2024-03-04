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
	public class rigid_constraintTechnique_commonSpring
	{
		private rigid_constraintTechnique_commonSpringAngular angularField;

		private rigid_constraintTechnique_commonSpringLinear linearField;

		public rigid_constraintTechnique_commonSpringAngular angular
		{
			get
			{
				return angularField;
			}
			set
			{
				angularField = value;
			}
		}

		public rigid_constraintTechnique_commonSpringLinear linear
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
