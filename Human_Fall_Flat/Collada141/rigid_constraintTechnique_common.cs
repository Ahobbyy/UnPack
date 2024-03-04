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
	public class rigid_constraintTechnique_common
	{
		private rigid_constraintTechnique_commonEnabled enabledField;

		private rigid_constraintTechnique_commonInterpenetrate interpenetrateField;

		private rigid_constraintTechnique_commonLimits limitsField;

		private rigid_constraintTechnique_commonSpring springField;

		public rigid_constraintTechnique_commonEnabled enabled
		{
			get
			{
				return enabledField;
			}
			set
			{
				enabledField = value;
			}
		}

		public rigid_constraintTechnique_commonInterpenetrate interpenetrate
		{
			get
			{
				return interpenetrateField;
			}
			set
			{
				interpenetrateField = value;
			}
		}

		public rigid_constraintTechnique_commonLimits limits
		{
			get
			{
				return limitsField;
			}
			set
			{
				limitsField = value;
			}
		}

		public rigid_constraintTechnique_commonSpring spring
		{
			get
			{
				return springField;
			}
			set
			{
				springField = value;
			}
		}
	}
}
