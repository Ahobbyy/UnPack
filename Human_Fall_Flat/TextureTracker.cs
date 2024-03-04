using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureTracker
{
	public delegate void CleanUpFunc(Object res);

	public class TrackedResource
	{
		public Object resource;

		public CleanUpFunc cleanUp;

		public List<Object> owners = new List<Object>();

		public TrackedResource(Object res, CleanUpFunc func, Object initOwner)
		{
			resource = res;
			cleanUp = func;
			owners.Add(initOwner);
		}

		public void AddOwner(Object newOwner)
		{
			if (newOwner != (Object)null && !owners.Contains(newOwner))
			{
				owners.Add(newOwner);
			}
		}

		public int RemoveOwner(Object oldOwner)
		{
			for (int num = owners.Count - 1; num >= 0; num--)
			{
				if (owners[num] == oldOwner || owners[num] == (Object)null)
				{
					owners.RemoveAt(num);
				}
			}
			return owners.Count;
		}

		public int Prune()
		{
			for (int num = owners.Count - 1; num >= 0; num--)
			{
				if (owners[num] == (Object)null)
				{
					owners.RemoveAt(num);
				}
			}
			return owners.Count;
		}
	}

	public static TextureTracker instance = new TextureTracker();

	private List<TrackedResource> resourceList = new List<TrackedResource>();

	public static void DontUnloadAsset(Object res)
	{
	}

	public static void UnloadAsset(Object res)
	{
		if (res != (Object)null)
		{
			Resources.UnloadAsset(res);
		}
	}

	public void AddMapping(Object owner, Object resource, CleanUpFunc cleanUp)
	{
		lock (this)
		{
			if (resource == (Object)null)
			{
				return;
			}
			int count = resourceList.Count;
			for (int i = 0; i < count; i++)
			{
				if (resourceList[i].resource == resource)
				{
					resourceList[i].AddOwner(owner);
					return;
				}
			}
			resourceList.Add(new TrackedResource(resource, cleanUp, owner));
		}
	}

	public void RemoveMapping(Object owner, Object resource)
	{
		lock (this)
		{
			int num = resourceList.Count;
			for (int i = 0; i < num; i++)
			{
				TrackedResource trackedResource = resourceList[i];
				if (trackedResource.resource == (Object)null)
				{
					resourceList.RemoveAt(i);
					i--;
					num--;
					if (trackedResource.cleanUp != null)
					{
						trackedResource.cleanUp(trackedResource.resource);
					}
				}
				else if (trackedResource.resource == resource && trackedResource.RemoveOwner(owner) == 0)
				{
					resourceList.RemoveAt(i);
					if (trackedResource.cleanUp != null)
					{
						trackedResource.cleanUp(resource);
					}
					break;
				}
			}
		}
	}

	public int GetNumOwners(Object resource)
	{
		lock (this)
		{
			if (resource != (Object)null)
			{
				int count = resourceList.Count;
				for (int i = 0; i < count; i++)
				{
					if (resourceList[i].resource == resource)
					{
						return resourceList[i].owners.Count;
					}
				}
			}
			return 0;
		}
	}

	public void Dump()
	{
		lock (this)
		{
			Console.WriteLine("TextureTracker list:");
			int count = resourceList.Count;
			for (int i = 0; i < count; i++)
			{
				TrackedResource trackedResource = resourceList[i];
				Console.WriteLine("  {0}: id={4}, owners={1}, type={2}, valid={3}", i, trackedResource.owners.Count, (trackedResource.resource != (Object)null) ? ((object)trackedResource.resource).GetType().Name : "?", trackedResource.resource != (Object)null, (trackedResource.resource != (Object)null) ? trackedResource.resource.GetInstanceID() : (-1));
			}
		}
	}
}
