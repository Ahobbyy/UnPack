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
	public class InputLocalOffset
	{
		private ulong offsetField;

		private string semanticField;

		private ulong setField;

		private bool setFieldSpecified;

		private string sourceField;

		[XmlAttribute]
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

		[XmlAttribute(DataType = "NMTOKEN")]
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

		[XmlAttribute]
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
		public ulong set
		{
			get
			{
				return setField;
			}
			set
			{
				setField = value;
			}
		}

		[XmlIgnore]
		public bool setSpecified
		{
			get
			{
				return setFieldSpecified;
			}
			set
			{
				setFieldSpecified = value;
			}
		}
	}
}
