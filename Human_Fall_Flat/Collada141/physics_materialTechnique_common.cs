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
	public class physics_materialTechnique_common
	{
		private TargetableFloat dynamic_frictionField;

		private TargetableFloat restitutionField;

		private TargetableFloat static_frictionField;

		public TargetableFloat dynamic_friction
		{
			get
			{
				return dynamic_frictionField;
			}
			set
			{
				dynamic_frictionField = value;
			}
		}

		public TargetableFloat restitution
		{
			get
			{
				return restitutionField;
			}
			set
			{
				restitutionField = value;
			}
		}

		public TargetableFloat static_friction
		{
			get
			{
				return static_frictionField;
			}
			set
			{
				static_frictionField = value;
			}
		}
	}
}
