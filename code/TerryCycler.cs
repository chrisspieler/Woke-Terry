using Duccsoft.Terry;

public sealed class TerryCycler : Component
{
	[Property, Range(0.1f, 1f, 0.1f)] public float CycleIntervalSeconds { get; set; } = 1f;
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
		if ( Input.Pressed( "speed_up" ) )
		{
			CycleIntervalSeconds -= 0.1f;
		}
		else if ( Input.Pressed( "speed_down" ) )
		{
			CycleIntervalSeconds += 0.1f;
		}
		CycleIntervalSeconds = CycleIntervalSeconds.Clamp( 0.1f, 1f );
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
		Target.DyeHair( clothing, data.Body.HairColor );
	}
}
