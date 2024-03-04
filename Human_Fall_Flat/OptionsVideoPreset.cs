public struct OptionsVideoPreset
{
	public int preset;

	public int clouds;

	public int shadows;

	public int texture;

	public int ao;

	public int aa;

	public int vsync;

	public int hdr;

	public int bloom;

	public int dof;

	public int chroma;

	public int exposure;

	public static OptionsVideoPreset Create(int qualityLevel)
	{
		OptionsVideoPreset result;
		switch (qualityLevel)
		{
		case 1:
			result = default(OptionsVideoPreset);
			result.preset = 1;
			result.clouds = 0;
			result.shadows = 0;
			result.texture = 0;
			result.ao = 0;
			result.aa = 0;
			result.vsync = 0;
			result.hdr = 0;
			result.bloom = 0;
			result.dof = 0;
			result.chroma = 0;
			result.exposure = 0;
			return result;
		case 2:
			result = default(OptionsVideoPreset);
			result.preset = 2;
			result.clouds = 1;
			result.shadows = 1;
			result.texture = 1;
			result.ao = 0;
			result.aa = 0;
			result.vsync = 0;
			result.hdr = 0;
			result.bloom = 0;
			result.dof = 0;
			result.chroma = 0;
			result.exposure = 0;
			return result;
		case 3:
			result = default(OptionsVideoPreset);
			result.preset = 3;
			result.clouds = 2;
			result.shadows = 2;
			result.texture = 2;
			result.ao = 0;
			result.aa = 0;
			result.vsync = 1;
			result.hdr = 1;
			result.bloom = 0;
			result.dof = 0;
			result.chroma = 0;
			result.exposure = 0;
			return result;
		case 4:
			result = default(OptionsVideoPreset);
			result.preset = 4;
			result.clouds = 3;
			result.shadows = 3;
			result.texture = 2;
			result.ao = 2;
			result.aa = 2;
			result.vsync = 1;
			result.hdr = 1;
			result.bloom = 1;
			result.dof = 0;
			result.chroma = 1;
			result.exposure = 1;
			return result;
		case 5:
			result = default(OptionsVideoPreset);
			result.preset = 5;
			result.clouds = 4;
			result.shadows = 4;
			result.texture = 2;
			result.ao = 1;
			result.aa = 3;
			result.vsync = 1;
			result.hdr = 1;
			result.bloom = 1;
			result.dof = 1;
			result.chroma = 1;
			result.exposure = 1;
			return result;
		default:
			result = default(OptionsVideoPreset);
			return result;
		}
	}
}
