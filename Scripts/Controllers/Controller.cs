using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Base class for all controllers
	/// </summary>
	public class Controller : MonoBehaviour
    {
        /// <summary>
        /// Location of this controller
        /// </summary>
        public ControllerLocation Location;

		[Tooltip("Fired when game object awakes")]
		public ControllerEvent OnAwake = new ControllerEvent();

		[Tooltip("Fired when game object starts")]
		public ControllerEvent OnStart = new ControllerEvent();

		/// <summary>
		/// Event handling for when the controller's InUse state changes
		/// </summary>
		public delegate void InUseChangedHandler(bool inUse);
		public event InUseChangedHandler InUseChangedEvent;

		// Event handling for when the active objects change
		public delegate void ActiveObjectsChangeHandler(GameObject[] oldActiveObjects, GameObject[] newActiveObjects);
		public event ActiveObjectsChangeHandler PreActiveObjectsChangedEvent;
		public event ActiveObjectsChangeHandler PostActiveObjectsChangedEvent;

		/// <summary>
		/// This controllers active objects
		/// </summary>
		[SerializeField]
		private List<GameObject> _activeObjects = new List<GameObject>();
		public GameObject[] ActiveObjects
		{
			get { return _activeObjects.ToArray(); }
			set { SetActive(value); }
		}

        /// <summary>
        /// Tracks the in use state of this controller
        /// </summary>
        private bool _inUse = true;
        public bool InUse
        {
            get { return _inUse; }
            set
            {
				if (_inUse != value) {
					_inUse = value;

					// Fire the change event
					if (InUseChangedEvent != null)
						InUseChangedEvent(_inUse);
				}
            }
        }

		protected virtual void Awake()
		{
			OnAwake.Invoke(this);
		}

        /// <summary>
        /// Registers this controller when the component starts
		/// Note:
		///		We do this on start so handlers can setup in their Awake functions
        /// </summary>
        protected virtual void Start()
        {
            ControllerManager.Instance.RegisterController(this);

			OnStart.Invoke(this);
        }

		/// <summary>
		/// Sets a new set of active objects. Overwrites the existing list.
		/// Fires the changed event.
		/// </summary>
		/// <param name="activeObjectsToSet">The new active objects</param>
		public void SetActive(params GameObject[] activeObjectsToSet)
		{
			List<GameObject> newActiveObjects = new List<GameObject>();
			if (activeObjectsToSet != null)
				newActiveObjects.AddRange(activeObjectsToSet);

			List<GameObject> oldActiveObjects = new List<GameObject>();
			foreach (GameObject activeObject in _activeObjects)
			{
				if (!newActiveObjects.Contains(activeObject))
					oldActiveObjects.Add(activeObject);
			}

			// If nothing changed return
			if (oldActiveObjects.Count == 0 && newActiveObjects.Count == _activeObjects.Count)
				return;

			GameObject[] oldActiveObjectsArray = oldActiveObjects.ToArray();

			if (PreActiveObjectsChangedEvent != null)
				PreActiveObjectsChangedEvent(oldActiveObjectsArray, newActiveObjects.ToArray());

			_activeObjects = newActiveObjects;

			if (PostActiveObjectsChangedEvent != null)
				PostActiveObjectsChangedEvent(oldActiveObjectsArray, ActiveObjects);
		}

		/// <summary>
		/// Add to the list of actives
		/// </summary>
		/// <param name="objsToAdd">Objects to add</param>
		public void AddActives(params GameObject[] objsToAdd)
		{
			List<GameObject> actives = ActiveObjects.ToList();
			actives.AddRange(objsToAdd);
			SetActive(actives.Distinct().ToArray());
		}

		/// <summary>
		/// Removes the given objects from the set of active objects
		/// </summary>
		/// <param name="activeObjectsToRemove">objects to remove from the active object set</param>
		public void RemoveActives(params GameObject[] activeObjectsToRemove)
		{
			if(activeObjectsToRemove != null)
			{
				// Remove the object from our list of actives
				// then set the new list active
				List<GameObject> newActiveObjects = new List<GameObject>(_activeObjects);
				foreach (GameObject activeObjectToRemove in activeObjectsToRemove)
					newActiveObjects.Remove(activeObjectToRemove);

				SetActive(newActiveObjects.ToArray());
			}
		}
	}

    [Serializable]
    public class ControllerEvent : UnityEvent<Controller> { }

    public enum ControllerLocation
    {
        LeftHand,
        RightHand,
        BothHands,
        Eyes,
    }
}