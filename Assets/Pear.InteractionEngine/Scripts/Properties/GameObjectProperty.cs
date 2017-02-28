using System;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Property the drives interactions between Events and EventHandlers
	/// </summary>
	/// <typeparam name="T">Type of proeprty</typeparam>
	public class GameObjectProperty<T> : Property<T>
	{
		// The GameObject this property is for
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
