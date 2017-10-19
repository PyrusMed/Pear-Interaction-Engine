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

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(NamedEvent))]
	public class NamedEventEditor : Editor
	{
		// Serialized name
		protected SerializedProperty _eventName;

		// Serialized event
		protected SerializedProperty _event;

		// Serialized value converter
		protected SerializedProperty _valueConverter;

		// Serialized event controller
		protected SerializedProperty _eventController;

		// Serialized property type
		protected SerializedProperty _eventPropertyType;

		// Serialized property type
		protected SerializedProperty _eventHandlerPropertyType;

		// All events in the scene
		protected List<MonoBehaviour> _events;

		// All converters in the scene
		protected List<MonoBehaviour> _valueConverters;

		private EventMap _eventMap;

		protected virtual void OnEnable()
		{
			_eventName = serializedObject.FindProperty("EventName");
			_event = serializedObject.FindProperty("Event");
			_valueConverter = serializedObject.FindProperty("ValueConverter");
			_eventController = serializedObject.FindProperty("EventController");
			_eventPropertyType = serializedObject.FindProperty("EventPropertyType");
			_eventHandlerPropertyType = serializedObject.FindProperty("EventHandlerPropertyType");
			_eventMap = FindObjectOfType<EventMap>();

			if (_eventMap == null)
			{
				Debug.LogError("Please add an EventMap to your scene");
			}

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

				_events = EditorHelpers.GetTypesInScene(eventsThatImplementIControllerBehavior);
			}

			// Get all of the value converters in the scene
			// Value converters are scripts that implement IPropertyConverter<,>
			_valueConverters = EditorHelpers.GetTypesInScene(ReflectionHelpers.GetTypesThatImplementInterface(typeof(IPropertyConverter<,>)));
		}

		/// <summary>
		/// Draw UI in the inspector
		/// </summary>
		public override void OnInspectorGUI()
		{
			// Make sure all serialized properties are up to date
			serializedObject.Update();

			RenderEventNameDropdown();

			if (!string.IsNullOrEmpty(_eventName.stringValue))
			{
				RenderEventDropdown();

				// If the user has selected an event,
				// render the value converter dropdown based on
				// the property type the event modifies 
				if (_event.objectReferenceValue != null)
				{
					RenderValueConverterDropdown();
				}
			}

			// Save any changes that were made
			serializedObject.ApplyModifiedProperties();
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
				string lastEventHandlerPropertyType = _eventHandlerPropertyType.stringValue;

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
					_eventHandlerPropertyType.stringValue = eventNameToType[_eventName.stringValue].AssemblyQualifiedName;
				}
				else
				{
					_eventName.stringValue = null;
					_eventHandlerPropertyType.stringValue = null;
				}

				if (string.IsNullOrEmpty(_eventHandlerPropertyType.stringValue) || lastEventHandlerPropertyType != _eventHandlerPropertyType.stringValue)
				{
					_valueConverter.objectReferenceValue = null;
				}
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Create a dropdown listing all scripts that generate events
		/// </summary>
		protected void RenderEventDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Event:", GUILayout.Width(100));
				
				// Get a list of value converters that can convert to the event handler's type
				List<Type> usableValueConverterTypes = _valueConverters
					.Where(vc => ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>))[1].AssemblyQualifiedName == _eventHandlerPropertyType.stringValue)
					.GroupBy(vc => ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>))[0])
					.Select(grp => grp.Key)
					.ToList();

				// Here we filter our list of Events down to those that
				// Can handle the property's value
				List<MonoBehaviour>  usableEvents = _events
					.Where(ev =>
					{
						Type eventPropertyType = ReflectionHelpers.GetGenericArgumentTypes(ev.GetType(), typeof(IEvent<>))[0];
						return eventPropertyType.AssemblyQualifiedName == _eventHandlerPropertyType.stringValue || usableValueConverterTypes.Contains(eventPropertyType);
					})
					.ToList();

				// Create an entry in the drowdown for each event in the scene
				string helpMessage = (usableEvents.Count > 0) ? "Select an event" : "Please add an event script to the scene";
				List<string> eventsInSceneNames = new List<string>() { helpMessage };
				eventsInSceneNames.AddRange(usableEvents.Select(c => EditorHelpers.GetNameForDropdown(c)));

				// Is an event already selected?
				// If so, make sure we select that in the dropdown
				int startIndex = 0;
				if (_event.objectReferenceValue != null)
					startIndex = usableEvents.IndexOf((MonoBehaviour)_event.objectReferenceValue) + 1;

				// Save the current event so we know if we change it
				UnityEngine.Object lastEvent = _event.objectReferenceValue;

				// Show the dropdown and get the event the user selects
				int selectedIndex = EditorGUILayout.Popup(startIndex, eventsInSceneNames.ToArray());

				// If the user selected an event (any item but the help message)
				// then save that event on our script
				if (selectedIndex > 0)
				{
					_event.objectReferenceValue = usableEvents[selectedIndex - 1];

					// Save the property's type
					Type propertyType = ReflectionHelpers.GetGenericArgumentTypes(_event.objectReferenceValue.GetType(), typeof(IEvent<>))[0];
					_eventPropertyType.stringValue = propertyType.AssemblyQualifiedName;

					// Save a reference to the event's controller
					_eventController.objectReferenceValue = (Controller)_event.objectReferenceValue.GetType().GetProperty("Controller").GetValue(_event.objectReferenceValue, null);
				}

				// If the event changed make sure we reset the event handler and converter
				if (lastEvent != _event.objectReferenceValue)
				{
					_valueConverter.objectReferenceValue = null;
				}
			}
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Show a dropdown of potential converters
		/// </summary>
		protected void RenderValueConverterDropdown()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Converter", GUILayout.Width(100));

				// Get the list of converters that can convert from the event into another type
				List<MonoBehaviour> usableValueConverters = _valueConverters
					.Where(vc =>
					{
						Type[] converterTypes = ReflectionHelpers.GetGenericArgumentTypes(vc.GetType(), typeof(IPropertyConverter<,>));
						return converterTypes[0].AssemblyQualifiedName == _eventPropertyType.stringValue &&
							converterTypes[1].AssemblyQualifiedName == _eventHandlerPropertyType.stringValue;
					})
					.ToList();

				// If the property types are not the same we need a converter
				bool needsConverter = _eventPropertyType.stringValue != _eventHandlerPropertyType.stringValue;

				// Get a list of usable converter names
				List<string> usableValueConverterNames = usableValueConverters
					.Select(converter => EditorHelpers.GetNameForDropdown(converter))
					.ToList();
				List<string> dropdownValues = new List<string>();
				if (!needsConverter)
					dropdownValues.Add("None");
				dropdownValues.AddRange(usableValueConverterNames);

				// If a converter is already selected choose that
				int converterDropdownStartIndex = 0;
				if (_valueConverter.objectReferenceValue != null)
					converterDropdownStartIndex = usableValueConverters.IndexOf((MonoBehaviour)_valueConverter.objectReferenceValue) + (!needsConverter ? 1 : 0);

				// Show the dropdown and get the index the user selects
				int converterDropdownSelectedIndex = EditorGUILayout.Popup(converterDropdownStartIndex, dropdownValues.ToArray());

				if (!needsConverter)
				{
					// If "None" was selected set the converter reference to null
					if (converterDropdownSelectedIndex == 0)
						_valueConverter.objectReferenceValue = null;
					else
						_valueConverter.objectReferenceValue = usableValueConverters[converterDropdownSelectedIndex - 1];
				}
				// Otherwise set the converter
				else
				{
					_valueConverter.objectReferenceValue = usableValueConverters[converterDropdownSelectedIndex];
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
