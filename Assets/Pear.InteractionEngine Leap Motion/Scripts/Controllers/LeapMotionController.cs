using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	public class LeapMotionController : Controller
	{
		[Tooltip("The hand that effects physics in the world")]
		[SerializeField]
		private RigidHand _physicsHand;

		/// <summary>
		/// The hand associated with this leap motion controller
		/// NOTE:
		///		Each hand will have its own leap motion controller object
		/// </summary>
		public HandModel Hand
		{
			get;
			private set;
		}

		void Awake()
		{
			InUse = true;
		}

		// Hook up events
		void Start()
		{
			Hand = GetComponent<HandModel>();

			// When either controller is enabled activate it in the hierarchy
			OnStartUsing.AddListener((c) => SetActiveFromEnabledDisabled(true));

			// When either controller is disabled deactivate it in the hierarchy
			OnStopUsing.AddListener((c) => SetActiveFromEnabledDisabled(false));
		}

		void OnEnable()
		{
			if (!InUse)
				SetActiveFromEnabledDisabled(false);
		}

		/// <summary>
		/// Update whether this hand is enabled or disabled, both logically and visually
		/// </summary>
		/// <param name="active">Is the hand active?</param>
		void SetActiveFromEnabledDisabled(bool active)
		{
			gameObject.SetActive(active);
			_physicsHand.gameObject.SetActive(active);
		}
	}
}
