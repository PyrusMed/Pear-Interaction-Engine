using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;
using Pear.InteractionEngine.Events;
using System;
using Pear.InteractionEngine.Interactions;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Rotate a game object based on the value of a property
	/// </summary>
	public class Rotate : MonoBehaviour, IEventListener<Vector3>
	{
        [Tooltip("Rotation speed")]
        public float RotateSpeed = 10f;

		// Velocity vector
		private Vector3 _volocity = Vector3.zero; 

		/// <summary>
		/// Rotates the game object with the given velocity
		/// </summary>
		void Update()
		{
			transform.GetOrAddComponent<ObjectWithAnchor>()
				.AnchorElement
				.transform
				.Rotate(_volocity * RotateSpeed * Time.deltaTime, Space.World);
		}

		/// <summary>
		/// Updates the velocity based on the event's value
		/// </summary>
		/// <param name="args"></param>
		public void ValueChanged(EventArgs<Vector3> args)
		{
			_volocity = args.NewValue;
		}
	}
}
