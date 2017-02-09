using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles zooming with the Touch controller
/// </summary>
public class TouchZoom : MonoBehaviour {

	[Tooltip("Percentage per second")]
	public float ZoomSensitivity = 50;

	[Tooltip("Fired when the controller starts zooming")]
	public TouchEvent OnStart;

	[Tooltip("Fired when the controller stops zooming")]
	public TouchEvent OnEnd;

	private TouchController _touchController;

	/// <summary>
	/// Is the controller zoomiing? Fires events when zooming changes
	/// </summary>
	private bool _isZooming = false;
	private bool IsZooming
	{
		get
		{
			return _isZooming;
        }
		
		set
		{
			bool wasZooming = _isZooming;
			_isZooming = value;

			// Handle events
			if (wasZooming && !_isZooming)
				OnEnd.Invoke(_touchController.ActiveObject);
			if (!wasZooming && _isZooming)
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

		IsZooming = OVRInput.Get(OVRInput.Touch.One, _touchController.Controller) || OVRInput.Get(OVRInput.Touch.Two, _touchController.Controller);
        if (IsZooming)
		{
			int direction = 0;
			if (OVRInput.Get(OVRInput.Button.One, _touchController.Controller))
				direction = -1;
			if (OVRInput.Get(OVRInput.Button.Two, _touchController.Controller))
				direction = 1;

			float percentagePerSecond = (ZoomSensitivity / 100) * Time.deltaTime * direction;
			float newScale = _touchController.ActiveObject.transform.localScale.x + _touchController.ActiveObject.transform.localScale.x * percentagePerSecond;
			_touchController.ActiveObject.transform.localScale = new Vector3(newScale, newScale, newScale);
		}
	}
}
