//#define PIE_DEBUG       // uncomment to show debug logs

using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public static class EventToEventListenerDispatcher<TEventListener>
	{
		public const string LOG_TAG = "[EventToEventListenerDispatcher]";

		private static Dictionary<string, EventListenerCollection> _eventNameToListeners = new Dictionary<string, EventListenerCollection>();

		public static void AddListener(string eventName, IEventListener<TEventListener> listener, GameObject owner, ReceiveEventStates receiveEventState)
		{
			EventListenerCollection listeners;
			if (!_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
				listeners = new EventListenerCollection();
				_eventNameToListeners[eventName] = listeners;

#if PIE_DEBUG
				Debug.Log(String.Format("{0} Creating new array of listeners for event '{1}'", LOG_TAG, eventName));
#endif
			}

#if PIE_DEBUG
			Debug.Log(String.Format("{0} Adding '{1}' to the event '{2}'", LOG_TAG, owner.name, eventName));
#endif

			listeners.Add(listener, owner, receiveEventState);
		}

		public static void RemoveListener(string eventName, IEventListener<TEventListener> listener, GameObject owner)
		{
			EventListenerCollection listeners;
			if (!_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
#if PIE_DEBUG
				Debug.Log(String.Format("{0} Removing '{1}' from event '{2}'", LOG_TAG, owner.name, eventName));
#endif

				listeners.Remove(listener, owner);
			}
			else
			{
#if PIE_DEBUG
				Debug.Log(String.Format("{0} Failed to remove '{1}'. Not listeners exist for event '{2}'", LOG_TAG, owner.name, eventName));
#endif
			}
		}

		public static int DispatchToListeners(string eventName, EventArgs<TEventListener> args, GameObject[] actives = null)
		{
			bool getListenersForActivesOnly = actives != null;
			GameObject[] targetGameObjects = getListenersForActivesOnly ? actives : args.Source.ActiveObjects;
			EventListenerCollection listeners;
			if (_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
				IEventListener<TEventListener>[] gameObjectListeners = listeners.GetListeners(targetGameObjects, activesOnly: getListenersForActivesOnly);
				foreach (IEventListener<TEventListener> listener in gameObjectListeners)
				{
					listener.ValueChanged(args);
				}
				return gameObjectListeners.Length;
			}

			return 0;
		}

		private class EventListenerCollection
		{
			private Dictionary<GameObject, List<EventListenerInfo>> _gameObjToListenersMap = new Dictionary<GameObject, List<EventListenerInfo>>();

			private List<EventListenerInfo> _alwaysReceiveEvents = new List<EventListenerInfo>();

			public void Add(IEventListener<TEventListener> listener, GameObject owner, ReceiveEventStates receiveEventState)
			{
				EventListenerInfo info = new EventListenerInfo
				{
					Listener = listener,
					Owner = owner,
					ReceiveEventState = receiveEventState,
				};

				if (receiveEventState == ReceiveEventStates.Always)
				{
					_alwaysReceiveEvents.Add(info);
				}
				else
				{
					List<EventListenerInfo> listeners;
					if (!_gameObjToListenersMap.TryGetValue(owner, out listeners))
					{
						listeners = new List<EventListenerInfo>();
						_gameObjToListenersMap[owner] = listeners;
					}

					listeners.Add(info);
				}				
			}

			public void Remove(IEventListener<TEventListener> listener, GameObject owner)
			{
				List<EventListenerInfo> listeners;
				if (_gameObjToListenersMap.TryGetValue(owner, out listeners))
				{
					for(int infoIndex = 0; infoIndex < listeners.Count; infoIndex++)
					{
						EventListenerInfo info = listeners[infoIndex];
						if(info.Listener == listener)
						{
							listeners.Remove(info);
							break;
						}
					}
				}
			}

			public IEventListener<TEventListener>[] GetListeners(GameObject[] activeObjs, bool activesOnly)
			{
				List<EventListenerInfo> allListeners = new List<EventListenerInfo>();

				if(!activesOnly)
					allListeners.AddRange(_alwaysReceiveEvents);

				foreach (GameObject obj in activeObjs)
				{
					List<EventListenerInfo> gameObjlisteners;
					if (_gameObjToListenersMap.TryGetValue(obj, out gameObjlisteners))
					{
						allListeners.AddRange(gameObjlisteners);
					}
				}

				return allListeners.Select(info => info.Listener).ToArray();
			}

			private class EventListenerInfo
			{
				public IEventListener<TEventListener> Listener;
				public GameObject Owner;
				public ReceiveEventStates ReceiveEventState;
			}
		}
	}
}
