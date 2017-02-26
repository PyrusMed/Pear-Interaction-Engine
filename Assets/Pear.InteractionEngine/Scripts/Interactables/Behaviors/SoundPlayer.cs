using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
	public class SoundPlayer : MonoBehaviour, IPropertyAction<bool>
	{
		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			throw new NotImplementedException();
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			throw new NotImplementedException();
		}
	}
}
