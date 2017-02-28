using System;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
	public class ResizeWithKeyboard : ControllerBehavior<Controller>, IGameObjectPropertyChanger<int>
    {
        [Tooltip("Resize property name")]
        public string ResizePropertyName = "pie.resize";

        [Tooltip("Key used to make the object bigger")]
        public KeyCode MakeSmallerKey = KeyCode.Alpha1;

        [Tooltip("Key used to make the object smaller")]
        public KeyCode MakeBiggerKey = KeyCode.Alpha2;

		private List<GameObjectProperty<int>> _properties;

        // Update is called once per frame
        void Update()
        {
            if (Controller.ActiveObject == null)
                return;

			GameObjectProperty<int> activeObjectProperty = _properties.FirstOrDefault(p => p.Owner == Controller.ActiveObject);
			if (activeObjectProperty == null)
				return;

			int resizeVal = 0;
			if (Input.GetKey(MakeSmallerKey))
				resizeVal = -1;
			else if (Input.GetKey(MakeBiggerKey))
				resizeVal = 1;

			activeObjectProperty.Value = resizeVal;
        }

		public void RegisterProperty(GameObjectProperty<int> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<int> property)
		{
			_properties.Remove(property);
		}
	}
}