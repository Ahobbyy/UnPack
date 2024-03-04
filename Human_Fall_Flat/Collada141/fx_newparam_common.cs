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
	public class fx_newparam_common
	{
		private fx_annotate_common[] annotateField;

		private string bool2Field;

		private string bool3Field;

		private string bool4Field;

		private bool boolField;

		private string enumField;

		private double float1x1Field;

		private string float1x2Field;

		private string float1x3Field;

		private string float1x4Field;

		private string float2Field;

		private string float2x1Field;

		private string float2x2Field;

		private string float2x3Field;

		private string float2x4Field;

		private string float3Field;

		private string float3x1Field;

		private string float3x2Field;

		private string float3x3Field;

		private string float3x4Field;

		private string float4Field;

		private string float4x1Field;

		private string float4x2Field;

		private string float4x3Field;

		private string float4x4Field;

		private double floatField;

		private string int2Field;

		private string int3Field;

		private string int4Field;

		private long intField;

		private fx_modifier_enum_common modifierField;

		private bool modifierFieldSpecified;

		private fx_sampler1D_common sampler1DField;

		private fx_sampler2D_common sampler2DField;

		private fx_sampler3D_common sampler3DField;

		private fx_samplerCUBE_common samplerCUBEField;

		private fx_samplerDEPTH_common samplerDEPTHField;

		private fx_samplerRECT_common samplerRECTField;

		private string semanticField;

		private string sidField;

		private fx_surface_common surfaceField;

		[XmlElement("annotate")]
		public fx_annotate_common[] annotate
		{
			get
			{
				return annotateField;
			}
			set
			{
				annotateField = value;
			}
		}

		[XmlElement(DataType = "NCName")]
		public string semantic
		{
			get
			{
				return semanticField;
			}
			set
			{
				semanticField = value;
			}
		}

		public fx_modifier_enum_common modifier
		{
			get
			{
				return modifierField;
			}
			set
			{
				modifierField = value;
			}
		}

		[XmlIgnore]
		public bool modifierSpecified
		{
			get
			{
				return modifierFieldSpecified;
			}
			set
			{
				modifierFieldSpecified = value;
			}
		}

		public bool @bool
		{
			get
			{
				return boolField;
			}
			set
			{
				boolField = value;
			}
		}

		public string bool2
		{
			get
			{
				return bool2Field;
			}
			set
			{
				bool2Field = value;
			}
		}

		public string bool3
		{
			get
			{
				return bool3Field;
			}
			set
			{
				bool3Field = value;
			}
		}

		public string bool4
		{
			get
			{
				return bool4Field;
			}
			set
			{
				bool4Field = value;
			}
		}

		public long @int
		{
			get
			{
				return intField;
			}
			set
			{
				intField = value;
			}
		}

		public string int2
		{
			get
			{
				return int2Field;
			}
			set
			{
				int2Field = value;
			}
		}

		public string int3
		{
			get
			{
				return int3Field;
			}
			set
			{
				int3Field = value;
			}
		}

		public string int4
		{
			get
			{
				return int4Field;
			}
			set
			{
				int4Field = value;
			}
		}

		public double @float
		{
			get
			{
				return floatField;
			}
			set
			{
				floatField = value;
			}
		}

		public string float2
		{
			get
			{
				return float2Field;
			}
			set
			{
				float2Field = value;
			}
		}

		public string float3
		{
			get
			{
				return float3Field;
			}
			set
			{
				float3Field = value;
			}
		}

		public string float4
		{
			get
			{
				return float4Field;
			}
			set
			{
				float4Field = value;
			}
		}

		public double float1x1
		{
			get
			{
				return float1x1Field;
			}
			set
			{
				float1x1Field = value;
			}
		}

		public string float1x2
		{
			get
			{
				return float1x2Field;
			}
			set
			{
				float1x2Field = value;
			}
		}

		public string float1x3
		{
			get
			{
				return float1x3Field;
			}
			set
			{
				float1x3Field = value;
			}
		}

		public string float1x4
		{
			get
			{
				return float1x4Field;
			}
			set
			{
				float1x4Field = value;
			}
		}

		public string float2x1
		{
			get
			{
				return float2x1Field;
			}
			set
			{
				float2x1Field = value;
			}
		}

		public string float2x2
		{
			get
			{
				return float2x2Field;
			}
			set
			{
				float2x2Field = value;
			}
		}

		public string float2x3
		{
			get
			{
				return float2x3Field;
			}
			set
			{
				float2x3Field = value;
			}
		}

		public string float2x4
		{
			get
			{
				return float2x4Field;
			}
			set
			{
				float2x4Field = value;
			}
		}

		public string float3x1
		{
			get
			{
				return float3x1Field;
			}
			set
			{
				float3x1Field = value;
			}
		}

		public string float3x2
		{
			get
			{
				return float3x2Field;
			}
			set
			{
				float3x2Field = value;
			}
		}

		public string float3x3
		{
			get
			{
				return float3x3Field;
			}
			set
			{
				float3x3Field = value;
			}
		}

		public string float3x4
		{
			get
			{
				return float3x4Field;
			}
			set
			{
				float3x4Field = value;
			}
		}

		public string float4x1
		{
			get
			{
				return float4x1Field;
			}
			set
			{
				float4x1Field = value;
			}
		}

		public string float4x2
		{
			get
			{
				return float4x2Field;
			}
			set
			{
				float4x2Field = value;
			}
		}

		public string float4x3
		{
			get
			{
				return float4x3Field;
			}
			set
			{
				float4x3Field = value;
			}
		}

		public string float4x4
		{
			get
			{
				return float4x4Field;
			}
			set
			{
				float4x4Field = value;
			}
		}

		public fx_surface_common surface
		{
			get
			{
				return surfaceField;
			}
			set
			{
				surfaceField = value;
			}
		}

		public fx_sampler1D_common sampler1D
		{
			get
			{
				return sampler1DField;
			}
			set
			{
				sampler1DField = value;
			}
		}

		public fx_sampler2D_common sampler2D
		{
			get
			{
				return sampler2DField;
			}
			set
			{
				sampler2DField = value;
			}
		}

		public fx_sampler3D_common sampler3D
		{
			get
			{
				return sampler3DField;
			}
			set
			{
				sampler3DField = value;
			}
		}

		public fx_samplerCUBE_common samplerCUBE
		{
			get
			{
				return samplerCUBEField;
			}
			set
			{
				samplerCUBEField = value;
			}
		}

		public fx_samplerRECT_common samplerRECT
		{
			get
			{
				return samplerRECTField;
			}
			set
			{
				samplerRECTField = value;
			}
		}

		public fx_samplerDEPTH_common samplerDEPTH
		{
			get
			{
				return samplerDEPTHField;
			}
			set
			{
				samplerDEPTHField = value;
			}
		}

		public string @enum
		{
			get
			{
				return enumField;
			}
			set
			{
				enumField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string sid
		{
			get
			{
				return sidField;
			}
			set
			{
				sidField = value;
			}
		}
	}
}
