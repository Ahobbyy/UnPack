using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour2 : MonoBehaviour
{
	public Rigidbody[] m_Tail;

	public float m_Strength = 10f;

	public float m_ShortFlop = 4f;

	public float m_LongFlop = 4f;

	public SharkState2 m_State = SharkState2.Flopping;

	private float m_nextFlop;

	public Vector3 dir;

	private Vector3 newDir;

	private Rigidbody rb;

	[SerializeField]
	private Rigidbody noseRigidbody;

	[SerializeField]
	private FishBox fishBox;

	[SerializeField]
	private List<Transform> sharkTargets;

	private int sharkTarget;

	public float duration = 20f;

	private void Start()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		if (m_State == SharkState2.Water)
		{
			rb.set_useGravity(false);
			Rigidbody[] tail = m_Tail;
			for (int i = 0; i < tail.Length; i++)
			{
				tail[i].set_useGravity(false);
			}
		}
		else if (m_nextFlop < Time.get_time())
		{
			((MonoBehaviour)this).StartCoroutine(Flop());
			m_nextFlop += ((Random.get_value() < 0.95f) ? m_ShortFlop : m_LongFlop) * Random.Range(0.5f, 1.5f) + duration * 2f * Time.get_fixedDeltaTime();
		}
		if (Vector3.Distance(((Component)this).get_transform().get_position(), sharkTargets[0].get_position()) < 14f && m_State != SharkState2.BreakingTank && fishBox.hasFishOnIt)
		{
			m_State = SharkState2.BreakingTank;
		}
		if (m_State != SharkState2.BreakingTank)
		{
			return;
		}
		noseRigidbody.AddForce((sharkTargets[sharkTarget].get_position() - ((Component)this).get_transform().get_position()) * 300f);
		if (Vector3.Distance(((Component)this).get_transform().get_position(), sharkTargets[sharkTarget].get_position()) < 3f)
		{
			if (sharkTarget < sharkTargets.Count - 1)
			{
				sharkTarget++;
			}
			else
			{
				m_State = SharkState2.Idle;
			}
		}
	}

	private IEnumerator Flop()
	{
		float strength2 = m_Strength * Mathf.Sign((float)Random.Range(-1, 1));
		for (int j = 0; (float)j < duration; j++)
		{
			Rigidbody[] tail = m_Tail;
			foreach (Rigidbody obj in tail)
			{
				((Component)this).get_transform().InverseTransformDirection(dir);
				obj.AddRelativeTorque(dir * strength2, (ForceMode)1);
			}
			yield return (object)new WaitForFixedUpdate();
		}
		strength2 *= -0.5f;
		for (int j = 0; (float)j < duration; j++)
		{
			Rigidbody[] tail = m_Tail;
			foreach (Rigidbody obj2 in tail)
			{
				((Component)this).get_transform().InverseTransformDirection(dir);
				obj2.AddRelativeTorque(dir * strength2, (ForceMode)1);
			}
			yield return (object)new WaitForFixedUpdate();
		}
		yield return null;
	}

	public SharkBehaviour2()
		: this()
	{
	}
}
