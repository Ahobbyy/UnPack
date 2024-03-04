using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class MessageSystem
	{
		[Flags]
		internal enum MessageFlag
		{
			None = 0x0,
			Info = 0x80,
			EmptyBundle = 0x81,
			EmptyFolder = 0x82,
			Warning = 0x8000,
			WarningInChildren = 0x8100,
			AssetsDuplicatedInMultBundles = 0x8200,
			VariantBundleMismatch = 0x8400,
			Error = 0x800000,
			ErrorInChildren = 0x810000,
			SceneBundleConflict = 0x820000,
			DependencySceneConflict = 0x840000
		}

		internal class MessageState
		{
			private MessageFlag m_MessageFlags;

			private HashSet<MessageFlag> m_MessageSet;

			internal MessageState()
			{
				m_MessageFlags = MessageFlag.None;
				m_MessageSet = new HashSet<MessageFlag>();
			}

			internal void Clear()
			{
				m_MessageFlags = MessageFlag.None;
				m_MessageSet.Clear();
			}

			internal void SetFlag(MessageFlag flag, bool on)
			{
				if (flag != MessageFlag.Info && flag != MessageFlag.Warning && flag != MessageFlag.Error)
				{
					if (on)
					{
						m_MessageFlags |= flag;
						m_MessageSet.Add(flag);
					}
					else
					{
						m_MessageFlags &= ~flag;
						m_MessageSet.Remove(flag);
					}
				}
			}

			internal bool IsSet(MessageFlag flag)
			{
				return (m_MessageFlags & flag) == flag;
			}

			internal bool HasMessages()
			{
				return m_MessageFlags != MessageFlag.None;
			}

			internal MessageType HighestMessageLevel()
			{
				if (!IsSet(MessageFlag.Error))
				{
					if (!IsSet(MessageFlag.Warning))
					{
						if (!IsSet(MessageFlag.Info))
						{
							return (MessageType)0;
						}
						return (MessageType)1;
					}
					return (MessageType)2;
				}
				return (MessageType)3;
			}

			internal MessageFlag HighestMessageFlag()
			{
				MessageFlag messageFlag = MessageFlag.None;
				foreach (MessageFlag item in m_MessageSet)
				{
					if (item > messageFlag)
					{
						messageFlag = item;
					}
				}
				return messageFlag;
			}

			internal List<Message> GetMessages()
			{
				List<Message> list = new List<Message>();
				foreach (MessageFlag item in m_MessageSet)
				{
					list.Add(GetMessage(item));
				}
				return list;
			}
		}

		internal class Message
		{
			internal MessageType severity;

			internal string message;

			internal Texture2D icon => GetIcon(severity);

			internal Message(string msg, MessageType sev)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				message = msg;
				severity = sev;
			}
		}

		private static Texture2D s_ErrorIcon;

		private static Texture2D s_WarningIcon;

		private static Texture2D s_InfoIcon;

		private static Dictionary<MessageFlag, Message> s_MessageLookup;

		internal static Texture2D GetIcon(MessageType sev)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			if ((int)sev == 3)
			{
				return GetErrorIcon();
			}
			if ((int)sev == 2)
			{
				return GetWarningIcon();
			}
			if ((int)sev == 1)
			{
				return GetInfoIcon();
			}
			return null;
		}

		private static Texture2D GetErrorIcon()
		{
			if ((Object)(object)s_ErrorIcon == (Object)null)
			{
				FindMessageIcons();
			}
			return s_ErrorIcon;
		}

		private static Texture2D GetWarningIcon()
		{
			if ((Object)(object)s_WarningIcon == (Object)null)
			{
				FindMessageIcons();
			}
			return s_WarningIcon;
		}

		private static Texture2D GetInfoIcon()
		{
			if ((Object)(object)s_InfoIcon == (Object)null)
			{
				FindMessageIcons();
			}
			return s_InfoIcon;
		}

		private static void FindMessageIcons()
		{
			s_ErrorIcon = EditorGUIUtility.FindTexture("console.errorIcon");
			s_WarningIcon = EditorGUIUtility.FindTexture("console.warnicon");
			s_InfoIcon = EditorGUIUtility.FindTexture("console.infoIcon");
		}

		internal static Message GetMessage(MessageFlag lookup)
		{
			if (s_MessageLookup == null)
			{
				InitMessages();
			}
			Message value = null;
			s_MessageLookup.TryGetValue(lookup, out value);
			if (value == null)
			{
				value = s_MessageLookup[MessageFlag.None];
			}
			return value;
		}

		private static void InitMessages()
		{
			s_MessageLookup = new Dictionary<MessageFlag, Message>();
			s_MessageLookup.Add(MessageFlag.None, new Message(string.Empty, (MessageType)0));
			s_MessageLookup.Add(MessageFlag.EmptyBundle, new Message("This bundle is empty.  Empty bundles cannot get saved with the scene and will disappear from this list if Unity restarts or if various other bundle rename or move events occur.", (MessageType)1));
			s_MessageLookup.Add(MessageFlag.EmptyFolder, new Message("This folder is either empty or contains only empty children.  Empty bundles cannot get saved with the scene and will disappear from this list if Unity restarts or if various other bundle rename or move events occur.", (MessageType)1));
			s_MessageLookup.Add(MessageFlag.WarningInChildren, new Message("Warning in child(ren)", (MessageType)2));
			s_MessageLookup.Add(MessageFlag.AssetsDuplicatedInMultBundles, new Message("Assets being pulled into this bundle due to dependencies are also being pulled into another bundle.  This will cause duplication in memory", (MessageType)2));
			s_MessageLookup.Add(MessageFlag.VariantBundleMismatch, new Message("Variants of a given bundle must have exactly the same assets between them based on a Name.Extension (without Path) comparison. These bundle variants fail that check.", (MessageType)2));
			s_MessageLookup.Add(MessageFlag.ErrorInChildren, new Message("Error in child(ren)", (MessageType)3));
			s_MessageLookup.Add(MessageFlag.SceneBundleConflict, new Message("A bundle with one or more scenes must only contain scenes.  This bundle has scenes and non-scene assets.", (MessageType)3));
			s_MessageLookup.Add(MessageFlag.DependencySceneConflict, new Message("The folder added to this bundle has pulled in scenes and non-scene assets.  A bundle must only have one type or the other.", (MessageType)3));
		}
	}
}
