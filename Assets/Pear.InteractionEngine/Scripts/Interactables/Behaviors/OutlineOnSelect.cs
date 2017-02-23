using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Outline interactables when they're selected
	/// </summary>
	public class OutlineOnSelect : MonoBehaviour
	{
        [Tooltip("Name of the selected property")]
        public string SelectedPropertyName = "pie.select";

		[Tooltip("Outline material")]
		public Material Outline;

		// Use this for initialization
		void Awake()
		{
            GameObjectPropertyManager<bool>.Get(SelectedPropertyName).OnAdded += selectedProperty =>
            {
                Renderer renderer = selectedProperty.GetComponent<Renderer>();
                Material originalMaterial = renderer.material;

                Material outlineMaterial = new Material(Outline);
                outlineMaterial.color = originalMaterial.color;

                selectedProperty.OnChange.AddListener((oldValue, isSelected) =>
                {
                    if(isSelected)
                    {
                        renderer.materials = new Material[] {
                            originalMaterial,
                            outlineMaterial
                        };
                    }
                    else
                    {
                        selectedProperty.GetComponent<Renderer>().materials = new Material[] { originalMaterial };
                    }
                });
            };
		}
	}
}