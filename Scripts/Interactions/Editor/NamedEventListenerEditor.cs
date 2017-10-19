using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Converters;
using Pear.InteractionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Pear.InteractionEngine.Interactions;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(NamedEventListener))]
	[CanEditMultipleObjects]
	public class NamedEventListenerEditor : Editor
	{
		// Name of the event
		private SerializedProperty _eventName;

		// Serialized event listener
		private SerializedProperty _eventListener;

		// Serialized property type
		private SerializedProperty _eventListenerPropertyType;

		// When to receive the event
		private SerializedProperty _receiveEventState;

		// All event handlers in the scene
		private List<MonoBehaviour> _eventHandlers;

		private EventMap _eventMap;

		void OnEnable()
		{
			_eventName = serializedObject.FindProperty("EventName");
			_eventListener = serializedObject.FindProperty("EventListener");
			_eventListenerPropertyType = serializedObject.FindProperty("EventListenerPropertyType");
			_receiveEventState = serializedObject.FindProperty("ReceiveEventState");
			_eventMap = FindObjectOfType<EventMap>();

			if(_eventMap == null)
			{
				Debug.LogError("Please add an EventMap to your scene");
			}

			// Get all of the event handlers in the scene
			// Event handlers are scripts that implement IGameObjectPropertyEventHandler
			_eventHandlers = EditorHelpers.GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IEventListener<>)));
		}

		/// <summary>
		/// Draw UI in the inspector
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Make sure all serialized properties are up to date
			serializedObject.Update();

			RenderRecieveEventStateDropdown();

			RenderEventNameDropdown();

			if(!string.IsNullOrEmpty(_eventListenerPropertyType.stringValue))
				RenderEventListenerDropdown();

			// Save any changes that were made
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Create a dropdown listing when the object should receive events
		/// </summary>
		private void RenderRecieveEventStateDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Receive:", GUILayout.Width(100));
				_receiveEventState.enumValueIndex = EditorGUILayout.Popup(_receiveEventState.enumValueIndex, _receiveEventState.enumDisplayNames);
			}
			GUILayout.EndHorizontal();
		}

		private void RenderEventNameDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Event Name:", GUILayout.Width(100));

				Dictionary<string, Type> eventNameToType = _eventMap.EventNameToType;

				// Now that we have our list of EventHandlers, create a list of names that we'll use in our dropdown
				string helpMessage = (eventNameToType.Count > 0) ? "Select an event..." : "Please add an event to the scene";
				List<string> eventNames = new List<string>() { helpMessage };
				eventNames.AddRange(eventNameToType.Select(kvp => kvp.Key));

				// Save the current event type
				string lastEventPropertyType = _eventListenerPropertyType.stringValue;

				// Is a property type already selected?
				// If so, show that in the dropdown
				int startIndex = 0;
				if (!string.IsNullOrEmpty(_eventName.stringValue))
					startIndex = eventNames.IndexOf(_eventName.stringValue);

				// Show the dropdown and get the index the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, eventNames.ToArray());

				// If the user selects an EventHandler (not the help message) save it
				if (selectedIndex > 0)
				{
					_eventName.stringValue = eventNames[selectedIndex];
					_eventListenerPropertyType.stringValue = eventNameToType[_eventName.stringValue].AssemblyQualifiedName;
				}
				else
				{
					_eventName.stringValue = null;
					_eventListenerPropertyType.stringValue = null;
				}

				if(string.IsNullOrEmpty(_eventListenerPropertyType.stringValue) || lastEventPropertyType != _eventListenerPropertyType.stringValue)
				{
					_eventListener.objectReferenceValue = null;
				}
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Show a dropwdown for our event handlers
		/// </summary>
		private void RenderEventListenerDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("EventHandler:", GUILayout.Width(100));

				// Get a list of event listeners that are of the given type
				List<MonoBehaviour> usableListeners = _eventHandlers
					.Where(eh => ReflectionHelpers.GetGenericArgumentTypes(eh.GetType(), typeof(IEventListener<>))[0].AssemblyQualifiedName == _eventListenerPropertyType.stringValue)
					.ToList();

				// Now that we have our list of EventHandlers, create a list of names that we'll use in our dropdown
				string helpMessage = (usableListeners.Count > 0) ? "Select an event handler..." : "Please add an event handler to the scene";
				List<string> listenersInScene = new List<string>() { helpMessage };
				listenersInScene.AddRange(usableListeners.Select(a => EditorHelpers.GetNameForDropdown(a)));

				// Save the current event handler so we know if we change it
				UnityEngine.Object lastEventHandler = _eventListener.objectReferenceValue;

				// Is an EventHandler already selected?
				// If so, show that in the dropdown
				int startIndex = 0;
				if (_eventListener.objectReferenceValue != null)
					startIndex = usableListeners.IndexOf((MonoBehaviour)_eventListener.objectReferenceValue) + 1;

				// Show the dropdown and get the index the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, listenersInScene.ToArray());

				// If the user selects an EventHandler (not the help message) save it on our script
				if (selectedIndex > 0)
				{
					_eventListener.objectReferenceValue = usableListeners[selectedIndex - 1];
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
