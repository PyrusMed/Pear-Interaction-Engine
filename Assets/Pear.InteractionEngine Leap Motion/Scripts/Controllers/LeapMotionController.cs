using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	[RequireComponent(typeof(IHandModel))]
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
		private IHandModel _hand;
		public IHandModel Hand
		{
			get
			{
				return _hand ?? (_hand = GetComponent<IHandModel>());
			}
		}

		// Hook up events
		public override void Start()
		{
			base.Start();

			// Enable or disable this controller based on its InUse state
			InUseChangedEvent += inUse => SetActiveFromEnabledDisabled(inUse);
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
