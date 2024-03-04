using System;
using System.Runtime.InteropServices;

namespace nn.ngc
{
	public sealed class ProfanityFilter : IDisposable
	{
		public enum MaskMode
		{
			OverWrite,
			ReplaceByOneCharacter
		}

		public enum SkipMode
		{
			NotSkip,
			SkipAtSign
		}

		[Flags]
		public enum PatternList
		{
			Japanese = 0x1,
			AmericanEnglish = 0x2,
			CanadianFrench = 0x4,
			LatinAmericanSpanish = 0x8,
			BritishEnglish = 0x10,
			French = 0x20,
			German = 0x40,
			Italian = 0x80,
			Spanish = 0x100,
			Dutch = 0x200,
			Korean = 0x400,
			SimplifiedChinese = 0x800,
			Portuguese = 0x1000,
			Russian = 0x2000,
			SouthAmericanPortuguese = 0x4000,
			TraditionalChinese = 0x8000,
			Max = 0x10
		}

		private IntPtr _profanityFilter = IntPtr.Zero;

		private IntPtr _ngcWorkBuffer = IntPtr.Zero;

		public ProfanityFilter()
		{
			Initialize(ref _profanityFilter, ref _ngcWorkBuffer, checkDesiredLanguage: true);
		}

		public ProfanityFilter(bool checkDesiredLanguage)
		{
			Initialize(ref _profanityFilter, ref _ngcWorkBuffer, checkDesiredLanguage);
		}

		~ProfanityFilter()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_profanityFilter != IntPtr.Zero && _ngcWorkBuffer != IntPtr.Zero)
			{
				Destroy(_profanityFilter, _ngcWorkBuffer);
				_profanityFilter = IntPtr.Zero;
				_ngcWorkBuffer = IntPtr.Zero;
			}
		}

		public uint GetContentVersion()
		{
			return GetContentVersion(_profanityFilter);
		}

		public Result CheckProfanityWords([Out] PatternList[] checkResults, PatternList patterns, string[] words)
		{
			return CheckProfanityWords(_profanityFilter, checkResults, patterns, words, words.Length);
		}

		public Result MaskProfanityWordsInText(ref int profanityWordCount, string inText, out string outText, PatternList patterns)
		{
			outText = string.Copy(inText);
			Result result = MaskProfanityWordsInText(_profanityFilter, ref profanityWordCount, outText, patterns);
			outText = outText.TrimEnd(default(char));
			return result;
		}

		public void SetMaskMode(MaskMode mode)
		{
			SetMaskMode(_profanityFilter, mode);
		}

		public void SkipAtSignCheck(SkipMode skipMode)
		{
			SkipAtSignCheck(_profanityFilter, skipMode);
		}

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_ProfanityFilterDestroy")]
		private static extern void Destroy(IntPtr profanityFilter, IntPtr ngcWorkBuffer);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_ProfanityFilterInitialize")]
		private static extern Result Initialize(ref IntPtr profanityFilter, ref IntPtr ngcWorkBuffer, bool checkDesiredLanguage);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_GetContentVersion")]
		private static extern uint GetContentVersion(IntPtr profanityFilter);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "nn_ngc_CheckProfanityWords")]
		private static extern Result CheckProfanityWords(IntPtr profanityFilter, [Out] PatternList[] checkResults, PatternList patterns, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] words, long wordCount);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "nn_ngc_MaskProfanityWordsInText")]
		private static extern Result MaskProfanityWordsInText(IntPtr profanityFilter, ref int profanityWordCount, string text, PatternList patterns);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_SetMaskMode")]
		private static extern void SetMaskMode(IntPtr profanityFilter, MaskMode mode);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_SkipAtSignCheck")]
		private static extern void SkipAtSignCheck(IntPtr profanityFilter, SkipMode skipMode);
	}
}
