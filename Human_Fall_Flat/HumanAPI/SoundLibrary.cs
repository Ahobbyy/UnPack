using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;

namespace HumanAPI
{
	public class SoundLibrary : MonoBehaviour
	{
		[Serializable]
		public class SerializedSampleLibrary
		{
			[NonSerialized]
			public SoundLibrary library;

			public List<SerializedSample> samples = new List<SerializedSample>();

			[NonSerialized]
			public Dictionary<string, SerializedSample> map = new Dictionary<string, SerializedSample>();

			[NonSerialized]
			public List<SampleCategory> categories = new List<SampleCategory>();

			[NonSerialized]
			public Dictionary<string, SampleCategory> categoryMap = new Dictionary<string, SampleCategory>();

			public SerializedSample GetSample(string name)
			{
				map.TryGetValue(name, out var value);
				return value;
			}

			public void AddSample(SerializedSample sample)
			{
				map[sample.name] = sample;
				samples.Add(sample);
				sample.library = this;
				if (!categoryMap.TryGetValue(sample.category, out var value))
				{
					value = new SampleCategory
					{
						name = sample.category
					};
					categoryMap[sample.category] = value;
					categories.Add(value);
				}
				value.samples.Add(sample);
			}

			public void ReloadSamples()
			{
				for (int i = 0; i < samples.Count; i++)
				{
					samples[i].CheckClipFileChange();
				}
			}

			public void MarkAllMissing()
			{
				for (int i = 0; i < samples.Count; i++)
				{
					SerializedSample serializedSample = samples[i];
					for (int j = 0; j < serializedSample.clips.Count; j++)
					{
						if (!serializedSample.clips[j].isEmbedded)
						{
							serializedSample.clips[j].missingFile = true;
						}
					}
				}
			}

			public void RemoveMissing()
			{
				map.Clear();
				Dictionary<string, SampleCategory> dictionary = categoryMap;
				categoryMap = new Dictionary<string, SampleCategory>();
				categories.Clear();
				for (int num = samples.Count - 1; num >= 0; num--)
				{
					SerializedSample serializedSample = samples[num];
					serializedSample.RemoveMissingCips();
					if (serializedSample.clips.Count == 0)
					{
						samples.RemoveAt(num);
					}
					else
					{
						map[serializedSample.name] = serializedSample;
						if (!categoryMap.TryGetValue(serializedSample.category, out var value))
						{
							if (dictionary.TryGetValue(serializedSample.category, out value))
							{
								value.samples.Clear();
							}
							else
							{
								value = new SampleCategory
								{
									name = serializedSample.category
								};
							}
							categoryMap[serializedSample.category] = value;
							categories.Add(value);
						}
						value.samples.Add(serializedSample);
					}
				}
			}
		}

		public class SampleCategory
		{
			public string name;

			public List<SerializedSample> samples = new List<SerializedSample>();
		}

		public interface IClip
		{
			string id { get; set; }
		}

		public interface IClipContainer : IClip
		{
			void SetChild(string localId, IClip container);

			SerializedClip GetClip(string currentId, char choice, SampleContainerChildType loopType);
		}

		public class RandomContainer : IClipContainer, IClip
		{
			private Dictionary<string, IClip> children = new Dictionary<string, IClip>();

			private List<IClip> list = new List<IClip>();

			public string id { get; set; }

			public void SetChild(string localId, IClip container)
			{
				if (children.TryGetValue(localId, out var value))
				{
					if (value == container)
					{
						return;
					}
					list.Remove(value);
				}
				children[localId] = container;
				list.Add(container);
			}

			public SerializedClip GetClip(string currentId, char choice, SampleContainerChildType loopType)
			{
				IClip clip = null;
				if (string.IsNullOrEmpty(currentId) || !currentId.StartsWith(id))
				{
					clip = list[Random.Range(0, list.Count)];
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (currentId.StartsWith(list[i].id))
						{
							clip = list[i];
							break;
						}
					}
				}
				if (clip is SerializedClip)
				{
					return clip as SerializedClip;
				}
				return (clip as IClipContainer).GetClip(currentId, choice, loopType);
			}

			public override string ToString()
			{
				return $"Random {id}";
			}
		}

		public class LoopContainer : IClipContainer, IClip
		{
			private IClip start;

