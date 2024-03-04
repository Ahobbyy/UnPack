using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HFFResources : MonoBehaviour
{
	public Texture2D[] SkinTextures;

	public Sprite[] LevelImages;

	public Sprite[] LobbyImages;

	public List<MultiplayerLobbyParams> LobbyParams;

	public TextAsset[] VideoTextAssets;

	public TMP_Settings TMPSettings;

	public TMP_StyleSheet StyleSheet;

	public TextAsset[] VideoSrts;

	public Texture[] VideoThumbs;

	public AudioClip[] Music;

	public TextAsset HFFMasterLocalisationFile;

	public Material PCLinearMovieFixMenu;

	public Material PCLinearMovieFixGame;

	public static HFFResources instance;

	public const string kLevelImages = "LevelImages";

	public const string kLobbyImages = "LobbyImages";

	public const string kSkinTextures = "SkinTextures";

	public const char kPathSeparator = '/';

	private int mNumberLevelImages;

	private int mNumberLobbyImages;

	private int mNumberSkinTextures;

	private int mNumberVideoSrts;

	private int mNumberVideoThumbs;

	private int mNumberMusic;

	private string[] mLevelNames;

	private string[] mLobbyNames;

	private string[] mSkinNames;

	private string[] mVideoSrtNames;

	private string[] mVideoThumbsNames;

	private string[] mMusicNames;

	private List<string> skinMasksProcessed = new List<string>();

	private void BuildNameArray(out string[] nameArray, Object[] objects, int numberObjects)
	{
		nameArray = new string[numberObjects];
		for (int i = 0; i < numberObjects; i++)
		{
			if (objects[i] != (Object)null)
			{
				nameArray[i] = objects[i].get_name();
			}
		}
	}

	private void Awake()
	{
		instance = this;
		mNumberLevelImages = LevelImages.Length;
		mNumberLobbyImages = LobbyImages.Length;
		mNumberSkinTextures = SkinTextures.Length;
		mNumberVideoSrts = VideoSrts.Length;
		mNumberVideoThumbs = VideoThumbs.Length;
		mNumberMusic = Music.Length;
		ref string[] nameArray = ref mLevelNames;
		Object[] levelImages = (Object[])(object)LevelImages;
		BuildNameArray(out nameArray, levelImages, mNumberLevelImages);
		ref string[] nameArray2 = ref mLobbyNames;
		levelImages = (Object[])(object)LobbyImages;
		BuildNameArray(out nameArray2, levelImages, mNumberLobbyImages);
		ref string[] nameArray3 = ref mSkinNames;
		levelImages = (Object[])(object)SkinTextures;
		BuildNameArray(out nameArray3, levelImages, mNumberSkinTextures);
		ref string[] nameArray4 = ref mVideoSrtNames;
		levelImages = (Object[])(object)VideoSrts;
		BuildNameArray(out nameArray4, levelImages, mNumberVideoSrts);
		ref string[] nameArray5 = ref mVideoThumbsNames;
		levelImages = (Object[])(object)VideoThumbs;
		BuildNameArray(out nameArray5, levelImages, mNumberVideoThumbs);
		ref string[] nameArray6 = ref mMusicNames;
		levelImages = (Object[])(object)Music;
		BuildNameArray(out nameArray6, levelImages, mNumberMusic);
		Object.DontDestroyOnLoad((Object)(object)this);
	}

	private T FindResource<T>(string[] names, T[] objects, string name, int numberObjects) where T : class
	{
		for (int i = 0; i < numberObjects; i++)
		{
			if (string.Equals(names[i], name, StringComparison.OrdinalIgnoreCase))
			{
				return objects[i];
			}
		}
		return null;
	}

	private string GetName(string path)
	{
		int num = path.LastIndexOf('/');
		if (num != -1)
		{
			return path.Substring(num + 1);
		}
		return path;
	}

	public Texture2D FindTextureResource(string path)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		if (path.Contains("LevelImages"))
		{
			string name = GetName(path);
			for (int i = 0; i < mNumberLevelImages; i++)
			{
				if (string.Equals(mLevelNames[i], name, StringComparison.OrdinalIgnoreCase))
				{
					return LevelImages[i].get_texture();
				}
			}
		}
		if (path.Contains("LobbyImages"))
		{
			string name2 = GetName(path);
			for (int j = 0; j < mNumberLobbyImages; j++)
			{
				if (string.Equals(mLobbyNames[j], name2, StringComparison.OrdinalIgnoreCase))
				{
					return LobbyImages[j].get_texture();
				}
			}
		}
		if (path.Contains("SkinTextures"))
		{
			string name3 = GetName(path);
			Texture2D val = FindResource(mSkinNames, SkinTextures, name3, mNumberSkinTextures);
			if ((Object)(object)val != (Object)null && path.Contains("Mask") && !skinMasksProcessed.Contains(path) && (int)val.get_format() == 2)
			{
				Color32[] pixels = val.GetPixels32(0);
				int num = ((Texture)val).get_width() * ((Texture)val).get_height();
				skinMasksProcessed.Add(path);
				for (int k = 0; k < num; k++)
				{
					int num2 = pixels[k].r + pixels[k].g + pixels[k].b;
					if (pixels[k].r > 0)
					{
						pixels[k].r = byte.MaxValue;
					}
					if (pixels[k].g > 0)
					{
						pixels[k].g = byte.MaxValue;
					}
					if (pixels[k].b > 0)
					{
						pixels[k].b = byte.MaxValue;
					}
					if (num2 > 255)
					{
						num2 = 255;
					}
					pixels[k].a = (byte)num2;
				}
				val.SetPixels32(pixels);
				val.Apply(false, false);
			}
			return val;
		}
		return null;
	}

	public TextAsset GetVideoSrt(string path)
	{
		string name = GetName(path);
		return FindResource(mVideoSrtNames, VideoSrts, name, mNumberVideoSrts);
	}

	public Texture GetVideoThumb(string path)
	{
		string name = GetName(path);
		return FindResource(mVideoThumbsNames, VideoThumbs, name, mNumberVideoThumbs);
	}

	public AudioClip GetMusicTrack(string path)
	{
		string name = GetName(path);
		return FindResource(mMusicNames, Music, name, mNumberMusic);
	}

	public HFFResources()
		: this()
	{
	}
}
