using Leap.Unity;
using Pear.InteractionEngine.Utils;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{

	/// <summary>
	/// This is essentially a copy of LeapRTS with a few small modifications
	/// </summary>
	public class LeapRTSMoveHelper : MonoBehaviour
	{

		public enum RotationMethod
		{
			None,
			Single,
			Full
		}

		private bool _changedSinceLastFrame = false;

		[SerializeField]
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
				_changedSinceLastFrame = true;
			}
		}

		[SerializeField]
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
				_changedSinceLastFrame = true;
			}
		}

		public RotationMethod OneHandedRotationMethod;

		public RotationMethod TwoHandedRotationMethod;

		[SerializeField]
		private bool _allowScale = true;

		[Header("GUI Options")]
		[SerializeField]
		private KeyCode _toggleGuiState = KeyCode.None;

		[SerializeField]
		private bool _showGUI = true;

		private Transform _anchor;

		private float _defaultNearClip;

		void Start()
		{
			_anchor = transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement.transform;
		}

		void Update()
		{
			if (Input.GetKeyDown(_toggleGuiState))
			{
				_showGUI = !_showGUI;
			}

			bool didUpdate = _changedSinceLastFrame;
			if (_pinchDetectorA != null)
				didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
			if (_pinchDetectorB != null)
				didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

			_changedSinceLastFrame = false;

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

			if (didUpdate)
			{
				transform.SetParent(_anchor, true);
			}
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
