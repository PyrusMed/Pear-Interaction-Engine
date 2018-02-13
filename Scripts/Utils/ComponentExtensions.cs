using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
	/// <summary>
	/// Extension methods for components
	/// </summary>
	static public class ComponentExtensions
    {
		/// <summary>
		/// Gets or add a component. Usage example:
		/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
		/// </summary>
		public static T GetOrAddComponent<T>(this Component child, Action<T> onAdd = null) where T : Component
        {
			T result = child.GetComponent<T>();
			if (result == null)
			{
				result = child.gameObject.AddComponentWithInit<T>(onAdd);
			}
			return result;
        }

		public static T AddComponentWithInit<T>(this GameObject child, Action<T> onAdd) where T : Component
		{
			if(onAdd == null)
			{
				return child.AddComponent<T>();
			}

			bool oldState = child.activeInHierarchy;
			child.SetActive(false);

			T comp = child.AddComponent<T>();

			if (onAdd != null)
				onAdd(comp);

			child.SetActive(oldState);

			return comp;
		}

		/// <summary>
		/// Copies values from one component to another
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="copyTo">Component to copy to</param>
		/// <param name="copyFrom">Component to copy from</param>
		/// <returns></returns>
		public static T GetCopyOf<T>(this Component copyTo, T copyFrom) where T : Component
		{
			Type type = copyTo.GetType();
			if (type != copyFrom.GetType()) return null; // type mis-match
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			IEnumerable<PropertyInfo> pinfos = type.GetProperties(flags);
			foreach (var pinfo in pinfos)
			{
				if (pinfo.CanWrite)
				{
					try
					{
						pinfo.SetValue(copyTo, pinfo.GetValue(copyFrom, null), null);
					}
					catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
				}
			}

			IEnumerable<FieldInfo> finfos = type.GetFields(flags);
			foreach (var finfo in finfos)
			{
				finfo.SetValue(copyTo, finfo.GetValue(copyFrom));
			}

			return copyTo as T;
		}

		/// <summary>
		/// Gets all scripts 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="component"></param>
		/// <returns></returns>
		public static T[] GetScripts<T>(this Component component) where T : class {
			Component[] components = component.GetComponents(typeof(T));
			return components.Select(c => (T)(object)c).ToArray();
		}
	}
}