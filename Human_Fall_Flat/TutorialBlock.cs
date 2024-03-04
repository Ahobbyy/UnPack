using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TutorialBlock : Node
{
	public float dismissTime = 1.5f;

	public int maxShowCount = 5;

	public float maxDisplayTime = 30f;

	public float activateNonsenseTime = 10f;

	public int activateEnterCount = 3;

	public float deactivateMeaningfulTime = 1f;

	public float nonsenseTimer;

	public float meaningfulTimer;

	public float entryCount;

	public bool show;

	public bool nonsenseDetectedExternally;

	private float displayStart = -1f;

	private string oldText = string.Empty;

	private static List<TutorialBlock> allBlocks = new List<TutorialBlock>();

	public bool playerInside;

	private float leaveTime;

	public void OnFail()
	{
		if (((Behaviour)this).get_enabled() && displayStart == -1f)
		{
			Show();
			show = true;
		}
	}

	public void OnSucceed()
	{
		Hide();
		((Component)this).get_gameObject().SetActive(false);
		show = false;
	}

	public void OnDestroy()
	{
		Hide();
	}

	public static void RefreshTextOnAllBlocks()
	{
		int i = 0;
		for (int count = allBlocks.Count; i < count; i++)
		{
			TutorialBlock tutorialBlock = allBlocks[i];
			if ((Object)(object)tutorialBlock != (Object)null && tutorialBlock.displayStart != -1f && SubtitleManager.instance.instructionText.text == tutorialBlock.oldText)
			{
				string tutorialText = TutorialRepository.instance.GetTutorialText(((Object)tutorialBlock).get_name());
				if (tutorialText != tutorialBlock.oldText)
				{
					tutorialBlock.oldText = tutorialText;
					SubtitleManager.instance.SetInstruction(tutorialText);
				}
			}
		}
	}

	private void Show()
	{
		if (maxShowCount != 0)
		{
			string instruction = (oldText = TutorialRepository.instance.GetTutorialText(((Object)this).get_name()));
			allBlocks.Add(this);
			SubtitleManager.instance.SetInstruction(instruction);
			if (displayStart == -1f)
			{
				displayStart = Time.get_time();
			}
		}
	}

	private void Hide()
	{
		allBlocks.Remove(this);
		if (displayStart != -1f)
		{
			maxShowCount--;
			float num = displayStart + dismissTime - Time.get_time();
			if (num <= 0f)
			{
				SubtitleManager.instance.ClearInstruction();
			}
			else
			{
				SubtitleManager.instance.ClearInstruction(num);
			}
			oldText = string.Empty;
			displayStart = -1f;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() != "Player")
		{
			return;
		}
		playerInside = true;
		if (show)
		{
			Show();
		}
		else if (Time.get_time() - leaveTime > 0.5f)
		{
			entryCount += 1f;
			if (entryCount >= (float)activateEnterCount && !IsPlayerActivityMakingSense())
			{
				OnFail();
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (!(((Component)other).get_tag() != "Player"))
		{
			playerInside = false;
			leaveTime = Time.get_time();
			Hide();
		}
	}

	public virtual bool IsPlayerActivityMakingSense()
	{
		return false;
	}

	public virtual bool CheckInstantSuccess(bool playerInside)
	{
		return false;
	}

	public void Update()
	{
		if (CheckInstantSuccess(playerInside))
		{
			OnSucceed();
		}
		else
		{
			if (!playerInside && !nonsenseDetectedExternally)
			{
				return;
			}
			if (displayStart != -1f && Time.get_time() - displayStart > maxDisplayTime)
			{
				OnSucceed();
			}
			else if (IsPlayerActivityMakingSense())
			{
				meaningfulTimer += Time.get_deltaTime();
				if (meaningfulTimer > deactivateMeaningfulTime)
				{
					OnSucceed();
				}
			}
			else
			{
				nonsenseTimer += Time.get_deltaTime();
				if (nonsenseTimer > activateNonsenseTime)
				{
					OnFail();
				}
			}
		}
	}

	public void ReportNonsense()
	{
		nonsenseDetectedExternally = true;
	}

	public void UnreportNonsense()
	{
		if (nonsenseDetectedExternally)
		{
			nonsenseDetectedExternally = false;
			if (!playerInside)
			{
				leaveTime = Time.get_time();
				Hide();
			}
		}
	}
}
