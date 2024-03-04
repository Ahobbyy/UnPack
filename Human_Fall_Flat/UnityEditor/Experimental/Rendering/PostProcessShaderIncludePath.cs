using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.Rendering
{
	internal static class PostProcessShaderIncludePath
	{
		public static string[] GetPaths()
		{
			string text = Directory.GetFiles(Application.get_dataPath(), "POSTFXMARKER", SearchOption.AllDirectories).FirstOrDefault();
			string[] array = new string[(text == null) ? 1 : 2];
			int num = 0;
			if (text != null)
			{
				array[num] = Directory.GetParent(text).ToString();
				num++;
			}
			array[num] = Path.GetFullPath("Packages/com.unity.postprocessing");
			return array;
		}
	}
}
