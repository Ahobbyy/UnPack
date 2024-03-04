using nn.util;

namespace nn.irsensor
{
	public struct ClusteringData
	{
		public float averageIntensity;

		public Float2 centroid;

		public int pixelCount;

		public Rect bound;

		public override string ToString()
		{
			return $"({averageIntensity} {centroid.ToString()} {pixelCount} {bound.ToString()})";
		}
	}
}
