using Pear.InteractionEngine.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	public class KeyboardController : Controller
	{
		void Start()
		{
			SelectWithKeyboard keyboardSelection = GetComponent<SelectWithKeyboard>();
			keyboardSelection.SelectedEvent += selectedObject => ActiveObject = selectedObject;
		}
	}
}
