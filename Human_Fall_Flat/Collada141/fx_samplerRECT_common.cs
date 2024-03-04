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
	public class fx_samplerRECT_common
	{
		private string border_colorField;

		private extra[] extraField;

		private fx_sampler_filter_common magfilterField;

		private fx_sampler_filter_common minfilterField;

		private fx_sampler_filter_common mipfilterField;

		private float mipmap_biasField;

		private byte mipmap_maxlevelField;

		private string sourceField;

		private fx_sampler_wrap_common wrap_sField;

		private fx_sampler_wrap_common wrap_tField;

		[XmlElement(DataType = "NCName")]
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

		[DefaultValue(fx_sampler_wrap_common.WRAP)]
		public fx_sampler_wrap_common wrap_s
		{
			get
			{
				return wrap_sField;
			}
			set
			{
				wrap_sField = value;
			}
		}

		[DefaultValue(fx_sampler_wrap_common.WRAP)]
		public fx_sampler_wrap_common wrap_t
		{
			get
			{
				return wrap_tField;
			}
			set
			{
				wrap_tField = value;
			}
		}

		[DefaultValue(fx_sampler_filter_common.NONE)]
		public fx_sampler_filter_common minfilter
		{
			get
			{
				return minfilterField;
			}
			set
			{
				minfilterField = value;
			}
		}

		[DefaultValue(fx_sampler_filter_common.NONE)]
		public fx_sampler_filter_common magfilter
		{
			get
			{
				return magfilterField;
			}
			set
			{
				magfilterField = value;
			}
		}

		[DefaultValue(fx_sampler_filter_common.NONE)]
		public fx_sampler_filter_common mipfilter
		{
			get
			{
				return mipfilterField;
			}
			set
			{
				mipfilterField = value;
			}
		}

		public string border_color
		{
			get
			{
				return border_colorField;
			}
			set
			{
				border_colorField = value;
			}
		}

		[DefaultValue(typeof(byte), "255")]
		public byte mipmap_maxlevel
		{
			get
			{
				return mipmap_maxlevelField;
			}
			set
			{
				mipmap_maxlevelField = value;
			}
		}

		[DefaultValue(typeof(float), "0")]
		public float mipmap_bias
		{
			get
			{
				return mipmap_biasField;
			}
			set
			{
				mipmap_biasField = value;
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

		public fx_samplerRECT_common()
		{
			wrap_sField = fx_sampler_wrap_common.WRAP;
			wrap_tField = fx_sampler_wrap_common.WRAP;
			minfilterField = fx_sampler_filter_common.NONE;
			magfilterField = fx_sampler_filter_common.NONE;
			mipfilterField = fx_sampler_filter_common.NONE;
			mipmap_maxlevelField = byte.MaxValue;
			mipmap_biasField = 0f;
		}
	}
}
