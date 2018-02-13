using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Pear.InteractionEngine.Interactions.Pointers;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Input module for Pear adapped from VRTK
	/// https://github.com/thestonefox/VRTK/blob/25991cd6b9f935de07f2dcbd9ed1be82055e14f5/Assets/VRTK/Scripts/Internal/VRTK_VRInputModule.cs
	/// </summary>
	public class Pear_InputModule : PointerInputModule
	{
		// UIPointers registered by the UIPointer class
		public List<UIPointer> pointers = new List<UIPointer>();

		// Needed to allow other regular (non-VR) InputModules in combination with VRTK_EventSystem
		public override bool IsModuleSupported()
		{
			return false;
		}

		/// <summary>
		/// Process each pointer
		/// </summary>
		public override void Process()
		{
			for (int i = 0; i < pointers.Count; i++)
			{
				UIPointer pointer = pointers[i];
				if (pointer.gameObject.activeInHierarchy && pointer.enabled)
				{
					List<RaycastResult> results = new List<RaycastResult>();
					if (pointer.IsActive)
						results = CheckRaycasts(pointer);

					//Process events
					Hover(pointer, results);
					Click(pointer, results);
				}
			}
		}

		/// <summary>
		/// Gets raycast resutls from the specified pointer
		/// </summary>
		/// <param name="pointer">pointer to get results from</param>
		/// <returns>Raycast results</returns>
		protected virtual List<RaycastResult> CheckRaycasts(UIPointer pointer)
		{
			// Pear_GraphicRaycaster uses worldPosition and worldNormal
			// to raycast
			var raycastResult = new RaycastResult();
			raycastResult.worldPosition = pointer.GetOriginPosition();
			raycastResult.worldNormal = pointer.GetOriginForward();

			pointer.pointerEventData.pointerCurrentRaycast = raycastResult;

			List<RaycastResult> raycasts = new List<RaycastResult>();
			eventSystem.RaycastAll(pointer.pointerEventData, raycasts);
			return raycasts;
		}

		/// <summary>
		/// Recursively checks the target's parent tree in search of the source
		/// </summary>
		/// <param name="target">target to check</param>
		/// <param name="source">source to check</param>
		/// <returns>True if source is in target's parent tree. False otherwise.</returns>
		protected virtual bool CheckTransformTree(Transform target, Transform source)
		{
			if (target == null)
			{
				return false;
			}

			if (target.Equals(source))
			{
				return true;
			}

			return CheckTransformTree(target.parent, source);
		}

		/// <summary>
		/// Checks whether there was a valid collision
		/// </summary>
		/// <param name="pointer">Pointer that raycasted</param>
		/// <param name="results">Results from raycast</param>
		/// <returns></returns>
		protected virtual bool NoValidCollision(UIPointer pointer, List<RaycastResult> results)
		{
			return (results.Count == 0 || !CheckTransformTree(results[0].gameObject.transform, pointer.pointerEventData.pointerEnter.transform));
		}

		/// <summary>
		/// Determines whether the pointer is hovering over an object
		/// </summary>
		/// <param name="pointer">Pointer to check</param>
		/// <returns>True if pointer is hovering over an object. False otherwise</returns>
		protected virtual bool IsHovering(UIPointer pointer)
		{
			foreach (var hoveredObject in pointer.pointerEventData.hovered)
			{
				if (pointer.pointerEventData.pointerEnter && hoveredObject && CheckTransformTree(hoveredObject.transform, pointer.pointerEventData.pointerEnter.transform))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks whether the GameObject is a valid element
		/// </summary>
		/// <param name="pointer">Pointer used during determination</param>
		/// <param name="obj">Object to check</param>
		/// <returns>True if object is valid. False otherwise.</returns>
		protected virtual bool ValidElement(UIPointer pointer, GameObject obj)
		{
			var canvasCheck = obj.GetComponentInParent<Canvas>();
			return pointer.IsValidElement(obj) && (canvasCheck && canvasCheck.enabled ? true : false);
		}

		/// <summary>
		/// Attempts to set hover properties if the pointer is hovering over an object
		/// </summary>
		/// <param name="pointer">Pointer to update</param>
		/// <param name="results">Raycast results</param>
		protected virtual void Hover(UIPointer pointer, List<RaycastResult> results)
		{
			if (pointer.pointerEventData.pointerEnter)
			{
				if (!ValidElement(pointer, pointer.pointerEventData.pointerEnter))
				{
					pointer.pointerEventData.pointerEnter = null;
					return;
				}

				if (NoValidCollision(pointer, results))
				{
					ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerEnter, pointer.pointerEventData, ExecuteEvents.pointerExitHandler);
					pointer.pointerEventData.hovered.Remove(pointer.pointerEventData.pointerEnter);
					pointer.pointerEventData.pointerEnter = null;
				}
			}
			else
			{
				foreach (var result in results)
				{
					if (!ValidElement(pointer, result.gameObject))
					{
						continue;
					}

					var target = ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.pointerEnterHandler);
					if (target != null)
					{
						var selectable = target.GetComponent<Selectable>();
						if (selectable)
						{
							var noNavigation = new Navigation();
							noNavigation.mode = Navigation.Mode.None;
							selectable.navigation = noNavigation;
						}

						pointer.HoveredElement = target;
						pointer.pointerEventData.pointerCurrentRaycast = result;
						pointer.pointerEventData.pointerEnter = target;
						pointer.pointerEventData.hovered.Add(pointer.pointerEventData.pointerEnter);
						break;
					}
					else
					{
						pointer.HoveredElement = result.gameObject;
					}
				}

				if (pointer.HoveredElement && results.Count == 0)
				{
					pointer.HoveredElement = null;
				}
			}
		}

		/// <summary>
		/// Clicks the pointer
		/// </summary>
		/// <param name="pointer">Pointer to click</param>
		/// <param name="results">Raycast results</param>
		protected virtual void Click(UIPointer pointer, List<RaycastResult> results)
		{
			switch (pointer.clickMethod)
			{
				case UIPointer.ClickMethods.ClickOnButtonUp:
					ClickOnUp(pointer, results);
					break;
				case UIPointer.ClickMethods.ClickOnButtonDown:
					ClickOnDown(pointer, results);
					break;
			}
		}

		/// <summary>
		/// Clicks the pointer when click stops
		/// </summary>
		/// <param name="pointer">Pointer to click</param>
		/// <param name="results">Raycast results</param>
		protected virtual void ClickOnUp(UIPointer pointer, List<RaycastResult> results)
		{
			pointer.pointerEventData.eligibleForClick = pointer.Click;

			if (!AttemptClick(pointer))
			{
				IsEligibleClick(pointer, results);
			}
		}

		/// <summary>
		/// Clicks the pointer when click starts
		/// </summary>
		/// <param name="pointer">Pointer to click</param>
		/// <param name="results">Raycast results</param>
		protected virtual void ClickOnDown(UIPointer pointer, List<RaycastResult> results, bool forceClick = false)
		{
			pointer.pointerEventData.eligibleForClick = (forceClick ? true : pointer.Click);

			if (IsEligibleClick(pointer, results))
			{
				pointer.pointerEventData.eligibleForClick = false;
				AttemptClick(pointer);
			}
		}

		/// <summary>
		/// Determines if the pointer is eligible for a click
		/// </summary>
		/// <param name="pointer">Pointer to check</param>
		/// <param name="results">Raycast results</param>
		/// <returns>True if pointer is eligible. False otherwise.</returns>
		protected virtual bool IsEligibleClick(UIPointer pointer, List<RaycastResult> results)
		{
			if (pointer.pointerEventData.eligibleForClick)
			{
				foreach (var result in results)
				{
					if (!ValidElement(pointer, result.gameObject))
					{
						continue;
					}

					var target = ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.pointerDownHandler);
					if (target != null)
					{
						pointer.pointerEventData.pressPosition = pointer.pointerEventData.position;
						pointer.pointerEventData.pointerPressRaycast = result;
						pointer.pointerEventData.pointerPress = target;
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Attempts to click the pointer
		/// </summary>
		/// <param name="pointer">Pointer to attempt to click</param>
		/// <returns>True if attempt successfull. False otherwise.</returns>
		protected virtual bool AttemptClick(UIPointer pointer)
		{
			if (pointer.pointerEventData.pointerPress)
			{
				if (!ValidElement(pointer, pointer.pointerEventData.pointerPress))
				{
					pointer.pointerEventData.pointerPress = null;
					return true;
				}

				if (pointer.pointerEventData.eligibleForClick)
				{
					if (!IsHovering(pointer))
					{
						ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerUpHandler);
						pointer.pointerEventData.pointerPress = null;
					}
				}
				else
				{
					ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerClickHandler);
					ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerUpHandler);
					pointer.pointerEventData.pointerPress = null;
				}
				return true;
			}
			return false;
		}
	}
}