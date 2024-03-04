using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

public class LocalizeOnAwake : MonoBehaviour
{
	public string term;

	[SerializeField]
	public LocalizeEvent onLocalize;

	public void Awake()
	{
		Localize();
	}

	public void Localize()
	{
		((UnityEvent<string>)onLocalize).Invoke(ScriptLocalization.Get(term));
	}

	public LocalizeOnAwake()
		: this()
	{
	}
}
