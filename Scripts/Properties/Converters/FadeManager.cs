using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	public class FadeManager : MonoBehaviour
	{
		public event Action<int> FadableObjectCountChangedEvent;

		private Dictionary<GameObject, int> _fadableObjects = new Dictionary<GameObject, int>();

		public bool IsRegistered(GameObject gameObject)
		{
			return _fadableObjects.ContainsKey(gameObject);
		}

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

		private void FireCountChangedEvent()
		{
			if (FadableObjectCountChangedEvent != null)
				FadableObjectCountChangedEvent(_fadableObjects.Count);
		}
	}
}
