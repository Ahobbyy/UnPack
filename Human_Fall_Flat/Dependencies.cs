using System;
using System.Collections.Generic;
using UnityEngine;

public static class Dependencies
{
	private static List<Type> initializing = new List<Type>();

	private static Dictionary<Type, IDependency> initialized = new Dictionary<Type, IDependency>();

	public static void Initialize<T>() where T : MonoBehaviour, IDependency
	{
		if (!initialized.ContainsKey(typeof(T)))
		{
			if (initializing.Contains(typeof(T)))
			{
				throw new InvalidOperationException("Loop of initialization");
			}
			initializing.Add(typeof(T));
			T val = Object.FindObjectOfType<T>();
			if ((Object)(object)val == (Object)null)
			{
				Debug.LogErrorFormat("Dependency {0} not found", new object[1] { typeof(T) });
			}
			val.Initialize();
		}
	}

	public static void OnInitialized<T>(T dependency) where T : MonoBehaviour, IDependency
	{
		Type type = typeof(T);
		Type[] interfaces = type.GetInterfaces();
		foreach (Type key in interfaces)
		{
			initialized[key] = dependency;
		}
		while (type != null)
		{
			initialized[type] = dependency;
			type = type.BaseType;
		}
	}

	public static T Get<T>() where T : IDependency
	{
		if (!initialized.ContainsKey(typeof(T)))
		{
			throw new InvalidOperationException($"Dependency {typeof(T)} not found");
		}
		return (T)initialized[typeof(T)];
	}

	public static bool IsInitialized<T>() where T : IDependency
	{
		if (!initialized.ContainsKey(typeof(T)))
		{
			return false;
		}
		return initialized[typeof(T)] != null;
	}
}
