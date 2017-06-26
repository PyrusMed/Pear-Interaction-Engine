using UnityEngine;
using Pear.InteractionEngine.Events;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Outline objects based on property changes
	/// </summary>
	public class Outline : MonoBehaviour, IEventListener<bool>
	{
		[Tooltip("Outline material")]
		public Material OutlineMaterial;

		// The object's renderer
		private Renderer _renderer;

		// The material the object started with
		private Material _originalMaterial;

		private void Start()
		{
			_renderer = GetComponent<Renderer>();
			if(_renderer != null)
				_originalMaterial = _renderer.material;
		}

		/// <summary>
		/// When true, add the outline material
		/// </summary>
		/// <param name="args">Event args</param>
		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue)
			{
				_renderer.materials = new Material[]
				{
					_originalMaterial,
					OutlineMaterial
				};
			}
			else
			{
				_renderer.materials = new Material[] { _originalMaterial };
			}
		}
	}
}