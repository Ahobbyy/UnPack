using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	public sealed class CurveEditor
	{
		private enum EditMode
		{
			None,
			Moving,
			TangentEdit
		}

		private enum Tangent
		{
			In,
			Out
		}

		public struct Settings
		{
			public Rect bounds;

			public RectOffset padding;

			public Color selectionColor;

			public float curvePickingDistance;

			public float keyTimeClampingDistance;

			public static Settings defaultSettings
			{
				get
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Expected O, but got Unknown
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					Settings result = default(Settings);
					result.bounds = new Rect(0f, 0f, 1f, 1f);
					result.padding = new RectOffset(10, 10, 10, 10);
					result.selectionColor = Color.get_yellow();
					result.curvePickingDistance = 6f;
					result.keyTimeClampingDistance = 0.0001f;
					return result;
				}
			}
		}

		public struct CurveState
		{
			public bool visible;

			public bool editable;

			public uint minPointCount;

			public float zeroKeyConstantValue;

			public Color color;

			public float width;

			public float handleWidth;

			public bool showNonEditableHandles;

			public bool onlyShowHandlesOnSelection;

			public bool loopInBounds;

			public static CurveState defaultState
			{
				get
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0033: Unknown result type (might be due to invalid IL or missing references)
					CurveState result = default(CurveState);
					result.visible = true;
					result.editable = true;
					result.minPointCount = 2u;
					result.zeroKeyConstantValue = 0f;
					result.color = Color.get_white();
					result.width = 2f;
					result.handleWidth = 2f;
					result.showNonEditableHandles = true;
					result.onlyShowHandlesOnSelection = false;
					result.loopInBounds = false;
					return result;
				}
			}
		}

		public struct Selection
		{
			public SerializedProperty curve;

			public int keyframeIndex;

			public Keyframe? keyframe;

			public Selection(SerializedProperty curve, int keyframeIndex, Keyframe? keyframe)
			{
				this.curve = curve;
				this.keyframeIndex = keyframeIndex;
				this.keyframe = keyframe;
			}
		}

		internal struct MenuAction
		{
			internal SerializedProperty curve;

			internal int index;

			internal Vector3 position;

			internal MenuAction(SerializedProperty curve)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				this.curve = curve;
				index = -1;
				position = Vector3.get_zero();
			}

			internal MenuAction(SerializedProperty curve, int index)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				this.curve = curve;
				this.index = index;
				position = Vector3.get_zero();
			}

			internal MenuAction(SerializedProperty curve, Vector3 position)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				this.curve = curve;
				index = -1;
				this.position = position;
			}
		}

		private readonly Dictionary<SerializedProperty, CurveState> m_Curves;

		private Rect m_CurveArea;

		private SerializedProperty m_SelectedCurve;

		private int m_SelectedKeyframeIndex = -1;

		private EditMode m_EditMode;

		private Tangent m_TangentEditMode;

		private bool m_Dirty;

		public Settings settings { get; private set; }

		public CurveEditor()
			: this(Settings.defaultSettings)
		{
		}

		public CurveEditor(Settings settings)
		{
			this.settings = settings;
			m_Curves = new Dictionary<SerializedProperty, CurveState>();
		}

		public void Add(params SerializedProperty[] curves)
		{
			foreach (SerializedProperty curve in curves)
			{
				Add(curve, CurveState.defaultState);
			}
		}

		public void Add(SerializedProperty curve)
		{
			Add(curve, CurveState.defaultState);
		}

		public void Add(SerializedProperty curve, CurveState state)
		{
			if (curve.get_animationCurveValue() == null)
			{
				throw new ArgumentException("curve");
			}
			if (m_Curves.ContainsKey(curve))
			{
				Debug.LogWarning((object)"Curve has already been added to the editor");
			}
			m_Curves.Add(curve, state);
		}

		public void Remove(SerializedProperty curve)
		{
			m_Curves.Remove(curve);
		}

		public void RemoveAll()
		{
			m_Curves.Clear();
		}

		public CurveState GetCurveState(SerializedProperty curve)
		{
			if (!m_Curves.TryGetValue(curve, out var value))
			{
				throw new KeyNotFoundException("curve");
			}
			return value;
		}

		public void SetCurveState(SerializedProperty curve, CurveState state)
		{
			if (!m_Curves.ContainsKey(curve))
			{
				throw new KeyNotFoundException("curve");
			}
			m_Curves[curve] = state;
		}

		public Selection GetSelection()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			Keyframe? keyframe = null;
			if (m_SelectedKeyframeIndex > -1)
			{
				AnimationCurve animationCurveValue = m_SelectedCurve.get_animationCurveValue();
				if (m_SelectedKeyframeIndex >= animationCurveValue.get_length())
				{
					m_SelectedKeyframeIndex = -1;
				}
				else
				{
					keyframe = animationCurveValue.get_Item(m_SelectedKeyframeIndex);
				}
			}
			return new Selection(m_SelectedCurve, m_SelectedKeyframeIndex, keyframe);
		}

		public void SetKeyframe(SerializedProperty curve, int keyframeIndex, Keyframe keyframe)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			AnimationCurve animationCurveValue = curve.get_animationCurveValue();
			SetKeyframe(animationCurveValue, keyframeIndex, keyframe);
			SaveCurve(curve, animationCurveValue);
		}

		public bool OnGUI(Rect rect)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if ((int)Event.get_current().get_type() == 7)
			{
				m_Dirty = false;
			}
			GUI.BeginClip(rect);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(Vector2.get_zero(), ((Rect)(ref rect)).get_size());
			m_CurveArea = settings.padding.Remove(val);
			foreach (KeyValuePair<SerializedProperty, CurveState> curf in m_Curves)
			{
				OnCurveGUI(val, curf.Key, curf.Value);
			}
			OnGeneralUI(val);
			GUI.EndClip();
			return m_Dirty;
		}

		private void OnCurveGUI(Rect rect, SerializedProperty curve, CurveState state)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Expected O, but got Unknown
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Invalid comparison between Unknown and I4
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Unknown result type (might be due to invalid IL or missing references)
			//IL_0695: Unknown result type (might be due to invalid IL or missing references)
			//IL_0697: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_0718: Invalid comparison between Unknown and I4
			//IL_073a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Invalid comparison between Unknown and I4
			//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0824: Unknown result type (might be due to invalid IL or missing references)
			//IL_0829: Unknown result type (might be due to invalid IL or missing references)
			//IL_082f: Unknown result type (might be due to invalid IL or missing references)
			//IL_083c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0853: Expected O, but got Unknown
			//IL_0853: Expected O, but got Unknown
			//IL_0861: Unknown result type (might be due to invalid IL or missing references)
			//IL_086f: Unknown result type (might be due to invalid IL or missing references)
			//IL_087f: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fd: Invalid comparison between Unknown and I4
			//IL_090e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0923: Unknown result type (might be due to invalid IL or missing references)
			//IL_093b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0952: Unknown result type (might be due to invalid IL or missing references)
			if (!state.visible)
			{
				return;
			}
			AnimationCurve animationCurveValue = curve.get_animationCurveValue();
			Keyframe[] keys = animationCurveValue.get_keys();
			int num = keys.Length;
			Color color = state.color;
			if (!state.editable || !GUI.get_enabled())
			{
				color.a *= 0.5f;
			}
			Handles.set_color(color);
			Rect bounds = this.settings.bounds;
			switch (num)
			{
			case 0:
			{
				Vector3 val6 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMin(), state.zeroKeyConstantValue));
				Vector3 val7 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMax(), state.zeroKeyConstantValue));
				Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[2] { val6, val7 });
				break;
			}
			case 1:
			{
				Vector3 val8 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMin(), ((Keyframe)(ref keys[0])).get_value()));
				Vector3 val9 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMax(), ((Keyframe)(ref keys[0])).get_value()));
				Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[2] { val8, val9 });
				break;
			}
			default:
			{
				Keyframe start = keys[0];
				for (int i = 1; i < num; i++)
				{
					Keyframe val = keys[i];
					Vector3[] array = BezierSegment(start, val);
					if (float.IsInfinity(((Keyframe)(ref start)).get_outTangent()) || float.IsInfinity(((Keyframe)(ref val)).get_inTangent()))
					{
						Vector3[] array2 = HardSegment(start, val);
						Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[3]
						{
							array2[0],
							array2[1],
							array2[2]
						});
					}
					else
					{
						Handles.DrawBezier(array[0], array[3], array[1], array[2], color, (Texture2D)null, state.width);
					}
					start = val;
				}
				Settings settings;
				if (((Keyframe)(ref keys[0])).get_time() > ((Rect)(ref bounds)).get_xMin())
				{
					if (state.loopInBounds)
					{
						Keyframe start2 = keys[num - 1];
						float time = ((Keyframe)(ref start2)).get_time();
						settings = this.settings;
						((Keyframe)(ref start2)).set_time(time - ((Rect)(ref settings.bounds)).get_width());
						Keyframe end = keys[0];
						Vector3[] array3 = BezierSegment(start2, end);
						if (float.IsInfinity(((Keyframe)(ref start2)).get_outTangent()) || float.IsInfinity(((Keyframe)(ref end)).get_inTangent()))
						{
							Vector3[] array4 = HardSegment(start2, end);
							Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[3]
							{
								array4[0],
								array4[1],
								array4[2]
							});
						}
						else
						{
							Handles.DrawBezier(array3[0], array3[3], array3[1], array3[2], color, (Texture2D)null, state.width);
						}
					}
					else
					{
						Vector3 val2 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMin(), ((Keyframe)(ref keys[0])).get_value()));
						Vector3 val3 = CurveToCanvas(keys[0]);
						Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[2] { val2, val3 });
					}
				}
				if (!(((Keyframe)(ref keys[num - 1])).get_time() < ((Rect)(ref bounds)).get_xMax()))
				{
					break;
				}
				if (state.loopInBounds)
				{
					Keyframe start3 = keys[num - 1];
					Keyframe end2 = keys[0];
					float time2 = ((Keyframe)(ref end2)).get_time();
					settings = this.settings;
					((Keyframe)(ref end2)).set_time(time2 + ((Rect)(ref settings.bounds)).get_width());
					Vector3[] array5 = BezierSegment(start3, end2);
					if (float.IsInfinity(((Keyframe)(ref start3)).get_outTangent()) || float.IsInfinity(((Keyframe)(ref end2)).get_inTangent()))
					{
						Vector3[] array6 = HardSegment(start3, end2);
						Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[3]
						{
							array6[0],
							array6[1],
							array6[2]
						});
					}
					else
					{
						Handles.DrawBezier(array5[0], array5[3], array5[1], array5[2], color, (Texture2D)null, state.width);
					}
				}
				else
				{
					Vector3 val4 = CurveToCanvas(keys[num - 1]);
					Vector3 val5 = this.CurveToCanvas(new Vector3(((Rect)(ref bounds)).get_xMax(), ((Keyframe)(ref keys[num - 1])).get_value()));
					Handles.DrawAAPolyLine(state.width, (Vector3[])(object)new Vector3[2] { val4, val5 });
				}
				break;
			}
			}
			bool flag = curve == m_SelectedCurve;
			if (flag && m_SelectedKeyframeIndex >= num)
			{
				m_SelectedKeyframeIndex = -1;
			}
			if (!state.editable)
			{
				m_SelectedKeyframeIndex = -1;
			}
			float num2 = (GUI.get_enabled() ? 1f : 0.8f);
			Rect val11 = default(Rect);
			Rect val16 = default(Rect);
			Rect val17 = default(Rect);
			for (int j = 0; j < num; j++)
			{
				bool flag2 = j == m_SelectedKeyframeIndex;
				Event current = Event.get_current();
				Vector3 val10 = CurveToCanvas(keys[j]);
				((Rect)(ref val11))._002Ector(val10.x - 8f, val10.y - 8f, 16f, 16f);
				RectOffset val12 = (flag ? new RectOffset(5, 5, 5, 5) : new RectOffset(6, 6, 6, 6));
				Vector3 val13 = CurveTangentToCanvas(((Keyframe)(ref keys[j])).get_outTangent());
				Vector3 val14 = val10 + ((Vector3)(ref val13)).get_normalized() * 40f;
				val13 = CurveTangentToCanvas(((Keyframe)(ref keys[j])).get_inTangent());
				Vector3 val15 = val10 - ((Vector3)(ref val13)).get_normalized() * 40f;
				((Rect)(ref val16))._002Ector(val15.x - 7f, val15.y - 7f, 14f, 14f);
				((Rect)(ref val17))._002Ector(val14.x - 7f, val14.y - 7f, 14f, 14f);
				if ((state.editable || state.showNonEditableHandles) && (int)current.get_type() == 7)
				{
					Color val18 = ((flag && flag2) ? this.settings.selectionColor : state.color);
					EditorGUI.DrawRect(val12.Remove(val11), val18 * num2);
					if (flag && (!state.onlyShowHandlesOnSelection || (state.onlyShowHandlesOnSelection && flag2)))
					{
						Handles.set_color(val18 * num2);
						if (j > 0 || state.loopInBounds)
						{
							Handles.DrawAAPolyLine(state.handleWidth, (Vector3[])(object)new Vector3[2] { val10, val15 });
							EditorGUI.DrawRect(val12.Remove(val16), val18);
						}
						if (j < num - 1 || state.loopInBounds)
						{
							Handles.DrawAAPolyLine(state.handleWidth, (Vector3[])(object)new Vector3[2] { val10, val14 });
							EditorGUI.DrawRect(val12.Remove(val17), val18);
						}
					}
				}
				if (!state.editable)
				{
					continue;
				}
				if (m_EditMode == EditMode.Moving && (int)current.get_type() == 3 && flag && flag2)
				{
					EditMoveKeyframe(animationCurveValue, keys, j);
				}
				if (m_EditMode == EditMode.TangentEdit && (int)current.get_type() == 3 && flag && flag2)
				{
					bool flag3 = !Mathf.Approximately(((Keyframe)(ref keys[j])).get_inTangent(), ((Keyframe)(ref keys[j])).get_outTangent()) && (!float.IsInfinity(((Keyframe)(ref keys[j])).get_inTangent()) || !float.IsInfinity(((Keyframe)(ref keys[j])).get_outTangent()));
					EditMoveTangent(animationCurveValue, keys, j, m_TangentEditMode, current.get_shift() || (!flag3 && !current.get_control()));
				}
				if ((int)current.get_type() == 0 && ((Rect)(ref rect)).Contains(current.get_mousePosition()) && ((Rect)(ref val11)).Contains(current.get_mousePosition()))
				{
					if (current.get_button() == 0)
					{
						SelectKeyframe(curve, j);
						m_EditMode = EditMode.Moving;
						current.Use();
					}
					else if (current.get_button() == 1)
					{
						GenericMenu val19 = new GenericMenu();
						val19.AddItem(new GUIContent("Delete Key"), false, (MenuFunction2)delegate(object x)
						{
							MenuAction menuAction = (MenuAction)x;
							AnimationCurve animationCurveValue2 = menuAction.curve.get_animationCurveValue();
							menuAction.curve.get_serializedObject().Update();
							RemoveKeyframe(animationCurveValue2, menuAction.index);
							m_SelectedKeyframeIndex = -1;
							SaveCurve(menuAction.curve, animationCurveValue2);
							menuAction.curve.get_serializedObject().ApplyModifiedProperties();
						}, (object)new MenuAction(curve, j));
						val19.ShowAsContext();
						current.Use();
					}
				}
				if ((int)current.get_type() == 0 && ((Rect)(ref rect)).Contains(current.get_mousePosition()))
				{
					if (((Rect)(ref val16)).Contains(current.get_mousePosition()) && (j > 0 || state.loopInBounds))
					{
						SelectKeyframe(curve, j);
						m_EditMode = EditMode.TangentEdit;
						m_TangentEditMode = Tangent.In;
						current.Use();
					}
					else if (((Rect)(ref val17)).Contains(current.get_mousePosition()) && (j < num - 1 || state.loopInBounds))
					{
						SelectKeyframe(curve, j);
						m_EditMode = EditMode.TangentEdit;
						m_TangentEditMode = Tangent.Out;
						current.Use();
					}
				}
				if ((int)current.get_rawType() == 1 && m_EditMode != 0)
				{
					m_EditMode = EditMode.None;
				}
				EditorGUIUtility.AddCursorRect(val11, (MouseCursor)8);
				if (j > 0 || state.loopInBounds)
				{
					EditorGUIUtility.AddCursorRect(val16, (MouseCursor)9);
				}
				if (j < num - 1 || state.loopInBounds)
				{
					EditorGUIUtility.AddCursorRect(val17, (MouseCursor)9);
				}
			}
			Handles.set_color(Color.get_white());
			SaveCurve(curve, animationCurveValue);
		}

		private void OnGeneralUI(Rect rect)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_0183: Expected O, but got Unknown
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Expected O, but got Unknown
			//IL_02a2: Expected O, but got Unknown
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Expected O, but got Unknown
			//IL_02bf: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Invalid comparison between Unknown and I4
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Invalid comparison between Unknown and I4
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Invalid comparison between Unknown and I4
			Event current = Event.get_current();
			if ((int)current.get_type() == 0)
			{
				GUI.FocusControl((string)null);
				m_SelectedCurve = null;
				m_SelectedKeyframeIndex = -1;
				bool flag = false;
				Vector3 hit = CanvasToCurve(Vector2.op_Implicit(current.get_mousePosition()));
				float y = CurveToCanvas(hit).y;
				MenuFunction2 val3 = default(MenuFunction2);
				foreach (KeyValuePair<SerializedProperty, CurveState> curf in m_Curves)
				{
					if (!curf.Value.editable || !curf.Value.visible)
					{
						continue;
					}
					SerializedProperty key = curf.Key;
					CurveState value = curf.Value;
					AnimationCurve animationCurveValue = key.get_animationCurveValue();
					float num = ((animationCurveValue.get_length() == 0) ? value.zeroKeyConstantValue : animationCurveValue.Evaluate(hit.x));
					if (!(Mathf.Abs(this.CurveToCanvas(new Vector3(hit.x, num)).y - y) < settings.curvePickingDistance))
					{
						continue;
					}
					m_SelectedCurve = key;
					if (current.get_clickCount() == 2 && current.get_button() == 0)
					{
						EditCreateKeyframe(animationCurveValue, hit, createOnCurve: true, value.zeroKeyConstantValue);
						SaveCurve(key, animationCurveValue);
					}
					else
					{
						if (current.get_button() != 1)
						{
							continue;
						}
						GenericMenu val = new GenericMenu();
						GUIContent val2 = new GUIContent("Add Key");
						MenuFunction2 obj = val3;
						if (obj == null)
						{
							MenuFunction2 val4 = delegate(object x)
							{
								//IL_002b: Unknown result type (might be due to invalid IL or missing references)
								MenuAction menuAction = (MenuAction)x;
								AnimationCurve animationCurveValue4 = menuAction.curve.get_animationCurveValue();
								menuAction.curve.get_serializedObject().Update();
								EditCreateKeyframe(animationCurveValue4, hit, createOnCurve: true, 0f);
								SaveCurve(menuAction.curve, animationCurveValue4);
								menuAction.curve.get_serializedObject().ApplyModifiedProperties();
							};
							MenuFunction2 val5 = val4;
							val3 = val4;
							obj = val5;
						}
						val.AddItem(val2, false, obj, (object)new MenuAction(key, hit));
						val.ShowAsContext();
						current.Use();
						flag = true;
					}
				}
				if (current.get_clickCount() == 2 && current.get_button() == 0 && m_SelectedCurve == null)
				{
					foreach (KeyValuePair<SerializedProperty, CurveState> curf2 in m_Curves)
					{
						if (curf2.Value.editable && curf2.Value.visible)
						{
							SerializedProperty key2 = curf2.Key;
							CurveState value2 = curf2.Value;
							AnimationCurve animationCurveValue2 = key2.get_animationCurveValue();
							EditCreateKeyframe(animationCurveValue2, hit, current.get_alt(), value2.zeroKeyConstantValue);
							SaveCurve(key2, animationCurveValue2);
						}
					}
				}
				else if (!flag && current.get_button() == 1)
				{
					GenericMenu val6 = new GenericMenu();
					val6.AddItem(new GUIContent("Add Key At Position"), false, (MenuFunction)delegate
					{
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						ContextMenuAddKey(hit, createOnCurve: false);
					});
					val6.AddItem(new GUIContent("Add Key On Curves"), false, (MenuFunction)delegate
					{
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						ContextMenuAddKey(hit, createOnCurve: true);
					});
					val6.ShowAsContext();
				}
				current.Use();
			}
			if ((int)current.get_type() == 4 && ((int)current.get_keyCode() == 127 || (int)current.get_keyCode() == 8) && m_SelectedKeyframeIndex != -1 && m_SelectedCurve != null)
			{
				AnimationCurve animationCurveValue3 = m_SelectedCurve.get_animationCurveValue();
				int length = animationCurveValue3.get_length();
				if (m_Curves[m_SelectedCurve].minPointCount < length && length >= 0)
				{
					EditDeleteKeyframe(animationCurveValue3, m_SelectedKeyframeIndex);
					m_SelectedKeyframeIndex = -1;
					SaveCurve(m_SelectedCurve, animationCurveValue3);
				}
				current.Use();
			}
		}

		private void SaveCurve(SerializedProperty prop, AnimationCurve curve)
		{
			prop.set_animationCurveValue(curve);
		}

		private void Invalidate()
		{
			m_Dirty = true;
		}

		private void SelectKeyframe(SerializedProperty curve, int keyframeIndex)
		{
			m_SelectedKeyframeIndex = keyframeIndex;
			m_SelectedCurve = curve;
			Invalidate();
		}

		private void ContextMenuAddKey(Vector3 hit, bool createOnCurve)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SerializedObject val = null;
			foreach (KeyValuePair<SerializedProperty, CurveState> curf in m_Curves)
			{
				if (curf.Value.editable && curf.Value.visible)
				{
					SerializedProperty key = curf.Key;
					CurveState value = curf.Value;
					if (val == null)
					{
						val = key.get_serializedObject();
						val.Update();
					}
					AnimationCurve animationCurveValue = key.get_animationCurveValue();
					EditCreateKeyframe(animationCurveValue, hit, createOnCurve, value.zeroKeyConstantValue);
					SaveCurve(key, animationCurveValue);
				}
			}
			if (val != null)
			{
				val.ApplyModifiedProperties();
			}
			Invalidate();
		}

		private void EditCreateKeyframe(AnimationCurve curve, Vector3 position, bool createOnCurve, float zeroKeyConstantValue)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			float num = EvaluateTangent(curve, position.x);
			if (createOnCurve)
			{
				position.y = ((curve.get_length() == 0) ? zeroKeyConstantValue : curve.Evaluate(position.x));
			}
			AddKeyframe(curve, new Keyframe(position.x, position.y, num, num));
		}

		private void EditDeleteKeyframe(AnimationCurve curve, int keyframeIndex)
		{
			RemoveKeyframe(curve, keyframeIndex);
		}

		private void AddKeyframe(AnimationCurve curve, Keyframe newValue)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			curve.AddKey(newValue);
			Invalidate();
		}

		private void RemoveKeyframe(AnimationCurve curve, int keyframeIndex)
		{
			curve.RemoveKey(keyframeIndex);
			Invalidate();
		}

		private void SetKeyframe(AnimationCurve curve, int keyframeIndex, Keyframe newValue)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			Keyframe[] keys = curve.get_keys();
			if (keyframeIndex > 0)
			{
				((Keyframe)(ref newValue)).set_time(Mathf.Max(((Keyframe)(ref keys[keyframeIndex - 1])).get_time() + settings.keyTimeClampingDistance, ((Keyframe)(ref newValue)).get_time()));
			}
			if (keyframeIndex < keys.Length - 1)
			{
				((Keyframe)(ref newValue)).set_time(Mathf.Min(((Keyframe)(ref keys[keyframeIndex + 1])).get_time() - settings.keyTimeClampingDistance, ((Keyframe)(ref newValue)).get_time()));
			}
			curve.MoveKey(keyframeIndex, newValue);
			Invalidate();
		}

		private void EditMoveKeyframe(AnimationCurve curve, Keyframe[] keys, int keyframeIndex)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = CanvasToCurve(Vector2.op_Implicit(Event.get_current().get_mousePosition()));
			float inTangent = ((Keyframe)(ref keys[keyframeIndex])).get_inTangent();
			float outTangent = ((Keyframe)(ref keys[keyframeIndex])).get_outTangent();
			SetKeyframe(curve, keyframeIndex, new Keyframe(val.x, val.y, inTangent, outTangent));
		}

		private void EditMoveTangent(AnimationCurve curve, Keyframe[] keys, int keyframeIndex, Tangent targetTangent, bool linkTangents)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = CanvasToCurve(Vector2.op_Implicit(Event.get_current().get_mousePosition()));
			float time = ((Keyframe)(ref keys[keyframeIndex])).get_time();
			float value = ((Keyframe)(ref keys[keyframeIndex])).get_value();
			val -= new Vector3(time, value);
			if (targetTangent == Tangent.In && val.x > 0f)
			{
				val.x = 0f;
			}
			if (targetTangent == Tangent.Out && val.x < 0f)
			{
				val.x = 0f;
			}
			float num = ((!Mathf.Approximately(val.x, 0f)) ? (val.y / val.x) : ((val.y < 0f) ? float.PositiveInfinity : float.NegativeInfinity));
			float num2 = ((Keyframe)(ref keys[keyframeIndex])).get_inTangent();
			float num3 = ((Keyframe)(ref keys[keyframeIndex])).get_outTangent();
			if (targetTangent == Tangent.In || linkTangents)
			{
				num2 = num;
			}
			if (targetTangent == Tangent.Out || linkTangents)
			{
				num3 = num;
			}
			SetKeyframe(curve, keyframeIndex, new Keyframe(time, value, num2, num3));
		}

		private Vector3 CurveToCanvas(Keyframe keyframe)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return this.CurveToCanvas(new Vector3(((Keyframe)(ref keyframe)).get_time(), ((Keyframe)(ref keyframe)).get_value()));
		}

		private Vector3 CurveToCanvas(Vector3 position)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			Rect bounds = settings.bounds;
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector((position.x - ((Rect)(ref bounds)).get_x()) / (((Rect)(ref bounds)).get_xMax() - ((Rect)(ref bounds)).get_x()), (position.y - ((Rect)(ref bounds)).get_y()) / (((Rect)(ref bounds)).get_yMax() - ((Rect)(ref bounds)).get_y()));
			val.x = val.x * (((Rect)(ref m_CurveArea)).get_xMax() - ((Rect)(ref m_CurveArea)).get_xMin()) + ((Rect)(ref m_CurveArea)).get_xMin();
			val.y = (1f - val.y) * (((Rect)(ref m_CurveArea)).get_yMax() - ((Rect)(ref m_CurveArea)).get_yMin()) + ((Rect)(ref m_CurveArea)).get_yMin();
			return val;
		}

		private Vector3 CanvasToCurve(Vector3 position)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			Rect bounds = settings.bounds;
			Vector3 val = position;
			val.x = (val.x - ((Rect)(ref m_CurveArea)).get_xMin()) / (((Rect)(ref m_CurveArea)).get_xMax() - ((Rect)(ref m_CurveArea)).get_xMin());
			val.y = (val.y - ((Rect)(ref m_CurveArea)).get_yMin()) / (((Rect)(ref m_CurveArea)).get_yMax() - ((Rect)(ref m_CurveArea)).get_yMin());
			val.x = Mathf.Lerp(((Rect)(ref bounds)).get_x(), ((Rect)(ref bounds)).get_xMax(), val.x);
			val.y = Mathf.Lerp(((Rect)(ref bounds)).get_yMax(), ((Rect)(ref bounds)).get_y(), val.y);
			return val;
		}

		private Vector3 CurveTangentToCanvas(float tangent)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (!float.IsInfinity(tangent))
			{
				Rect bounds = settings.bounds;
				float num = ((Rect)(ref m_CurveArea)).get_width() / ((Rect)(ref m_CurveArea)).get_height() / ((((Rect)(ref bounds)).get_xMax() - ((Rect)(ref bounds)).get_x()) / (((Rect)(ref bounds)).get_yMax() - ((Rect)(ref bounds)).get_y()));
				Vector3 val = new Vector3(1f, (0f - tangent) / num);
				return ((Vector3)(ref val)).get_normalized();
			}
			if (!float.IsPositiveInfinity(tangent))
			{
				return Vector3.get_down();
			}
			return Vector3.get_up();
		}

		private Vector3[] BezierSegment(Keyframe start, Keyframe end)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] obj = new Vector3[4]
			{
				this.CurveToCanvas(new Vector3(((Keyframe)(ref start)).get_time(), ((Keyframe)(ref start)).get_value())),
				default(Vector3),
				default(Vector3),
				this.CurveToCanvas(new Vector3(((Keyframe)(ref end)).get_time(), ((Keyframe)(ref end)).get_value()))
			};
			float num = ((Keyframe)(ref start)).get_time() + (((Keyframe)(ref end)).get_time() - ((Keyframe)(ref start)).get_time()) * 0.333333f;
			float num2 = ((Keyframe)(ref start)).get_time() + (((Keyframe)(ref end)).get_time() - ((Keyframe)(ref start)).get_time()) * 0.666666f;
			obj[1] = this.CurveToCanvas(new Vector3(num, ProjectTangent(((Keyframe)(ref start)).get_time(), ((Keyframe)(ref start)).get_value(), ((Keyframe)(ref start)).get_outTangent(), num)));
			obj[2] = this.CurveToCanvas(new Vector3(num2, ProjectTangent(((Keyframe)(ref end)).get_time(), ((Keyframe)(ref end)).get_value(), ((Keyframe)(ref end)).get_inTangent(), num2)));
			return (Vector3[])(object)obj;
		}

		private Vector3[] HardSegment(Keyframe start, Keyframe end)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			return (Vector3[])(object)new Vector3[3]
			{
				CurveToCanvas(start),
				this.CurveToCanvas(new Vector3(((Keyframe)(ref end)).get_time(), ((Keyframe)(ref start)).get_value())),
				CurveToCanvas(end)
			};
		}

		private float ProjectTangent(float inPosition, float inValue, float inTangent, float projPosition)
		{
			return inValue + (projPosition - inPosition) * inTangent;
		}

		private float EvaluateTangent(AnimationCurve curve, float time)
		{
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < curve.get_keys().Length && time > ((Keyframe)(ref curve.get_keys()[i])).get_time(); i++)
			{
				num = i;
				num2 = i + 1;
			}
			if (num2 == 0)
			{
				return 0f;
			}
			if (num == curve.get_keys().Length - 1)
			{
				return 0f;
			}
			float num3 = Mathf.Max(time - 0.001f, ((Keyframe)(ref curve.get_keys()[num])).get_time());
			float num4 = Mathf.Min(time + 0.001f, ((Keyframe)(ref curve.get_keys()[num2])).get_time());
			float num5 = curve.Evaluate(num3);
			float num6 = curve.Evaluate(num4);
			if (Mathf.Approximately(num4, num3))
			{
				if (!(num6 - num5 > 0f))
				{
					return float.NegativeInfinity;
				}
				return float.PositiveInfinity;
			}
			return (num6 - num5) / (num4 - num3);
		}
	}
}
