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

		// LeapRTS is what actually moves the object. We need to update it to make
		// sure it's in the correct state
		private LeapRTSMoveHelper _rts;

		void Start()
		{
			_rts = transform.GetOrAddComponent<LeapRTSMoveHelper>();
			_rts.OneHandedRotationMethod = OneHandedRotationMethod;
			_rts.TwoHandedRotationMethod = TwoHandedRotationMethod;
		}

		public void RegisterProperty(GameObjectProperty<IHandModel> property)
		{
			property.ChangeEvent += GrabChanged;
		}

		public void UnregisterProperty(GameObjectProperty<IHandModel> property)
		{
			property.ChangeEvent -= GrabChanged;
		}

		/// <summary>
		/// Called when the hand starts or ends grabbing.
		/// Updates the LeapRTS object, which implements movement
		/// </summary>
		/// <param name="oldHandValue">old value</param>
		/// <param name="newHandValue">new value</param>
		private void GrabChanged(IHandModel oldHandValue, IHandModel newHandValue)
		{
			// LeapRTS uses the PinchDetector to track movement
			PinchDetector detector = null;
			if(newHandValue != null)
				detector = newHandValue.gameObject.GetComponentInChildren<PinchDetector>();

			bool isLeftHand = (newHandValue ?? oldHandValue).GetLeapHand().IsLeft;
			if (isLeftHand)
				_rts.PinchDetectorA = detector;
			else
				_rts.PinchDetectorB = detector;
		}
	}
}
