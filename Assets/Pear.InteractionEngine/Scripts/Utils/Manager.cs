using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
    public abstract class Manager<T> : Singleton<Manager<T>>
    {
        public delegate void AddedEvent(T obj);
        public delegate void RemovedEvent(T obj);

        /// <summary>
        /// Event fired when a object is added
        /// </summary>
        public event AddedEvent OnAdded;

        /// <summary>
        /// Event fired when a object is removed
        /// </summary>
        public RemovedEvent OnRemoved;

        // List of all objects
        private List<T> _allObjects = new List<T>();

        /// <summary>
        /// All objects that have been loaded
        /// </summary>
        public T[] AllObjects
        {
            get
            {
                return _allObjects.ToArray();
            }
        }

        /// <summary>
        /// Add a object and let listeners know about it
        /// </summary>
        /// <param name="name">name of obj</param>
        /// <param name="obj">obj to add</param>
        public void Add(T obj)
        {
            _allObjects.Add(obj);
            if (OnAdded != null)
                OnAdded(obj);
        }

        /// <summary>
        /// Remove and object and let listeners know about it
        /// </summary>
        /// <param name="obj">obj to be removed</param>
        public void Remove(T obj)
        {
            _allObjects.Remove(obj);
            if (OnRemoved != null)
                OnRemoved(obj);
        }
    }
}