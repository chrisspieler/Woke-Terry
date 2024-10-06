public sealed class RandomPronoun : Component
{
	[Property] public TextRenderer Text { get; set; }
	[Property] public Curve Alpha { get; set; }
	[Property] public Curve SpeedFactor { get; set; }
	[Property] public float Lifetime { get; set; } = 1f;
	[Property] public Vector3 TranslationPerSecond { get; set; }
	[Property] public List<PronounSet> PronounSets { get; set; } = new();

	private TimeSince _sincePronounCreated;

	protected override void OnStart()
	{
		Text.Text = GetRandomPronoun();
		_sincePronounCreated = 0f;
	}

	protected override void OnUpdate()
	{
		var lifetimeProgress = _sincePronounCreated / Lifetime;
		Transform.Position += TranslationPerSecond * Time.Delta * SpeedFactor.Evaluate( lifetimeProgress );
		Text.Color = Color.White.WithAlpha( Alpha.Evaluate( lifetimeProgress ) );
		if ( _sincePronounCreated > Lifetime )
		{
			GameObject.Destroy();
		}
	}

	private static List<string> _randomPronouns = new()
	{
		"I",
		"you",
		"it",
		"my",
		"your",
		"its",
		"this",
		"that",
		"these",
		"those",
		"who",
		"whom",
		"which",
		"what",
		"all",
		"any",
		"each",
		"every",
		"myself",
		"yourself",
		"itself",
		"some",
		"anybody",
		"anything",
		"none",
		"ourselves",
	};

	public string GetRandomPronoun()
	{
		List<string> pronouns = PronounSets.SelectMany( p => p.Pronouns ).ToList();
		return Game.Random.FromList( pronouns );
	}
}
