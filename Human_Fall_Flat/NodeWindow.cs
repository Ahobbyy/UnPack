using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HumanAPI;
using UnityEditor;
using UnityEngine;

public class NodeWindow : EditorWindow
{
	public NodeGraph activeGraph;

	private List<Node> graphNodes;

	private List<NodeRect> nodes = new List<NodeRect>();

	private Dictionary<NodeSocket, NodeSocketRect> sockets = new Dictionary<NodeSocket, NodeSocketRect>();

	private NodeGraph pendingGraphRebuildNode;

	private NodePropWindowBase propWindowHelper = new NodePropWindowBase();

	private SplitterPanel divider;

	private int grid = 32;

	private float width = 1650f;

	private float height = 1100f;

	private NodeRect selectedNode;

	private NodeRect pendingSelect;

	private bool inRender;

	private bool refreshProperties;

	public static Sprite circle;

	private Vector2 scrollPos;

	public const float boxWidth = 150f;

	public const float signalHeight = 16f;

	public const float headerHeight = 16f;

	public const float classHeight = 20f;

	public const float footerHeight = 4f;

	private bool liveUpdate;

	private bool trackSelection = true;

	private bool updateSelection;

	private GenericMenu componentMenu;

	private Rect componentMenuRect;

	private bool scrolling;

	private bool dragging;

	private Vector2 dragStart;

	private Vector2 dragStop;

	public static NodeWindow Init(NodeGraph graph)
	{
		NodeWindow obj = EditorWindow.GetWindow(typeof(NodeWindow)) as NodeWindow;
		obj.activeGraph = graph;
		obj.OnEnable();
		return obj;
	}

	public void ChangeSelectedNode(NodeRect newNode)
	{
		refreshProperties = true;
		if (inRender)
		{
			pendingSelect = newNode;
			return;
		}
		pendingSelect = null;
		selectedNode = newNode;
		((EditorWindow)this).Repaint();
	}

	public void RebuildGraph(NodeGraph graph)
	{
		pendingGraphRebuildNode = graph;
	}

	public void RebuildGraphInternal(NodeGraph graph)
	{
		Transform transform = ((Component)graph).get_transform();
		graphNodes = new List<Node>();
		CollectNodes(graphNodes, graph, transform);
		Object[] array = (Object[])(object)new Object[graphNodes.Count];
		for (int i = 0; i < graphNodes.Count; i++)
		{
			array[i] = (Object)(object)graphNodes[i];
		}
		Undo.RecordObjects(array, "rebuild graph");
		for (int j = 0; j < graphNodes.Count; j++)
		{
			graphNodes[j].RebuildSockets();
		}
		nodes.Clear();
		NodeRect newNode = null;
		for (int k = 0; k < graphNodes.Count; k++)
		{
			Node node = graphNodes[k];
			if ((Object)(object)node == (Object)(object)activeGraph)
			{
				NodeRect nodeRect = new NodeRect(this);
				nodeRect.InitializeGraphInput(activeGraph);
				nodes.Add(nodeRect);
				NodeRect nodeRect2 = new NodeRect(this);
				nodeRect2.InitializeGraphOutput(activeGraph);
				nodes.Add(nodeRect2);
				if (selectedNode != null && (Object)(object)selectedNode.node != (Object)null && (Object)(object)selectedNode.node == (Object)(object)activeGraph)
				{
					if (selectedNode.Type == NodeRect.NodeRectType.GraphInputs)
					{
						newNode = nodeRect;
					}
					else if (selectedNode.Type == NodeRect.NodeRectType.GraphOutputs)
					{
						newNode = nodeRect2;
					}
				}
			}
			else if (((object)node).GetType() == typeof(NodeComment))
			{
				NodeRect nodeRect3 = new NodeRect(this);
				nodeRect3.InitializeComment(node);
				nodes.Add(nodeRect3);
				if (selectedNode != null && (Object)(object)selectedNode.node != (Object)null && (Object)(object)selectedNode.node == (Object)(object)node)
				{
					newNode = nodeRect3;
				}
			}
			else
			{
				NodeRect nodeRect4 = new NodeRect(this);
				nodeRect4.Initialize(node);
				nodes.Add(nodeRect4);
				if (selectedNode != null && (Object)(object)selectedNode.node != (Object)null && (Object)(object)selectedNode.node == (Object)(object)node)
				{
					newNode = nodeRect4;
				}
			}
		}
		ChangeSelectedNode(newNode);
		sockets.Clear();
		for (int l = 0; l < nodes.Count; l++)
		{
			NodeRect nodeRect5 = nodes[l];
			nodeRect5.UpdateLayout();
			for (int m = 0; m < nodeRect5.sockets.Count; m++)
			{
				sockets[nodeRect5.sockets[m].socket] = nodeRect5.sockets[m];
			}
		}
	}

