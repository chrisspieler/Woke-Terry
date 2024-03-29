public sealed partial class RandomOutfit : Component
{
	private Clothing GetRandomFacialOrnament( Configuration config )
	{
		// Beards conflict with the facial ornament, so skip this.
		if ( config.HasFacialHair )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>()
			.Where( IsFacialOrnament );
		return Game.Random.FromList( options.ToList() );
	}
}
