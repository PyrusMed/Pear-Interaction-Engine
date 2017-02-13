using Pear.Core.Controllers;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Pear.Core.Controllers.Behaviors
{
    /// <summary>
    /// Base class for drag manipulations. This class recognizes a navigation event,
    /// and allows derriving classes to perform an action based on a navigation factor (offset * MaxSpeed * delta time)
    /// </summary>
    public abstract class DragBehavior : ControllerBehavior<HoloLensController>
    {

        [Tooltip("Max speed")]
        public float MaxSpeed = 3f;

        // Used to listen for navigation gestures
        private GestureRecognizer _navigationRecognizer;

        void Start()
        {
            // Start listening for navigation events
            _navigationRecognizer = new GestureRecognizer();
            _navigationRecognizer.SetRecognizableGestures(
                    GestureSettings.NavigationX |
                    GestureSettings.NavigationY |
                    GestureSettings.NavigationZ
            );
            _navigationRecognizer.NavigationUpdatedEvent += OnNavigationUpdated;
            _navigationRecognizer.StartCapturingGestures();
        }

        /// <summary>
        /// Handles navigation event
        /// </summary>
        /// <param name="source">Hand source</param>
        /// <param name="relativePosition">Position relative to when navigation started</param>
        /// <param name="ray"></param>
        private void OnNavigationUpdated(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // If the controller has an active object perform the action
            if (Controller.ActiveObject != null)
                PerformAction(relativePosition * MaxSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Perform the action based on the given factor
        /// </summary>
        /// <param name="actionFactor">relative position * MaxSpeed</param>
        protected abstract void PerformAction(Vector3 actionFactor);
    }
}