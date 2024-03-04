using ProBuilder2.Common;
using UnityEngine;

public class HighlightNearestFace : MonoBehaviour
{
	public float travel = 50f;

	public float speed = 0.2f;

	private pb_Object target;

	private pb_Face nearest;

	private void Start()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		target = pb_ShapeGenerator.PlaneGenerator(travel, travel, 25, 25, (Axis)2, false);
		target.SetFaceMaterial(target.get_faces(), pb_Constant.get_DefaultMaterial());
		((Component)target).get_transform().set_position(new Vector3(travel * 0.5f, 0f, travel * 0.5f));
		target.ToMesh();
		target.Refresh((RefreshMask)255);
		Camera main = Camera.get_main();
		((Component)main).get_transform().set_position(new Vector3(25f, 40f, 0f));
		((Component)main).get_transform().set_localRotation(Quaternion.Euler(new Vector3(65f, 0f, 0f)));
	}

	private void Update()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		float num = Time.get_time() * speed;
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(Mathf.PerlinNoise(num, num) * travel, 2f, Mathf.PerlinNoise(num + 1f, num + 1f) * travel);
		((Component)this).get_transform().set_position(position);
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogWarning((object)"Missing the ProBuilder Mesh target!");
			return;
		}
		Vector3 val = ((Component)target).get_transform().InverseTransformPoint(((Component)this).get_transform().get_position());
		if (nearest != null)
		{
			target.SetFaceColor(nearest, Color.get_white());
		}
		int num2 = target.get_faces().Length;
		float num3 = float.PositiveInfinity;
		nearest = target.get_faces()[0];
		for (int i = 0; i < num2; i++)
		{
			float num4 = Vector3.Distance(val, FaceCenter(target, target.get_faces()[i]));
			if (num4 < num3)
			{
				num3 = num4;
				nearest = target.get_faces()[i];
			}
		}
		target.SetFaceColor(nearest, Color.get_blue());
		target.RefreshColors();
	}

	private Vector3 FaceCenter(pb_Object pb, pb_Face face)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] vertices = pb.get_vertices();
		Vector3 zero = Vector3.get_zero();
		int[] distinctIndices = face.get_distinctIndices();
		foreach (int num in distinctIndices)
		{
			zero.x += vertices[num].x;
			zero.y += vertices[num].y;
			zero.z += vertices[num].z;
		}
		float num2 = face.get_distinctIndices().Length;
		zero.x /= num2;
		zero.y /= num2;
		zero.z /= num2;
		return zero;
	}

	public HighlightNearestFace()
		: this()
	{
	}
}
