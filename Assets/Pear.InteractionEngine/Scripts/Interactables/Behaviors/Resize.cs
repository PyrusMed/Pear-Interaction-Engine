using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
	public class Resize : MonoBehaviour, IGameObjectPropertyEventHandler<int>
	{
		private List<GameObjectProperty<int>> _properties = new List<GameObjectProperty<int>>();

		void Update()
		{
			foreach(GameObjectProperty<int> property in _properties)
			{
				Anchor anchor = property.Owner.transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement;
				float currentScale = anchor.transform.localScale.x;
				float scaleAmount = property.Value * Time.deltaTime;
				float newScale = currentScale * (1 + scaleAmount);

				// Apply the new scale
				anchor.transform.localScale = Vector3.one * newScale;
			}
		}

		public void RegisterProperty(GameObjectProperty<int> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<int> property)
		{
			_properties.Remove(property);
		}
	}
}
