using System;

public class FlagEmitter : Component
{
	[Property] public Vector3 SequenceElementOffset { get; set; } = new Vector3( 0f, 0f, -48f );
	[Property] public Vector3 FlagTranslationPerSecond { get; set; } = new Vector3( 0f, -50f, 0f );
	[Property] public PrideFlag TransPrideFlag { get; set; }
	[Property, Range(0f, 100f)] public float BaseHeterosexualPercent { get; set; } = 86f;
	[Property, Range(0f, 100f)] public float TransPercent { get; set; } = 1f;
	[Property] public float TransQueernessMultiplier { get; set; } = 5f;
	[Property, Range( 0f, 100f )] public float NonBinaryPercent { get; set; } = 2f;
	[Property] public float NonBinaryQueernessMultiplier { get; set; } = 2f;

	public void EmitFlags( IEnumerable<PrideFlag> flags )
	{
		var spawnPos = WorldPosition;
		foreach( var flag in flags )
		{
			if ( !flag.IsValid() || !flag.FlagModel.IsValid() )
				continue;

			var flagInstance = new GameObject( true, $"{flag.Name}" );
			flagInstance.WorldPosition = spawnPos;
			flagInstance.WorldRotation = Rotation.FromRoll( -90f );

			var renderer = flagInstance.AddComponent<ModelRenderer>();
			renderer.Model = flag.FlagModel;
			var mover = flagInstance.AddComponent<Mover>();
			mover.Direction = FlagTranslationPerSecond.Normal;
			mover.Speed = FlagTranslationPerSecond.Length;
			var selfDestruct = flagInstance.AddComponent<SelfDestruct>();
			selfDestruct.Lifetime = 10f;

			spawnPos += SequenceElementOffset;
		}
	}

	public IEnumerable<PrideFlag> GetRandomPrideFlags()
	{
		var flags = new List<PrideFlag>();
		var heterosexualPercent = BaseHeterosexualPercent;
		var nonbinaryPercent = NonBinaryPercent;

		var isTrans = Game.Random.Float( 0f, 100f ) < TransPercent;
		if ( isTrans && TransPrideFlag.IsValid() )
		{
			if ( TransPrideFlag.IsValid() )
			{
				// There's only one trans pride flag, so just use that.
				flags.Add( TransPrideFlag );
			}
			nonbinaryPercent *= TransQueernessMultiplier;
			heterosexualPercent -= ( 100f - BaseHeterosexualPercent ) * TransQueernessMultiplier;
		}

		var isNonbinary = Game.Random.Float( 0f, 100f ) < nonbinaryPercent;
		if ( isNonbinary )
		{
			var genderIdentityFlag = PrideFlag.RandomGenderIdentity;
			if ( genderIdentityFlag.IsValid() )
			{
				flags.Add( PrideFlag.RandomGenderIdentity );
			}
			// If we didn't already subtract from heterosexual percent, do it now.
			if ( !isTrans )
			{
				heterosexualPercent -= ( 100f - BaseHeterosexualPercent ) * NonBinaryQueernessMultiplier;
			}
		}
		heterosexualPercent = MathF.Max( 0f, heterosexualPercent );

		var isHeterosexual = Game.Random.Float( 0f, 100f ) < heterosexualPercent;
		if ( !isHeterosexual )
		{
			var sexualOrientationFlag = PrideFlag.RandomSexualOrientation;
			if ( sexualOrientationFlag.IsValid() )
			{
				flags.Add( sexualOrientationFlag );
			}
		}

		return flags;
	}
}
