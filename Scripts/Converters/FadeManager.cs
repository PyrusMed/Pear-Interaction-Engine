using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Manages objects that fade
	/// </summary>
	public class FadeManager : MonoBehaviour
	{
		// Event fired when the number of registered objects changes
		public event Action<int> FadableObjectCountChangedEvent;

		// Maps an object to the number of times it's been registered
		private Dictionary<GameObject, int> _fadableObjects = new Dictionary<GameObject, int>();

		/// <summary>
		/// Tells whether an object is currently registered
		/// </summary>
		/// <param name="gameObject">Object to check</param>
		/// <returns>True if the object has been registered. False otherwise.</returns>
		public bool IsRegistered(GameObject gameObject)
		{
			return _fadableObjects.ContainsKey(gameObject);
		}

		/// <summary>
		/// Registers an object
		/// </summary>
		/// <param name="gameObject">Object to register</param>
		public void RegisterFadableObject(GameObject gameObject)
		{
			if (IsRegistered(gameObject))
			{
				_fadableObjects[gameObject]++;
			}
			else
			{
				_fadableObjects[gameObject] = 0;
				FireCountChangedEvent();
			}
		}

		/// <summary>
		/// Unregister the given object
		/// </summary>
		/// <param name="gameObject">Object to unregister</param>
		public void UnregisterFadableObject(GameObject gameObject)
		{
			if (IsRegistered(gameObject))
			{
				_fadableObjects[gameObject]--;
				if(_fadableObjects[gameObject] <= 0)
				{
					_fadableObjects.Remove(gameObject);
					FireCountChangedEvent();
				}
			}
		}

		/// <summary>
		/// Fires the count changed event if there are listeners
		/// </summary>
		private void FireCountChangedEvent()
		{
			if (FadableObjectCountChangedEvent != null)
				FadableObjectCountChangedEvent(_fadableObjects.Count);
		}
	}
}
