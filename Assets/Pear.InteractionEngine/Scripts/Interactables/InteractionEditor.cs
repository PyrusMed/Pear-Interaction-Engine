using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Interactables
{
	[CustomEditor(typeof(Interaction))]
	[CanEditMultipleObjects]
	public class InteractionEditor : Editor
	{
		private SerializedProperty _event;
		private SerializedProperty _eventHandler;

		private List<MonoBehaviour> _events;
		private List<MonoBehaviour> _eventHandlers;

		void OnEnable()
		{
			_event = serializedObject.FindProperty("Event");
			_eventHandler = serializedObject.FindProperty("EventHandler");

			_events = GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IGameObjectPropertyEvent<>)));
			_eventHandlers = GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IGameObjectPropertyEventHandler<>)));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			RenderChangerDropdown();

			if(_event.objectReferenceValue != null)
			{
				RenderActionDropdown();
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void RenderChangerDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Event:", GUILayout.Width(100));

				string helpMessage = (_events.Count > 0) ? "Select an event" : "Please add an event script to the scene";
				List<string> changersInSceneNames = new List<string>() { helpMessage };
				changersInSceneNames.AddRange(_events.Select(c => GetNameForDropdown(c)));

				int startIndex = 0;
				if (_event.objectReferenceValue != null)
					startIndex = _events.IndexOf((MonoBehaviour)_event.objectReferenceValue) + 1;

				UnityEngine.Object lastEvent = _event.objectReferenceValue;

				int selectedIndex = EditorGUILayout.Popup(startIndex, changersInSceneNames.ToArray());
				if (selectedIndex > 0)
					_event.objectReferenceValue = _events[selectedIndex - 1];

				if (lastEvent != _event.objectReferenceValue)
					_eventHandler.objectReferenceValue = null;
			}
			GUILayout.EndHorizontal();
		}

		private void RenderActionDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("EventHandler", GUILayout.Width(100));

				Type templateArgument = ReflectionHelpers.GetGenericArgumentType(_event.objectReferenceValue.GetType(), typeof(IGameObjectPropertyEvent<>))[0];
				List<MonoBehaviour> actionsInScene = _eventHandlers
					.Where(eh => ReflectionHelpers.GetGenericArgumentType(eh.GetType(), typeof(IGameObjectPropertyEventHandler<>))[0] == templateArgument)
					.ToList();

				string helpMessage = (actionsInScene.Count > 0) ? "Select an event handler..." : "Please add an event handler to the scene";
				List<string> actionsInSceneNames = new List<string>() { helpMessage };
				actionsInSceneNames.AddRange(actionsInScene.Select(a => GetNameForDropdown(a)));

				int startIndex = 0;
				if (_eventHandler.objectReferenceValue != null)
					startIndex = _eventHandlers.IndexOf((MonoBehaviour)_eventHandler.objectReferenceValue) + 1;

				int selectedIndex = EditorGUILayout.Popup(startIndex, actionsInSceneNames.ToArray());
				if (selectedIndex > 0)
					_eventHandler.objectReferenceValue = actionsInScene[selectedIndex - 1];
			}
			GUILayout.EndHorizontal();
		}

		private List<MonoBehaviour> GetTypesInScene(List<Type> types)
		{
			Type[] monobehaviors = types
				.Where(t => !t.IsAbstract && t != typeof(MonoBehaviour) && typeof(MonoBehaviour).IsAssignableFrom(t))
				.ToArray();

			List<MonoBehaviour> selectables = new List<MonoBehaviour>();
			foreach (Type scriptType in monobehaviors)
			{
				UnityEngine.Object[] objectsInScene = FindObjectsOfType(scriptType);
				if (objectsInScene != null)
				{
					foreach (UnityEngine.Object mono in objectsInScene)
						selectables.Add((MonoBehaviour)mono);
				}
			}

			return selectables;
		}

		private string GetNameForDropdown(MonoBehaviour mono)
		{
			return string.Format("{0} ({1})", ObjectNames.NicifyVariableName(mono.GetType().Name), mono.name);
		}
	}
}
