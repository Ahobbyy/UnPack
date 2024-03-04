using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	[DisallowMultipleComponent]
	public class TMP_SpriteAnimator : MonoBehaviour
	{
		private Dictionary<int, bool> m_animations = new Dictionary<int, bool>(16);

		private TMP_Text m_TextComponent;

		private void Awake()
		{
			m_TextComponent = ((Component)this).GetComponent<TMP_Text>();
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void StopAllAnimations()
		{
			((MonoBehaviour)this).StopAllCoroutines();
			m_animations.Clear();
		}

		public void DoSpriteAnimation(int currentCharacter, TMP_SpriteAsset spriteAsset, int start, int end, int framerate)
		{
			bool value = false;
			if (!m_animations.TryGetValue(currentCharacter, out value))
			{
				((MonoBehaviour)this).StartCoroutine(DoSpriteAnimationInternal(currentCharacter, spriteAsset, start, end, framerate));
				m_animations.Add(currentCharacter, value: true);
			}
		}

		private IEnumerator DoSpriteAnimationInternal(int currentCharacter, TMP_SpriteAsset spriteAsset, int start, int end, int framerate)
		{
			if ((Object)(object)m_TextComponent == (Object)null)
			{
				yield break;
			}
			yield return null;
			int currentFrame = start;
			if (end > spriteAsset.spriteInfoList.Count)
			{
				end = spriteAsset.spriteInfoList.Count - 1;
			}
			TMP_CharacterInfo charInfo = m_TextComponent.textInfo.characterInfo[currentCharacter];
			int materialIndex = charInfo.materialReferenceIndex;
			int vertexIndex = charInfo.vertexIndex;
			TMP_MeshInfo meshInfo = m_TextComponent.textInfo.meshInfo[materialIndex];
			float elapsedTime = 0f;
			float targetTime = 1f / (float)Mathf.Abs(framerate);
			Vector2 val = default(Vector2);
			Vector3 val2 = default(Vector3);
			Vector3 val3 = default(Vector3);
			Vector3 val4 = default(Vector3);
			Vector3 val5 = default(Vector3);
			Vector2 val6 = default(Vector2);
			Vector2 val7 = default(Vector2);
			Vector2 val8 = default(Vector2);
			Vector2 val9 = default(Vector2);
			while (true)
			{
				if (elapsedTime > targetTime)
				{
					elapsedTime = 0f;
					TMP_Sprite tMP_Sprite = spriteAsset.spriteInfoList[currentFrame];
					Vector3[] vertices = meshInfo.vertices;
					((Vector2)(ref val))._002Ector(charInfo.origin, charInfo.baseLine);
					float num = charInfo.fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * charInfo.scale;
					((Vector3)(ref val2))._002Ector(val.x + tMP_Sprite.xOffset * num, val.y + (tMP_Sprite.yOffset - tMP_Sprite.height) * num);
					((Vector3)(ref val3))._002Ector(val2.x, val.y + tMP_Sprite.yOffset * num);
					((Vector3)(ref val4))._002Ector(val.x + (tMP_Sprite.xOffset + tMP_Sprite.width) * num, val3.y);
					((Vector3)(ref val5))._002Ector(val4.x, val2.y);
					vertices[vertexIndex] = val2;
					vertices[vertexIndex + 1] = val3;
					vertices[vertexIndex + 2] = val4;
					vertices[vertexIndex + 3] = val5;
					Vector2[] uvs = meshInfo.uvs0;
					((Vector2)(ref val6))._002Ector(tMP_Sprite.x / (float)spriteAsset.spriteSheet.get_width(), tMP_Sprite.y / (float)spriteAsset.spriteSheet.get_height());
					((Vector2)(ref val7))._002Ector(val6.x, (tMP_Sprite.y + tMP_Sprite.height) / (float)spriteAsset.spriteSheet.get_height());
					((Vector2)(ref val8))._002Ector((tMP_Sprite.x + tMP_Sprite.width) / (float)spriteAsset.spriteSheet.get_width(), val7.y);
					((Vector2)(ref val9))._002Ector(val8.x, val6.y);
					uvs[vertexIndex] = val6;
					uvs[vertexIndex + 1] = val7;
					uvs[vertexIndex + 2] = val8;
					uvs[vertexIndex + 3] = val9;
					meshInfo.mesh.set_vertices(vertices);
					meshInfo.mesh.set_uv(uvs);
					m_TextComponent.UpdateGeometry(meshInfo.mesh, materialIndex);
					currentFrame = ((framerate > 0) ? ((currentFrame >= end) ? start : (currentFrame + 1)) : ((currentFrame <= start) ? end : (currentFrame - 1)));
				}
				elapsedTime += Time.get_deltaTime();
				yield return null;
			}
		}

		public TMP_SpriteAnimator()
			: this()
		{
		}
	}
}
