using UnityEngine;

public class DumpsterTracker : MonoBehaviour, IReset
{
	private Vector3 startPos;

	private Collider trackedHuman;

	public void OnTriggerEnter(Collider other)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)other).get_tag() == "Player")
		{
			trackedHuman = other;
			startPos = ((Component)this).get_transform().get_position();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)other == (Object)(object)trackedHuman)
		{
			trackedHuman = null;
			Vector2 val = (startPos - ((Component)this).get_transform().get_position()).To2D();
			StatsAndAchievements.AddDumpster(((Vector2)(ref val)).get_magnitude());
		}
	}

	public void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)trackedHuman != (Object)null)
		{
			Vector2 val = (startPos - ((Component)this).get_transform().get_position()).To2D();
			float magnitude = ((Vector2)(ref val)).get_magnitude();
			if (magnitude > 2f)
			{
				StatsAndAchievements.AddDumpster(magnitude);
				startPos = ((Component)this).get_transform().get_position();
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		trackedHuman = null;
	}

	public DumpsterTracker()
		: this()
	{
	}
}
