using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Checks whether the given object derives from the generic type
		/// </summary>
		/// <param name="objToCheck">Object to check</param>
		/// <param name="generic">Generic type to check against</param>
		/// <returns>True if object derives from generic. False otherwise.</returns>
		public static bool DerivesFromGeneric<T>(this T objToCheck, Type generic)
		{
			if (objToCheck == null)
				return false;

			Type typeToCheck = objToCheck.GetType();
			while (typeToCheck != typeof(object))
			{
				Type cur = typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;
				if (generic == cur)
				{
					return true;
				}

				typeToCheck = typeToCheck.BaseType;
			}

			return false;
		}
	}
}
