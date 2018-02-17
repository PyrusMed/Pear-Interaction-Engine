using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	public class Outline : MonoBehaviour, IEventListener<RaycastHit?>
	{
		private const string LOG_TAG = "[Outline]";

		// The value of the outline width when the outline is not showing
		private const float NoOutlineValue = 0f;

		// The name of the material
		private const string MaterialName = "__outlineMat__";

		[Tooltip("Outline material")]
		public Material OutlineMaterialTemplate;

		// If true the outline will show
		// Will not show otherwise
		private static int counter = 0;
		private bool _showOutline = false;
		public bool ShowOutline
		{
			get { return _showOutline; }
			set
			{
				// If nothing changed return
				if (_showOutline == value)
					return;

				// Always remove any existing outline (brute force)
				RemoveOutline();

				// If we're showing the outline add it
				if (value)
					AddOutline();

				// Update the internal value
				_showOutline = value;
			}
		}

		private void Awake()
		{
			
		}

		public void ValueChanged(EventArgs<RaycastHit?> args)
		{
			ShowOutline = args.NewValue.HasValue;
		}

		private void AddOutline()
		{
			//Debug.Log(String.Format("{0} adding outline to {1}", LOG_TAG, name));

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				Material outlineMaterial = Instantiate(OutlineMaterialTemplate);
				outlineMaterial.name = MaterialName;

				List<Material> materials = new List<Material>(renderer.sharedMaterials);
				materials.Add(outlineMaterial);
				renderer.sharedMaterials = materials.ToArray();
			}
		}

		private void RemoveOutline()
		{
			//Debug.Log(String.Format("{0} removing outline from {1}", LOG_TAG, name));

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				// Remove any outline materials
				List<Material> newMaterials = new List<Material>(renderer.sharedMaterials);
				newMaterials.RemoveAll(material => material.name.Contains(MaterialName));
				renderer.sharedMaterials = newMaterials.ToArray();
			}
		}
	}
}