			private IClip loop;

			private IClip stop;

			public string id { get; set; }

			public void SetChild(string localId, IClip container)
			{
				switch (SerializedSample.GetChildType(localId))
				{
				case SampleContainerChildType.Start:
					start = container;
					break;
				case SampleContainerChildType.Stop:
					stop = container;
					break;
				default:
					loop = container;
					break;
				}
			}

			public SerializedClip GetClip(string currentId, char choice, SampleContainerChildType loopType)
			{
				IClip clip = loopType switch
				{
					SampleContainerChildType.Start => start, 
					SampleContainerChildType.Stop => stop, 
					_ => loop, 
				};
				if (clip == null)
				{
					return null;
				}
				if (clip is SerializedClip)
				{
					return clip as SerializedClip;
				}
				return (clip as IClipContainer).GetClip(currentId, choice, loopType);
			}

			public override string ToString()
			{
				return $"Loop {id}";
			}
		}

		public class SwitchContainer : IClipContainer, IClip
		{
			private Dictionary<char, IClip> choices = new Dictionary<char, IClip>();

			public string id { get; set; }

			public void SetChild(string localId, IClip container)
			{
				choices[char.ToUpper(localId[0])] = container;
			}

			public SerializedClip GetClip(string currentId, char choice, SampleContainerChildType loopType)
			{
				IClip value = null;
				while (value == null && choice >= 'A')
				{
					choices.TryGetValue(choice--, out value);
				}
				if (value == null)
				{
					return null;
				}
				if (value is SerializedClip)
				{
					return value as SerializedClip;
				}
				return (value as IClipContainer).GetClip(currentId, choice, loopType);
			}

			public override string ToString()
			{
				return $"Switch {id}";
			}
		}

		public enum SampleContainerChildType
		{
			None,
			Random,
			Choice,
			Start,
			Loop,
			Stop
		}

		[Serializable]
		public class SerializedSample
		{
			[NonSerialized]
			public SerializedSampleLibrary library;

			public string name;

			public string category;

			public bool isSwitch;

			public bool isLoop;

			public float crossFade = 0.1f;

			public List<SerializedClip> clips;

			public float vB = 1f;

			public float vR;

			public float pB = 1f;

			public float pR;

			private Dictionary<string, IClipContainer> containers = new Dictionary<string, IClipContainer>();

			private IClip root;

			[NonSerialized]
			public bool loaded;

			[NonSerialized]
			public bool builtIn;

			private int loading;

			private bool terminateLoading;

			public float volume
			{
				get
				{
					if (vR != 0f)
					{
						return vB * AudioUtils.DBToValue(Random.Range(0f - vR, vR));
					}
					return vB;
				}
			}

			public float pitch
			{
				get
				{
					if (pR != 0f)
					{
						return pB * AudioUtils.CentsToRatio(Random.Range(0f - pR, pR));
					}
					return pB;
				}
			}

			public bool hasSubscribers => this.onLoaded != null;

			private event Action onLoaded;

			public void Merge(SerializedSample serialized)
			{
				if (category != serialized.category)
				{
					Debug.Log((object)"Category change not supported");
				}
				isSwitch = serialized.isSwitch;
				isLoop = serialized.isLoop;
				crossFade = serialized.crossFade;
				vB = serialized.vB;
				vR = serialized.vR;
				pB = serialized.pB;
				pR = serialized.pR;
				for (int i = 0; i < serialized.clips.Count; i++)
				{
					AddClip(serialized.clips[i].filename, serialized.clips[i].id, serialized.clips[i]);
				}
			}

