using Pear.InteractionEngine.Controllers;

namespace Pear.InteractionEngine.Examples
{
	public class KeyboardController : Controller
	{
		public override void Start()
		{
			SelectWithKeyboard keyboardSelection = GetComponent<SelectWithKeyboard>();
			keyboardSelection.SelectedEvent += selectedObject => ActiveObject = selectedObject;
		}
	}
}
