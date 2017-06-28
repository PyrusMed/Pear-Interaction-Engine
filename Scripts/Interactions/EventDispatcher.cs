using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Converters;
using System;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Sends an event's value from an IEvent to an IEventHandler
	/// </summary>
	/// <typeparam name="TEvent">Type of event value</typeparam>
	/// <typeparam name="TEventListener">Type of value listened for by the event listener</typeparam>
	public class EventDispatcher<TEvent, TEventListener> : IEventDispatcher
	{
		// Initial event value
		public TEvent DefaultEventValue = default(TEvent);

		// Event source
		private Controller _controller;

		// Game object listening to event
		private GameObject _listenerGameObject;

		// The event
		private IEvent<TEvent> _event;

		// Function used to convert from the event's value to
		// a value the event listener understands
		private Func<TEvent, TEventListener> _convertFunc;

		// Listens to events
		private IEventListener<TEventListener> _listener;

		// Is the dispatcher enabled
		// i.e. is the listener listening
		private bool _eventingEnabled = false;
		public bool EventingEnabled
		{
			get { return _eventingEnabled; }
			set
			{
				bool oldEnabled = _eventingEnabled;
				_eventingEnabled = value;
				if(oldEnabled != _eventingEnabled)
				{
					if (_eventingEnabled)
						AttachEvents();
					else
						DetachEvents();
				}
			}
		}

		// Defines when the listener receives event values
		public Interaction.ReceiveEventStates ReceiveEventState { get; set; }

		// Does the listener always receive event values?
		private bool AlwaysReceiveEvents
		{
			get { return ReceiveEventState == Interaction.ReceiveEventStates.Always; }
		}

		// Does the listener only receive events when it's the controller's active object?
		private bool ReceiveEventsWhenObjectActive
		{
			get { return ReceiveEventState == Interaction.ReceiveEventStates.WhenObjectActive; }
		}

		public EventDispatcher(Controller controller,
			GameObject listenerGameObject,
			IEvent<TEvent> ev,
			IPropertyConverter<TEvent, TEventListener> converter,
			IEventListener<TEventListener> listener)
		{
			_controller = controller;
			_listenerGameObject = listenerGameObject;
			_event = ev;
			_listener = listener;

			// If there's a converter, use it
			if (converter != null)
				_convertFunc = converter.Convert;
			// Otherwise, TEvent must == TEventListener
			// so we can do a direct conversion with our default function
			else
				_convertFunc = eventVal => { return (TEventListener)(eventVal as object); };
		}

		/// <summary>
		/// When the controller's active object changes update the listener
		/// </summary>
		/// <param name="oldActiveObject">controller's old active object</param>
		/// <param name="newActiveObject">controller's new active object</param>
		private void OnControllerActiveObjectChanged(GameObject oldActiveObject, GameObject newActiveObject)
		{
			// If the listener is the old active object
			// make sure it receives the default, or inital, value as it's new value
			if(_listenerGameObject == oldActiveObject)
			{
				Dispatch(
					oldValue: _event.Event.Value,
					newValue: DefaultEventValue);
			}
			// Otherwise, if it's the new active object,
			// make sure it's updated with the latest event value
			else if (_listenerGameObject == newActiveObject)
			{
				Dispatch(
					oldValue: DefaultEventValue,
					newValue: _event.Event.Value);
			}
		}

		/// <summary>
		/// When the event's value changes attempt to dispatch values
		/// to the listener
		/// </summary>
		/// <param name="oldValue">Old event value</param>
		/// <param name="newValue">New event value</param>
		private void OnEventValueChanged(TEvent oldValue, TEvent newValue)
		{
			if(AlwaysReceiveEvents || ReceiveEventsWhenObjectActive && _controller.ActiveObject == _listenerGameObject)
			{
				Dispatch(
					oldValue: oldValue,
					newValue: newValue);
			}
		}

		/// <summary>
		/// Convert the event's value to a value the listener understands,
		/// then send those values to the listener
		/// </summary>
		/// <param name="oldValue">Old event value</param>
		/// <param name="newValue">New event value</param>
		public void Dispatch(TEvent oldValue, TEvent newValue)
		{
			TEventListener oldValueForListener = _convertFunc(oldValue);
			TEventListener newValueForListener = _convertFunc(newValue);
			if (!Property<TEventListener>.AreEqual(oldValueForListener, newValueForListener))
			{
				EventArgs<TEventListener> eventArgs = new EventArgs<TEventListener>()
				{
					Source = _controller,
					OldValue = oldValueForListener,
					NewValue = newValueForListener,
				};

				_listener.ValueChanged(eventArgs);
			}
		}

		/// <summary>
		/// Make sure the dispatcher can dispatch to the listener at the correct time
		/// </summary>
		private void AttachEvents()
		{
			// If the listener is only receiving event values when the object is active
			// we need to listen for when the object becomes active on the controller
			if (ReceiveEventsWhenObjectActive)
				_controller.ActiveObjectChangedEvent += OnControllerActiveObjectChanged;

			// Listen for event value changes
			_event.Event.ValueChangeEvent += OnEventValueChanged;
		}

		/// <summary>
		/// Make sure the dispatcher stops dispatching events to the listener
		/// </summary>
		private void DetachEvents()
		{
			if (ReceiveEventsWhenObjectActive)
				_controller.ActiveObjectChangedEvent -= OnControllerActiveObjectChanged;

			_event.Event.ValueChangeEvent -= OnEventValueChanged;
		}
	}

	/// <summary>
	/// Helper interface that makes dispatching events easier
	/// </summary>
	internal interface IEventDispatcher
	{
		bool EventingEnabled { get; set; }
		Interaction.ReceiveEventStates ReceiveEventState { get; set; }
	}
}