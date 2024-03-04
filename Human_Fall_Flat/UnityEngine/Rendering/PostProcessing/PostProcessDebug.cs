using System;
using UnityEditor;

namespace UnityEngine.Rendering.PostProcessing
{
	[ExecuteInEditMode]
	[AddComponentMenu("Rendering/Post-process Debug", 1002)]
	public sealed class PostProcessDebug : MonoBehaviour
	{
		public PostProcessLayer postProcessLayer;

		private PostProcessLayer m_PreviousPostProcessLayer;

		public bool lightMeter;

		public bool histogram;

		public bool waveform;

		public bool vectorscope;

		public DebugOverlay debugOverlay;

		private Camera m_CurrentCamera;

		private CommandBuffer m_CmdAfterEverything;

		private void OnEnable()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			CommandBuffer val = new CommandBuffer();
			val.set_name("Post-processing Debug Overlay");
			m_CmdAfterEverything = val;
			EditorApplication.update = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(UpdateStates));
		}

		private void OnDisable()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			EditorApplication.update = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(UpdateStates));
			if ((Object)(object)m_CurrentCamera != (Object)null)
			{
				m_CurrentCamera.RemoveCommandBuffer((CameraEvent)19, m_CmdAfterEverything);
			}
			m_CurrentCamera = null;
			m_PreviousPostProcessLayer = null;
		}

		private void Reset()
		{
			postProcessLayer = ((Component)this).GetComponent<PostProcessLayer>();
		}

		private void UpdateStates()
		{
			if ((Object)(object)m_PreviousPostProcessLayer != (Object)(object)postProcessLayer)
			{
				if ((Object)(object)m_CurrentCamera != (Object)null)
				{
					m_CurrentCamera.RemoveCommandBuffer((CameraEvent)19, m_CmdAfterEverything);
					m_CurrentCamera = null;
				}
				m_PreviousPostProcessLayer = postProcessLayer;
				if ((Object)(object)postProcessLayer != (Object)null)
				{
					m_CurrentCamera = ((Component)postProcessLayer).GetComponent<Camera>();
					m_CurrentCamera.AddCommandBuffer((CameraEvent)19, m_CmdAfterEverything);
				}
			}
			if (!((Object)(object)postProcessLayer == (Object)null) && ((Behaviour)postProcessLayer).get_enabled())
			{
				if (lightMeter)
				{
					postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.LightMeter);
				}
				if (histogram)
				{
					postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Histogram);
				}
				if (waveform)
				{
					postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Waveform);
				}
				if (vectorscope)
				{
					postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Vectorscope);
				}
				postProcessLayer.debugLayer.RequestDebugOverlay(debugOverlay);
			}
		}

		private void OnPostRender()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			m_CmdAfterEverything.Clear();
			if (!((Object)(object)postProcessLayer == (Object)null) && ((Behaviour)postProcessLayer).get_enabled() && postProcessLayer.debugLayer.debugOverlayActive)
			{
				m_CmdAfterEverything.Blit((Texture)(object)postProcessLayer.debugLayer.debugOverlayTarget, RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2));
			}
		}

		private void OnGUI()
		{
			if (!((Object)(object)postProcessLayer == (Object)null) && ((Behaviour)postProcessLayer).get_enabled())
			{
				RenderTexture.set_active((RenderTexture)null);
				Rect rect = default(Rect);
				((Rect)(ref rect))._002Ector(5f, 5f, 0f, 0f);
				PostProcessDebugLayer debugLayer = postProcessLayer.debugLayer;
				DrawMonitor(ref rect, debugLayer.lightMeter, lightMeter);
				DrawMonitor(ref rect, debugLayer.histogram, histogram);
				DrawMonitor(ref rect, debugLayer.waveform, waveform);
				DrawMonitor(ref rect, debugLayer.vectorscope, vectorscope);
			}
		}

		private void DrawMonitor(ref Rect rect, Monitor monitor, bool enabled)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (enabled && !((Object)(object)monitor.output == (Object)null))
			{
				((Rect)(ref rect)).set_width((float)((Texture)monitor.output).get_width());
				((Rect)(ref rect)).set_height((float)((Texture)monitor.output).get_height());
				GUI.DrawTexture(rect, (Texture)(object)monitor.output);
				((Rect)(ref rect)).set_x(((Rect)(ref rect)).get_x() + ((float)((Texture)monitor.output).get_width() + 5f));
			}
		}

		public PostProcessDebug()
			: this()
		{
		}
	}
}
