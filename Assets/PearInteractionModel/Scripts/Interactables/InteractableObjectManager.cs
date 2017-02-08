using PearMed.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PearMed.Interactables
{
    /// <summary>
    /// Manages every interactable object that's added and removed
    /// </summary>
    public class InteractableObjectManager : Singleton<InteractableObjectManager>
    {
        /// <summary>
        /// Event fired when a object is added
        /// </summary>
        public InteractableObjectEvent OnAdded;

        /// <summary>
        /// Event fired when a object is removed
        /// </summary>
        public InteractableObjectEvent OnRemoved;

        // List of all objects
        private List<InteractableObject> _allObjects = new List<InteractableObject>();

        /// <summary>
        /// All objects that have been loaded
        /// </summary>
        public InteractableObject[] AllObjects
        {
            get
            {
                return _allObjects.ToArray();
            }
        }

        private void Awake()
        {
            OnAdded = OnAdded ?? new InteractableObjectEvent();
            OnRemoved = OnRemoved ?? new InteractableObjectEvent();
        }

        /// <summary>
        /// Add a object and let listeners know about it
        /// </summary>
        /// <param name="interactable">object to add</param>
        public void Add(InteractableObject interactable)
        {
            _allObjects.Add(interactable);
            OnAdded.Invoke(interactable);
        }

        /// <summary>
        /// Remove a object and let listeners know about it
        /// </summary>
        /// <param name="interactable">obj to remove </param>
        public void Remove(InteractableObject interactable)
        {
            _allObjects.Remove(interactable);
            OnRemoved.Invoke(interactable);
        }
    }
}