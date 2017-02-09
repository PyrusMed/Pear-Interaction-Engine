using Pear.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Pear.Core.Interactables
{
    /// <summary>
    /// Tracks the state for interactable objects
    /// </summary>
    [Serializable]
    public class InteractableObjectState
    {
        /// <summary>
        /// Event list for when this state first starts
        /// </summary>
        public InteractableObjectControllerEvent OnStart;

        /// <summary>
        /// Event list for when this state changes
        /// </summary>
        public InteractableObjectControllerEvent OnChange;

        /// <summary>
        /// Event list for when this state ends
        /// </summary>
        public InteractableObjectControllerEvent OnEnd;

        /// <summary>
        /// Parent interactable object
        /// </summary>
        private InteractableObject _obj;

        /// <summary>
        /// Controllers that caused this state
        /// </summary>
        public List<Controller> Controllers
        {
            get;
            private set;
        }

        public InteractableObjectState(InteractableObject obj)
        {
            _obj = obj;
            OnStart = new InteractableObjectControllerEvent();
            OnChange = new InteractableObjectControllerEvent();
            OnEnd = new InteractableObjectControllerEvent();

            Controllers = new List<Controller>();
        }

        /// <summary>
        /// Clean up events when this state is destroyed
        /// </summary>
        public void Destroy()
        {
            OnStart.RemoveAllListeners();
            OnChange.RemoveAllListeners();
            OnEnd.RemoveAllListeners();
        }

        /// <summary>
        /// Is this state true?
        /// </summary>
        /// <returns>True if the state is interacting with controllers. False otherwise.</returns>
        public bool IsTrue()
        {
            return Controllers.Count > 0;
        }

        /// <summary>
        /// Add new controllers to this state indicating that the controllers are interacting with it.
        /// If the state was false before the call and is set to true, fire the OnStart event.
        /// </summary>
        /// <param name="controllers">controllers to add</param>
        public void Add(params Controller[] controllers)
        {
            bool wasTrue = IsTrue();
            Controllers.AddRange(controllers);
            if (!wasTrue && IsTrue())
            {
                OnStart.Invoke(new InteractableObjectControllerEventData
                {
                    Obj = _obj,
                    Controllers = Controllers.ToArray(),
                });
            }

            OnChange.Invoke(new InteractableObjectControllerEventData
            {
                Obj = _obj,
                Controllers = Controllers.ToArray(),
            });
        }

        /// <summary>
        /// Remove controllers from this state indicating that the controllers are no longer interacting with it
        /// If the state was true before the call and is set to false, fire the OnEnd event.
        /// </summary>
        /// <param name="controllers">controllers to remove</param>
        public void Remove(params Controller[] controllers)
        {
            bool wasTrue = IsTrue();

            foreach (Controller controller in controllers)
                Controllers.Remove(controller);

            if (wasTrue && !IsTrue())
            {
                OnEnd.Invoke(new InteractableObjectControllerEventData
                {
                    Obj = _obj,
                    Controllers = Controllers.ToArray(),
                });
            }

            OnChange.Invoke(new InteractableObjectControllerEventData
            {
                Obj = _obj,
                Controllers = Controllers.ToArray(),
            });
        }

        /// <summary>
        /// Checks whether the specified controllers are affecting the state
        /// </summary>
        /// <param name="controllers">controllers</param>
        /// <returns>True if all controllers are affecting with the given state</returns>
        public bool Contains(params Controller[] controllers)
        {
            return !controllers.Except(Controllers).Any();
        }
    }

    /// <summary>
    /// Fired when the object interacts with controllers
    /// </summary>
    [Serializable]
    public class InteractableObjectControllerEvent : UnityEvent<InteractableObjectControllerEventData> { }

    /// <summary>
    /// Data for events that involve controllers
    /// </summary>
    public class InteractableObjectControllerEventData
    {
        public InteractableObject Obj;
        public Controller[] Controllers;
    }
}