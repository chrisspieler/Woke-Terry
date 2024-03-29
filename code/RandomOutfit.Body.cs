public sealed partial class RandomOutfit : Component
{
	public Clothing GetSkin( Configuration config )
	{
		return config.IsElder
			? config.Skin.ElderSkin
			: config.Skin.YoungSkin;
	}

	private Clothing GetRandomHair( Configuration config )
	{
		if ( config.IsHeadShaved )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>()
			.Where( c => c.Category == Clothing.ClothingCategory.Hair );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomFacialHair( Configuration config )
	{
		if ( !config.HasFacialHair )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>()
			.Where( c => c.Category == Clothing.ClothingCategory.Facial && c.SubCategory == "Beards" );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomLips( Configuration config )
	{
		if ( !config.HasPlumpLips )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsLips );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomEyebrows( Configuration config )
	{
		if ( !config.HasEyebrows )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( IsEyebrows );
		return Game.Random.FromList( options.ToList() );
	}

	private static void DyeHair( SkinnedModelRenderer citizen, ClothingContainer clothing, Configuration config )
	{
		if ( !citizen.IsValid() )
			return;

		var hairClothing = clothing.Clothing
			.Select( c => c.Clothing )
			.Where( IsHair );
		foreach( var child in citizen.GameObject.Children )
		{
			if ( hairClothing.Any( c => child.Name.Contains( c.ResourceName ) ) )
			{
				var renderer = child.Components.Get<SkinnedModelRenderer>();
				renderer.Tint = config.HairColor;
			}
		}
	}

	private static bool IsHair( Clothing clothing )
	{
		return clothing.Category == Clothing.ClothingCategory.Hair
			|| clothing.SubCategory == "Beards";
	}

	private static bool IsEyebrows( Clothing clothing )
	{
		return clothing.Category == Clothing.ClothingCategory.Facial
			&& clothing.SubCategory == "eyebrows";
	}

	private static bool IsLips( Clothing clothing )
	{
		return clothing.Category == Clothing.ClothingCategory.Facial
			&& clothing.SubCategory == "Lips";
	}

	private static bool IsFacialOrnament( Clothing clothing )
	{
		return clothing.Category == Clothing.ClothingCategory.Facial
			&& !IsEyebrows( clothing )
			&& !IsLips( clothing )
			&& !IsHair( clothing );
	}
}
