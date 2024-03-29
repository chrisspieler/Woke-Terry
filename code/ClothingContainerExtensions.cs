public static class ClothingContainerExtensions
{
	public static void Add( this ClothingContainer container, Clothing clothing, bool removeIncompatible = true )
	{
		if ( clothing is null )
			return;

		if ( removeIncompatible )
		{
			container.Clothing.RemoveAll( x => !x.Clothing.CanBeWornWith( clothing ) );
		}
		container.Clothing.Add( new( clothing ) );
	}
}
