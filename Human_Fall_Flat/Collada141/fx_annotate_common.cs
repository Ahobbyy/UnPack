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
	public class fx_annotate_common
	{
		private string bool2Field;

		private string bool3Field;

		private string bool4Field;

		private bool boolField;

		private string float2Field;

		private string float2x2Field;

		private string float3Field;

		private string float3x3Field;

		private string float4Field;

		private string float4x4Field;

		private double floatField;

		private string int2Field;

		private string int3Field;

		private string int4Field;

		private long intField;

		private string nameField;

		private string stringField;

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

		public string @string
		{
			get
			{
				return stringField;
			}
			set
			{
				stringField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string name
		{
			get
			{
				return nameField;
			}
			set
			{
				nameField = value;
			}
		}
	}
}
