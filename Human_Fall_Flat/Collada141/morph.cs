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
	public class morph
	{
		private extra[] extraField;

		private MorphMethodType methodField;

		private string source1Field;

		private source[] sourceField;

		private morphTargets targetsField;

		[XmlElement("source")]
		public source[] source
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

		public morphTargets targets
		{
			get
			{
				return targetsField;
			}
			set
			{
				targetsField = value;
			}
		}

		[XmlElement("extra")]
		public extra[] extra
		{
			get
			{
				return extraField;
			}
			set
			{
				extraField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(MorphMethodType.NORMALIZED)]
		public MorphMethodType method
		{
			get
			{
				return methodField;
			}
			set
			{
				methodField = value;
			}
		}

		[XmlAttribute("source", DataType = "anyURI")]
		public string source1
		{
			get
			{
				return source1Field;
			}
			set
			{
				source1Field = value;
			}
		}

		public morph()
		{
			methodField = MorphMethodType.NORMALIZED;
		}
	}
}
