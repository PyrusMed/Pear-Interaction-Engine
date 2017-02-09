using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using UnityEngine.Events;
using PearMed.Controllers;

/// <summary>
/// Controls rotating and zooming the entire model
/// </summary>
public class WholeModelInteractions : ControllerBehavior<LeapMotionController> {

	public float RotateSensitivity = 10;

	public float ZoomSensitivity = 10;

	private LeapMotionController _controller;

	// Position of finger during the last frame
	private Vector3 _lastPosition;

	// Are we rotating or zooming?
	private bool _rotate;
	private bool _zoom;
	private float _modelSpecificzoomSensitivity;

	void Start () {
		_controller = GetComponent<LeapMotionController> ();
	}

	// Update is called once per frame
	void Update () {
		// If the model is loaded and we're rotating, update the rotation of the model
		if (Controller.ActiveObject != null && _lastPosition != null) {
			// Get the difference in position since the last frame
			Vector3 palmPosition = GetPalmPosition ();
			Vector3 delta = _lastPosition - palmPosition;

			if (_rotate) {
				// Moving your thumb left and right should rotate around the y axis
				// and moving up and down should rotate around the y axis
				Vector3 rotation = new Vector3 (-delta.y, delta.x, 0);

                // Rotate the model
                Controller.ActiveObject.transform.Rotate (rotation * 10000 * RotateSensitivity * Time.deltaTime, Space.World);
			}

			if (_zoom) {
				float scaledDelta = delta.z * _modelSpecificzoomSensitivity * Time.deltaTime;
				Vector3 currentScale = Controller.ActiveObject.transform.localScale;
				Vector3 newScale = new Vector3 (
					currentScale.x + scaledDelta,
					currentScale.y + scaledDelta,
					currentScale.z + scaledDelta
				);

                Controller.ActiveObject.transform.localScale = newScale;
			}

			// Save the current position
			_lastPosition = palmPosition;
		}
	}

	/// <summary>
	/// Starts rotating.
	/// </summary>
	public void StartRotating () {
		if (Controller.ActiveObject != null) {
			_rotate = true;
			_lastPosition = GetPalmPosition ();
            Controller.ActiveObject.Rotating.Add(_controller);
		}
	}

	/// <summary>
	/// Stops rotating.
	/// </summary>
	public void StopRotating () {
		_rotate = false;
		if (Controller.ActiveObject != null)
            Controller.ActiveObject.Rotating.Remove(_controller);
	}

	/// <summary>
	/// Start zooming
	/// </summary>
	public void StartZooming () {
		if (Controller.ActiveObject != null) {
			_zoom = true;
			_lastPosition = GetPalmPosition ();
			_modelSpecificzoomSensitivity = ZoomSensitivity * Controller.ActiveObject.transform.localScale.x * 100;
            Controller.ActiveObject.Resizing.Add(_controller);
        }
	}

	/// <summary>
	/// Stops zooming.
	/// </summary>
	public void StopZooming () {
		_zoom = false;
		if (Controller.ActiveObject != null)
            Controller.ActiveObject.Resizing.Remove(_controller);
	}

	/// <summary>
	/// Gets palm position
	/// </summary>
	/// <returns>The palm position.</returns>
	Vector3 GetPalmPosition() {
		Hand leap_hand = _controller.Hand.GetLeapHand();
		return new Vector3 (leap_hand.PalmPosition.x, leap_hand.PalmPosition.y, leap_hand.PalmPosition.z);
	}
}
