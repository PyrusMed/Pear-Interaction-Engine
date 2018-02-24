using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Makes the collising object a ghost of it's former self
	/// </summary>
	public class SnapBackCollider : AggregateMeshCollider
	{
		private void Start()
		{
			FadeHelper fader = transform.GetOrAddComponent<FadeHelper>();
			fader.fadeAplha = 0.01f;
			fader.FadeOut();
		}
	}
}
