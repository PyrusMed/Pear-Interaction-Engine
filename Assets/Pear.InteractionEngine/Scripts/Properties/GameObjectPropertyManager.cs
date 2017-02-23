using Pear.InteractionEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
    public class GameObjectPropertyManager<T>
    {
        // Maps the name of a property to its coleciton
        private static Dictionary<string, GameObjectPropertyCollection<T>> _nameToCollection = new Dictionary<string, GameObjectPropertyCollection<T>>();

        public static GameObjectPropertyCollection<T> Get(string name)
        {
            if(name != null)
                name = name.ToLower();

            GameObjectPropertyCollection<T> properties;
            if (!_nameToCollection.TryGetValue(name, out properties))
                _nameToCollection[name] = properties = new GameObjectPropertyCollection<T>();

            return properties;
        }
    }

    public class GameObjectPropertyCollection<T>
    {
        public delegate void AddedEvent(GameObjectProperty<T> obj);
        public delegate void RemovedEvent(GameObjectProperty<T> obj);

        /// <summary>
        /// Event fired when a object is added
        /// </summary>
        public event AddedEvent OnAdded;

        /// <summary>
        /// Event fired when a object is removed
        /// </summary>
        public RemovedEvent OnRemoved;

        private List<GameObjectProperty<T>> _all;
        public GameObjectProperty<T>[] All
        {
            get { return _all.ToArray(); }
        }

        public GameObjectPropertyCollection()
        {
            _all = new List<GameObjectProperty<T>>();
        }

        public void Add(GameObjectProperty<T> property)
        {
            _all.Add(property);
            if (OnAdded != null)
                OnAdded(property);
        }

        public void Remove(GameObjectProperty<T> property)
        {
            _all.Remove(property);
            if (OnRemoved != null)
                OnRemoved(property);
        }
    }
}