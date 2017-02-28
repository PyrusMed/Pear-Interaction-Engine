using System;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	public class ResizeWithKeyboard : ControllerBehavior<Controller>, IPropertyChanger<int>
    {
        [Tooltip("Resize property name")]
        public string ResizePropertyName = "pie.resize";

        [Tooltip("Key used to make the object bigger")]
        public KeyCode MakeSmallerKey = KeyCode.Alpha1;

        [Tooltip("Key used to make the object smaller")]
        public KeyCode MakeBiggerKey = KeyCode.Alpha2;

        [Tooltip("The percent per second in which the object changes while scaling")]
        public float ScalePercentage = 0.1f;

        // Update is called once per frame
        void Update()
        {
            if (Controller.ActiveObject == null)
                return;

			BoolGameObjectProperty resizeProperty = Controller.ActiveObject.gameObject.GetProperty<BoolGameObjectProperty, bool>(ResizePropertyName);
            if (resizeProperty == null)
                return;

            bool resized = true;
            if (Input.GetKey(MakeSmallerKey))
                Resize(increase: false);
            else if (Input.GetKey(MakeBiggerKey))
                Resize(increase: true);
            else
                resized = false;

            resizeProperty.Value = resized;
        }

        /// <summary>
        /// Resize the active object
        /// </summary>
        /// <param name="increase"></param>
        void Resize(bool increase)
        {
            int direction = increase ? 1 : -1;
            float scaleAmount = ScalePercentage * Time.deltaTime;

            InteractableObject interactable = Controller.ActiveObject.transform.GetOrAddComponent<InteractableObject>();
            Anchor anchor = interactable.AnchorElement;
            float currentScale = anchor.transform.localScale.x;
            float newScale = currentScale * (1 + scaleAmount * direction);

            // Apply the new scale
            anchor.transform.localScale = Vector3.one * newScale;
        }

		public void RegisterProperty(GameObjectProperty<int> property)
		{
			throw new NotImplementedException();
		}

		public void UnregisterProperty(GameObjectProperty<int> property)
		{
			throw new NotImplementedException();
		}
	}
}