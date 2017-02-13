using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.Core.Interactables
{
    public class InteractableObject : MonoBehaviour
    {

        /// <summary>
        /// Keeps track of everything related to hover events
        /// </summary>
        public InteractableObjectState Hovering;

        /// <summary>
        /// Keeps track of everything related to moving events
        /// </summary>
        public InteractableObjectState Moving;

        /// <summary>
        /// Keeps track of everything related to resizing events
        /// </summary>
        public InteractableObjectState Resizing;

        /// <summary>
        /// Keeps track of everything related to rotating events
        /// </summary>
        public InteractableObjectState Rotating;

        /// <summary>
        /// Keeps track of everything related to selected events
        /// </summary>
        public InteractableObjectState Selected;

        /// <summary>
        /// Used to move and manipulate this object
        /// </summary>
        [HideInInspector]
        public Anchor AnchorElement;

        void Awake()
        {
            // Create new state objects
            Hovering = new InteractableObjectState(this);
            Moving = new InteractableObjectState(this);
            Resizing = new InteractableObjectState(this);
            Rotating = new InteractableObjectState(this);
            Selected = new InteractableObjectState(this);

            // Create the anchor element
            GameObject anchor = new GameObject("Anchor");
            AnchorElement = anchor.AddComponent<Anchor>();
            AnchorElement.Interactable = this;
            AnchorElement.transform.position = transform.position;
            anchor.transform.SetParent(transform.parent, true);
            transform.SetParent(AnchorElement.transform, true);

            // Let the manager know we're here so it can let others
            // know we're here
            InteractableObjectManager.Instance.Add(this);
        }

        /// <summary>
        /// Clean up
        /// </summary>
        void OnDestroy()
        {
            // Let everyone know we're leaving
            if(InteractableObjectManager.Instance != null)      // If the app is quitting the manager may have been destroyed
                InteractableObjectManager.Instance.Remove(this);

            // Detach all events
            Hovering.Destroy();
            Moving.Destroy();
            Resizing.Destroy();
            Rotating.Destroy();
            Selected.Destroy();
        }
    }

    /// <summary>
    /// Event for interactable objects
    /// </summary>
    [Serializable]
    public class InteractableObjectEvent : UnityEvent<InteractableObject> { }

}