			public void RemoveMissingCips()
			{
				isSwitch = (isLoop = false);
				root = null;
				containers.Clear();
				List<SerializedClip> list = clips;
				clips = new List<SerializedClip>();
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].missingFile)
					{
						AddClip(list[i].filename, list[i].id, list[i]);
					}
				}
			}

			private void ParseId(string childId, out string containerId, out string localId)
			{
				int num = childId.LastIndexOf('-');
				if (num >= 0)
				{
					containerId = childId.Substring(0, num);
					localId = childId.Substring(num + 1);
				}
				else
				{
					containerId = "";
					localId = childId;
				}
			}

			private IClipContainer GetContainer(string containerId)
			{
				containers.TryGetValue(containerId, out var value);
				return value;
			}

			public SerializedClip AddClip(string path, string id, SerializedClip copyFrom = null, AudioClip loadedClip = null)
			{
				if (containers == null)
				{
					containers = new Dictionary<string, IClipContainer>();
				}
				if (clips == null)
				{
					clips = new List<SerializedClip>();
				}
				SerializedClip serializedClip = null;
				for (int i = 0; i < clips.Count; i++)
				{
					if (clips[i].id == id)
					{
						serializedClip = clips[i];
						break;
					}
				}
				if (serializedClip == null)
				{
					serializedClip = new SerializedClip
					{
						id = id,
						filename = path
					};
					clips.Add(serializedClip);
				}
				serializedClip.filename = path;
				if ((Object)(object)loadedClip != (Object)null)
				{
					serializedClip.clip = loadedClip;
					serializedClip.isEmbedded = true;
					serializedClip.clipDirty = false;
					serializedClip.missingFile = false;
				}
				if (copyFrom != null)
				{
					serializedClip.Merge(copyFrom);
				}
				serializedClip.sample = this;
				serializedClip.CheckClipFileChange();
				IClipContainer clipContainer = EnsureContainerFor(id);
				if (clipContainer != null)
				{
					ParseId(id, out var _, out var localId);
					clipContainer.SetChild(localId, serializedClip);
				}
				else
				{
					root = serializedClip;
				}
				return serializedClip;
			}

			private IClipContainer EnsureContainerFor(string childId)
			{
				ParseId(childId, out var containerId, out var localId);
				if (string.IsNullOrEmpty(containerId))
				{
					return null;
				}
				IClipContainer clipContainer = GetContainer(containerId);
				if (clipContainer == null)
				{
					SampleContainerChildType childType = GetChildType(localId);
					clipContainer = CreateContainer(containerId, childType);
					containers[containerId] = clipContainer;
					IClipContainer clipContainer2 = EnsureContainerFor(containerId);
					if (clipContainer2 == null)
					{
						root = clipContainer;
					}
					else
					{
						ParseId(containerId, out var _, out var localId2);
						clipContainer2.SetChild(localId2, clipContainer);
					}
					if (clipContainer is LoopContainer)
					{
						isLoop = true;
					}
					if (clipContainer is SwitchContainer)
					{
						isSwitch = true;
					}
				}
				return clipContainer;
			}

			public static SampleContainerChildType GetChildType(string localChildId)
			{
				if (localChildId.Equals("start", StringComparison.InvariantCultureIgnoreCase) || localChildId.Equals("begin", StringComparison.InvariantCultureIgnoreCase))
				{
					return SampleContainerChildType.Start;
				}
				if (localChildId.Equals("loop", StringComparison.InvariantCultureIgnoreCase) || localChildId.Equals("run", StringComparison.InvariantCultureIgnoreCase))
				{
					return SampleContainerChildType.Loop;
				}
				if (localChildId.Equals("stop", StringComparison.InvariantCultureIgnoreCase) || localChildId.Equals("end", StringComparison.InvariantCultureIgnoreCase))
				{
					return SampleContainerChildType.Stop;
				}
				if (localChildId.Length == 1 && char.IsLetter(localChildId[0]))
				{
					return SampleContainerChildType.Choice;
				}
				if (int.TryParse(localChildId, out var _))
				{
					return SampleContainerChildType.Random;
				}
				return SampleContainerChildType.None;
			}

			private IClipContainer CreateContainer(string containerId, SampleContainerChildType childType)
			{
				switch (childType)
				{
				case SampleContainerChildType.Random:
					return new RandomContainer
					{
						id = containerId
					};
				case SampleContainerChildType.Choice:
					return new SwitchContainer
					{
						id = containerId
					};
				case SampleContainerChildType.Start:
				case SampleContainerChildType.Loop:
				case SampleContainerChildType.Stop:
					return new LoopContainer
					{
						id = containerId
					};
				case SampleContainerChildType.None:
					return new RandomContainer
					{
						id = containerId
					};
				default:
					throw new InvalidOperationException();
				}
			}

			private SerializedClip GetClip(string id)
			{
				for (int i = 0; i < clips.Count; i++)
				{
					if (clips[i].id.Equals(id))
					{
						return clips[i];
					}
				}
				return null;
			}

			public SerializedClip GetClip(SerializedClip currentClip, char choice, SampleContainerChildType loopType)
			{
				if (root == null)
				{
					return null;
				}
				if (root is SerializedClip)
				{
					return root as SerializedClip;
				}
				return (root as IClipContainer).GetClip(currentClip?.id, choice, loopType);
			}

			public void Subscribe(Action callback)
			{
				if (this.onLoaded == null && !loaded)
				{
					Coroutines.StartGlobalCoroutine(LoadClips());
				}
				else
				{
					callback();
				}
				onLoaded += callback;
			}

			public void Unsubscribe(Action callback)
			{
				onLoaded -= callback;
				_ = this.onLoaded;
			}

			public void CheckClipFileChange()
			{
				bool flag = false;
				for (int i = 0; i < clips.Count; i++)
				{
					SerializedClip serializedClip = clips[i];
					serializedClip.CheckClipFileChange();
					if (serializedClip.clipDirty)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Coroutines.StartGlobalCoroutine(LoadClips());
				}
			}

			public IEnumerator LoadClips()
			{
				terminateLoading = true;
				while (loading > 0)
				{
					yield return null;
				}
				terminateLoading = false;
				loading++;
				if (builtIn)
				{
					while ((Object)(object)SoundSourcePool.instance == (Object)null && !terminateLoading)
					{
						yield return null;
					}
				}
				else
				{
					for (int i = 0; i < clips.Count; i++)
					{
						if (terminateLoading)
						{
							break;
						}
						SerializedClip clip = clips[i];
						if ((Object)(object)clip.clip == (Object)null || clip.clipDirty)
						{
							DateTime timestamp = File.GetLastWriteTimeUtc(Path.Combine(library.library.wavPath, clip.filename));
							WWW www = new WWW("file://" + Path.GetFullPath(Path.Combine(library.library.wavPath, clip.filename)));
							while (!www.get_isDone())
							{
								yield return null;
							}
							clip.LoadClip(www.GetAudioClip(true, false), timestamp);
						}
					}
				}
				loading--;
				if (!terminateLoading)
				{
					loaded = true;
					if (this.onLoaded != null)
					{
						this.onLoaded();
					}
				}
			}
		}

		[Serializable]
		public class SerializedClip : IClip
		{
			[NonSerialized]
			public SerializedSample sample;

			[NonSerialized]
			public bool missingFile;

			private DateTime fileTimestamp;

			[NonSerialized]
			public AudioClip clip;

			[NonSerialized]
			public bool clipDirty;

			[NonSerialized]
			public bool isEmbedded;

			[SerializeField]
			private string _id;

			public string filename;

			public float vB = 1f;

			public float vR;

			public float pB = 1f;

			public float pR;

			public string id
			{
				get
				{
					if (string.IsNullOrEmpty(_id))
					{
						_id = Path.GetFileNameWithoutExtension(filename);
					}
					return _id;
				}
				set
				{
					_id = value;
				}
			}

			public float volume
			{
				get
				{
					if (vR != 0f)
					{
						return vB * AudioUtils.DBToValue(Random.Range(0f - vR, vR));
					}
					return vB;
				}
			}

			public float pitch
			{
				get
				{
					if (pR != 0f)
					{
						return pB * AudioUtils.CentsToRatio(Random.Range(0f - pR, pR));
					}
					return pB;
				}
			}

			public void Merge(SerializedClip serialized)
			{
				clip = serialized.clip;
				fileTimestamp = serialized.fileTimestamp;
				vB = serialized.vB;
				vR = serialized.vR;
				pB = serialized.pB;
				pR = serialized.pR;
			}

			public void CheckClipFileChange()
			{
				if (!isEmbedded && !clipDirty && (Object)(object)clip != (Object)null && fileTimestamp != File.GetLastWriteTimeUtc(Path.Combine(sample.library.library.wavPath, filename)))
				{
					clipDirty = true;
				}
			}

			public void LoadClip(AudioClip clip, DateTime timestamp)
			{
				fileTimestamp = timestamp;
				this.clip = clip;
				((Object)clip).set_name(id);
				clipDirty = false;
			}

			public override string ToString()
			{
				return $"Clip {id} {filename}";
			}
		}

		public string path = "Assets/Audio/Library";

		public bool isMain;

		public static SoundLibrary main;

		public static SoundLibrary level;

		[NonSerialized]
		public SerializedSampleLibrary library;

		public string jsonPath => Path.Combine(wavPath, "library.txt");

		public string wavPath => "Assets/Audio/" + path;

		private void OnEnable()
		{
		}

		public static SerializedSample GetSample(string name)
		{
			SerializedSample serializedSample = null;
			if ((Object)(object)level != (Object)null)
			{
				serializedSample = level.library.GetSample(name);
			}
			if (serializedSample == null && (Object)(object)main != (Object)null)
			{
				serializedSample = main.library.GetSample(name);
			}
			return serializedSample;
		}

		public void Load()
		{
			SerializedSampleLibrary serializedSampleLibrary = ReadJson(jsonPath);
			if (serializedSampleLibrary != null)
			{
				LoadJson(serializedSampleLibrary);
			}
			LoadFilesystem();
		}

		public static SerializedSampleLibrary ReadJson(string path)
		{
			try
			{
				SerializedSampleLibrary serializedSampleLibrary = JsonUtility.FromJson<SerializedSampleLibrary>(File.ReadAllText(path));
				for (int i = 0; i < serializedSampleLibrary.samples.Count; i++)
				{
					serializedSampleLibrary.map[serializedSampleLibrary.samples[i].name] = serializedSampleLibrary.samples[i];
				}
				return serializedSampleLibrary;
			}
			catch
			{
				return null;
			}
		}

		public void LoadJson(SerializedSampleLibrary savedLibrary)
		{
			for (int i = 0; i < savedLibrary.samples.Count; i++)
			{
				SerializedSample serializedSample = savedLibrary.samples[i];
				SerializedSample serializedSample2 = library.GetSample(serializedSample.name);
				if (serializedSample2 == null)
				{
					serializedSample2 = new SerializedSample
					{
						category = serializedSample.category,
						name = serializedSample.name
					};
					library.AddSample(serializedSample2);
				}
				serializedSample2.Merge(serializedSample);
			}
		}

		public void Save()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			string contents = JsonUtility.ToJson((object)library, true);
			Provider.Checkout(new Asset(jsonPath), (CheckoutMode)1).Wait();
			File.WriteAllText(jsonPath, contents);
		}

		public void LoadFilesystem()
		{
			library.MarkAllMissing();
			string fullPath = Path.GetFullPath(wavPath);
			try
			{
				string[] directories = Directory.GetDirectories(fullPath);
				for (int i = 0; i < directories.Length; i++)
				{
					string[] files = Directory.GetFiles(directories[i], "*.wav");
					for (int j = 0; j < files.Length; j++)
					{
						ParseSamplePath(files[j], out var category, out var name, out var id);
						if (!name.StartsWith("._"))
						{
							SerializedSample serializedSample = library.GetSample(name);
							if (serializedSample == null)
							{
								serializedSample = new SerializedSample
								{
									category = category,
									name = name
								};
								library.AddSample(serializedSample);
							}
							serializedSample.category = category;
							serializedSample.AddClip(GetRelativePathTo(fullPath, files[j]), id).missingFile = false;
						}
					}
				}
				library.RemoveMissing();
				library.ReloadSamples();
				SoundManager.main.ReapplySamples();
				SoundManager.level.ReapplySamples();
			}
			catch
			{
			}
		}

		public static string GetRelativePathTo(string fromPath, string toPath)
		{
			Uri uri = new Uri(fromPath + "/");
			Uri uri2 = new Uri(toPath);
			return Uri.UnescapeDataString(uri.MakeRelativeUri(uri2).ToString());
		}

		private void ParseSamplePath(string path, out string category, out string name, out string id)
		{
			category = Path.GetFileName(Path.GetDirectoryName(path));
			id = Path.GetFileNameWithoutExtension(path);
			int num = id.IndexOf('-');
			if (num < 0)
			{
				name = id;
			}
			else
			{
				name = id.Substring(0, num);
			}
		}

		public SoundLibrary()
			: this()
		{
		}
	}
}
