using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pear.InteractionEngine.Controllers;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
    public class RotateWithKeyboard : ControllerBehavior<KeyboardController>, IGameObjectPropertyEvent<Vector3>
    {
        [Tooltip("Key used to rotate the object left")]
        public KeyCode RotateLeftKey = KeyCode.J;

        [Tooltip("Key used to rotate the object right")]
        public KeyCode RotateRightKey = KeyCode.L;

        [Tooltip("Key used to rotate the object up")]
        public KeyCode RotateUpKey = KeyCode.I;

        [Tooltip("Key used to rotate the object down")]
        public KeyCode RotateDownKey = KeyCode.K;

        private List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        // Update is called once per frame
        void Update()
        {
            if (Controller.ActiveObject == null)
                return;

            Vector3 resizeVal = Vector3.zero;
            if (Input.GetKey(RotateLeftKey))
                resizeVal.x = -1;
            if (Input.GetKey(RotateRightKey))
                resizeVal.x = 1;
            if (Input.GetKey(RotateUpKey))
                resizeVal.y = 1;
            else if (Input.GetKey(RotateDownKey))
                resizeVal.y = -1;

            _properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = resizeVal);
        }

        public void RegisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Add(property);
        }

        public void UnregisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Remove(property);
        }
    }
}
