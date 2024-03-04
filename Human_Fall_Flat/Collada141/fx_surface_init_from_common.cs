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
	public class fx_surface_init_from_common
	{
		private fx_surface_face_enum faceField;

		private uint mipField;

		private uint sliceField;

		private string valueField;

		[XmlAttribute]
		[DefaultValue(typeof(uint), "0")]
		public uint mip
		{
			get
			{
				return mipField;
			}
			set
			{
				mipField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(uint), "0")]
		public uint slice
		{
			get
			{
				return sliceField;
			}
			set
			{
				sliceField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(fx_surface_face_enum.POSITIVE_X)]
		public fx_surface_face_enum face
		{
			get
			{
				return faceField;
			}
			set
			{
				faceField = value;
			}
		}

		[XmlText(DataType = "IDREF")]
		public string Value
		{
			get
			{
				return valueField;
			}
			set
			{
				valueField = value;
			}
		}

		public fx_surface_init_from_common()
		{
			mipField = 0u;
			sliceField = 0u;
			faceField = fx_surface_face_enum.POSITIVE_X;
		}
	}
}
