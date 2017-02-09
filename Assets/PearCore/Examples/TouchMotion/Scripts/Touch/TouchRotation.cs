using Pear.Core.Interactables;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles rotation with the Touch controller
/// </summary>
public class TouchRotation : MonoBehaviour {

	[Tooltip("Degrees per second")]
	public float RotationSensitivity = 300;

	[Tooltip("Fired when the controller starts rotating")]
	public TouchEvent OnStart;

	[Tooltip("Fired when the controller stops rotating")]
	public TouchEvent OnEnd;

	private TouchController _touchController;

	/// <summary>
	/// Is the controller rotating? Fires events when rotation changes
	/// </summary>
	private bool _isRotating = false;
	private bool IsRotating
	{
		get
		{
			return _isRotating;
        }
		
		set
		{
			bool wasRotating = _isRotating;
			_isRotating = value;

			// Handle events
			if (wasRotating && !_isRotating)
				OnEnd.Invoke(_touchController.ActiveObject);
			if (!wasRotating && _isRotating)
				OnStart.Invoke(_touchController.ActiveObject);
        }
	}

	void Awake()
	{
		OnStart = OnStart ?? new TouchEvent();
		OnEnd = OnEnd ?? new TouchEvent();
	}

	void Start()
	{
		_touchController = GetComponent<TouchController>();
	}

	// Update is called once per frame
	void Update () {
		if (_touchController.ActiveObject == null || !_touchController.ActiveObject.gameObject.activeInHierarchy)
			return;

		IsRotating = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, _touchController.Controller);
        if (IsRotating)
		{
            float degreesPerSecond = RotationSensitivity * Time.deltaTime;
			Vector2 rotationAmount = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, _touchController.Controller);
			_touchController.ActiveObject.transform.Rotate(new Vector3(rotationAmount.y * degreesPerSecond, -rotationAmount.x * degreesPerSecond, 0), Space.World);
		}
	}
}

/// <summary>
/// Rotating events
/// </summary>
public class TouchEvent : UnityEvent<InteractableObject> { }
