using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	[XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class COLLADA
	{
		private asset assetField;

		private extra[] extraField;

		private object[] itemsField;

		private COLLADAScene sceneField;

		private VersionType versionField;

		private static Regex regex = new Regex("\\s+");

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

		[XmlElement("library_animation_clips", typeof(library_animation_clips))]
		[XmlElement("library_animations", typeof(library_animations))]
		[XmlElement("library_cameras", typeof(library_cameras))]
		[XmlElement("library_controllers", typeof(library_controllers))]
		[XmlElement("library_effects", typeof(library_effects))]
		[XmlElement("library_force_fields", typeof(library_force_fields))]
		[XmlElement("library_geometries", typeof(library_geometries))]
		[XmlElement("library_images", typeof(library_images))]
		[XmlElement("library_lights", typeof(library_lights))]
		[XmlElement("library_materials", typeof(library_materials))]
		[XmlElement("library_nodes", typeof(library_nodes))]
		[XmlElement("library_physics_materials", typeof(library_physics_materials))]
		[XmlElement("library_physics_models", typeof(library_physics_models))]
		[XmlElement("library_physics_scenes", typeof(library_physics_scenes))]
		[XmlElement("library_visual_scenes", typeof(library_visual_scenes))]
		public object[] Items
		{
			get
			{
				return itemsField;
			}
			set
			{
				itemsField = value;
			}
		}

		public COLLADAScene scene
		{
			get
			{
				return sceneField;
			}
			set
			{
				sceneField = value;
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
		public VersionType version
		{
			get
			{
				return versionField;
			}
			set
			{
				versionField = value;
			}
		}

		public static string ConvertFromArray<T>(IList<T> array)
		{
			if (array == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (typeof(T) == typeof(double))
			{
				for (int i = 0; i < array.Count; i++)
				{
					stringBuilder.Append(((double)(object)array[i]).ToString("0.000000", NumberFormatInfo.InvariantInfo));
					if (i + 1 < array.Count)
					{
						stringBuilder.Append(" ");
					}
				}
			}
			else
			{
				for (int j = 0; j < array.Count; j++)
				{
					stringBuilder.Append(Convert.ToString(array[j], NumberFormatInfo.InvariantInfo));
					if (j + 1 < array.Count)
					{
						stringBuilder.Append(" ");
					}
				}
			}
			return stringBuilder.ToString();
		}

		internal static string[] ConvertStringArray(string arrayStr)
		{
			string[] array = regex.Split(arrayStr.Trim());
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = array[i];
			}
			return array2;
		}

		internal static int[] ConvertIntArray(string arrayStr)
		{
			string[] array = regex.Split(arrayStr.Trim());
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}

		internal static double[] ConvertDoubleArray(string arrayStr)
		{
			string[] array = regex.Split(arrayStr.Trim());
			double[] array2 = new double[array.Length];
			try
			{
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = double.Parse(array[i], NumberStyles.Float, CultureInfo.InvariantCulture);
				}
				return array2;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				return array2;
			}
		}

		internal static bool[] ConvertBoolArray(string arrayStr)
		{
			string[] array = regex.Split(arrayStr.Trim());
			bool[] array2 = new bool[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = bool.Parse(array[i]);
			}
			return array2;
		}

		public static COLLADA Load(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open);
			try
			{
				return Load(fileStream);
			}
			finally
			{
				fileStream.Close();
			}
		}

		public static COLLADA Load(Stream stream)
		{
			StreamReader textReader = new StreamReader(stream);
			return (COLLADA)new XmlSerializer(typeof(COLLADA)).Deserialize(textReader);
		}

		public void Save(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Create);
			try
			{
				Save(fileStream);
			}
			finally
			{
				fileStream.Close();
			}
		}

		public void Save(Stream stream)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(COLLADA));
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlSerializer.Serialize(xmlTextWriter, this);
		}
	}
}
