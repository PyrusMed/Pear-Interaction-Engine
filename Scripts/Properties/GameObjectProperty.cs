using Pear.InteractionEngine.Controllers;
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

		// The controlle that is affecting this property
		public Controller EventController
		{
			get;
			private set;
		}

		public GameObjectProperty(GameObject owner, Controller eventController)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			if (owner == null)
				throw new ArgumentNullException("eventController");

			Owner = owner;
			EventController = eventController;
		}
	}
}
