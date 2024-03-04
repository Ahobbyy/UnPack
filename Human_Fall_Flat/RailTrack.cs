using UnityEngine;

public class RailTrack : MonoBehaviour
{
	public GameObject m_mineCart;

	public float m_derailedTime = 0.5f;

	public int m_minimumContacts = 3;

	public Collider m_centralRail;

	private float m_derailTimer;

	private int m_contactCount;

	private bool m_hasHadContact;

	private void Start()
	{
		ResetState();
	}

	private void LateUpdate()
	{
		if (m_contactCount >= m_minimumContacts)
		{
			m_derailTimer = 0f;
		}
		else if (m_hasHadContact)
		{
			m_derailTimer += Time.get_deltaTime();
			if (m_derailTimer > m_derailedTime)
			{
				m_centralRail.set_enabled(false);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.get_gameObject().get_transform().IsChildOf(m_mineCart.get_transform()))
		{
			m_hasHadContact = true;
			m_contactCount++;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.get_gameObject().get_transform().IsChildOf(m_mineCart.get_transform()))
		{
			m_contactCount--;
		}
	}

	public void OnCartRespawn()
	{
		ResetState();
	}

	private void ResetState()
	{
		m_hasHadContact = false;
		m_contactCount = 0;
		m_derailTimer = 0f;
		m_centralRail.set_enabled(true);
	}

	public RailTrack()
		: this()
	{
	}
}
