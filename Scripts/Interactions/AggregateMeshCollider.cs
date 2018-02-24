using Pear.InteractionEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions {
	/// <summary>
	/// Aggregates collisions on child objects and reports them to listeners
	/// </summary>
	public class AggregateMeshCollider : MonoBehaviour
	{
		private const string LOG_TAG = "[AggregateMeshCollider]";

		// Collision events
		public event Action<Collision> OnCollisionEnterEvent;
		public event Action<Collision> OnCollisionExitEvent;
		public Func<Collision, bool> IsValidCollision;

		// The number of active collisions
		private int _activeCollisions = 0;

		void Start()
		{
			if(IsValidCollision == null)
			{
				Debug.LogError(String.Format("{0} IsValidCollision must be set"));
				return;
			}

			ListenForCollisions();
		}

		/// <summary>
		/// Creates child collision detectors
		/// </summary>
		private void ListenForCollisions()
		{
			// Get all colliders and attach the detector script and
			// listen for collision events
			foreach (MeshCollider meshCollider in GetComponentsInChildren<MeshCollider>())
			{
				// Make sure we can collide
				meshCollider.convex = true;
				meshCollider.isTrigger = true;

				CollisionDetector collisionDetector = meshCollider.GetOrAddComponent<CollisionDetector>();

				// Listen for collisions
				collisionDetector.OnEnterEvent = OnCollisionDetectorEnter;
				collisionDetector.OnExitEvent = OnCollisionDetectorExit;
			}
		}

		/// <summary>
		/// Listen for collision enter events and notify listeners when appropriate
		/// </summary>
		/// <param name="collision">The collision</param>
		private void OnCollisionDetectorEnter(Collision collision)
		{
			if (!IsValidCollision(collision))
				return;

			_activeCollisions++;

			// If this is the first collision let the listeners know
			if (_activeCollisions == 1 && OnCollisionEnterEvent != null)
			{
				Debug.Log(String.Format("{0} firing collision enter event", LOG_TAG));
				OnCollisionEnterEvent(collision);
			}
		}

		/// <summary>
		/// Listen for collision exit events and notify listeners when appropriate
		/// </summary>
		/// <param name="collision">The collision</param>
		private void OnCollisionDetectorExit(Collision collision)
		{
			if (!IsValidCollision(collision))
				return;

			_activeCollisions--;

			// If this was the last exit let the listeners know
			if (_activeCollisions == 0 && OnCollisionExitEvent != null)
			{
				Debug.Log(String.Format("{0} firing collision exit event", LOG_TAG));
				OnCollisionExitEvent(collision);
			}
		}
	}
}
