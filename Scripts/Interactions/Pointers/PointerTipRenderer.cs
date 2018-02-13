using Pear.InteractionEngine.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.Pointers
{
	public class PointerTipRenderer : MonoBehaviour
	{

		[Tooltip("The BasePointer script that determines what we're pointing at")]
		public BasePointer Pointer;

		// Use this for initialization
		void Start()
		{
			Pointer.HoverChangedEvent += (oldHovered, newHovered) =>
			{
				gameObject.SetActive(newHovered != null);
			};

			// Disable by default
			gameObject.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
			if (Pointer.IsActive && Pointer.HoveredElement != null &&
				Vector3.Distance(Pointer.RaycastResult.worldPosition, Pointer.GetOriginPosition()) > 0.1f)
			{
				transform.position = Pointer.RaycastResult.worldPosition;
				transform.LookAt(Pointer.GetOriginPosition());
			}
		}

		private void OnEnable()
		{
			gameObject.SetActive(true);
		}

		private void OnDisable()
		{
			gameObject.SetActive(false);
		}
	}
}
