using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pear.InteractionEngine.Interactions.Pointers
{
	public class PhysicsPointer : BasePointer
	{
		// Information about the raycast hit
		[HideInInspector]
		private RaycastResult raycastResult;

		/// <summary>
		/// The result of the raycast
		/// </summary>
		public override RaycastResult RaycastResult { get { return raycastResult; } }

		public void Update()
		{
			// Check for 3d objects
			Ray pointerRaycast = new Ray(GetOriginPosition(), GetOriginForward());
			RaycastHit hitInfo;
			if (Physics.Raycast(pointerRaycast, out hitInfo, 1000, CollisionLayers))
			{
				raycastResult = new RaycastResult
				{
					distance = hitInfo.distance,
					gameObject = hitInfo.transform.gameObject,
					worldNormal = hitInfo.normal,
					worldPosition = hitInfo.point,
				};

				HoveredElement = hitInfo.transform.gameObject;
			}
			else
			{
				HoveredElement = null;
			}
		}
	}
}
