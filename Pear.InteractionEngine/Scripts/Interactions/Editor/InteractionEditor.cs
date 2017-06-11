using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Creates UI in the inspector that makes linking an Event and EventHandler easy.
	/// First, this class populates an Event dropdown with every Event in the scene
	/// Then, it populates the EventHandler dropdown with all handlers in the scene that can handle the data the Event modifies
	/// </summary>
	[CustomEditor(typeof(Interaction))]
	[CanEditMultipleObjects]
	public class InteractionEditor : Editor
	{
		// Serialized event
		private SerializedProperty _event;

		// Serialized event controller
		private SerializedProperty _eventController;

		// Serialized event handler
		private SerializedProperty _eventHandler;

        // Serialized property type
        private SerializedProperty _propertyType;

        // All events in the scene
        private List<MonoBehaviour> _events;

		// All event handlers in the scene
		private List<MonoBehaviour> _eventHandlers;

		void OnEnable()
		{
			_event = serializedObject.FindProperty("Event");
			_eventController = serializedObject.FindProperty("EventController");
			_eventHandler = serializedObject.FindProperty("EventHandler");
            _propertyType = serializedObject.FindProperty("PropertyType");

			// Get all of the events in the scene
			// Events are scripts that implement IGameObjectPropertyEvent
			// and have a Controller (implement IControllerBehavior)
			{
				List<Type> eventTypes = ReflectionHelpers.GetTypesThatImplementInterface(typeof(IGameObjectPropertyEvent<>));

				// Filter out types that do  not implement IControllerBehavior<T>
				// and warn the user when a type does not implement that interface
				Type iControllerBehavior = typeof(IControllerBehavior<>);
				IEnumerable<Type> eventsThatImplementIControllerBehavior = eventTypes.Where(eventType => {
					bool implementsIControllerBehavior = ReflectionHelpers.GetInterfaceImplementationType(eventType, iControllerBehavior) != null;
					if (!implementsIControllerBehavior)
						Debug.LogError(string.Format("{0} does not implement {1}. Consider inheriting from ControllerBehavior<T>", eventType, iControllerBehavior));
					return implementsIControllerBehavior;
				});

				_events = GetTypesInScene(eventsThatImplementIControllerBehavior);
			}
            

			// Get all of the event handlers in the scene
			// Event handlers are scripts that implement IGameObjectPropertyEventHandler
			_eventHandlers = GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IGameObjectPropertyEventHandler<>)));
		}

		/// <summary>
		/// Draw UI in the inspector
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Make sure all serialized properties are up to date
			serializedObject.Update();

			RenderEventDropdown();

			// If the user has selected an event,
			// render the event handler dropdown based on
			// the property type the event modifies 
			if(_event.objectReferenceValue != null)
				RenderEventHandlerDropdown();

			// Save any changes that were made
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Create a dropdown listing all scripts that generate events
		/// </summary>
		private void RenderEventDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Event:", GUILayout.Width(100));

				// Create an entry in the drowdown for each event in the scene
				string helpMessage = (_events.Count > 0) ? "Select an event" : "Please add an event script to the scene";
				List<string> eventsInSceneNames = new List<string>() { helpMessage };
				eventsInSceneNames.AddRange(_events.Select(c => GetNameForDropdown(c)));

				// Is an event already selected?
				// If so, make sure we select that in the dropdown
				int startIndex = 0;
				if (_event.objectReferenceValue != null)
					startIndex = _events.IndexOf((MonoBehaviour)_event.objectReferenceValue) + 1;

				// Save the current event so we know if we change it
				UnityEngine.Object lastEvent = _event.objectReferenceValue;

				// Show the dropdown and get the event the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, eventsInSceneNames.ToArray());

                // If the user selected an event (any item but the help message)
                // then save that event on our script
                if (selectedIndex > 0)
                {
                    _event.objectReferenceValue = _events[selectedIndex - 1];

					// Save the property's type
                    Type propertyType = ReflectionHelpers.GetGenericArgumentTypes(_event.objectReferenceValue.GetType(), typeof(IGameObjectPropertyEvent<>))[0];
                    _propertyType.stringValue = propertyType.AssemblyQualifiedName;

					// Save a reference to the event's controller
					_eventController.objectReferenceValue = (Controller)_event.objectReferenceValue.GetType().GetProperty("Controller").GetValue(_event.objectReferenceValue, null);
				}

				// If the event changed make sure we reset the event handler
				if (lastEvent != _event.objectReferenceValue)
					_eventHandler.objectReferenceValue = null;
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Show a dropwdown for our event handlers
		/// </summary>
		private void RenderEventHandlerDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("EventHandler", GUILayout.Width(100));

				// The selected Event deals with a specific property type (e.g. bool, string, int, etc..).
				// The associated EventHandler needs to deal with the same type.
				// Here we filter our list of EventHandlers down to those that deal with the same type as the Event
				Type templateArgument = ReflectionHelpers.GetGenericArgumentTypes(_event.objectReferenceValue.GetType(), typeof(IGameObjectPropertyEvent<>))[0];
				List<MonoBehaviour> eventHandlersInScene = _eventHandlers
					.Where(eh => ReflectionHelpers.GetGenericArgumentTypes(eh.GetType(), typeof(IGameObjectPropertyEventHandler<>))[0] == templateArgument)
					.ToList();

				// Now that we have our list of EventHandlers, create a list of names that we'll use in our dropdown
				string helpMessage = (eventHandlersInScene.Count > 0) ? "Select an event handler..." : "Please add an event handler to the scene";
				List<string> actionsInSceneNames = new List<string>() { helpMessage };
				actionsInSceneNames.AddRange(eventHandlersInScene.Select(a => GetNameForDropdown(a)));

				// Is an EventHandler already selected?
				// If so, show that in the dropdown
				int startIndex = 0;
				if (_eventHandler.objectReferenceValue != null)
					startIndex = eventHandlersInScene.IndexOf((MonoBehaviour)_eventHandler.objectReferenceValue) + 1;

				// Show the dropdown and get the index the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, actionsInSceneNames.ToArray());

				// If the user selects an EventHandler (not the help message) save it on our script
				if (selectedIndex > 0)
					_eventHandler.objectReferenceValue = eventHandlersInScene[selectedIndex - 1];
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Creates a list of all scripts in the scene that are of the given types
		/// </summary>
		/// <param name="types">types to search for</param>
		/// <returns>List of scripts that are of the given types</returns>
		private List<MonoBehaviour> GetTypesInScene(IEnumerable<Type> types)
		{
			// Make sure the scripts are not abstract base classes, and make sure they implement MonoBehavior,
			// or in other words, make sure they are scripts that can be in the scene so we don't crash Unity
			// when we search the scene hierarchy
			Type[] monobehaviors = types
				.Where(t => !t.IsAbstract && t != typeof(MonoBehaviour) && typeof(MonoBehaviour).IsAssignableFrom(t))
				.ToArray();

			List<MonoBehaviour> inScene = new List<MonoBehaviour>();
			foreach (Type scriptType in monobehaviors)
			{
				// Search the scene for objects of the given type
				UnityEngine.Object[] objectsInScene = FindObjectsOfType(scriptType);

				// Did we find any objects of the given type?
				if (objectsInScene != null)
				{

					// If so, add them to our list
					foreach (UnityEngine.Object mono in objectsInScene)
						inScene.Add((MonoBehaviour)mono);
				}
			}

			return inScene;
		}

		/// <summary>
		///	We need to display Event and EventHandlers names to users. This function formats the names
		///	to help users choose the right one
		/// </summary>
		/// <param name="mono">Event or EventHandler</param>
		/// <returns></returns>
		private string GetNameForDropdown(MonoBehaviour mono)
		{
			string hierarchy = mono.name;
			Transform parent = mono.transform.parent;
			while(parent != null)
			{
				hierarchy += " -> " + parent.name;
				parent = parent.parent;
			}

			return string.Format("{0} | {1}", ObjectNames.NicifyVariableName(mono.GetType().Name), hierarchy);
		}
	}
}
