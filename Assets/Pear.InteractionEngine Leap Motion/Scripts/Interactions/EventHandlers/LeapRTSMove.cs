using Leap.Unity;
using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	public class LeapRTSMove : MonoBehaviour, IGameObjectPropertyEventHandler<IHandModel>
	{
		[Tooltip("Method used to rotate with one hand")]
		public LeapRTSMoveHelper.RotationMethod OneHandedRotationMethod;

		[Tooltip("Method used to rotate with two hands")]
		public LeapRTSMoveHelper.RotationMethod TwoHandedRotationMethod;

		// Change handlers map. Allows us to keep properties in context of thier change handlers
		private Dictionary<GameObjectProperty<IHandModel>, Property<IHandModel>.OnPropertyChangeEventHandler> _changeHandlers =
			new Dictionary<GameObjectProperty<IHandModel>, Property<IHandModel>.OnPropertyChangeEventHandler>();

		public void RegisterProperty(GameObjectProperty<IHandModel> property)
		{
			// LeapRTSMoveHelper is what actually moves the object. We need to update it to make
			// sure it's in the correct state
			LeapRTSMoveHelper rtsHelper = property.Owner.transform.GetOrAddComponent<LeapRTSMoveHelper>();
			rtsHelper.OneHandedRotationMethod = OneHandedRotationMethod;
			rtsHelper.TwoHandedRotationMethod = TwoHandedRotationMethod;

			Property<IHandModel>.OnPropertyChangeEventHandler onChanged = (oldHandValue, newHandValue) =>
			{
				// LeapRTS uses the PinchDetector to track movement
				PinchDetector detector = null;
				if (newHandValue != null)
					detector = newHandValue.gameObject.GetComponentInChildren<PinchDetector>();

				bool isLeftHand = (newHandValue ?? oldHandValue).GetLeapHand().IsLeft;
				if (isLeftHand)
					rtsHelper.PinchDetectorA = detector;
				else
					rtsHelper.PinchDetectorB = detector;
			};

			_changeHandlers[property] = onChanged;
			property.ChangeEvent += onChanged;
		}

		public void UnregisterProperty(GameObjectProperty<IHandModel> property)
		{
			property.ChangeEvent -= _changeHandlers[property];
		}
	}
}
