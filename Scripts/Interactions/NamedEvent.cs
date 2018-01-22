using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Converters;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class NamedEvent : MonoBehaviour
	{
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

		private IEventDispatcher _eventDispatcher;

		public Type EventType { get { return Type.GetType(EventPropertyType); } }

		public Type EventHandlerType { get { return Type.GetType(EventHandlerPropertyType); } }

		void Start()
		{
			if(!IsValid())
			{
				Debug.LogError("Named event is invalid for object " + name);
				return;
			}

			Type eventPropertyType = Type.GetType(EventPropertyType);
			Type eventHandlerPropertyType = Type.GetType(EventHandlerPropertyType);
			Type eventDispatcherType = typeof(EventDispatcher<,>);
			Type instantiableEventDispatcherType = eventDispatcherType.MakeGenericType(eventPropertyType, eventHandlerPropertyType);
			_eventDispatcher = (IEventDispatcher)Activator.CreateInstance(instantiableEventDispatcherType, EventName, Event, EventController, ValueConverter);

			_eventDispatcher.Enabled = true;
		}

		private void OnEnable()
		{
			if(_eventDispatcher != null)
				_eventDispatcher.Enabled = true;
		}

		private void OnDisable()
		{
			if (_eventDispatcher != null)
				_eventDispatcher.Enabled = false;
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
			private string _eventName;
			private IEvent<TEvent> _event;
			private Controller _controller;
			private Func<TEvent, TEventListener> _converter;
			private Property<TEventListener> _convertedValueProperty;

			private bool _enabled = false;
			public bool Enabled
			{
				get { return _enabled; }

				set
				{
					if (value != _enabled)
					{
						if (value)
							_convertedValueProperty.ValueChangeEvent += OnValueChanged;
						else
							_convertedValueProperty.ValueChangeEvent -= OnValueChanged;
					}

					_enabled = value;
				}
			}

			public EventDispatcher(string eventName, IEvent<TEvent> ev, Controller controller, IPropertyConverter<TEvent, TEventListener> converter)
			{
				_eventName = eventName;
				_event = ev;
				_controller = controller;

				if (converter != null)
					_converter = converter.Convert;
				else
					_converter = eventVal => { return (TEventListener)(eventVal as object); };

				// Whenever the event's value changes, convert that value into the event listener's value type
				// This will ensure we only fire events when the event listener's value type changes
				_convertedValueProperty = new Property<TEventListener>();
				_event.Event.ValueChangeEvent += (oldVal, newVal) => _convertedValueProperty.Value = _converter(newVal);
			}

			public void OnValueChanged(TEventListener oldValue, TEventListener newValue)
			{
				EventArgs<TEventListener> eventArgs = new EventArgs<TEventListener>()
				{
					Source = _controller,
					OldValue = oldValue,
					NewValue = newValue,
				};

				EventToEventListenerDispatcher<TEventListener>.DispatchToListeners(_eventName, eventArgs);
			}
		}
	}
}
