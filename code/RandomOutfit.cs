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
		public bool ShouldWearShirt { get; set; }
		public bool ShouldWearJacket { get; set; }
		public bool ShouldWearBottom { get; set; }
		public bool ShouldWearFootwear { get; set; }
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
	[Property, Range( 0, 100 )] public float ShirtPercent { get; set; } = 95f;
	[Property, Range( 0, 100 )] public float JacketPercent { get; set; } = 20f;
	[Property, Range( 0, 100 )] public float BottomPercent { get; set; } = 100f;
	[Property, Range( 0, 100 )] public float FootwearPercent { get; set; } = 80f;

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
		container.Add( GetSkin( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomHair( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomFacialHair( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomLips( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomEyebrows( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomFacialOrnament( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomShirt( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomJacket( config ), ClothingConflictResolver.RemoveSelf );
		container.Add( GetRandomBottoms( config ), ClothingConflictResolver.DoNothing );
		container.Add( GetRandomFootwear( config ), ClothingConflictResolver.DoNothing );
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
			ShouldWearShirt = ShirtPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearJacket = JacketPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearBottom = BottomPercent >= Game.Random.Float( 0, 100 ),
			ShouldWearFootwear = FootwearPercent >= Game.Random.Float( 0, 100 ),
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
		if ( config.IsElder && Game.Random.Float() < 0.5f )
		{
			gradientProgress -= 0.5f;
			gradientProgress = MathF.Max( 0f, gradientProgress );
		}
		else
		{
			gradientProgress += 0.25f;
			gradientProgress = MathF.Min( 1f, gradientProgress );
		}
		return config.Skin.NaturalHairColor.Evaluate( gradientProgress );
	}

	private Color GetDyedHairColor()
	{
		return new ColorHsv()
			.WithHue( Game.Random.Float( 0, 360 ) )
			.WithSaturation( Game.Random.Float( 0.1f, 0.6f ) )
			.WithValue( Game.Random.Float( 1f ) )
			.WithAlpha( 1f );
	}
}
