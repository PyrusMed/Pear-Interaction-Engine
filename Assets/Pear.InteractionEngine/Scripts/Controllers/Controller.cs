using System;
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

		/// <summary>
		/// Event handling for when the controller's InUse state changes
		/// </summary>
		public delegate void InUseChangedHandler(bool inUse);
		public event InUseChangedHandler InUseChangedEvent;

		// Event handling for when the active object changes
		public delegate void ActiveObjectChangedHandler(GameObject oldActiveObject, GameObject newActiveObject);
		public event ActiveObjectChangedHandler ActiveObjectChangedEvent;

		/// <summary>
		/// This controllers active object
		/// </summary>
		[SerializeField]
		private GameObject _activeObject;
		public GameObject ActiveObject
		{
			get { return _activeObject; }
			set
			{
				// If the active object changes, update it and fire the event handler
				if(_activeObject != value)
				{
					GameObject oldActiveObject = _activeObject;
					_activeObject = value;
					if (ActiveObjectChangedEvent != null)
						ActiveObjectChangedEvent(oldActiveObject, _activeObject);
				}
			}
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

        /// <summary>
        /// Registers this controller when the component starts
		/// Note:
		///		We do this on start so handlers can setup in their Awake functions
        /// </summary>
        public virtual void Start()
        {
            ControllerManager.Instance.RegisterController(this);
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