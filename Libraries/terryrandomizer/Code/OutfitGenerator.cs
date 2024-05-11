using Sandbox;
using System.Linq;

namespace Duccsoft.Terry;

public class OutfitGenerator
{
	public OutfitGenerator( OutfitGeneratorConfig config )
	{
		_config = config;
	}

	private OutfitGeneratorConfig _config;

	public OutfitData GenerateData()
	{
		var data = new OutfitData()
		{
			ShouldWearShirt = _config.ShirtPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearJacket = _config.JacketPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearBottom = _config.BottomPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearFootwear = _config.FootwearPercent >= Game.Random.Float( 0, 100 )
		};
		data.Resources = GetClothingResources( data );
		return data;
	}

	private ClothingContainer GetClothingResources( OutfitData data )
	{
		var clothing = new ClothingContainer();
		clothing.Add( GetRandomShirt( data ), ClothingConflictResolver.DoNothing );
		clothing.Add( GetRandomJacket( data ), ClothingConflictResolver.RemoveSelf );
		clothing.Add( GetRandomBottoms( data ), ClothingConflictResolver.DoNothing );
		clothing.Add( GetRandomFootwear( data ), ClothingConflictResolver.DoNothing );
		return clothing;
	}

	private Clothing GetRandomShirt( OutfitData data )
	{
		if ( !data.ShouldWearShirt )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( ClothingCategories.IsShirt );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomJacket( OutfitData data )
	{
		// Assumes that someone would not wear a jacket without also wearing a shirt.
		if ( !data.ShouldWearShirt || !data.ShouldWearJacket )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( ClothingCategories.IsJacket );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomBottoms( OutfitData data )
	{
		if ( !data.ShouldWearBottom )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( ClothingCategories.IsBottom );
		return Game.Random.FromList( options.ToList() );
	}

	private Clothing GetRandomFootwear( OutfitData data )
	{
		if ( !data.ShouldWearFootwear )
			return null;

		var options = ResourceLibrary.GetAll<Clothing>().Where( ClothingCategories.IsFootwear );
		return Game.Random.FromList( options.ToList() );
	}
}
