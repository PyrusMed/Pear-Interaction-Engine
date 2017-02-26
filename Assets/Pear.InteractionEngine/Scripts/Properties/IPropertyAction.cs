using System;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Properties
{
	public interface IPropertyAction<T>
	{
		void RegisterProperty(GameObjectProperty<T> property);
		void UnregisterProperty(GameObjectProperty<T> property);
	}
}