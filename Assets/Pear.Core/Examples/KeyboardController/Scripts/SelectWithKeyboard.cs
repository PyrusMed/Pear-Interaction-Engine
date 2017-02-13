﻿using Pear.Core.Controllers;
using Pear.Core.Controllers.Behaviors;
using Pear.Core.Interactables;
using UnityEngine;

namespace Pear.Core.Examples
{
    public class SelectWithKeyboard : ControllerBehavior<Controller>
    {

        public KeyCode SelectKey = KeyCode.P;

        HoverOnGaze _hoverOnGaze;

        InteractableObject _lastSelected;

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
                InteractableObjectState selectedState = _hoverOnGaze.HoveredObject.Selected;
                if (selectedState.Contains(Controller))
                {
                    selectedState.Remove(Controller);
                    Controller.ActiveObject = null;
                }
                else
                {
                    selectedState.Add(Controller);
                    Controller.ActiveObject = _hoverOnGaze.HoveredObject;

                    if (_lastSelected != _hoverOnGaze.HoveredObject && _lastSelected != null)
                        _lastSelected.Selected.Remove(Controller);

                    _lastSelected = _hoverOnGaze.HoveredObject;
                }
            }
        }
    }
}