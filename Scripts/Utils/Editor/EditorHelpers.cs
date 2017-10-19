using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
	public static class EditorHelpers
	{
		/// <summary>
		/// Creates a list of all scripts in the scene that are of the given types
		/// </summary>
		/// <param name="types">types to search for</param>
		/// <returns>List of scripts that are of the given types</returns>
		public static List<T> GetTypesInScene<T>(IEnumerable<Type> types) where T : MonoBehaviour
		{
			// Make sure the scripts are not abstract base classes, and make sure they implement MonoBehavior,
			// or in other words, make sure they are scripts that can be in the scene so we don't crash Unity
			// when we search the scene hierarchy
			Type[] monobehaviors = types
				.Where(t => !t.IsAbstract && t != typeof(MonoBehaviour) && typeof(MonoBehaviour).IsAssignableFrom(t))
				.ToArray();

			List<T> inScene = new List<T>();
			foreach (Type scriptType in monobehaviors)
			{
				// Search the scene for objects of the given type
				UnityEngine.Object[] objectsInScene = Resources.FindObjectsOfTypeAll(scriptType);

				// Did we find any objects of the given type?
				if (objectsInScene != null)
				{
					// If so, add them to our list
					foreach (UnityEngine.Object mono in objectsInScene)
						inScene.Add((T)mono);
				}
			}

			return inScene;
		}

		/// <summary>
		/// Creates a list of all scripts in the scene that are of the given types
		/// </summary>
		/// <param name="types">types to search for</param>
		/// <returns>List of scripts that are of the given types</returns>
		public static List<MonoBehaviour> GetTypesInScene(IEnumerable<Type> types)
		{
			return GetTypesInScene<MonoBehaviour>(types);
		}

		public static List<T> GetTypeInScene<T>() where T : MonoBehaviour
		{
			return GetTypesInScene<T>(new Type[] { typeof(T) });
		}

		/// <summary>
		///	We need to display Event and EventHandlers names to users. This function formats the names
		///	to help users choose the right one
		/// </summary>
		/// <param name="mono">Event or EventHandler</param>
		/// <returns></returns>
		public static string GetNameForDropdown(MonoBehaviour mono, string scriptName = null)
		{
			string hierarchy = mono.name;
			Transform parent = mono.transform.parent;
			while (parent != null)
			{
				hierarchy += " -> " + parent.name;
				parent = parent.parent;
			}

			if (scriptName == null)
				scriptName = ObjectNames.NicifyVariableName(mono.GetType().Name);
			return string.Format("{0} | {1}", scriptName, hierarchy);
		}
	}
}
