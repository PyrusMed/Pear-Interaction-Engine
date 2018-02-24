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
		private const string LOG_TAG = "[Controller]";

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
		/// Tells whether this controller has active objects
		/// </summary>
		public bool HasActiveObjects { get { return _activeObjects.Any(); } }

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
			if (activeObjectsToSet == null)
				return;

			// Update our lists to reflect what was actually added and removed
			GameObject[] removedActives = _activeObjects.Where(active => !activeObjectsToSet.Any(toSet => toSet == active)).ToArray();
			GameObject[] newActives = activeObjectsToSet.Where(toSet => !_activeObjects.Any(active => toSet == active)).ToArray();

			/*Debug.Log(String.Format("{0}.{1} adding ({2}) removing ({3}) actives: ({4})", LOG_TAG,
				name,
				string.Join(",", newActives.Select(na => na.name).ToArray()),
				string.Join(",", removedActives.Select(rm => rm.name).ToArray()),
				string.Join(",", activeObjectsToSet.Select(set => set.name).ToArray())
			));*/

			if (PreActiveObjectsChangedEvent != null)
				PreActiveObjectsChangedEvent(removedActives, newActives);

			_activeObjects = new List<GameObject>(activeObjectsToSet);

			if (PostActiveObjectsChangedEvent != null)
				PostActiveObjectsChangedEvent(removedActives, newActives);
		}

		/// <summary>
		/// Add to the list of actives
		/// </summary>
		/// <param name="objsToAdd">Objects to add</param>
		public void AddActives(params GameObject[] objsToAdd)
		{
			List<GameObject> newActives = new List<GameObject>(_activeObjects);
			newActives.AddRange(objsToAdd);

			SetActive(newActives.Distinct().ToArray());
		}

		/// <summary>
		/// Removes the given objects from the set of active objects
		/// </summary>
		public void RemoveActives(params GameObject[] objsToRemove)
		{
			SetActive(_activeObjects.Where(active => !objsToRemove.Contains(active)).ToArray());
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