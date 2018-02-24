using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Detects a collision and tells the listener about it
	/// </summary>
	public class CollisionDetector : MonoBehaviour
	{
		private const string LOG_TAG = "[AggregateMeshColliderCollisionDetector]";

		// Collision events
		public Action<Collision> OnEnterEvent;
		public Action<Collision> OnExitEvent;

		private void OnCollisionEnter(Collision collision)
		{
			if (OnEnterEvent != null)
				OnEnterEvent(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			if (OnExitEvent != null)
				OnExitEvent(collision);
		}
	}
}
