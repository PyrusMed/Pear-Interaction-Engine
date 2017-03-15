using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.EventHandlers
{
	public class ZoomSingleDirection : MonoBehaviour, IGameObjectPropertyEventHandler<bool>
	{
		[Tooltip("Direction to zoom in")]
		public ZoomDirection Direction;

		[Tooltip("Rate to zoom. (percentage per second)")]
		public int ZoomRate = 1;

		// The objects that should be zoomed
		List<GameObject> _objectsToZoom = new List<GameObject>();

		// Change handlers
		Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChangeEventHandler> _changeHandlers =
			new Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChangeEventHandler>();

		// Update is called once per frame
		void Update()
		{
			int direction = (Direction == ZoomDirection.ZoomIn) ? 1 : -1;
			float percentagePerSecond = (ZoomRate / 100) * Time.deltaTime * direction;

			// Zoom in each object who's property changed
			_objectsToZoom.ForEach(go =>
			{
				// This logic assumes objects have uniform scale
				float newScale = go.transform.localScale.x + go.transform.localScale.x * percentagePerSecond;
				go.transform.localScale = new Vector3(newScale, newScale, newScale);
			});
		}

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent += _changeHandlers[property] = (oldValue, newValue) =>
			{
				if (newValue)
					_objectsToZoom.Add(property.Owner);
				else
					_objectsToZoom.Remove(property.Owner);
			};
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent -= _changeHandlers[property];
		}
	}

	public enum ZoomDirection
	{
		ZoomIn,
		ZoomOut,
	}
}