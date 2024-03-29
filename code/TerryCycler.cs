public sealed class TerryCycler : Component
{
	[Property] public float CycleIntervalSeconds { get; set; } = 1f;
	[Property] public RandomOutfit Outfit { get; set; }

	private TimeUntil _nextCycle;

	protected override void OnStart()
	{
		Outfit ??= Components.Get<RandomOutfit>();
		_nextCycle = CycleIntervalSeconds;
	}

	protected override void OnUpdate()
	{
		if ( _nextCycle )
		{
			_nextCycle = CycleIntervalSeconds;
			Outfit.DressCitizen( Outfit.Citizen );
		}
	}
}
