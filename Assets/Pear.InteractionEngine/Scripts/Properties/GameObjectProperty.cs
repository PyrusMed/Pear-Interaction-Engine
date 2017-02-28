using System;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	public class GameObjectProperty<T> : Property<T>
	{
		public GameObject Owner
		{
			get;
			private set;
		}

		public GameObjectProperty(GameObject owner)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			Owner = owner;
		}
	}
}
