using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	internal class DoCreatePostProcessProfile : EndNameEditAction
	{
		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			ProjectWindowUtil.ShowCreatedAsset((Object)(object)ProfileFactory.CreatePostProcessProfileAtPath(pathName));
		}

		public DoCreatePostProcessProfile()
			: this()
		{
		}
	}
}
