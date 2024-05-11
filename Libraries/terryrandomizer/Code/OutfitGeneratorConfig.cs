using Sandbox;
using System;

namespace Duccsoft.Terry;

[GameResource( "Clothing Generator Config", "clthcfg", "Defines what clothing items may be returned by a ClothingGenerator.")]
public class OutfitGeneratorConfig : GameResource
{
	[Property, Range( 0, 100 )] public float ShirtPercent { get; set; } = 95f;
	[Property, Range( 0, 100 )] public float JacketPercent { get; set; } = 20f;
	[Property, Range( 0, 100 )] public float BottomPercent { get; set; } = 100f;
	[Property, Range( 0, 100 )] public float FootwearPercent { get; set; } = 80f;
}
