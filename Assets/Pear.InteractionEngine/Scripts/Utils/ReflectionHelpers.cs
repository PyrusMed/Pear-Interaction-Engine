using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ReflectionHelpers {

	public static List<Type> GetTypesThatImplementInterface(Type interfaceType)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		List<Type> implementers = new List<Type>();
		foreach (Assembly assembly in assemblies)
		{
			implementers.AddRange(assembly.GetTypes().Where(t => GetInterfaceImplementationType(t, interfaceType) != null));
		}

		return implementers;
	}

	public static Type GetInterfaceImplementationType(Type implementation, Type interfaceType)
	{
		return implementation.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
	}

	public static Type[] GetGenericArgumentType(Type implementation, Type interfaceType)
	{
		Type interfaceImplementationType = GetInterfaceImplementationType(implementation, interfaceType);
		if (interfaceImplementationType == null)
			throw new MissingReferenceException(string.Format("Template type {0} not implemented on generic class {1}", interfaceType, implementation));

		return implementation.GetInterface(interfaceType.Name).GetGenericArguments();
	}
}
