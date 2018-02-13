using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	public class Outline : MonoBehaviour, IEventListener<RaycastHit?>
	{
		private const string LOG_TAG = "[HoverOutline]";

		// The value of the outline width when the outline is not showing
		private const float NoOutlineValue = 0f;

		[Tooltip("Outline material")]
		public Material OutlineMaterialTemplate;

		[Tooltip("The value of the outline width when the outline is showing")]
		public float HoveredOutlineValue = 0.2f;

		// The instance of the template
		private Material _outlineMaterial;

		// If true the outline will show
		// Will not show otherwise
		public bool ShowOutline
		{
			get { return _outlineMaterial != null && _outlineMaterial.GetFloat("_Outline") > 0; }
			set
			{
				if (_outlineMaterial == null)
					CreateOutlineMaterial();

				if(_outlineMaterial != null && ShowOutline != value)
				{
					float outlineValue = value ? HoveredOutlineValue : NoOutlineValue;
					_outlineMaterial.SetFloat("_Outline", outlineValue);
				}
			}
		}

		public void ValueChanged(EventArgs<RaycastHit?> args)
		{
			ShowOutline = args.NewValue.HasValue;
		}

		/// <summary>
		/// If this object has renderers on itself or in at least one of it's children
		/// create a new outline material and add it to each renderer
		/// </summary>
		private void CreateOutlineMaterial()
		{
			// If this object has renderers Get them
			Renderer[] renderers = GetComponentsInChildren<Renderer>();

			// If there are no renderers we can't add the material
			if (renderers == null || renderers.Length == 0)
			{
				Debug.LogError(String.Format("{0} unable to create outline material for {1}. No renderers", LOG_TAG, name));
				return;
			}

			// Create an instance of the outline material
			_outlineMaterial = Instantiate(OutlineMaterialTemplate);

			// Add the outline material to each renderer
			foreach (Renderer renderer in renderers)
			{
				List<Material> materials = new List<Material>(renderer.materials);
				materials.Add(_outlineMaterial);
				renderer.materials = materials.ToArray();
			}
		}
	}
}
