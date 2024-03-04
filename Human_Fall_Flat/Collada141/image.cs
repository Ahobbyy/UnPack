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
	public class image
	{
		private asset assetField;

		private ulong depthField;

		private extra[] extraField;

		private string formatField;

		private ulong heightField;

		private bool heightFieldSpecified;

		private string idField;

		private object itemField;

		private string nameField;

		private ulong widthField;

		private bool widthFieldSpecified;

		public asset asset
		{
			get
			{
				return assetField;
			}
			set
			{
				assetField = value;
			}
		}

		[XmlElement("data", typeof(byte[]), DataType = "hexBinary")]
		[XmlElement("init_from", typeof(string), DataType = "anyURI")]
		public object Item
		{
			get
			{
				return itemField;
			}
			set
			{
				itemField = value;
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

		[XmlAttribute(DataType = "ID")]
		public string id
		{
			get
			{
				return idField;
			}
			set
			{
				idField = value;
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

		[XmlAttribute(DataType = "token")]
		public string format
		{
			get
			{
				return formatField;
			}
			set
			{
				formatField = value;
			}
		}

		[XmlAttribute]
		public ulong height
		{
			get
			{
				return heightField;
			}
			set
			{
				heightField = value;
			}
		}

		[XmlIgnore]
		public bool heightSpecified
		{
			get
			{
				return heightFieldSpecified;
			}
			set
			{
				heightFieldSpecified = value;
			}
		}

		[XmlAttribute]
		public ulong width
		{
			get
			{
				return widthField;
			}
			set
			{
				widthField = value;
			}
		}

		[XmlIgnore]
		public bool widthSpecified
		{
			get
			{
				return widthFieldSpecified;
			}
			set
			{
				widthFieldSpecified = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(ulong), "1")]
		public ulong depth
		{
			get
			{
				return depthField;
			}
			set
			{
				depthField = value;
			}
		}

		public image()
		{
			depthField = 1uL;
		}
	}
}
