using System;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Controllers.Behaviors;
using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	public class SelectWithKeyboard : ControllerBehavior<Controller>, IPropertyChanger<bool>
    {
        [Tooltip("Select property name")]
        public string SelectPropertyName = "pie.select";

        public KeyCode SelectKey = KeyCode.P;

        HoverOnGaze _hoverOnGaze;

        BoolGameObjectProperty _lastSelectedProperty;

        // Use this for initialization
        void Start()
        {
            _hoverOnGaze = GetComponent<HoverOnGaze>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(SelectKey) && _hoverOnGaze.HoveredObject != null)
            {
				BoolGameObjectProperty selectedProperty = _hoverOnGaze.HoveredObject.gameObject.GetProperty<BoolGameObjectProperty, bool>(SelectPropertyName);
                if(selectedProperty != null)
                {
                    if (selectedProperty.Value)
                    {
                        selectedProperty.Value = false;
                        _lastSelectedProperty = null;
                    }
                    else
                    {
                        selectedProperty.Value = true;
                        Controller.ActiveObject = _hoverOnGaze.HoveredObject;
                        if (_lastSelectedProperty != null)
                            _lastSelectedProperty.Value = false;

						_lastSelectedProperty = selectedProperty;
					}
                }
            }
        }

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			throw new NotImplementedException();
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			throw new NotImplementedException();
		}
	}
}