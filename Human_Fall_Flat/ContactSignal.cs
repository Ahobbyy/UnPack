using System.Linq;
using HumanAPI;
using UnityEngine;

public class ContactSignal : Node
{
	public NodeOutput value;

	private int contacts;

	public bool OneShotOn;

	public GameObject[] contactObjects;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (contactObjects.Any((GameObject x) => collision.get_gameObject().get_transform().IsChildOf(x.get_transform())))
		{
			contacts++;
			value.SetValue(1f);
			Debug.Log((object)"OCEn");
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (contactObjects.Any((GameObject x) => collision.get_gameObject().get_transform().IsChildOf(x.get_transform())))
		{
			contacts--;
			if (contacts == 0 && !OneShotOn)
			{
				Debug.Log((object)"OCEx");
				value.SetValue(0f);
			}
		}
	}
}
