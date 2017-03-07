using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
	/// <summary>
	/// Helper that encapsulate using reflection to scan assemblies
	/// </summary>
	public static class ReflectionHelpers
	{
		/// <summary>
		/// Given an interface, get all types that inmplement that interface
		/// </summary>
		/// <param name="interfaceType">interface</param>
		/// <returns>List of types that implement the given interface</returns>
		public static List<Type> GetTypesThatImplementInterface(Type interfaceType)
		{
			// Get all of the assemblies
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			// In each assembly, look for the types that ipmlement the given interface
			List<Type> implementers = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				implementers.AddRange(assembly.GetTypes().Where(t => GetInterfaceImplementationType(t, interfaceType) != null));
			}

			return implementers;
		}

		/// <summary>
		/// Get the specific interface definition that the given implementation implements
		/// </summary>
		/// <param name="implementation"></param>
		/// <param name="interfaceType"></param>
		/// <returns>The specific interface definition that the given implementation implements if it exists</returns>
		public static Type GetInterfaceImplementationType(Type implementation, Type interfaceType)
		{
			return implementation.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
		}

		/// <summary>
		/// Get the generic argument types that are used in the implementation to implement the given interface
		/// </summary>
		/// <param name="implementation"></param>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		public static Type[] GetGenericArgumentTypes(Type implementation, Type interfaceType)
		{
			Type interfaceImplementationType = GetInterfaceImplementationType(implementation, interfaceType);
			if (interfaceImplementationType == null)
				throw new MissingReferenceException(string.Format("Template type {0} not implemented on generic class {1}", interfaceType, implementation));

			return implementation.GetInterface(interfaceType.Name).GetGenericArguments();
		}
	}
}
