using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Properties.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class EventDispatcher<TEvent, TEventListener>
	{
		public TEvent DefaultValue = default(TEvent);

		private Controller _controller;

		private GameObject _owner;

		private IEvent<TEvent> _event;

		private Func<TEvent, TEventListener> _convertFunc;

		private IEventListener<TEventListener> _listener;

		public bool Enabled { get; set; }

		public ReceiveEventStates ReceiveEventState { get; set; }

		private bool AlwaysReceiveEvents { get { return ReceiveEventState == ReceiveEventStates.Always; } }

		public EventDispatcher(Controller controller,
			GameObject owner,
			IEvent<TEvent> ev,
			IPropertyConverter<TEvent, TEventListener> converter,
			IEventListener<TEventListener> listener)
		{
			_controller = controller;
			_owner = owner;
			_event = ev;

			if (converter != null)
				_convertFunc = converter.Convert;
			else
				_convertFunc = eventVal => { return (TEventListener)(eventVal as object); };

			_listener = listener;

			if(!AlwaysReceiveEvents)
				_controller.ActiveObjectChangedEvent += OnActiveObjectChanged;

			_event.Event.ValueChangeEvent += OnEventValueChanged;
		}

		private void OnActiveObjectChanged(GameObject oldActiveObject, GameObject newActiveObject)
		{
			if(_owner == oldActiveObject)
			{
				Dispatch(
					oldValue: _event.Event.Value,
					newValue: DefaultValue);
			}
			else if (_owner == newActiveObject)
			{
				Dispatch(
					oldValue: DefaultValue,
					newValue: _event.Event.Value);
			}
		}

		private void OnEventValueChanged(TEvent oldValue, TEvent newValue)
		{
			if(AlwaysReceiveEvents || _controller.ActiveObject == _owner)
			{
				Dispatch(
					oldValue: oldValue,
					newValue: newValue);
			}
		}

		public void Dispatch(TEvent oldValue, TEvent newValue)
		{
			if (!Enabled)
				return;

			TEventListener oldValueForListener = _convertFunc(oldValue);
			TEventListener newValueForListener = _convertFunc(newValue);
			if (!Property<TEventListener>.AreEqual(oldValueForListener, newValueForListener))
			{
				InteractionEngine.Events.EventArgs<TEventListener> eventArgs = new InteractionEngine.Events.EventArgs<TEventListener>()
				{
					Source = _controller,
					OldValue = oldValueForListener,
					NewValue = newValueForListener,
				};

				_listener.ValueChanged(eventArgs);
			}
		}
	}
}