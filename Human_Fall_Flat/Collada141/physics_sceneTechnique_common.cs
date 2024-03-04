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
	public class physics_sceneTechnique_common
	{
		private TargetableFloat3 gravityField;

		private TargetableFloat time_stepField;

		public TargetableFloat3 gravity
		{
			get
			{
				return gravityField;
			}
			set
			{
				gravityField = value;
			}
		}

		public TargetableFloat time_step
		{
			get
			{
				return time_stepField;
			}
			set
			{
				time_stepField = value;
			}
		}
	}
}
