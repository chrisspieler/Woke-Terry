[GameResource( "Pride Flag", "pflag", "A flag representing a particular queer identity.", Icon = "looks")]
public class PrideFlag : GameResource
{
	public enum PrideCategory
	{
		GenderIdentity,
		SexualOrientation,
		Trans
	}

	public string Name { get; set; }
	public Model FlagModel { get; set; }
	public PrideCategory Category { get; set; }
	public float RelativePercentChance { get; set; } = 50f;

	public static PrideFlag RandomGenderIdentity => GetRandomFromCategory( PrideCategory.GenderIdentity );
	public static PrideFlag RandomSexualOrientation => GetRandomFromCategory( PrideCategory.SexualOrientation );

	public static PrideFlag GetRandomFromCategory( PrideCategory category )
	{
		var flags = ResourceLibrary.GetAll<PrideFlag>()
			.Where( f => f.Category == category );

		if ( !flags.Any() )
			return null;

		var chancer = new RandomChancer<PrideFlag>();
		foreach( var flag in flags )
		{
			chancer.AddItem( flag, flag.RelativePercentChance );
		}
		return chancer.GetNext();
	}
}
