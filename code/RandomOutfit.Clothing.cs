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

	private bool IsFullOutfit( Clothing clothing )
	{
		return clothing.SubCategory == "Full Outfit";
	}

	private Clothing GetRandomShirt( Configuration config )
	{
		if ( !config.ShouldWearShirt )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsShirt );
		return Game.Random.FromList( options.ToList() );
	}

	private bool IsTop( Clothing clothing )
	{
		return clothing.Category == Clothing.ClothingCategory.Tops
			&& !clothing.HideBody.HasFlag( Clothing.BodyGroups.Legs )
			&& !clothing.SlotsOver.HasFlag( Clothing.Slots.Groin );
	}

	private bool IsShirt( Clothing clothing )
	{
		if ( clothing is null )
			return false;

		if ( clothing.ResourceName.Contains( "chainmail" ) )
			return false;

		return IsShirt( clothing.Parent )
			|| IsTop( clothing )
			&& !IsDress( clothing )
			&& clothing.SlotsOver.HasFlag( Clothing.Slots.Chest )
			&& !IsFullOutfit( clothing );
	}

	private Clothing GetRandomJacket( Configuration config )
	{
		// Assumes that someone would not wear a jacket without also wearing a shirt.
		if ( !config.ShouldWearShirt || !config.ShouldWearJacket )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsJacket );
		return Game.Random.FromList( options.ToList() );
	}

	private bool IsJacket( Clothing clothing )
	{
		if ( clothing is null )
			return false;

		return IsJacket( clothing.Parent )
			|| IsTop( clothing )
			&& !IsDress( clothing )
			&& !clothing.SlotsOver.HasFlag( Clothing.Slots.Chest )
			&& !IsFullOutfit( clothing )
			&& clothing.SubCategory != "Vests";
	}

	private bool IsDress( Clothing clothing )
	{
		if ( clothing is null )
			return false;

		return IsDress( clothing.Parent )
			|| clothing.SubCategory == "Dresses";
	}

	private Clothing GetRandomBottoms( Configuration config )
	{
		if ( !config.ShouldWearBottom )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsBottom );
		return Game.Random.FromList( options.ToList() );
	}

	private bool IsBottom( Clothing clothing )
	{
		if ( clothing is null )
			return false;

		var excluded = new[] { "armour", "cardboard", "binman" };
		if ( excluded.Any( clothing.ResourceName.Contains ) )
		{
			return false;
		}

		return IsBottom( clothing.Parent )
			|| clothing.Category == Clothing.ClothingCategory.Bottoms;
	}

	private Clothing GetRandomFootwear( Configuration config )
	{
		if ( !config.ShouldWearFootwear )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsFootwear );
		return Game.Random.FromList( options.ToList() );
	}

	private bool IsFootwear( Clothing clothing )
	{
		if ( clothing is null )
			return false;

		return IsFootwear( clothing.Parent )
			|| clothing.Category == Clothing.ClothingCategory.Footwear;
	}
}
