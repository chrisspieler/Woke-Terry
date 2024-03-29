[GameResource( "Skin Tone", "skin", "A mapping between some common skin tones and Clothing options.")]
public class SkinTone : GameResource
{
	public string Name { get; set; }
	public Clothing YoungSkin { get; set; }
	public Clothing ElderSkin { get; set; }
	public Gradient NaturalHairColor { get; set; }
}
