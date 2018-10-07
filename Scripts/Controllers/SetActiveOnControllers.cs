using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers {
	/// <summary>
	/// Sets a set of game objects active on a set of controllers
	/// </summary>
	public class SetActiveOnControllers : MonoBehaviour {
		[Tooltip("Controllers to update")]
		public Controller[] Controllers;

		[Tooltip("Game objects to set active")]
		public GameObject[] Actives;

		void Start() {
			foreach (Controller controller in Controllers)
				controller.SetActive(Actives);
		}
	}
}
