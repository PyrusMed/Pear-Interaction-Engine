using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	public interface IGameObjectPropertyManager<T>
	{
		void RegisterProperty(GameObjectProperty<T> property);
		void UnregisterProperty(GameObjectProperty<T> property);
	}
}
