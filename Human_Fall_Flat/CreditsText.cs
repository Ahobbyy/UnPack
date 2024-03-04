using System.Collections.Generic;
using UnityEngine;

public class CreditsText : MonoBehaviour
{
	public static CreditsText instance;

	private Dictionary<char, List<CreditsLetter>> repository = new Dictionary<char, List<CreditsLetter>>();

	private Dictionary<char, CreditsLetter> masters = new Dictionary<char, CreditsLetter>();

	public CreditsBlock blockPrefab;

	private List<CreditsBlock> blockRepository = new List<CreditsBlock>();

	private void OnEnable()
	{
		instance = this;
		InitData();
	}

	public void OnDestroy()
	{
		instance = null;
	}

	public void InitData()
	{
		if (masters.Count > 0)
		{
			return;
		}
		CreditsLetter[] componentsInChildren = ((Component)this).GetComponentsInChildren<CreditsLetter>(true);
		foreach (CreditsLetter creditsLetter in componentsInChildren)
		{
			if (masters.ContainsKey(creditsLetter.character))
			{
				Debug.LogError((object)"Multiple instances of letter", (Object)(object)creditsLetter);
			}
			masters[creditsLetter.character] = creditsLetter;
			repository[creditsLetter.character] = new List<CreditsLetter> { creditsLetter };
			((Component)creditsLetter).get_gameObject().SetActive(false);
		}
		((Component)blockPrefab).get_gameObject().SetActive(false);
	}

	public float GetLetterWidth(char character)
	{
		if (masters.ContainsKey(character))
		{
			return masters[character].width;
		}
		return 0f;
	}

	public CreditsLetter GetLetter(char character)
	{
		if (!repository.ContainsKey(character))
		{
			Debug.LogError((object)("Letter missing: " + character));
			if (character == 'é')
			{
				character = 'e';
			}
		}
		List<CreditsLetter> list = repository[character];
		CreditsLetter creditsLetter = null;
		if (list.Count > 0)
		{
			creditsLetter = list[0];
			list.RemoveAt(0);
		}
		else
		{
			creditsLetter = Object.Instantiate<GameObject>(((Component)masters[character]).get_gameObject()).GetComponent<CreditsLetter>();
		}
		return creditsLetter;
	}

	public void ReleaseLetter(CreditsLetter letter)
	{
		repository[letter.character].Add(letter);
		((Component)letter).get_gameObject().SetActive(false);
		((Component)letter).GetComponent<Rigidbody>().set_isKinematic(true);
	}

	public CreditsText()
		: this()
	{
	}
}
