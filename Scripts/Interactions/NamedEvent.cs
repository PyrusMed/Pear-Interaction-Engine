using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Converters;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class NamedEvent : MonoBehaviour
	{
		private const string LOG_TAG = "[NamedEvent]";

		// Name of the event
		public string EventName;

		// Fires the event
		public MonoBehaviour Event;

		// Converts the event's value into a value
		// the event lister understands
		public MonoBehaviour ValueConverter;

		// The controller that emits the event
		public Controller EventController;

		// Used for serialization, this variable stores
		// the type of property (e.g. float, bool, ect)
		// that the event fires
		public string EventPropertyType;

		// Used for serialization, this variable stores
		// the type of property (e.g. float, bool, ect)
		// that the event listener fires
		public string EventHandlerPropertyType;

		// True if we should show debug logs
		public bool ShowDebugLogs = false;

		private IEventDispatcher _eventDispatcher;

		public Type EventType { get { return Type.GetType(EventPropertyType); } }

		public Type EventHandlerType { get { return Type.GetType(EventHandlerPropertyType); } }

		void Start()
		{
			if (!IsValid())
			{
				Debug.LogError("Named event is invalid for object " + name);
				return;
			}

			if (ShowDebugLogs)
				Debug.Log(String.Format("{0} {1} creating named event {1}", LOG_TAG, name, EventName));

			Type eventPropertyType = Type.GetType(EventPropertyType);
			Type eventHandlerPropertyType = Type.GetType(EventHandlerPropertyType);
			Type eventDispatcherType = typeof(EventDispatcher<,>);
			Type instantiableEventDispatcherType = eventDispatcherType.MakeGenericType(eventPropertyType, eventHandlerPropertyType);
			_eventDispatcher = (IEventDispatcher)Activator.CreateInstance(instantiableEventDispatcherType, EventName, Event, EventController, ValueConverter, ShowDebugLogs);

			_eventDispatcher.Enabled = true;
		}

		private void OnEnable()
		{
			if (_eventDispatcher != null)
			{
				if (ShowDebugLogs)
					Debug.Log(String.Format("{0} {1} enabled", LOG_TAG, name));

				_eventDispatcher.Enabled = true;
			}
		}

		private void OnDisable()
		{
			if (_eventDispatcher != null)
			{
				if (ShowDebugLogs)
					Debug.Log(String.Format("{0} {1} disabled", LOG_TAG, name));

				_eventDispatcher.Enabled = false;
			}
		}

		private bool IsValid()
		{
			if (Event == null)
			{
				Debug.LogError("The Event needs to be set.");
				return false;
			}

			if (EventPropertyType == null || EventHandlerPropertyType == null)
			{
				Debug.LogError("Interaction property type not set.");
				return false;
			}

			if (EventType == null || EventHandlerType == null)
			{
				Debug.LogError(string.Format("Event or handler property type is null. Event '{0}'. EventHandler '{1}'",
					EventType == null ? null : EventType.Name,
					EventHandlerType == null ? null : EventHandlerType.Name));
				return false;
			}

			if (EventPropertyType != EventHandlerPropertyType && ValueConverter == null)
			{
				Debug.LogError(string.Format("[{0}] This interaction needs a value converter. '{1}' -> '{2}'", name, EventPropertyType, EventHandlerPropertyType));
				return false;
			}

			return true;
		}

		public interface IEventDispatcher
		{
			bool Enabled { get; set; }
		}

		public class EventDispatcher<TEvent, TEventListener> : IEventDispatcher
		{
			private const string ED_LOG_TAG = "[EventDispatcher]";

			private string _eventName;
			private IEvent<TEvent> _event;
			private Controller _controller;
			private Func<TEvent, TEventListener> _converter;
			private Property<TEventListener> _convertedValueProperty;
			private bool _showDebugLogs = false;

			private bool _enabled = false;
			public bool Enabled
			{
				get { return _enabled; }

				set
				{
					if (value != _enabled)
					{
						if (value)
						{
							if (_showDebugLogs)
								Debug.Log(String.Format("{0} {1} attaching converted value change listener", ED_LOG_TAG, _eventName));

							_convertedValueProperty.ValueChangeEvent += OnEventListenerValueChanged;
						}
						else
						{
							if (_showDebugLogs)
								Debug.Log(String.Format("{0} {1} detaching converted value change listener", ED_LOG_TAG, _eventName));
							_convertedValueProperty.ValueChangeEvent -= OnEventListenerValueChanged;

						}
					}

					_enabled = value;
				}
			}

			public EventDispatcher(string eventName, IEvent<TEvent> ev, Controller controller, IPropertyConverter<TEvent, TEventListener> converter, bool showDebugLogs)
			{
				_eventName = eventName;
				_event = ev;
				_controller = controller;
				_showDebugLogs = showDebugLogs;

				if (converter != null)
					_converter = converter.Convert;
				else
					_converter = eventVal => { return (TEventListener)(eventVal as object); };

				// Whenever the event's value changes, convert that value into the event listener's value type
				// This will ensure we only fire events when the event listener's value type changes
				_convertedValueProperty = new Property<TEventListener>();
				_event.Event.ValueChangeEvent += OnEventValueChanged;

				_event.Event.ShowLogs = _showDebugLogs;
				_convertedValueProperty.ShowLogs = _showDebugLogs;

				controller.PostActiveObjectsChangedEvent += OnControllerActivesChanged;
			}

			private void OnEventValueChanged(TEvent oldValue, TEvent newValue)
			{
				if(_showDebugLogs)
					Debug.Log(String.Format("{0} {1} event value changed. Old: {2}, New: {3}", ED_LOG_TAG, _eventName, oldValue, newValue));

				_convertedValueProperty.Value = _converter(newValue);

				if (_showDebugLogs)
					Debug.Log(String.Format("{0} {1} Conversion: Event '{2}' -> EventListener {3}", ED_LOG_TAG, _eventName, newValue, _convertedValueProperty.Value));
			}

			private void OnEventListenerValueChanged(TEventListener oldValue, TEventListener newValue)
			{
				EventArgs<TEventListener> eventArgs = new EventArgs<TEventListener>()
				{
					Source = _controller,
					OldValue = oldValue,
					NewValue = newValue,
				};

				if (_showDebugLogs)
					Debug.Log(String.Format("{0} {1} event listener value changed. Old: {2}, New: {3}", ED_LOG_TAG, _eventName, oldValue, newValue));

				int numListeners = EventToEventListenerDispatcher<TEventListener>.DispatchToListeners(_eventName, eventArgs);

				if (_showDebugLogs)
					Debug.Log(String.Format("{0} {1} dispatched to {2} listeners", ED_LOG_TAG, _eventName, numListeners));
			}

			/// <summary>
			/// When controllers are removed make sure the default value is set
			/// </summary>
			/// <param name="oldActives">removed actives</param>
			/// <param name="newActives">new actives</param>
			public void OnControllerActivesChanged(GameObject[] oldActives, GameObject[] newActives)
			{
				if (oldActives.Length == 0)
					return;

				EventArgs<TEventListener> eventArgs = new EventArgs<TEventListener>()
				{
					Source = _controller,
					OldValue = _convertedValueProperty.Value,
					NewValue = default(TEventListener),
				};

				// When actives are removed make sure the default value is set on them
				if(!Property<TEventListener>.AreEqual(eventArgs.OldValue, eventArgs.NewValue))
					EventToEventListenerDispatcher<TEventListener>.DispatchToListeners(_eventName, eventArgs, oldActives);
			}
		}
	}
}
