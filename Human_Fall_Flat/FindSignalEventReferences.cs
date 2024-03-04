using HumanAPI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class FindSignalEventReferences
{
	[MenuItem("GameObject/Find Signal Event Refs", false, 2)]
	private static void FindRefs()
	{
		GameObject activeGameObject = Selection.get_activeGameObject();
		SignalUnityEvent[] array = Object.FindObjectsOfType<SignalUnityEvent>();
		foreach (SignalUnityEvent signalUnityEvent in array)
		{
			if (DoesEventReferenceObject(signalUnityEvent.triggerEvent, activeGameObject) || DoesEventReferenceObject(signalUnityEvent.resetEvent, activeGameObject))
			{
				Debug.Log((object)(((Object)activeGameObject).get_name() + " referenced by " + ((Object)signalUnityEvent).get_name() + " (click to select)."), (Object)(object)signalUnityEvent);
			}
		}
	}

	private static bool DoesEventReferenceObject(UnityEvent e, GameObject obj)
	{
		for (int i = 0; i < ((UnityEventBase)e).GetPersistentEventCount(); i++)
		{
			Object persistentTarget = ((UnityEventBase)e).GetPersistentTarget(i);
			GameObject val = (GameObject)(object)((persistentTarget is GameObject) ? persistentTarget : null);
			if ((Object)(object)val == (Object)null)
			{
				Component val2 = (Component)(object)((persistentTarget is Component) ? persistentTarget : null);
				if ((Object)(object)val2 != (Object)null)
				{
					val = val2.get_gameObject();
				}
			}
			if ((Object)(object)val != (Object)null && (Object)(object)val == (Object)(object)obj)
			{
				return true;
			}
		}
		return false;
	}
}
