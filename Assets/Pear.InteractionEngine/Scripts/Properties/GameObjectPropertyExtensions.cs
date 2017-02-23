using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Properties {
    public static class GameObjectPropertyExtensions {

        public static T GetProperty<T, U>(this GameObject gameObject, string name) where T : GameObjectProperty<U>
        {
            if (gameObject == null)
                return null;

            T[] properties = gameObject.GetComponents<T>();
			if (properties != null)
				return properties.FirstOrDefault(property => property.Name == name);

			return null;
        }
    }
}