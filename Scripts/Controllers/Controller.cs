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
		public delegate void ActiveObjectsChangedHandler(GameObject[] oldActiveObjects, GameObject[] newActiveObjects);
		public event ActiveObjectsChangedHandler ActiveObjectsChangedEvent;

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
		/// <param name="newActiveObjects">The new active objects</param>
		public void SetActive(params GameObject[] newActiveObjects)
		{
			GameObject[] oldActiveObjects = ActiveObjects;

			_activeObjects.Clear();
			if (newActiveObjects != null)
				_activeObjects.AddRange(newActiveObjects);

			if (ActiveObjectsChangedEvent != null)
				ActiveObjectsChangedEvent(oldActiveObjects, ActiveObjects);
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