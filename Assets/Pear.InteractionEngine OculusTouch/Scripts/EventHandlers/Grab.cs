using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pear.InteractionEngine.Interactions;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.EventHandlers
{
	public class Grab : MonoBehaviour, IGameObjectPropertyEventHandler<float>
	{
		// Start grabbing the model after this threshold is reached
		public const float GrabThreshold = 0.9f;

		// Keep track of all of the event handlers so we can unregister them
		private Dictionary<GameObjectProperty<float>, Property<float>.OnPropertyChangeEventHandler> _eventHandlers =
			new Dictionary<GameObjectProperty<float>, Property<float>.OnPropertyChangeEventHandler>();

		public void RegisterProperty(GameObjectProperty<float> property)
		{
			Transform originalParent = null;
			ObjectWithAnchor objWithAnchor = property.Owner.transform.GetOrAddComponent<ObjectWithAnchor>();

			// This assumes that the object the event script is attached to is the object we follow
			Transform objToFollow = property.EventController.transform;

			// When the user exceeds the grab threshold
			property.ChangeEvent += _eventHandlers[property] = (oldValue, newValue) =>
			{
				// When the user starts grabbing, grab the object
				if (originalParent == null && newValue > GrabThreshold)
				{
					originalParent = GrabObj(objWithAnchor, objToFollow);
				}
				// When the user stops grabbing release the object
				else if (originalParent != null && newValue < GrabThreshold)
				{
					ReleaseObj(objWithAnchor, originalParent);
					originalParent = null;
				}
			};
		}

		public void UnregisterProperty(GameObjectProperty<float> property)
		{
			property.ChangeEvent -= _eventHandlers[property];
		}

		/// <summary>
		/// Make the object a child of the new parent
		/// </summary>
		/// <param name="objectToGrab">object to grab</param>
		/// <param name="newParent">New parent of the given object to grab</param>
		/// <returns>old parent</returns>
		private static Transform GrabObj(ObjectWithAnchor objectToGrab, Transform newParent)
		{
			Transform parent = objectToGrab.AnchorElement.transform.parent;
			objectToGrab.AnchorElement.transform.SetParent(newParent, true);
			return parent;
		}

		/// <summary>
		/// Release the object by reparenting the object to its original parent
		/// </summary>
		/// <param name="objectToRelease">object to release</param>
		/// <param name="originalParent">objects original parent</param>
		private void ReleaseObj(ObjectWithAnchor objectToRelease, Transform originalParent)
		{
			objectToRelease.AnchorElement.transform.SetParent(originalParent, true);
		}
	}
}