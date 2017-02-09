using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAdapter : MonoBehaviour {

	private HandModel _hand;
	private PinchDetector _pinchDetector;
	private ProximityDetector _proximityDetector;

	// The last leap RTS object that was last hovered over
	private LeapRTS _hoveringRts;

	// The RTS object that's currently being pinched
	private LeapRTS _pinchedRts;

	// Use this for initialization
	void Start () {
		_hand = GetComponent<HandModel>();
		_pinchDetector = _hand.gameObject.GetComponentInChildren<PinchDetector>();
		_proximityDetector = _hand.gameObject.GetComponentInChildren<ProximityDetector>();

		// Save the last RTS we hovered over
		_proximityDetector.OnProximity.AddListener((obj) =>
		{
			_hoveringRts = obj.GetComponent<LeapRTS>();
		});

		// Detect when we stop hovering
		_proximityDetector.OnDeactivate.AddListener(() =>
		{
			_hoveringRts = null;
		});
	}
	
	// Update is called once per frame
	void Update () {
		// If we started pinching and we're hoving over an object...grab it
		if (_pinchDetector.DidStartPinch && _hoveringRts != null)
		{
			// Grab the object
			UpdateGrabState(_hoveringRts, _pinchDetector);

			// Make sure we remember that we're grabbing this object just in case we
			// stop hovering over it while we're grabbing it
			_pinchedRts = _hoveringRts;
        }
		// Otherwise, if we stopped pinching and we were pinching an object...let it go
		else if (_pinchDetector.DidEndPinch && _pinchedRts)
		{
			// Let go of this object
			UpdateGrabState(_pinchedRts, null);
		}
	}

	/// <summary>
	/// Update the grab state of this object by setting a pinch detector. 
	/// </summary>
	/// <param name="rts">object to update</param>
	/// <param name="detector">pinch detector to set</param>
	private void UpdateGrabState(LeapRTS rts, PinchDetector detector)
	{
		if (_hand.GetLeapHand().IsLeft)
			rts.PinchDetectorA = detector;
		else
			rts.PinchDetectorB = detector;
	}
}
