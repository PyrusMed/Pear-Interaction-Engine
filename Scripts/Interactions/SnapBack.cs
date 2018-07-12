using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class SnapBack : MonoBehaviour
	{
		private Anchor _anchor;
		private StoreTransform _originalData;

		// Tells whether we are colliding with our collider mesh
		private bool _colliding = false;

		// Use this for initialization
		void Start()
		{
			_anchor = transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement;
			_originalData = _anchor.transform.SaveLocal();

			CreateCollider();
		}

		public void Snap()
		{
			_anchor.transform.LoadLocal(_originalData);
		}

		private void CreateCollider()
		{
			GameObject colliderParent = new GameObject();
			colliderParent.name = name + "__snapBackCollider";
			colliderParent.transform.SetParent(_anchor.transform.parent);
			colliderParent.transform.LoadLocal(_originalData);

			// Duplicate the mesh and make sure we can collide with it
			foreach(MeshCollider originalCollider in GetComponentsInChildren<MeshCollider>())
			{
				GameObject originalObject = originalCollider.gameObject;

				GameObject meshContainer = new GameObject();
				meshContainer.name = originalObject.name + "__duplicatedMesh";
				meshContainer.transform.SetParent(colliderParent.transform);
				meshContainer.transform.LoadLocal(originalObject.transform.SaveLocal());

				// Duplicate the materials
				MeshRenderer meshRenderer = meshContainer.AddComponent<MeshRenderer>();
				MeshRenderer originalMeshRenderer = originalObject.GetComponent<MeshRenderer>();
				List<Material> materials = new List<Material>();
				foreach (Material material in originalMeshRenderer.materials)
				{
					materials.Add(Instantiate(material));
				}
				meshRenderer.materials = materials.ToArray();
                meshRenderer.enabled = false;

				// Duplicate the mesh
				MeshFilter meshFilter = meshContainer.AddComponent<MeshFilter>();
				MeshFilter originalMeshFilter = originalMeshRenderer.GetComponent<MeshFilter>();
				meshFilter.mesh = Instantiate(originalMeshFilter.mesh);

				// Add the collider
				MeshCollider collider = meshContainer.AddComponent<MeshCollider>();
				collider.convex = true;
				collider.isTrigger = true;
				collider.sharedMesh = Instantiate(originalCollider.sharedMesh);

				// Add the snap back collider
				SnapBackCollider snapBackCollider = collider.transform.GetOrAddComponent<SnapBackCollider>();

				// Detect when we're colliding
				snapBackCollider.IsValidCollision += collision => collision.gameObject == gameObject;
				snapBackCollider.OnCollisionEnterEvent += collision => _colliding = true;
				snapBackCollider.OnCollisionExitEvent += collision => _colliding = false;
			}
		}
	}
}
