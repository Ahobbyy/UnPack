using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Multiplayer
{
	public class NetTransportUNet : NetTransport
	{
		private int serverConnectionId;

		private int hostId;

		private int reliableChannelId;

		private int unreliableChannelId;

		public override bool SupportsLobbyListings()
		{
			return false;
		}

		private int GetConnectionID(NetHost host)
		{
			return (int)host.connection;
		}

		public override bool ConnectionEquals(object connection, NetHost host)
		{
			return (int)connection == GetConnectionID(host);
		}

		public override string GetMyName()
		{
			return "";
		}

		public override void Init()
		{
			NetworkTransport.Init();
		}

		public override void StartServer(OnStartHostDelegate callback, object sessionArgs = null)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			Application.set_runInBackground(true);
			NetworkTransport.Init();
			ConnectionConfig val = new ConnectionConfig();
			reliableChannelId = val.AddChannel((QosType)3);
			unreliableChannelId = val.AddChannel((QosType)0);
			HostTopology val2 = new HostTopology(val, 4);
			hostId = NetworkTransport.AddHost(val2, 8888);
			if (hostId < 0)
			{
				callback("Server socket creation failed!");
			}
			else
			{
				callback(null);
			}
		}

		public override void JoinGame(object serverAddress, OnJoinGameDelegate callback)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			Application.set_runInBackground(true);
			NetworkTransport.Init();
			ConnectionConfig val = new ConnectionConfig();
			reliableChannelId = val.AddChannel((QosType)3);
			unreliableChannelId = val.AddChannel((QosType)0);
			HostTopology val2 = new HostTopology(val, 4);
			hostId = NetworkTransport.AddHost(val2);
			if (hostId < 0)
			{
				callback(null, "Server socket creation failed!");
				return;
			}
			byte b = default(byte);
			serverConnectionId = NetworkTransport.Connect(hostId, (string)serverAddress, 8888, 0, ref b);
			if (b != 0)
			{
				NetworkError val3 = (NetworkError)b;
				callback(null, "Error: " + ((object)(NetworkError)(ref val3)).ToString());
			}
			else
			{
				callback(serverConnectionId, null);
			}
		}

		public override void SendReliable(NetHost host, byte[] data, int len)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			byte b = default(byte);
			NetworkTransport.Send(hostId, GetConnectionID(host), reliableChannelId, data, len, ref b);
			if (b != 0)
			{
				NetworkError val = (NetworkError)b;
				Debug.LogError((object)("Error: " + ((object)(NetworkError)(ref val)).ToString()));
			}
		}

		public override void SendUnreliable(NetHost host, byte[] data, int len)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			byte b = default(byte);
			NetworkTransport.Send(hostId, GetConnectionID(host), unreliableChannelId, data, len, ref b);
			if (b != 0)
			{
				NetworkError val = (NetworkError)b;
				Debug.LogError((object)("Error: " + ((object)(NetworkError)(ref val)).ToString()));
			}
		}

		public override void OnUpdate()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected I4, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Invalid comparison between Unknown and I4
			NetworkEventType val = (NetworkEventType)0;
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int dataSize = default(int);
			byte b = default(byte);
			do
			{
				byte[] array = new byte[1024];
				val = NetworkTransport.Receive(ref num, ref num2, ref num3, array, array.Length, ref dataSize, ref b);
				switch ((int)val)
				{
				case 0:
					NetGame.instance.OnData(num2, array, -1, dataSize);
					break;
				case 1:
					NetGame.instance.OnConnect(num2);
					break;
				case 2:
					NetGame.instance.OnDisconnect(num2);
					break;
				}
			}
			while ((int)val != 3);
		}

		public override void StopServer()
		{
			throw new NotImplementedException();
		}

		public override void LeaveGame()
		{
			throw new NotImplementedException();
		}

		public override void StartThread()
		{
			throw new NotImplementedException();
		}

		public override void StopThread()
		{
			throw new NotImplementedException();
		}
	}
}
