using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class CreditsBlock : MonoBehaviour, INetBehavior
{
	private struct Character
	{
		internal char c;

		internal Vector3 spawnPos;

		internal float size;
	}

	public float space = 1f;

	public float lineHeight = 3f;

	[NonSerialized]
	public float lineY;

	[NonSerialized]
	public int bitsRequired;

	[NonSerialized]
	public NetIdentity blockId;

	[NonSerialized]
	public CreditsExperience.BlockBucket bucket;

	private CreditsText ctext;

	private Vector3 centerOffset;

	private int activeLetters;

	private List<Character> characters = new List<Character>();

	public List<CreditsLetter> letters = new List<CreditsLetter>();

	private static NetVector3Encoder posEncoder = new NetVector3Encoder(8000f, 22, 4, 10);

	private static NetQuaternionEncoder rotEncoder = new NetQuaternionEncoder(9, 4, 6);

	private static NetVector3Encoder posEncoderBlock = new NetVector3Encoder(500f, 18, 4, 8);

	private static NetQuaternionEncoder rotEncoderBlock = new NetQuaternionEncoder(16, 6, 10);

	public string text { get; protected set; }

	public bool isSpawned { get; protected set; }

	public int DebugCountRunning()
	{
		int num = 0;
		for (int num2 = letters.Count - 1; num2 >= 0; num2--)
		{
			if ((Object)(object)letters[num2] != (Object)null)
			{
				num++;
			}
		}
		return num;
	}

	public int DebugNumLetters()
	{
		return letters.Count;
	}

	public void Initialize(float lineY, string text, CreditsText ctext)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		this.lineY = lineY;
		this.text = text;
		this.ctext = ctext;
		((Component)this).get_gameObject().SetActive(false);
		isSpawned = false;
		Bounds val = default(Bounds);
		float num = 1f;
		Vector3 val2 = default(Vector3);
		((Vector3)(ref val2))._002Ector(1f, 0f, 0f);
		Vector3 val3 = default(Vector3);
		((Vector3)(ref val3))._002Ector(0f, 0f, -1f);
		Vector3 val4 = Vector3.get_zero();
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (c == '[' && text[i + 1] == 'H' && text[i + 3] == ']')
			{
				switch (text[i + 2])
				{
				case '1':
					num = 2f;
					break;
				case '2':
					num = 1.5f;
					break;
				case '3':
					num = 0.7f;
					break;
				case '4':
					num = 0.5f;
					break;
				}
				val4 += val3 * lineHeight * (num - 1f);
				i += 3;
				continue;
			}
			switch (c)
			{
			case ' ':
				val4 += val2 * space * num;
				break;
			case '\n':
				val4.x = 0f;
				val4 += val3 * lineHeight;
				num = 1f;
				break;
			default:
			{
				Character character = default(Character);
				character.c = c;
				character.spawnPos = val4;
				character.size = num;
				Character item = character;
				val4 += val2 * (ctext.GetLetterWidth(c) + 0.1f) * num;
				characters.Add(item);
				letters.Add(null);
				break;
			}
			case '\r':
				break;
			}
			((Bounds)(ref val)).Encapsulate(val4);
		}
		centerOffset = -((Bounds)(ref val)).get_center();
		bitsRequired = CalculateMaxDeltaSizeInBits();
	}

	public void SpawnLetters(Vector3 pos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		isSpawned = true;
		((Component)this).get_gameObject().SetActive(true);
		((Component)this).get_transform().set_position(pos);
		for (int i = 0; i < characters.Count; i++)
		{
			SpawnLetter(i);
		}
	}

	internal void DespawnAll()
	{
		if (!isSpawned)
		{
			return;
		}
		for (int num = letters.Count - 1; num >= 0; num--)
		{
			if ((Object)(object)letters[num] != (Object)null)
			{
				DespawnLetter(num);
			}
		}
		CheckEmpty();
	}

	internal void Despawn(float v)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		for (int num = letters.Count - 1; num >= 0; num--)
		{
			if ((Object)(object)letters[num] != (Object)null && ((Component)letters[num]).get_transform().get_position().y > v)
			{
				DespawnLetter(num);
			}
		}
		CheckEmpty();
	}

	public void Scroll(Vector3 offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)this).get_transform();
		transform.set_position(transform.get_position() + offset);
	}

	private void SpawnLetter(int idx)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)letters[idx] != (Object)null))
		{
			Character character = characters[idx];
			CreditsLetter letter = ctext.GetLetter(character.c);
			((Component)letter).get_transform().SetParent(((Component)this).get_transform(), false);
			((Component)letter).get_transform().set_localPosition(character.spawnPos);
			((Component)letter).get_transform().set_localScale(new Vector3(character.size, character.size, character.size));
			letter.Attach(this, centerOffset);
			letters[idx] = letter;
			activeLetters++;
		}
	}

	private void DespawnLetter(int idx)
	{
		if (!((Object)(object)letters[idx] == (Object)null))
		{
			ctext.ReleaseLetter(letters[idx]);
			letters[idx] = null;
			activeLetters--;
		}
	}

	private void CheckEmpty()
	{
		if (activeLetters == 0)
		{
			((Component)this).get_gameObject().SetActive(false);
			isSpawned = false;
		}
	}

	public void CollectState(NetStream stream)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		stream.Write(isSpawned);
		if (isSpawned)
		{
			posEncoderBlock.CollectState(stream, ((Component)this).get_transform().get_position());
			rotEncoderBlock.CollectState(stream, ((Component)this).get_transform().get_rotation());
			for (int i = 0; i < letters.Count; i++)
			{
				CreditsLetter creditsLetter = letters[i];
				if ((Object)(object)creditsLetter == (Object)null)
				{
					stream.Write(v: false);
					continue;
				}
				stream.Write(v: true);
				posEncoder.CollectState(stream, ((Component)creditsLetter).get_transform().get_localPosition() - characters[i].spawnPos);
				rotEncoder.CollectState(stream, ((Component)creditsLetter).get_transform().get_localRotation());
			}
		}
		bucket.Collecting();
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		bool flag = state0?.ReadBool() ?? false;
		bool flag2 = state1.ReadBool();
		if (flag2 != isSpawned)
		{
			if (flag2)
			{
				SpawnLetters(Vector3.get_zero());
			}
			else
			{
				DespawnAll();
			}
		}
		if (flag && flag2)
		{
			((Component)this).get_transform().set_position(posEncoderBlock.ApplyLerpedState(state0, state1, mix).SetY(lineY));
			((Component)this).get_transform().set_rotation(rotEncoderBlock.ApplyLerpedState(state0, state1, mix));
		}
		else if (flag2)
		{
			((Component)this).get_transform().set_position(posEncoderBlock.ApplyState(state1).SetY(lineY));
			((Component)this).get_transform().set_rotation(rotEncoderBlock.ApplyState(state1));
		}
		else if (flag)
		{
			posEncoderBlock.ApplyState(state0);
			rotEncoderBlock.ApplyState(state0);
		}
		for (int i = 0; i < letters.Count; i++)
		{
			bool flag3 = flag && state0.ReadBool();
			bool flag4 = flag2 && state1.ReadBool();
			CreditsLetter creditsLetter = letters[i];
			if ((Object)(object)creditsLetter != (Object)null && !flag4)
			{
				DespawnLetter(i);
				creditsLetter = null;
			}
			else if ((Object)(object)creditsLetter == (Object)null && flag4)
			{
				SpawnLetter(i);
				creditsLetter = letters[i];
			}
			if (flag3 && flag4)
			{
				((Component)creditsLetter).get_transform().set_localPosition(posEncoder.ApplyLerpedState(state0, state1, mix) + characters[i].spawnPos);
				((Component)creditsLetter).get_transform().set_localRotation(rotEncoder.ApplyLerpedState(state0, state1, mix));
			}
			else if (flag4)
			{
				((Component)creditsLetter).get_transform().set_localPosition(posEncoder.ApplyState(state1) + characters[i].spawnPos);
				((Component)creditsLetter).get_transform().set_localRotation(rotEncoder.ApplyState(state1));
			}
			else if (flag3)
			{
				posEncoder.ApplyState(state0);
				rotEncoder.ApplyState(state0);
			}
		}
	}

	public void ApplyState(NetStream state)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		bool flag = state.ReadBool();
		if (flag != isSpawned)
		{
			if (flag)
			{
				SpawnLetters(Vector3.get_zero());
			}
			else
			{
				DespawnAll();
			}
		}
		if (!flag)
		{
			return;
		}
		((Component)this).get_transform().set_position(posEncoderBlock.ApplyState(state).SetY(lineY));
		((Component)this).get_transform().set_rotation(rotEncoderBlock.ApplyState(state));
		for (int i = 0; i < letters.Count; i++)
		{
			bool flag2 = state.ReadBool();
			CreditsLetter creditsLetter = letters[i];
			if ((Object)(object)creditsLetter != (Object)null && !flag2)
			{
				DespawnLetter(i);
				creditsLetter = null;
			}
			else if ((Object)(object)creditsLetter == (Object)null && flag2)
			{
				SpawnLetter(i);
				creditsLetter = letters[i];
			}
			if (flag2)
			{
				((Component)creditsLetter).get_transform().set_localPosition(posEncoder.ApplyState(state) + characters[i].spawnPos);
				((Component)creditsLetter).get_transform().set_localRotation(rotEncoder.ApplyState(state));
			}
		}
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		bool flag = state0?.ReadBool() ?? false;
		bool flag2 = state1.ReadBool();
		delta.Write(flag2);
		if (!flag)
		{
			state0 = null;
		}
		if (flag2)
		{
			posEncoderBlock.CalculateDelta(state0, state1, delta);
			rotEncoderBlock.CalculateDelta(state0, state1, delta);
			for (int i = 0; i < letters.Count; i++)
			{
				bool flag3 = flag && state0.ReadBool();
				bool flag4 = flag2 && state1.ReadBool();
				delta.Write(flag4);
				if (flag4)
				{
					NetStream state2 = (flag3 ? state0 : null);
					posEncoder.CalculateDelta(state2, state1, delta);
					rotEncoder.CalculateDelta(state2, state1, delta);
				}
				else if (flag3)
				{
					posEncoder.ApplyState(state0);
					rotEncoder.ApplyState(state0);
				}
			}
		}
		else
		{
			if (!flag)
			{
				return;
			}
			posEncoderBlock.ApplyState(state0);
			rotEncoderBlock.ApplyState(state0);
			for (int j = 0; j < letters.Count; j++)
			{
				if (state0.ReadBool())
				{
					posEncoder.ApplyState(state0);
					rotEncoder.ApplyState(state0);
				}
			}
		}
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		bool flag = state0?.ReadBool() ?? false;
		bool flag2 = delta.ReadBool();
		result.Write(flag2);
		if (!flag)
		{
			state0 = null;
		}
		if (flag2)
		{
			posEncoderBlock.AddDelta(state0, delta, result);
			rotEncoderBlock.AddDelta(state0, delta, result);
			for (int i = 0; i < letters.Count; i++)
			{
				bool flag3 = flag && state0.ReadBool();
				bool flag4 = flag2 && delta.ReadBool();
				result.Write(flag4);
				if (flag4)
				{
					NetStream state = (flag3 ? state0 : null);
					posEncoder.AddDelta(state, delta, result);
					rotEncoder.AddDelta(state, delta, result);
				}
				else if (flag3)
				{
					posEncoder.ApplyState(state0);
					rotEncoder.ApplyState(state0);
				}
			}
		}
		else
		{
			if (!flag)
			{
				return;
			}
			posEncoderBlock.ApplyState(state0);
			rotEncoderBlock.ApplyState(state0);
			for (int j = 0; j < letters.Count; j++)
			{
				if (state0.ReadBool())
				{
					posEncoder.ApplyState(state0);
					rotEncoder.ApplyState(state0);
				}
			}
		}
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return (1 + posEncoder.CalculateMaxDeltaSizeInBits() + 1 + rotEncoder.CalculateMaxDeltaSizeInBits() + 1) * characters.Count + posEncoderBlock.CalculateMaxDeltaSizeInBits() + 1 + rotEncoderBlock.CalculateMaxDeltaSizeInBits() + 1 + 1;
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public CreditsBlock()
		: this()
	{
	}
}
