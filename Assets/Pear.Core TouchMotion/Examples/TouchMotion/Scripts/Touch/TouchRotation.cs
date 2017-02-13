using Pear.Core.Controllers;
using Pear.Core.Interactables;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.Core.Examples
{
    /// <summary>
    /// Handles rotation with the Touch controller
    /// </summary>
    public class TouchRotation : ControllerBehavior<TouchController>
    {

        [Tooltip("Degrees per second")]
        public float RotationSensitivity = 300;

        [Tooltip("Fired when the controller starts rotating")]
        public TouchEvent OnStart;

        [Tooltip("Fired when the controller stops rotating")]
        public TouchEvent OnEnd;

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
                    OnEnd.Invoke(Controller.ActiveObject);
                if (!wasRotating && _isRotating)
                    OnStart.Invoke(Controller.ActiveObject);
            }
        }

        void Awake()
        {
            OnStart = OnStart ?? new TouchEvent();
            OnEnd = OnEnd ?? new TouchEvent();
        }

        // Update is called once per frame
        void Update()
        {
            if (Controller.ActiveObject == null || !Controller.ActiveObject.gameObject.activeInHierarchy)
                return;

            IsRotating = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, Controller.OVRController);
            if (IsRotating)
            {
                float degreesPerSecond = RotationSensitivity * Time.deltaTime;
                Vector2 rotationAmount = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, Controller.OVRController);
                Controller.ActiveObject.transform.Rotate(new Vector3(rotationAmount.y * degreesPerSecond, -rotationAmount.x * degreesPerSecond, 0), Space.World);
            }
        }
    }

    /// <summary>
    /// Rotating events
    /// </summary>
    public class TouchEvent : UnityEvent<InteractableObject> { }
}