using System;

public sealed partial class RandomOutfit : Component
{
	public class Configuration
	{
		public SkinTone Skin { get; set; }
		public bool IsElder { get; set; }
		public bool IsHeadShaved { get; set; }
		public bool HasFacialHair { get; set; }
		public bool IsHairDyed { get; set; }
		public Color HairColor { get; set; }
		public bool HasPlumpLips { get; set; }
		public bool HasEyebrows { get; set; }
	}

	[Property] public SkinnedModelRenderer Citizen { get; set; }
	[Property] public bool DressOnStart { get; set; } = true;
	[Property, Range( 0, 100 )] public float ElderPercent { get; set; } = 16.8f;
	[Property, Range( 0, 100 )] public float CueballPercent { get; set; } = 4.5f;
	[Property, Range( 0, 100 )] public float FacialHairPercent { get; set; } = 15f;
	[Property, Range( 0, 100 )] public float HairDyePercent { get; set; } = 10f;
	[Property, Range( 0, 100 )] public float FacialOrnamentPercent { get; set; } = 50f;
	[Property, Range( 0, 100 )] public float PlumpLipsPercent { get; set; } = 50f;
	[Property, Range( 0, 100 )] public float EyebrowsPercent { get; set; } = 90f;

	protected override void OnStart()
	{
		if ( DressOnStart )
		{
			DressCitizen( Citizen );
		}
	}

	[Button( "Dress" )]
	public void DressButton()
	{
		DressCitizen( Citizen );
	}

	public void DressCitizen( SkinnedModelRenderer citizen )
	{
		if ( !citizen.IsValid() )
			return;

		var config = GetConfiguration();
		var container = new ClothingContainer();
		container.Add( GetSkin( config ) );
		container.Add( GetRandomHair( config ) );
		container.Add( GetRandomFacialHair( config ) );
		container.Add( GetRandomLips( config ) );
		container.Add( GetRandomEyebrows( config ) );
		container.Add( GetRandomFacialOrnament( config ) );
		container.Apply( citizen );
		DyeHair( citizen, container, config );
	}

	public Configuration GetConfiguration()
	{
		var config = new Configuration()
		{
			Skin			= Game.Random.FromList( ResourceLibrary.GetAll<SkinTone>().ToList() ),
			IsElder			= ElderPercent >= Game.Random.Float( 0, 100 ),
			IsHairDyed		= HairDyePercent >= Game.Random.Float( 0, 100 ),
			HasEyebrows		= EyebrowsPercent >= Game.Random.Float( 0, 100 ),
			HasPlumpLips	= PlumpLipsPercent >= Game.Random.Float( 0, 100 ),
			IsHeadShaved	= CueballPercent >= Game.Random.Float( 0, 100 ),
			HasFacialHair	= FacialHairPercent >= Game.Random.Float( 0, 100 ),
		};
		ConfigureHairColor( config );
		return config;
	}

	private void ConfigureHairColor( Configuration config )
	{
		config.HairColor = config.IsHairDyed
			? GetDyedHairColor()
			: GetNaturalHairColor( config );
	}

	private Color GetNaturalHairColor( Configuration config )
	{
		var gradientProgress = Game.Random.Float();
		// Shift elder hair towards the gray.
		if ( config.IsElder )
		{
			gradientProgress -= 0.7f;
			gradientProgress = MathF.Max( 0f, gradientProgress );
		}
		return config.Skin.NaturalHairColor.Evaluate( gradientProgress );
	}

	private Color GetDyedHairColor()
	{
		return new ColorHsv()
			.WithHue( Game.Random.Float( 0, 360 ) )
			.WithSaturation( Game.Random.Float( 0.5f, 1f ) )
			.WithValue( Game.Random.Float( 0.3f, 1f ) )
			.WithAlpha( 1f );
	}
}
