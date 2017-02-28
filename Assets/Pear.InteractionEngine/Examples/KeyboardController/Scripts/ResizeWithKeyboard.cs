using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
	public class ResizeWithKeyboard : ControllerBehavior<Controller>, IGameObjectPropertyEvent<int>
    {
        [Tooltip("Resize property name")]
        public string ResizePropertyName = "pie.resize";

        [Tooltip("Key used to make the object bigger")]
        public KeyCode MakeSmallerKey = KeyCode.Alpha1;

        [Tooltip("Key used to make the object smaller")]
        public KeyCode MakeBiggerKey = KeyCode.Alpha2;

		private List<GameObjectProperty<int>> _properties = new List<GameObjectProperty<int>>();

        // Update is called once per frame
        void Update()
        {
            if (Controller.ActiveObject == null)
                return;

			int resizeVal = 0;
			if (Input.GetKey(MakeSmallerKey))
				resizeVal = -1;
			else if (Input.GetKey(MakeBiggerKey))
				resizeVal = 1;

			_properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = resizeVal);
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