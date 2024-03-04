using Multiplayer;
using UnityEngine;

public class CementBagSegment : MonoBehaviour
{
	private CementBag bag;

	private uint evtCollision;

	private NetIdentity identity;

	private void OnEnable()
	{
		bag = ((Component)this).GetComponentInParent<CementBag>();
	}

	public void OnCollisionEnter(Collision collision)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			CementBag cementBag = bag;
			Vector3 impulse = collision.get_impulse();
			if (cementBag.ReportCollision(((Vector3)(ref impulse)).get_magnitude(), ((ContactPoint)(ref collision.get_contacts()[0])).get_point()) && (NetGame.isServer || ReplayRecorder.isRecording))
			{
				NetStream netStream = identity.BeginEvent(evtCollision);
				impulse = collision.get_impulse();
				int x = NetFloat.Quantize(((Vector3)(ref impulse)).get_magnitude(), 10000f, 11);
				netStream.Write(x, 10);
				NetVector3.Quantize(((ContactPoint)(ref collision.get_contacts()[0])).get_point() - ((Component)this).get_transform().get_position(), 100f, 10).Write(netStream, 3);
				identity.EndEvent();
			}
		}
	}

	public void OnCollisionStay(Collision collision)
	{
		OnCollisionEnter(collision);
	}

	public void Start()
	{
		identity = ((Component)this).GetComponentInParent<NetIdentity>();
		if ((Object)(object)identity != (Object)null)
		{
			evtCollision = identity.RegisterEvent(OnReportCollision);
		}
	}

	private void OnReportCollision(NetStream stream)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		float impulse = NetFloat.Dequantize(stream.ReadInt32(10), 10000f, 11);
		Vector3 val = NetVector3.Read(stream, 3, 10).Dequantize(100f);
		Vector3 pos = ((Component)this).get_transform().get_position() + val;
		bag.ReportCollision(impulse, pos);
	}

	public CementBagSegment()
		: this()
	{
	}
}
