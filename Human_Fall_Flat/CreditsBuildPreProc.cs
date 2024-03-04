using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsBuildPreProc : IProcessScene, IOrderedCallback
{
	public int callbackOrder => 101;

	public static void PreprocessCredits(Scene scene, RuntimeParams.SimpleExprEvaluator eval)
	{
		GameObject[] rootGameObjects = ((Scene)(ref scene)).GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			CreditsExperience[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<CreditsExperience>(true);
			foreach (CreditsExperience creditsExperience in componentsInChildren)
			{
				Apply(creditsExperience, ref creditsExperience.contentSA, eval, "UNITY_STANDALONE");
				Apply(creditsExperience, ref creditsExperience.contentPS, eval, "UNITY_PS4 && !CURVE_JPN");
				Apply(creditsExperience, ref creditsExperience.contentPSJP, eval, "UNITY_PS4 && CURVE_JPN");
				Apply(creditsExperience, ref creditsExperience.contentXB, eval, "UNITY_XBOXONE");
				Apply(creditsExperience, ref creditsExperience.contentSwitch, eval, "UNITY_SWITCH && !CURVE_JPN");
				Apply(creditsExperience, ref creditsExperience.contentSwitchJP, eval, "UNITY_SWITCH && CURVE_JPN");
				creditsExperience.ContentHash = 0u;
				if ((Object)(object)creditsExperience.ActiveContent != (Object)null)
				{
					creditsExperience.ContentHash = (uint)(Animator.StringToHash(creditsExperience.ActiveContent.get_text()) + 1) & 0xFFFFFFFFu;
				}
			}
		}
	}

	private static void Apply(CreditsExperience exp, ref TextAsset asset, RuntimeParams.SimpleExprEvaluator eval, string preprocexpr)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)asset != (Object)null)
		{
			if (eval.Evaluate(preprocexpr))
			{
				TextAsset val = Object.Instantiate<TextAsset>(asset);
				((Object)val).set_hideFlags((HideFlags)(((Object)val).get_hideFlags() | 4));
				exp.ActiveContent = val;
			}
			asset = null;
		}
	}

	public void OnProcessScene(Scene scene)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		RuntimeParams.SimpleExprEvaluator eval = new RuntimeParams.SimpleExprEvaluator(EditorUserBuildSettings.get_selectedBuildTargetGroup(), editor: false);
		PreprocessCredits(scene, eval);
	}

	public static void PlayInEditor()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Scene activeScene = SceneManager.GetActiveScene();
		RuntimeParams.SimpleExprEvaluator eval = new RuntimeParams.SimpleExprEvaluator((BuildTargetGroup)1, editor: true);
		PreprocessCredits(activeScene, eval);
	}
}
