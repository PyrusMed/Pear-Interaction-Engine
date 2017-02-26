using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Interactables
{
	[CustomEditor(typeof(InteractableObject))]
	public class InteractableObjectEditor : Editor
	{
		private List<Type> _changers;
		Dictionary<Type, List<Type>> _dict_actionTemplateArgToImplementations;
		Dictionary<Type, List<string>> _dict_actionTemplateArgToActionNames;

		private bool _addChanger = false;
		private int _changerClassNameDropdownIndex = -1;
		private int _actionClassNameDropdownIndex = -1;

		void OnEnable()
		{
			_changers = GetTypesThatImplementInterface(typeof(IPropertyChanger<>));
			//Dictionary<Type, List<Type>> dict_changerTemplateArgToImplementations = MapTemplateArgumentToImplementations(_changers, typeof(IPropertyChanger<>));

			List<Type> actions = GetTypesThatImplementInterface(typeof(IPropertyAction<>));
			_dict_actionTemplateArgToImplementations = MapTemplateArgumentToImplementations(actions, typeof(IPropertyAction<>));
			_dict_actionTemplateArgToActionNames = _dict_actionTemplateArgToImplementations.ToDictionary(
				kvp => kvp.Key,
				kvp =>
				{
					return kvp.Value
							.Select(actionType => actionType.FullName)
							.ToList();
				});

			Reset();
		}

		void Reset()
		{
			_addChanger = false;
			_changerClassNameDropdownIndex = 0;
			_actionClassNameDropdownIndex = 0;
	}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (GUILayout.Button("Add Interaction"))
				_addChanger = true;

			if (_addChanger)
			{
				GUILayout.BeginHorizontal("box");
				{
					EditorGUILayout.LabelField("Event", GUILayout.Width(50));
					List<string> changerClassNames = new List<string>() { "Select an event" };
					changerClassNames.AddRange(_changers.Select(t => t.FullName));
					_changerClassNameDropdownIndex = EditorGUILayout.Popup(_changerClassNameDropdownIndex, changerClassNames.ToArray());

				}
				GUILayout.EndHorizontal();
				
				if (_changerClassNameDropdownIndex > 0)
				{
					GUILayout.BeginHorizontal("box");
					{
						EditorGUILayout.LabelField("Action", GUILayout.Width(50));
						Type changer = _changers[_changerClassNameDropdownIndex - 1];
						Type templateArgument = GetGenericArgumentType(changer, typeof(IPropertyChanger<>));
						List<string> actionsForChanger;
						if (_dict_actionTemplateArgToActionNames.TryGetValue(templateArgument, out actionsForChanger))
						{
							List<string> actionClassNames = new List<string>() { "Select an action to execute when event fires" };
							actionClassNames.AddRange(actionsForChanger);
							_actionClassNameDropdownIndex = EditorGUILayout.Popup(_actionClassNameDropdownIndex, actionClassNames.ToArray());
						}
						else
						{
							string message = "There are no actions that support type " + templateArgument;
							EditorGUILayout.LabelField(new GUIContent(message, message));
						}
					}
					GUILayout.EndHorizontal();
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private List<Type> GetTypesThatImplementInterface(Type interfaceType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> implementers = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				implementers.AddRange(assembly.GetTypes().Where(t => GetInterfaceImplementationType(t, interfaceType) != null));
			}

			return implementers;
		}

		private Dictionary<Type, List<Type>> MapTemplateArgumentToImplementations(List<Type> implementations, Type interfaceType)
		{
			/*return implementations
						.GroupBy(implementation => implementation.GetGenericArguments()[0])
						.ToDictionary(val => val.Key, val => val.ToList());*/
			
			Dictionary<Type, List<Type>> dict_templateArgToImplementation = new Dictionary<Type, List<Type>>();
			foreach(Type implementation in implementations)
			{
				Type genericArgumentType = GetGenericArgumentType(implementation, interfaceType);

				List<Type> implementationsForType;
				if (!dict_templateArgToImplementation.TryGetValue(genericArgumentType, out implementationsForType))
					dict_templateArgToImplementation[genericArgumentType] = implementationsForType = new List<Type>();

				implementationsForType.Add(implementation);
			}

			return dict_templateArgToImplementation;
		}

		private Type GetInterfaceImplementationType(Type implementation, Type interfaceType)
		{
			foreach (Type i in implementation.GetInterfaces())
			{
				if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
					return i;
			}

			return null;
		}

		private Type GetGenericArgumentType(Type implementation, Type interfaceType)
		{
			Type interfaceImplementationType = GetInterfaceImplementationType(implementation, interfaceType);
			if (interfaceImplementationType == null)
				throw new MissingReferenceException(string.Format("Template type {0} not implemented on generic class {1}", interfaceType, implementation));

			return implementation.GetInterface(interfaceType.Name).GetGenericArguments()[0];
		}
	}
}