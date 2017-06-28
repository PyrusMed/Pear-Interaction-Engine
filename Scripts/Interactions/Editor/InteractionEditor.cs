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

		// Serialized value converter
		private SerializedProperty _valueConverter;

		// Serialized event controller
		private SerializedProperty _eventController;

		// Serialized event handler
		private SerializedProperty _eventHandler;

        // Serialized property type
        private SerializedProperty _eventPropertyType;

		// Serialized property type
		private SerializedProperty _eventHandlerPropertyType;

		private SerializedProperty _receiveEventState;

		// All events in the scene
		private List<MonoBehaviour> _events;

		// All converters in the scene
		private List<MonoBehaviour> _valueConverters;

		// All event handlers in the scene
		private List<MonoBehaviour> _eventHandlers;

		void OnEnable()
		{
			_event = serializedObject.FindProperty("Event");
			_valueConverter = serializedObject.FindProperty("ValueConverter");
			_eventController = serializedObject.FindProperty("EventController");
			_eventHandler = serializedObject.FindProperty("EventHandler");
            _eventPropertyType = serializedObject.FindProperty("EventPropertyType");
			_eventHandlerPropertyType = serializedObject.FindProperty("EventHandlerPropertyType");
			_receiveEventState = serializedObject.FindProperty("ReceiveEventState");

			// Get all of the events in the scene
			// Events are scripts that implement IGameObjectPropertyEvent
			// and have a Controller (implement IControllerBehavior)
			{
				List<Type> eventTypes = ReflectionHelpers.GetTypesThatImplementInterface(typeof(IEvent<>));

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

			// Get all of the value converters in the scene
			// Value converters are scripts that implement IPropertyConverter<,>
			_valueConverters = GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IPropertyConverter<,>)));

			// Get all of the event handlers in the scene
			// Event handlers are scripts that implement IGameObjectPropertyEventHandler
			_eventHandlers = GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IEventListener<>)));
		}

		/// <summary>
		/// Draw UI in the inspector
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Make sure all serialized properties are up to date
			serializedObject.Update();

			RenderRecieveEventStateDropdown();

			RenderEventDropdown();

			// If the user has selected an event,
			// render the event handler dropdown based on
			// the property type the event modifies 
			if (_event.objectReferenceValue != null)
			{
				RenderEventHandlerDropdown();

				// If the property types differ there must be a converter
				// Let the user pick the right one
				if (_eventHandler.objectReferenceValue != null && _eventHandlerPropertyType.stringValue != _eventPropertyType.stringValue)
					RenderValueConverterDropdown();
			}

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
                    Type propertyType = ReflectionHelpers.GetGenericArgumentTypes(_event.objectReferenceValue.GetType(), typeof(IEvent<>))[0];
                    _eventPropertyType.stringValue = propertyType.AssemblyQualifiedName;

					// Save a reference to the event's controller
					_eventController.objectReferenceValue = (Controller)_event.objectReferenceValue.GetType().GetProperty("Controller").GetValue(_event.objectReferenceValue, null);
				}

				// If the event changed make sure we reset the event handler and converter
				if (lastEvent != _event.objectReferenceValue)
				{
					_eventHandler.objectReferenceValue = null;
					_eventHandlerPropertyType.stringValue = null;

					_valueConverter.objectReferenceValue = null;
				}
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
				// Get that type
				Type eventPropertyType = ReflectionHelpers.GetGenericArgumentTypes(_event.objectReferenceValue.GetType(), typeof(IEvent<>))[0];

				// Get a list of value converters that can convert the event property
				List<Type> usableValueConverterTypes = _valueConverters
					.Where(vc => ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>))[0] == eventPropertyType)
					.GroupBy(vc => ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>))[1])
					.Select(grp => grp.Key)
					.ToList();

				// Here we filter our list of EventHandlers down to those that
				// Can handle the propertiy's value
				List<MonoBehaviour> eventHandlersInScene = _eventHandlers
					.Where(eh => {
						Type ehPropertyType = ReflectionHelpers.GetGenericArgumentTypes(eh.GetType(), typeof(IEventListener<>))[0];
						return ehPropertyType == eventPropertyType || usableValueConverterTypes.Contains(ehPropertyType);
					})
					.ToList();

				// Now that we have our list of EventHandlers, create a list of names that we'll use in our dropdown
				string helpMessage = (eventHandlersInScene.Count > 0) ? "Select an event handler..." : "Please add an event handler to the scene";
				List<string> actionsInSceneNames = new List<string>() { helpMessage };
				actionsInSceneNames.AddRange(eventHandlersInScene.Select(a => GetNameForDropdown(a)));

				// Save the current event handler so we know if we change it
				UnityEngine.Object lastEventHandler = _eventHandler.objectReferenceValue;

				// Is an EventHandler already selected?
				// If so, show that in the dropdown
				int startIndex = 0;
				if (_eventHandler.objectReferenceValue != null)
					startIndex = eventHandlersInScene.IndexOf((MonoBehaviour)_eventHandler.objectReferenceValue) + 1;

				// Show the dropdown and get the index the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, actionsInSceneNames.ToArray());

				// If the user selects an EventHandler (not the help message) save it on our script
				if (selectedIndex > 0)
				{
					MonoBehaviour eventHandler = eventHandlersInScene[selectedIndex - 1];
					Type eventHandlerPropertyType = ReflectionHelpers.GetGenericArgumentTypes(eventHandler.GetType(), typeof(IEventListener<>))[0];

					_eventHandler.objectReferenceValue = eventHandler;
					_eventHandlerPropertyType.stringValue = eventHandlerPropertyType.AssemblyQualifiedName;
				}

				if (lastEventHandler != _eventHandler.objectReferenceValue)
					_valueConverter.objectReferenceValue = null;
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Show a dropdown of potential converters
		/// </summary>
		private void RenderValueConverterDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Converter", GUILayout.Width(100));

				// Get the list of converters that can convert from the event into another type
				List<MonoBehaviour> usableValueConverters = _valueConverters
					.Where(vc => ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>))[0].AssemblyQualifiedName == _eventPropertyType.stringValue)
					.ToList();

				// Get a list of usable converter names
				List<string> usableValueConverterNames = usableValueConverters
				.Select(converter => GetNameForDropdown(converter))
				.ToList();

				// If a converter is already selected choose that
				int converterDropdownStartIndex = 0;
				if (_valueConverter.objectReferenceValue != null)
					converterDropdownStartIndex = usableValueConverters.IndexOf((MonoBehaviour)_valueConverter.objectReferenceValue);

				// Show the dropdown and get the index the user selects
				int converterDropdownSelectedIndex = EditorGUILayout.Popup(converterDropdownStartIndex, usableValueConverterNames.ToArray());

				_valueConverter.objectReferenceValue = usableValueConverters[converterDropdownSelectedIndex];
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
