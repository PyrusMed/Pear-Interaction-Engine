using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	public class OculusTouchController : Controller
	{
		// Hand element that will tell us if the controller is active or not
		private GameObject _handRenderElement;

		/// <summary>
		/// Specifies whether this is the left of right OVR controller
		/// </summary>
		public OVRInput.Controller OVRController
		{
			get;
			private set;
		}

		/// <summary>
		/// Specify the controller and get the hand render element
		/// which we use to tell if the controller is in use or not
		/// </summary>
		public override void Start()
		{
			base.Start();

			OVRController = Location == ControllerLocation.LeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;

			GetHandRenderElement();
		}

		void Update()
		{
			if (_handRenderElement != null && _handRenderElement.activeInHierarchy != InUse)
				InUse = _handRenderElement.activeInHierarchy;
		}

		/// <summary>
		/// Wait for the OVRAvatar assets to finish loading, then get the hand render element
		/// </summary>
		private void GetHandRenderElement()
		{
			// Get the hand element associated with this controller
			string nameOfHandElement = Location == ControllerLocation.LeftHand ? "hand_left" : "hand_right";
			Transform handElement = transform.parent.Find(nameOfHandElement);

			// When the assets are done loading the hand element will have a single child
			// That's the hand render element
			OvrAvatar avatar = FindObjectOfType<OvrAvatar>();
			avatar.AssetsDoneLoading.AddListener(() =>
			{
				_handRenderElement = handElement.GetChild(0).gameObject;
			});
		}
	}
}