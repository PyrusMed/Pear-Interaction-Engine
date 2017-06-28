using System;
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
		public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
			T result = child.GetComponent<T>();
			if (result == null)
			{
				result = child.gameObject.AddComponent<T>();
			}
			return result;
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
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			PropertyInfo[] pinfos = type.GetProperties(flags);
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

			FieldInfo[] finfos = type.GetFields(flags);
			foreach (var finfo in finfos)
			{
				finfo.SetValue(copyTo, finfo.GetValue(copyFrom));
			}

			return copyTo as T;
		}
	}
}