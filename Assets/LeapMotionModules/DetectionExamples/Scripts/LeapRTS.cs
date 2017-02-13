using Leap.Unity;
using Pear.Core.Interactables;
using UnityEngine;
using UnityEngine.Events;

namespace Leap.Unity
{

	/// <summary>
	/// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
	/// allows rotation, translation, and scale of the object (RTS).
	/// </summary>
	public class LeapRTS : MonoBehaviour
	{
		public LeapRTSGrabEvent OnGrabChanged;
		public LeapRTSGrabEvent OnZoomChanged;

		private bool _pinchDetectorAddedLastFrame = false;

		public enum RotationMethod
		{
			None,
			Single,
			Full
		}

		private PinchDetector _pinchDetectorA;
		public PinchDetector PinchDetectorA
		{
			get
			{
				return _pinchDetectorA;
			}
			set
			{
				_pinchDetectorA = value;
				_pinchDetectorAddedLastFrame = true;
            }
		}

		private PinchDetector _pinchDetectorB;
		public PinchDetector PinchDetectorB
		{
			get
			{
				return _pinchDetectorB;
			}
			set
			{
				_pinchDetectorB = value;
				_pinchDetectorAddedLastFrame = true;
            }
		}

		private bool _grabbing = false;
		public bool Grabbing
		{
			get
			{
				return _grabbing;
			}

			set
			{
				bool oldGrab = _grabbing;
				_grabbing = value;

				if(oldGrab != _grabbing)
					OnGrabChanged.Invoke(this);
			}
		}

		private bool _zooming = false;
		public bool Zooming
		{
			get
			{
				return _zooming;
			}

			set
			{
				bool oldZoom = _zooming;
				_zooming = value;

				if (oldZoom != _zooming)
					OnZoomChanged.Invoke(this);
			}
		}

		[SerializeField]
		public RotationMethod OneHandedRotationMethod;

		[SerializeField]
		public RotationMethod TwoHandedRotationMethod;

		[SerializeField]
		private bool _allowScale = true;

		[Header("GUI Options")]
		[SerializeField]
		private KeyCode _toggleGuiState = KeyCode.None;

		[SerializeField]
		private bool _showGUI = false;

		private Transform _anchor;

		private float _defaultNearClip;

		void Awake ()
		{
			OnGrabChanged = OnGrabChanged ?? new LeapRTSGrabEvent();
			OnZoomChanged = OnZoomChanged ?? new LeapRTSGrabEvent();
		}

		void Start()
		{
			//      if (_pinchDetectorA == null || _pinchDetectorB == null) {
			//        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
			//        enabled = false;
			//      }

			// There should be an anchor with all controllers, not just with LeapRTS
			_anchor = GetComponent<InteractableObject>().AnchorElement.transform;
		}

		void Update()
		{
			if (Input.GetKeyDown(_toggleGuiState))
			{
				_showGUI = !_showGUI;
			}


			bool didUpdate = _pinchDetectorAddedLastFrame;
			if (_pinchDetectorA != null)
				didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
			if (_pinchDetectorB != null)
				didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

			if (didUpdate)
			{
				transform.SetParent(null, true);
			}

			if (_pinchDetectorA != null && _pinchDetectorA.IsActive &&
			_pinchDetectorB != null && _pinchDetectorB.IsActive)
			{
				transformDoubleAnchor();
			}
			else if (_pinchDetectorA != null && _pinchDetectorA.IsActive)
			{
				transformSingleAnchor(_pinchDetectorA);
			}
			else if (_pinchDetectorB != null && _pinchDetectorB.IsActive)
			{
				transformSingleAnchor(_pinchDetectorB);
			}

			// Save whether or not one of the hands is grabbing
			Grabbing = _pinchDetectorA != null && _pinchDetectorA.IsActive ||
				_pinchDetectorB != null && _pinchDetectorB.IsActive;

			// Save whether or not the hands are zooming
			Zooming = _pinchDetectorA != null && _pinchDetectorA.IsActive &&
				_pinchDetectorB != null && _pinchDetectorB.IsActive;


			if (didUpdate)
			{
				transform.SetParent(_anchor, true);
			}

			_pinchDetectorAddedLastFrame = false;
        }

		void OnGUI()
		{
			if (_showGUI)
			{
				GUILayout.Label("One Handed Settings");
				doRotationMethodGUI(ref OneHandedRotationMethod);
				GUILayout.Label("Two Handed Settings");
				doRotationMethodGUI(ref TwoHandedRotationMethod);
				_allowScale = GUILayout.Toggle(_allowScale, "Allow Two Handed Scale");
			}
		}

		private void doRotationMethodGUI(ref RotationMethod rotationMethod)
		{
			GUILayout.BeginHorizontal();

			GUI.color = rotationMethod == RotationMethod.None ? Color.green : Color.white;
			if (GUILayout.Button("No Rotation"))
			{
				rotationMethod = RotationMethod.None;
			}

			GUI.color = rotationMethod == RotationMethod.Single ? Color.green : Color.white;
			if (GUILayout.Button("Single Axis"))
			{
				rotationMethod = RotationMethod.Single;
			}

			GUI.color = rotationMethod == RotationMethod.Full ? Color.green : Color.white;
			if (GUILayout.Button("Full Rotation"))
			{
				rotationMethod = RotationMethod.Full;
			}

			GUI.color = Color.white;

			GUILayout.EndHorizontal();
		}

		private void transformDoubleAnchor()
		{
			_anchor.position = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f;

			switch (TwoHandedRotationMethod)
			{
				case RotationMethod.None:
					break;
				case RotationMethod.Single:
					Vector3 p = _pinchDetectorA.Position;
					p.y = _anchor.position.y;
					_anchor.LookAt(p);
					break;
				case RotationMethod.Full:
					Quaternion pp = Quaternion.Lerp(_pinchDetectorA.Rotation, _pinchDetectorB.Rotation, 0.5f);
					Vector3 u = pp * Vector3.up;
					_anchor.LookAt(_pinchDetectorA.Position, u);
					break;
			}

			if (_allowScale)
			{
				_anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
			}
		}

		private void transformSingleAnchor(PinchDetector singlePinch)
		{
			_anchor.position = singlePinch.Position;

			switch (OneHandedRotationMethod)
			{
				case RotationMethod.None:
					break;
				case RotationMethod.Single:
					Vector3 p = singlePinch.Rotation * Vector3.right;
					p.y = _anchor.position.y;
					_anchor.LookAt(p);
					break;
				case RotationMethod.Full:
					_anchor.rotation = singlePinch.Rotation;
					break;
			}

			_anchor.localScale = Vector3.one;
		}
	}
}

/**
   * Called when grab changes
   */
[System.Serializable]
public class LeapRTSGrabEvent : UnityEvent<LeapRTS> { }

/**
   * Called when zoom changes
   */
[System.Serializable]
public class LeapRTSZoomEvent : UnityEvent<LeapRTS> { }