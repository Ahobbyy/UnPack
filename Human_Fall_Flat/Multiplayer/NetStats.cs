using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
	public class NetStats : MonoBehaviour
	{
		public NetGraph sendGraph;

		public NetGraph recvGraph;

		public NetGraph latencyGraph;

		public TextMeshProUGUI recvLabel;

		public TextMeshProUGUI sendLabel;

		public TextMeshProUGUI latencyLabel;

		private float recvSum;

		private float sendSum;

		private int frames;

		private int baseFrameId;

		private void Start()
		{
			Shell.RegisterCommand("netstats", OnNetStats, "netstats\r\nToggle netwok statistics display");
			((Component)((Component)this).get_transform().get_parent()).get_gameObject().SetActive(false);
		}

		private void OnNetStats()
		{
			((Component)((Component)this).get_transform().get_parent()).get_gameObject().SetActive(!((Component)((Component)this).get_transform().get_parent()).get_gameObject().get_activeSelf());
			if (((Component)((Component)this).get_transform().get_parent()).get_gameObject().get_activeSelf())
			{
				Shell.Print("netstats on");
			}
			else
			{
				Shell.Print("netstats off");
			}
		}

		private void Update()
		{
			frames++;
			sendSum = NetGame.instance.sendBps.kbps;
			recvSum = NetGame.instance.recvBps.kbps;
			if (frames == 4)
			{
				recvGraph.PushValue(recvSum);
				sendGraph.PushValue(sendSum);
				float max = sendGraph.GetMax();
				float max2 = recvGraph.GetMax();
				float range = Mathf.Max(max, max2);
				recvGraph.SetRange(range);
				sendGraph.SetRange(range);
				recvLabel.text = $"recv \t{recvSum:0.0}kbps \t{max2:0.0}kbps";
				sendLabel.text = $"send \t{sendSum:0.0}kbps \t{max:0.0}kbps";
				latencyLabel.text = $"buf \t {NetGame.instance.clientBuffer.latency:0.0}frames \tlag \t{NetGame.instance.clientLatency.latency * 1000f / 60f:0.0}ms";
				frames = 0;
				sendSum = (recvSum = 0f);
			}
		}

		private void OnGUI()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			if (Shell.visible)
			{
				GUILayout.BeginArea(new Rect(10f, (float)(Screen.get_height() / 2), (float)(Screen.get_width() - 20), (float)(Screen.get_height() / 2)));
			}
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_color(Color.get_black());
			GUIStyle label = GUI.get_skin().get_label();
			GUILayout.Label($"Send: {NetGame.instance.sendBps.kbps:00.0} kbps / Recv: {NetGame.instance.recvBps.kbps:00.0} kbps", label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			List<string> list = new List<string>();
			NetStream.DumpBufferStats(list);
			Color color = GUI.get_color();
			GUI.set_color(Color.get_cyan());
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				GUILayout.Label(list[i], label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			GUI.set_color(color);
			for (int j = 0; j < NetScope.all.Count; j++)
			{
				NetScope.all[j].RenderGUI(label);
			}
			GUILayout.EndVertical();
			if (Shell.visible)
			{
				GUILayout.EndArea();
			}
		}

		public NetStats()
			: this()
		{
		}
	}
}
