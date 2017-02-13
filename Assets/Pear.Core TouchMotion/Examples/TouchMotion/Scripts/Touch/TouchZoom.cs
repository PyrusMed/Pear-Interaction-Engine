using Pear.Core.Controllers;
using UnityEngine;

namespace Pear.Core.Examples
{
    /// <summary>
    /// Handles zooming with the Touch controller
    /// </summary>
    public class TouchZoom : ControllerBehavior<TouchController>
    {

        [Tooltip("Percentage per second")]
        public float ZoomSensitivity = 50;

        [Tooltip("Fired when the controller starts zooming")]
        public TouchEvent OnStart;

        [Tooltip("Fired when the controller stops zooming")]
        public TouchEvent OnEnd;

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
                    OnEnd.Invoke(Controller.ActiveObject);
                if (!wasZooming && _isZooming)
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

            IsZooming = OVRInput.Get(OVRInput.Touch.One, Controller.OVRController) || OVRInput.Get(OVRInput.Touch.Two, Controller.OVRController);
            if (IsZooming)
            {
                int direction = 0;
                if (OVRInput.Get(OVRInput.Button.One, Controller.OVRController))
                    direction = -1;
                if (OVRInput.Get(OVRInput.Button.Two, Controller.OVRController))
                    direction = 1;

                float percentagePerSecond = (ZoomSensitivity / 100) * Time.deltaTime * direction;
                float newScale = Controller.ActiveObject.transform.localScale.x + Controller.ActiveObject.transform.localScale.x * percentagePerSecond;
                Controller.ActiveObject.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
    }
}