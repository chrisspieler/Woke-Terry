using Duccsoft.Terry;

public sealed class TerryCycler : Component
{
	[Property] public float CycleIntervalSeconds { get; set; } = 1f;
	[Property] public SkinnedModelRenderer Target { get; set; }
	[Property] public BodyGeneratorConfig BodyConfig { get; set; }
	[Property] public OutfitGeneratorConfig OutfitConfig { get; set; }

	private TimeUntil _nextCycle;

	protected override void OnStart()
	{
		_nextCycle = CycleIntervalSeconds;
	}

	protected override void OnUpdate()
	{
		if ( _nextCycle )
		{
			_nextCycle = CycleIntervalSeconds;
			GenerateNewAppearance();
		}
	}

	[Button("Generate New Appearance")]
	public void GenerateNewAppearance()
	{
		var data = TerryData.Generate( BodyConfig, OutfitConfig );
		var clothing = data.Container;
		clothing.Apply( Target );
		DyeHair( Target, clothing, data.Body.HairColor );
	}

	private static void DyeHair( SkinnedModelRenderer citizen, ClothingContainer clothing, Color hairColor )
	{
		if ( !citizen.IsValid() )
			return;

		var hairClothing = clothing.Clothing
			.Select( c => c.Clothing )
			.Where( ClothingCategories.IsHair );
		foreach ( var child in citizen.GameObject.Children )
		{
			if ( hairClothing.Any( c => child.Name.Contains( c.ResourceName ) ) )
			{
				var renderer = child.Components.Get<SkinnedModelRenderer>();
				renderer.Tint = hairColor;
			}
		}
	}
}
