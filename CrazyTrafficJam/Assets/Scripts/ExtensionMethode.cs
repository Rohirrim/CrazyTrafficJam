using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public static class ExtensionMethode
	{
		public static void Shuffle<T>(this List<T> l)
		{
			int n = l.Count;
			System.Random rng = new System.Random();
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = l[k];
				l[k] = l[n];
				l[n] = value;
			}
		}

		public static T[] GetComponentsOnlyInChildren<T>(this MonoBehaviour script, bool includeInactive = false, bool firstDepthOnly = false) where T : class
		{
			List<T> comps = new List<T>();

			if (!typeof(T).IsInterface && !typeof(T).IsSubclassOf(typeof(Component)) &&
				typeof(T) != typeof(Component))
				return comps.ToArray();

			foreach (Transform child in script.transform)
			{
				if (firstDepthOnly)
				{
					if (child.parent == script.transform)
						comps.AddRange(child.GetComponents<T>());
				}
				else
					comps.AddRange(child.GetComponentsInChildren<T>(includeInactive));
			}

			return comps.ToArray();
		}
	}
}