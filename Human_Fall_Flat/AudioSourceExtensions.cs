using UnityEditor;
using UnityEngine;

public static class AudioSourceExtensions
{
	[MenuItem("CONTEXT/AudioSource/No Brakes Setup")]
	public static void RalisticRolloff(MenuCommand command)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Undo.RecordObject(command.context, "AudioSource Realistic Setup");
		RealisticRolloff((AudioSource)command.context);
		EditorUtility.SetDirty(command.context);
	}

	public static void RealisticRolloff(this AudioSource AS)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		AnimationCurve val = new AnimationCurve((Keyframe[])(object)new Keyframe[3]
		{
			new Keyframe(AS.get_minDistance(), 1f),
			new Keyframe(AS.get_minDistance() + (AS.get_maxDistance() - AS.get_minDistance()) / 4f, 0.35f),
			new Keyframe(AS.get_maxDistance(), 0f)
		});
		AS.set_rolloffMode((AudioRolloffMode)2);
		val.SmoothTangents(1, 0.025f);
		AS.SetCustomCurve((AudioSourceCurveType)0, val);
		AS.set_dopplerLevel(0f);
		AS.set_spread(60f);
	}
}
