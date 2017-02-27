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
		private Type _selectedChanger;
		private Type _selectedAction;
		private int _changerClassNameDropdownIndex = -1;
		private int _actionClassNameDropdownIndex = -1;

		void OnEnable()
		{
			_changers = GetTypesThatImplementInterface(typeof(IPropertyChanger<>));

			List<Type> actions = GetTypesThatImplementInterface(typeof(IPropertyAction<>));
			_dict_actionTemplateArgToImplementations = MapTemplateArgumentToImplementations(actions, typeof(IPropertyAction<>));
			_dict_actionTemplateArgToActionNames = _dict_actionTemplateArgToImplementations.ToDictionary(
				kvp => kvp.Key,
				kvp =>
				{
					return kvp.Value
							.Select(actionType => GetNameForDropdown(actionType, typeof(IPropertyAction<>)))
							.ToList();
				});

			Reset();
		}

		void Reset()
		{
			_selectedChanger = null;
			_selectedAction = null;
			_addChanger = false;
			_changerClassNameDropdownIndex = 0;
			_actionClassNameDropdownIndex = 0;
	}

		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Add Interaction"))
				_addChanger = true;

			if (_addChanger)
			{
				GUILayout.BeginVertical("box");
				{
					RenderChangerDropdown();

					if (_selectedChanger != null)
						RenderSelectedChangerArguments();

					if (_selectedChanger != null)
					{
						RenderActionDropdown();

						if (_selectedAction != null)
						{
							RenderSelectedActionArguments();

							GUILayout.Space(10);
							GUILayout.Button("Save Interaction");
						}
					}
				}
				GUILayout.EndVertical();
			}
		}

		private void RenderChangerDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Event", GUILayout.Width(50));
				List<string> changerClassNames = new List<string>() { "Select an event" };
				changerClassNames.AddRange(_changers.Select(t => GetNameForDropdown(t, typeof(IPropertyChanger<>))));
				_changerClassNameDropdownIndex = EditorGUILayout.Popup(_changerClassNameDropdownIndex, changerClassNames.ToArray());
				if(_changerClassNameDropdownIndex > 0)
					_selectedChanger = _changers[_changerClassNameDropdownIndex - 1];
			}
			GUILayout.EndHorizontal();
		}

		private void RenderSelectedChangerArguments()
		{
			GUILayout.BeginVertical();
			{
				SerializedFieldInfo[] changerFields = ExposeProperties.GetNonUnityProperties(_selectedChanger);
				ExposeProperties.Expose(changerFields);
			}
			GUILayout.EndVertical();
		}

		private void RenderActionDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Action", GUILayout.Width(50));
				Type templateArgument = GetGenericArgumentType(_selectedChanger, typeof(IPropertyChanger<>));
				List<string> actionsForChanger;
				if (_dict_actionTemplateArgToActionNames.TryGetValue(templateArgument, out actionsForChanger))
				{
					List<string> actionClassNames = new List<string>() { "Select an action to execute when event fires" };
					actionClassNames.AddRange(actionsForChanger);
					_actionClassNameDropdownIndex = EditorGUILayout.Popup(_actionClassNameDropdownIndex, actionClassNames.ToArray());
					if (_actionClassNameDropdownIndex > 0)
						_selectedAction = _dict_actionTemplateArgToImplementations[templateArgument][_actionClassNameDropdownIndex - 1];
				}
				else
				{
					string message = "There are no actions that support type " + templateArgument;
					EditorGUILayout.LabelField(new GUIContent(message, message));
				}
			}
			GUILayout.EndHorizontal();
		}

		private void RenderSelectedActionArguments()
		{
			GUILayout.BeginVertical();
			{
				SerializedFieldInfo[] actionFields = ExposeProperties.GetNonUnityProperties(_selectedAction);
				ExposeProperties.Expose(actionFields);
			}
			GUILayout.EndVertical();
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
			return implementations
						.GroupBy(implementation => GetGenericArgumentType(implementation, interfaceType))
						.ToDictionary(val => val.Key, val => val.ToList());
		}

		private Type GetInterfaceImplementationType(Type implementation, Type interfaceType)
		{
			return implementation.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
		}

		private Type GetGenericArgumentType(Type implementation, Type interfaceType)
		{
			Type interfaceImplementationType = GetInterfaceImplementationType(implementation, interfaceType);
			if (interfaceImplementationType == null)
				throw new MissingReferenceException(string.Format("Template type {0} not implemented on generic class {1}", interfaceType, implementation));

			return implementation.GetInterface(interfaceType.Name).GetGenericArguments()[0];
		}

		private string GetNameForDropdown(Type type, Type interfaceType)
		{
			return string.Format("{0} ({1})",
				ObjectNames.NicifyVariableName(type.Name),
				GetGenericArgumentType(type, interfaceType).Name);
		}
	}
}