using System.Collections.Generic;
using HumanAPI;
using UnityEditor;
using UnityEngine;

public class NodeRect
{
	public enum NodeRectType
	{
		Node,
		GraphInputs,
		GraphOutputs,
		Comment
	}

	public Rect rect;

	public Node node;

	public List<NodeSocketRect> sockets = new List<NodeSocketRect>();

	private NodeRectType type;

	public NodeWindow parentWindow;

	public NodeRectType Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public Vector2 nodePos
	{
		get
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = (Vector2)(type switch
			{
				NodeRectType.GraphInputs => (node as NodeGraph).inputsPos, 
				NodeRectType.GraphOutputs => (node as NodeGraph).outputsPos, 
				_ => node.pos, 
			});
			val.x = Mathf.Clamp(val.x, 0f, 1500f);
			val.y = Mathf.Clamp(val.y, 0f, 1000f);
			return val;
		}
		set
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			switch (type)
			{
			case NodeRectType.GraphInputs:
				(node as NodeGraph).inputsPos = value;
				break;
			case NodeRectType.GraphOutputs:
				(node as NodeGraph).outputsPos = value;
				break;
			default:
				node.pos = value;
				break;
			}
		}
	}

	public NodeRect(NodeWindow win)
	{
		parentWindow = win;
	}

	public void Initialize(Node node)
	{
		type = NodeRectType.Node;
		this.node = node;
		sockets.Clear();
		List<NodeSocket> list = node.ListNodeSockets();
		for (int i = 0; i < list.Count; i++)
		{
			sockets.Add(new NodeSocketRect
			{
				nodeRect = this,
				socket = list[i]
			});
		}
	}

	public void InitializeGraphInput(NodeGraph node)
	{
		type = NodeRectType.GraphInputs;
		this.node = node;
		sockets.Clear();
		for (int i = 0; i < node.inputs.Count; i++)
		{
			sockets.Add(new NodeSocketRect
			{
				nodeRect = this,
				socket = node.inputs[i].inputSocket,
				allowEdit = true
			});
		}
	}

	public void InitializeGraphOutput(NodeGraph node)
	{
		type = NodeRectType.GraphOutputs;
		this.node = node;
		sockets.Clear();
		for (int i = 0; i < node.outputs.Count; i++)
		{
			sockets.Add(new NodeSocketRect
			{
				nodeRect = this,
				socket = node.outputs[i].outputSocket,
				allowEdit = true
			});
		}
	}

	public void InitializeComment(Node node)
	{
		type = NodeRectType.Comment;
		this.node = node;
		sockets.Clear();
	}

	public void UpdateLayout()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		float num = 150f;
		float num2 = 20f;
		if (type == NodeRectType.Comment)
		{
			Vector2 val = GUI.get_skin().get_label().CalcSize(new GUIContent(((NodeComment)node).comment));
			num = Mathf.Max(num, 32f + val.x);
			num2 = val.y;
		}
		rect = new Rect(nodePos.x, nodePos.y, num, 16f + num2 + 4f + (float)sockets.Count * 16f);
		float num3 = 36f;
		for (int i = 0; i < sockets.Count; i++)
		{
			sockets[i].UpdateLayout(new Rect(0f, num3, num, 16f));
			num3 += 16f;
		}
	}

	public bool RenderWindow(int id, bool drawHighlight)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)node == (Object)null)
		{
			return false;
		}
		switch (type)
		{
		case NodeRectType.Node:
			if (node is NodeGraph)
			{
				GUI.set_backgroundColor(new Color(1f, 0.9f, 0.7f));
			}
			else
			{
				GUI.set_backgroundColor(node.nodeColour);
			}
			break;
		case NodeRectType.Comment:
			GUI.set_backgroundColor(Color.get_green());
			break;
		case NodeRectType.GraphInputs:
		case NodeRectType.GraphOutputs:
			GUI.set_backgroundColor(new Color(1f, 0.7f, 0.7f));
			break;
		}
		rect = GUI.Window(id, rect, new WindowFunction(Render), node.Title);
		if (nodePos != ((Rect)(ref rect)).get_position())
		{
			Undo.RecordObject((Object)(object)node, "Move");
			nodePos = ((Rect)(ref rect)).get_position();
			UpdateLayout();
		}
		if (drawHighlight)
		{
			DrawRing(rect);
		}
		return true;
	}

	private void DrawRing(Rect box)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		Rect input = ExpandBox(box, 4f, 4f);
		Rect val = ExpandBox(input, 4f, 4f);
		Color val2 = default(Color);
		((Color)(ref val2))._002Ector(1f, 0.5f, 0.3f, 0.5f);
		EditorGUI.DrawRect(Rect.MinMaxRect(((Rect)(ref val)).get_xMin(), ((Rect)(ref val)).get_yMin(), ((Rect)(ref input)).get_xMin(), ((Rect)(ref val)).get_yMax()), val2);
		EditorGUI.DrawRect(Rect.MinMaxRect(((Rect)(ref input)).get_xMax(), ((Rect)(ref val)).get_yMin(), ((Rect)(ref val)).get_xMax(), ((Rect)(ref val)).get_yMax()), val2);
		EditorGUI.DrawRect(Rect.MinMaxRect(((Rect)(ref input)).get_xMin(), ((Rect)(ref val)).get_yMin(), ((Rect)(ref input)).get_xMax(), ((Rect)(ref input)).get_yMin()), val2);
		EditorGUI.DrawRect(Rect.MinMaxRect(((Rect)(ref input)).get_xMin(), ((Rect)(ref input)).get_yMax(), ((Rect)(ref input)).get_xMax(), ((Rect)(ref val)).get_yMax()), val2);
	}

	private Rect ExpandBox(Rect input, float sizeX, float sizeY)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		((Rect)(ref input)).set_x(((Rect)(ref input)).get_x() - sizeX);
		((Rect)(ref input)).set_y(((Rect)(ref input)).get_y() - sizeY);
		((Rect)(ref input)).set_width(((Rect)(ref input)).get_width() + sizeX * 2f);
		((Rect)(ref input)).set_height(((Rect)(ref input)).get_height() + sizeY * 2f);
		return input;
	}

	public void Render(int id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		GUI.set_color(Color.get_gray());
		GUI.get_skin().get_label().set_alignment((TextAnchor)1);
		if (node is NodeGraph && type == NodeRectType.Node)
		{
			if (GUI.Button(new Rect(8f, 16f, 102f, 20f), ((Object)node).get_name()))
			{
				Selection.set_activeGameObject(((Component)node).get_gameObject());
				if ((Object)(object)parentWindow != (Object)null)
				{
					parentWindow.ChangeSelectedNode(this);
				}
			}
			if (GUI.Button(new Rect(110f, 16f, 32f, 20f), "▼"))
			{
				if ((Object)(object)parentWindow != (Object)null)
				{
					parentWindow.ChangeSelectedNode(this);
				}
				NodeWindow.Init(node as NodeGraph);
			}
		}
		else if (type == NodeRectType.Comment)
		{
			GUI.set_color(Color.get_green());
			string text = GUI.TextArea(new Rect(8f, 16f, ((Rect)(ref rect)).get_width() - 16f, ((Rect)(ref rect)).get_height() - 20f), ((NodeComment)node).comment);
			if (text != ((NodeComment)node).comment)
			{
				((NodeComment)node).comment = text;
				UpdateLayout();
			}
		}
		else if (GUI.Button(new Rect(8f, 16f, 134f, 20f), ((Object)node).get_name()))
		{
			Selection.set_activeGameObject(((Component)node).get_gameObject());
			if ((Object)(object)parentWindow != (Object)null)
			{
				parentWindow.ChangeSelectedNode(this);
			}
		}
		GUI.get_skin().get_label().set_alignment((TextAnchor)0);
		GUI.set_color(Color.get_white());
		for (int i = 0; i < sockets.Count; i++)
		{
			sockets[i].Render();
		}
		GUI.DragWindow();
	}

	public NodeSocketRect HitTest(Vector2 pos)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		pos.x -= ((Rect)(ref rect)).get_x();
		pos.y -= ((Rect)(ref rect)).get_y();
		for (int i = 0; i < sockets.Count; i++)
		{
			if (sockets[i].HitTest(pos))
			{
				return sockets[i];
			}
		}
		return null;
	}
}
