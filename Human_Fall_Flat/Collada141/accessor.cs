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
	[XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class accessor
	{
		private ulong countField;

		private ulong offsetField;

		private param[] paramField;

		private string sourceField;

		private ulong strideField;

		[XmlElement("param")]
		public param[] param
		{
			get
			{
				return paramField;
			}
			set
			{
				paramField = value;
			}
		}

		[XmlAttribute]
		public ulong count
		{
			get
			{
				return countField;
			}
			set
			{
				countField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(ulong), "0")]
		public ulong offset
		{
			get
			{
				return offsetField;
			}
			set
			{
				offsetField = value;
			}
		}

		[XmlAttribute(DataType = "anyURI")]
		public string source
		{
			get
			{
				return sourceField;
			}
			set
			{
				sourceField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(ulong), "1")]
		public ulong stride
		{
			get
			{
				return strideField;
			}
			set
			{
				strideField = value;
			}
		}

		public accessor()
		{
			offsetField = 0uL;
			strideField = 1uL;
		}
	}
}
