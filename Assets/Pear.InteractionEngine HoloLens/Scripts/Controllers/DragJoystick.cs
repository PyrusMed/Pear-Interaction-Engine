using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Pear.InteractionEngine.Interactions.Events
{
    /// <summary>
    /// UI for PinchAndDrag event
    /// </summary>
    public class DragJoystick : MonoBehaviour {

        [Tooltip("Behavior that's the source of our dragging values")]
        public PinchAndDrag DragSource;

        [Tooltip("Sphere that represents when the user is pinching")]
        public GameObject PinchPoint;

        // Joystick renderers
        private MeshRenderer[] _renderers;

        public void Start()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>();

            // When navigation starts show the joystick
            DragSource.NavigationRecognizer.NavigationStartedEvent += ShowJoystick;

            // When navigation changes update the joystick
            DragSource.NavigationRecognizer.NavigationUpdatedEvent += UpdateJoystick;

            // When navigation ends hide the joystick
            DragSource.NavigationRecognizer.NavigationCanceledEvent += HideJoystick;
            DragSource.NavigationRecognizer.NavigationCompletedEvent += HideJoystick;
		}

        /// <summary>
        /// Show the joystick
        /// </summary>
        private void ShowJoystick(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
			transform.position = ray.origin;
            SetVisibility(_renderers, true);
        }

        /// <summary>
        /// Hide the joystick
        /// </summary>
        private void HideJoystick(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            SetVisibility(_renderers, false);
        }

        /// <summary>
        /// Update the position of the joystick's pinch to zoom elements
        /// </summary>
        private void UpdateJoystick(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            PinchPoint.transform.localPosition = relativePosition;
        }

		/// <summary>
		/// Hide or show the joystick
		/// </summary>
		private void SetVisibility(MeshRenderer[] renderers, bool visible)
        {
            foreach (Renderer renderer in renderers)
                renderer.enabled = visible;
        }
    }
}
