using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	public interface IPropertyChanger<T>
	{
		void RegisterProperty(GameObjectProperty<T> property);
		void UnregisterProperty(GameObjectProperty<T> property);
	}
}