	private static void CollectNodes(List<Node> nodes, NodeGraph graph, Transform t)
	{
		if (!((Component)t).get_gameObject().get_activeSelf())
		{
			return;
		}
		NodeGraph component = ((Component)t).GetComponent<NodeGraph>();
		if ((Object)(object)component != (Object)null && (Object)(object)component != (Object)(object)graph)
		{
			nodes.Add(component);
			return;
		}
		Node[] components = ((Component)t).GetComponents<Node>();
		foreach (Node item in components)
		{
			nodes.Add(item);
		}
		for (int j = 0; j < t.get_childCount(); j++)
		{
			CollectNodes(nodes, graph, t.GetChild(j));
		}
	}

	private void OnEnable()
	{
		if ((Object)(object)circle == (Object)null)
		{
			circle = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
		}
		if ((Object)(object)activeGraph != (Object)null)
		{
			RebuildGraph(activeGraph);
		}
		dragging = false;
		((EditorWindow)this).Repaint();
	}

	private void UpdateFromSelection()
	{
		if (!updateSelection)
		{
			return;
		}
		updateSelection = false;
		if ((Object)(object)Selection.get_activeGameObject() != (Object)null)
		{
			NodeGraph componentInParent = Selection.get_activeGameObject().GetComponentInParent<NodeGraph>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				Init(componentInParent);
			}
		}
	}

	private void SidePanel(Event e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginArea(divider.RectPaneA);
		if (GUILayout.Button(((Object)activeGraph).get_name(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Selection.set_activeGameObject(((Component)activeGraph).get_gameObject());
		}
		liveUpdate = GUILayout.Toggle(liveUpdate, "Live Update", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		bool flag = GUILayout.Toggle(trackSelection, "Track Selection", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (!trackSelection && flag)
		{
			trackSelection = true;
			updateSelection = true;
		}
		trackSelection = flag;
		UpdateFromSelection();
		if (GUILayout.Button("Refresh", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			RebuildGraphInternal(activeGraph);
		}
		if (GUILayout.Button("▲ Up", (GUILayoutOption[])(object)new GUILayoutOption[0]) && Object.op_Implicit((Object)(object)((Component)activeGraph).get_transform().get_parent()))
		{
			NodeGraph componentInParent = ((Component)((Component)activeGraph).get_transform().get_parent()).GetComponentInParent<NodeGraph>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				Init(componentInParent);
			}
		}
		if (GUILayout.Button("Add Comment", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			((Component)activeGraph).get_gameObject().AddComponent<NodeComment>();
			RebuildGraphInternal(activeGraph);
		}
		bool flag2 = EditorGUILayout.DropdownButton(new GUIContent("Quick Add Node"), (FocusType)1, (GUILayoutOption[])null);
		if (GUILayout.Button("Snap to grid", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			foreach (NodeRect node in nodes)
			{
				int num = (int)(node.nodePos.x / (float)grid + 0.5f) * grid;
				int num2 = (int)(node.nodePos.y / (float)grid + 0.5f) * grid;
				((Rect)(ref node.rect)).set_position(new Vector2((float)num, (float)num2));
			}
		}
		if ((int)e.get_type() == 7)
		{
			componentMenuRect = GUILayoutUtility.GetLastRect();
		}
		if (flag2)
		{
			if (componentMenu == null)
			{
				CreateComponentMenu();
			}
			componentMenu.DropDown(componentMenuRect);
		}
		NodePropWindowBase nodePropWindowBase = propWindowHelper;
		List<Node> list = graphNodes;
		Node activeNode = ((selectedNode != null) ? selectedNode.node : null);
		bool forceSelectionChanged = refreshProperties;
		Rect rectPaneA = divider.RectPaneA;
		nodePropWindowBase.DrawProps(list, activeNode, forceSelectionChanged, ((Rect)(ref rectPaneA)).get_width());
		refreshProperties = false;
		GUILayout.EndArea();
	}

	private void CreateComponentMenu()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f7: Expected O, but got Unknown
		Type[] array = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
			from assemblyType in domainAssembly.GetTypes()
			where assemblyType.IsSubclassOf(typeof(Node)) && !assemblyType.IsAbstract && Attribute.IsDefined(assemblyType, typeof(AddNodeMenuItem))
			select assemblyType).ToArray();
		Array.Sort(array, (Type a, Type b) => a.Name.CompareTo(b.Name));
		componentMenu = new GenericMenu();
		Type[] array2 = array;
		foreach (Type type in array2)
		{
			componentMenu.AddItem(new GUIContent(type.Name), false, new MenuFunction2(AddComponentMenuCallback), (object)type);
		}
	}

	private void AddComponentMenuCallback(object data)
	{
		Type type = (Type)data;
		((Component)activeGraph).get_gameObject().AddComponent(type);
	}

	private void OnGUI()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		Event current = Event.get_current();
		if ((int)current.get_type() == 0 && current.get_button() == 2)
		{
			scrolling = true;
			return;
		}
		if (((int)current.get_type() == 1 || (int)current.get_rawType() == 1) && current.get_button() == 2)
		{
			scrolling = false;
			((EditorWindow)this).Repaint();
			return;
		}
		if ((int)current.get_type() == 3 && scrolling)
		{
			scrollPos -= current.get_delta();
			((EditorWindow)this).Repaint();
		}
		if ((Object)(object)pendingGraphRebuildNode != (Object)null)
		{
			RebuildGraphInternal(pendingGraphRebuildNode);
			pendingGraphRebuildNode = null;
		}
		if (trackSelection && (Object)(object)activeGraph == (Object)null)
		{
			updateSelection = true;
			UpdateFromSelection();
		}
		if ((Object)(object)activeGraph == (Object)null)
		{
			return;
		}
		if (liveUpdate && graphNodes != null)
		{
			Transform transform = ((Component)activeGraph).get_transform();
			List<Node> first = new List<Node>();
			CollectNodes(first, activeGraph, transform);
			if (!first.SequenceEqual(graphNodes))
			{
				RebuildGraphInternal(activeGraph);
			}
		}
		Rect position;
		if (divider == null)
		{
			divider = new SplitterPanel();
			SplitterPanelBase.InitialConfig config = default(SplitterPanelBase.InitialConfig);
			config.UpDownMode = false;
			config.SwitchPanels = false;
			config.CanSnapAClosed = true;
			config.Collapse = SplitterPanelBase.CollapseMode.Normal;
			config.PaneASize = 150f;
			position = ((EditorWindow)this).get_position();
			float num = ((Rect)(ref position)).get_width();
			position = ((EditorWindow)this).get_position();
			config.InitialBounds = new Rect(0f, 0f, num, ((Rect)(ref position)).get_height());
			config.MinSizeA = 213f;
			config.MinSizeB = 64f;
			divider.InitState(config);
		}
		SplitterPanel splitterPanel = divider;
		position = ((EditorWindow)this).get_position();
		float num2 = ((Rect)(ref position)).get_width();
		position = ((EditorWindow)this).get_position();
		splitterPanel.Bounds = new Rect(0f, 0f, num2, ((Rect)(ref position)).get_height());
		if (divider.OnGUI())
		{
			((EditorWindow)this).Repaint();
			return;
		}
		if (divider.DefaultCollapeButtons(16f, 2f, 17f))
		{
			((EditorWindow)this).Repaint();
			return;
		}
		if (!divider.DrawPaneB)
		{
			scrollPos = Vector2.get_zero();
		}
		else
		{
			inRender = true;
			pendingSelect = selectedNode;
			GUILayout.BeginArea(divider.RectPaneB);
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Color color = GUI.get_color();
			GUI.set_color(new Color(0.1f, 0.1f, 0.1f));
			GUI.set_color(color);
			GUILayout.Label("", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(width),
				GUILayout.Height(height)
			});
			GUI.set_color(new Color(0.5f, 0.5f, 0.5f, 0.15f));
			for (int i = 0; (float)i < width; i += grid)
			{
				GUI.DrawTexture(new Rect((float)i, 0f, 1f, height), (Texture)(object)Texture2D.get_whiteTexture(), (ScaleMode)0);
			}
			for (int j = 0; (float)j < height; j += grid)
			{
				GUI.DrawTexture(new Rect(0f, (float)j, width, 1f), (Texture)(object)Texture2D.get_whiteTexture(), (ScaleMode)0);
			}
			GUI.set_color(Color.get_white());
			if (scrolling)
			{
				EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), (MouseCursor)13);
			}
			DrawConnections();
			DropAreaGUI();
			Color backgroundColor = GUI.get_backgroundColor();
			((EditorWindow)this).BeginWindows();
			for (int k = 0; k < nodes.Count; k++)
			{
				NodeRect nodeRect = nodes[k];
				if (nodeRect.sockets.Count == 0 && nodeRect.Type != NodeRect.NodeRectType.Comment)
				{
					continue;
				}
				try
				{
					bool drawHighlight = nodeRect == selectedNode;
					if (!nodeRect.RenderWindow(k, drawHighlight))
					{
						RebuildGraph(activeGraph);
					}
				}
				catch
				{
					RebuildGraph(activeGraph);
				}
			}
			((EditorWindow)this).EndWindows();
			GUI.set_backgroundColor(backgroundColor);
			if (dragging)
			{
				DrawCurve(Vector2.op_Implicit(dragStart), Vector2.op_Implicit(dragStop), Color.get_white());
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndArea();
			inRender = false;
			if (pendingSelect != selectedNode)
			{
				ChangeSelectedNode(pendingSelect);
			}
			pendingSelect = null;
		}
		if (divider.DrawPaneA)
		{
			SidePanel(current);
		}
	}

	private void OnSelectionChange()
	{
		if (trackSelection)
		{
			updateSelection = true;
			((EditorWindow)this).Repaint();
		}
	}

	private void Update()
	{
		if (liveUpdate && EditorApplication.get_isPlaying())
		{
			((EditorWindow)this).Repaint();
		}
	}

	private void DrawConnections()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < nodes.Count; i++)
		{
			for (int j = 0; j < nodes[i].sockets.Count; j++)
			{
				NodeSocketRect nodeSocketRect = nodes[i].sockets[j];
				NodeInput nodeInput = nodeSocketRect.socket as NodeInput;
				if (nodeInput == null || nodeInput.GetConnectedOutput() == null)
				{
					continue;
				}
				NodeSocketRect value = null;
				if (sockets.TryGetValue(nodeInput.GetConnectedOutput(), out value))
				{
					if (nodeInput is NodeExit)
					{
						DrawCurve(Vector2.op_Implicit(nodeSocketRect.connectPoint), Vector2.op_Implicit(value.connectPoint), Color.get_white());
					}
					else
					{
						DrawCurve(Vector2.op_Implicit(value.connectPoint), Vector2.op_Implicit(nodeSocketRect.connectPoint), new Color(0.7f, 0.7f, 1f));
					}
				}
			}
		}
	}

	public void DropAreaGUI()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Invalid comparison between Unknown and I4
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Invalid comparison between Unknown and I4
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Invalid comparison between Unknown and I4
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		Event current = Event.get_current();
		EventType type = current.get_type();
		if ((int)type != 0)
		{
			if (type - 9 > 1)
			{
				if ((int)type == 15)
				{
					dragging = false;
				}
				return;
			}
			object genericData = DragAndDrop.GetGenericData("socket");
			NodeInput nodeInput = genericData as NodeInput;
			NodeOutput nodeOutput = genericData as NodeOutput;
			NodeExit nodeExit = genericData as NodeExit;
			NodeEntry nodeEntry = genericData as NodeEntry;
			if (nodeInput == null && nodeOutput == null && nodeEntry == null && nodeExit == null)
			{
				return;
			}
			NodeSocketRect nodeSocketRect = null;
			for (int i = 0; i < nodes.Count; i++)
			{
				nodeSocketRect = nodes[i].HitTest(current.get_mousePosition());
				if (nodeSocketRect != null)
				{
					break;
				}
			}
			bool flag = false;
			if (nodeSocketRect != null)
			{
				if (nodeInput != null)
				{
					NodeOutput nodeOutput2 = nodeSocketRect.socket as NodeOutput;
					if (nodeOutput2 != null)
					{
						flag = nodeInput.CanConnect(nodeOutput2);
						if (flag && (int)current.get_type() == 10)
						{
							DragAndDrop.AcceptDrag();
							Undo.RecordObjects((Object[])(object)new Object[2]
							{
								(Object)nodeInput.node,
								(Object)nodeOutput2.node
							}, "Connect");
							nodeInput.Connect(nodeOutput2);
						}
					}
				}
				else if (nodeOutput != null)
				{
					NodeInput nodeInput2 = nodeSocketRect.socket as NodeInput;
					if (nodeInput2 != null)
					{
						flag = nodeInput2.CanConnect(nodeOutput);
						if (flag && (int)current.get_type() == 10)
						{
							DragAndDrop.AcceptDrag();
							Undo.RecordObjects((Object[])(object)new Object[2]
							{
								(Object)nodeInput2.node,
								(Object)nodeOutput.node
							}, "Connect");
							nodeInput2.Connect(nodeOutput);
						}
					}
				}
			}
			DragAndDrop.set_visualMode((DragAndDropVisualMode)(flag ? 2 : 32));
			dragging = (int)current.get_type() == 9;
			if (nodeInput != null)
			{
				dragStart = current.get_mousePosition();
			}
			else
			{
				dragStop = current.get_mousePosition();
			}
			((EditorWindow)this).Repaint();
		}
		else
		{
			if (current.get_button() != 0)
			{
				return;
			}
			for (int j = 0; j < nodes.Count; j++)
			{
				NodeSocketRect nodeSocketRect2 = nodes[j].HitTest(current.get_mousePosition());
				if (nodeSocketRect2 != null)
				{
					DragAndDrop.PrepareStartDrag();
					DragAndDrop.SetGenericData("socket", (object)nodeSocketRect2.socket);
					NodeInput nodeInput3 = nodeSocketRect2.socket as NodeInput;
					if (nodeInput3 != null)
					{
						Undo.RecordObject((Object)(object)nodeInput3.node, "Disconnect");
						nodeInput3.Connect(null);
					}
					dragStart = (dragStop = nodeSocketRect2.connectPoint);
					DragAndDrop.set_paths((string[])null);
					DragAndDrop.set_objectReferences((Object[])(object)new Object[0]);
					DragAndDrop.StartDrag("Dragging connector");
					Event.get_current().Use();
					dragging = true;
					break;
				}
			}
		}
	}

	public static void DrawTextureGUI(Rect rect, Sprite sprite)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Rect rect2 = sprite.get_rect();
		float num = ((Rect)(ref rect2)).get_x() / (float)((Texture)sprite.get_texture()).get_width();
		rect2 = sprite.get_rect();
		float num2 = ((Rect)(ref rect2)).get_y() / (float)((Texture)sprite.get_texture()).get_height();
		rect2 = sprite.get_rect();
		float num3 = ((Rect)(ref rect2)).get_width() / (float)((Texture)sprite.get_texture()).get_width();
		rect2 = sprite.get_rect();
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(num, num2, num3, ((Rect)(ref rect2)).get_height() / (float)((Texture)sprite.get_texture()).get_height());
		Vector2 size = ((Rect)(ref rect)).get_size();
		ref float y = ref size.y;
		float num4 = y;
		rect2 = sprite.get_rect();
		float num5 = ((Rect)(ref rect2)).get_height();
		rect2 = sprite.get_rect();
		y = num4 * (num5 / ((Rect)(ref rect2)).get_width());
		GUI.DrawTextureWithTexCoords(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y() + (((Rect)(ref rect)).get_height() - size.y) / 2f, size.x, size.y), (Texture)(object)sprite.get_texture(), val);
	}

	private void DrawNodeCurve(Rect start, Rect end, Color color)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Vector3 startPos = new Vector3(((Rect)(ref start)).get_x() + ((Rect)(ref start)).get_width(), ((Rect)(ref start)).get_y() + ((Rect)(ref start)).get_height() / 2f, 0f);
		Vector3 endPos = default(Vector3);
		((Vector3)(ref endPos))._002Ector(((Rect)(ref end)).get_x(), ((Rect)(ref end)).get_y() + ((Rect)(ref end)).get_height() / 2f, 0f);
		DrawCurve(startPos, endPos, color);
	}

	private static void DrawCurve(Vector3 startPos, Vector3 endPos, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = startPos + Vector3.get_right() * 50f;
		Vector3 val2 = endPos + Vector3.get_left() * 50f;
		Color val3 = default(Color);
		((Color)(ref val3))._002Ector(0f, 0f, 0f, 0.06f);
		for (int i = 0; i < 3; i++)
		{
			Handles.DrawBezier(startPos, endPos, val, val2, val3, (Texture2D)null, (float)((i + 1) * 5));
		}
		Handles.DrawBezier(startPos, endPos, val, val2, color, (Texture2D)null, 2f);
	}

	public NodeWindow()
		: this()
	{
	}
}
