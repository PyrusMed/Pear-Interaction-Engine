using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions {
	public class UIPointerClick : MonoBehaviour, IGameObjectPropertyEventHandler<bool>
	{
		public UIPointer Pointer;

		Dictionary<GameObjectProperty<bool>, GameObjectProperty<bool>.OnPropertyChangeEventHandler> _handlers =
			new Dictionary<GameObjectProperty<bool>, Property<bool>.OnPropertyChangeEventHandler>();

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent += _handlers[property] = (oldVal, newVal) =>
			{
				Debug.Log("Pointer click set to: " + newVal);
				Pointer.Click = newVal;
			};
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent -= _handlers[property];
		}
	}
}
