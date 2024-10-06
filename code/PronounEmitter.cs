public sealed class PronounEmitter : Component
{
	[Property] public BBox Bounds { get; set; } = BBox.FromPositionAndSize( Vector3.Zero, 200f );
	[Property] public int PronounsPerSecond { get; set; } = 5;
	private float PronounDelay => 1f / PronounsPerSecond;
	[Property] public GameObject PronounPrefab { get; set; }
	[Property] public List<PronounSet> PronounSets { get; set; } = new();

	private TimeUntil _untilNextPronoun;

	protected override void OnStart()
	{
		_untilNextPronoun = 0f;
	}

	protected override void OnUpdate()
	{
		if ( _untilNextPronoun )
		{
			_untilNextPronoun = PronounDelay;
			CreatePronoun();
		}
	}

	protected override void DrawGizmos()
	{
		Gizmo.Draw.LineBBox( Bounds );
	}

	private void CreatePronoun()
	{
		var position = Transform.World.PointToWorld( Bounds.RandomPointInside );
		var pronoun = PronounPrefab.Clone( position );
		if ( pronoun.IsValid() && pronoun.Components.TryGet<RandomPronoun>( out var pronounComponent ) )
		{
			pronounComponent.PronounSets = PronounSets;
		}
	}
}
