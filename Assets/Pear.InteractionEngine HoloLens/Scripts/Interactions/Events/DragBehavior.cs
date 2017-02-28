using System;
using Pear.InteractionEngine.Interactions;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System.Linq;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
    /// <summary>
    /// Base class for drag manipulations. This class recognizes a navigation event,
    /// and allows derriving classes to perform an action based on a navigation factor (offset * MaxSpeed * delta time)
    /// </summary>
    public abstract class DragBehavior : ControllerBehavior<HoloLensController>, IGameObjectPropertyEvent<Vector3>
    {

        [Tooltip("Max speed")]
        public float MaxSpeed = 3f;

        // Used to listen for navigation gestures
        public GestureRecognizer NavigationRecognizer
        {
            get;
            private set;
        }

        // Properties to change
        private List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        void Start()
        {
            // Start listening for navigation events
            NavigationRecognizer = new GestureRecognizer();
            NavigationRecognizer.SetRecognizableGestures(
                    GestureSettings.NavigationX |
                    GestureSettings.NavigationY |
                    GestureSettings.NavigationZ
            );
            NavigationRecognizer.NavigationUpdatedEvent += OnNavigationUpdated;
            NavigationRecognizer.StartCapturingGestures();
        }

        /// <summary>
        /// Update each property when the navigation event is updated
        /// </summary>
        /// <param name="source">Hand source</param>
        /// <param name="relativePosition">Position relative to when navigation started</param>
        /// <param name="ray"></param>
        private void OnNavigationUpdated(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // If the controller has an active object update its properties
            if (Controller.ActiveObject != null)
            {
                Vector3 newValue = relativePosition * MaxSpeed;
                _properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = newValue);
            }
        }

        public void RegisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Add(property);
        }

        public void UnregisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Remove(property);
        }
    }
}