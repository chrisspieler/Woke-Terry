﻿public static class ClothingContainerExtensions
{
	public static void Add( this ClothingContainer container, Clothing clothing, ClothingConflictResolver conflictResolver )
	{
		if ( clothing is null )
			return;

		if ( conflictResolver == ClothingConflictResolver.RemoveOther )
		{
			container.Clothing.RemoveAll( x => !x.Clothing.CanBeWornWith( clothing ) );
		}
		else if ( conflictResolver == ClothingConflictResolver.RemoveSelf )
		{
			if ( container.Clothing.Any( c => !c.Clothing.CanBeWornWith( clothing ) ) )
			{
				return;
			}
		}
		container.Clothing.Add( new( clothing ) );
	}
}
