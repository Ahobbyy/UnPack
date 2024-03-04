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
	public class assetUnit
	{
		private double meterField;

		private string nameField;

		[XmlAttribute]
		[DefaultValue(1.0)]
		public double meter
		{
			get
			{
				return meterField;
			}
			set
			{
				meterField = value;
			}
		}

		[XmlAttribute(DataType = "NMTOKEN")]
		[DefaultValue("meter")]
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

		public assetUnit()
		{
			meterField = 1.0;
			nameField = "meter";
		}
	}
}
