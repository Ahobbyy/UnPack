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
	public class fx_surface_common
	{
		private extra[] extraField;

		private string formatField;

		private fx_surface_format_hint_common format_hintField;

		private object init_as_nullField;

		private object init_as_targetField;

		private fx_surface_init_cube_common init_cubeField;

		private fx_surface_init_from_common[] init_fromField;

		private fx_surface_init_planar_common init_planarField;

		private fx_surface_init_volume_common init_volumeField;

		private object itemField;

		private uint mip_levelsField;

		private bool mipmap_generateField;

		private bool mipmap_generateFieldSpecified;

		private fx_surface_type_enum typeField;

		public object init_as_null
		{
			get
			{
				return init_as_nullField;
			}
			set
			{
				init_as_nullField = value;
			}
		}

		public object init_as_target
		{
			get
			{
				return init_as_targetField;
			}
			set
			{
				init_as_targetField = value;
			}
		}

		public fx_surface_init_cube_common init_cube
		{
			get
			{
				return init_cubeField;
			}
			set
			{
				init_cubeField = value;
			}
		}

		public fx_surface_init_volume_common init_volume
		{
			get
			{
				return init_volumeField;
			}
			set
			{
				init_volumeField = value;
			}
		}

		public fx_surface_init_planar_common init_planar
		{
			get
			{
				return init_planarField;
			}
			set
			{
				init_planarField = value;
			}
		}

		[XmlElement("init_from")]
		public fx_surface_init_from_common[] init_from
		{
			get
			{
				return init_fromField;
			}
			set
			{
				init_fromField = value;
			}
		}

		[XmlElement(DataType = "token")]
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

		public fx_surface_format_hint_common format_hint
		{
			get
			{
				return format_hintField;
			}
			set
			{
				format_hintField = value;
			}
		}

		[XmlElement("size", typeof(long))]
		[XmlElement("viewport_ratio", typeof(double))]
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

		[DefaultValue(typeof(uint), "0")]
		public uint mip_levels
		{
			get
			{
				return mip_levelsField;
			}
			set
			{
				mip_levelsField = value;
			}
		}

		public bool mipmap_generate
		{
			get
			{
				return mipmap_generateField;
			}
			set
			{
				mipmap_generateField = value;
			}
		}

		[XmlIgnore]
		public bool mipmap_generateSpecified
		{
			get
			{
				return mipmap_generateFieldSpecified;
			}
			set
			{
				mipmap_generateFieldSpecified = value;
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
		public fx_surface_type_enum type
		{
			get
			{
				return typeField;
			}
			set
			{
				typeField = value;
			}
		}

		public fx_surface_common()
		{
			mip_levelsField = 0u;
		}
	}
}
