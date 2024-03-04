using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class Music : MonoBehaviour
	{
		public string song;

		public Music mainPlayer;

		public float restartInMinutes = 10f;

		public bool overrideShuffle;

		private bool playOnLoad;

		protected float lastPlayTime = float.MinValue;

		protected static Music currentMusic;

		public void Trigger()
		{
			if ((App.state == AppSate.PlayLevel || App.state == AppSate.ServerPlayLevel || App.state == AppSate.ClientPlayLevel) && Time.get_time() - lastPlayTime > restartInMinutes * 60f)
			{
				lastPlayTime = Time.get_time();
				PlayMusic();
			}
		}

		public void PlayMusic()
		{
			if ((!MusicManager.instance.shuffle || overrideShuffle) && (!MusicManager.instance.shuffle || !((Object)(object)currentMusic == (Object)(object)this)))
			{
				currentMusic = this;
				MusicManager.instance.PlayTriggeredMusic(song);
			}
		}

		public void StopMusic()
		{
			MusicManager.instance.Stop();
		}

		protected virtual void Update()
		{
			if ((App.state == AppSate.PlayLevel || App.state == AppSate.ServerPlayLevel || App.state == AppSate.ClientPlayLevel) && !MusicManager.instance.shuffle && (Object)(object)currentMusic == (Object)(object)this && Time.get_time() - lastPlayTime > restartInMinutes * 60f)
			{
				lastPlayTime = Time.get_time();
				PlayMusic();
			}
		}

		public Music()
			: this()
		{
		}
	}
}
