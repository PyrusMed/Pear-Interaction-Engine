using Pear.InteractionEngine.Interactables;
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
        /// Event fired when this controller is in use
        /// </summary>
        [HideInInspector]
        public ControllerEvent OnStartUsing = new ControllerEvent();

        /// <summary>
        /// Event fired when this controller is not in use
        /// </summary>
        [HideInInspector]
        public ControllerEvent OnStopUsing = new ControllerEvent();

        /// <summary>
        /// This controllers active object
        /// </summary>
        public GameObject ActiveObject;

        /// <summary>
        /// Tracks the in use state of this controller
        /// </summary>
        private bool _inUse = true;
        public bool InUse
        {
            get { return _inUse; }
            set
            {
                bool wasInUse = _inUse;
                _inUse = value;

                if (!wasInUse && _inUse)
                    OnStartUsing.Invoke(this);
                else if (wasInUse && !_inUse)
                    OnStopUsing.Invoke(this);
            }
        }

        /// <summary>
        /// Registers this controller when the component awakes
        /// </summary>
        void Awake()
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