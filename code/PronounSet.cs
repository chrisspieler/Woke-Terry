namespace Sandbox;

[GameResource( "Pronoun Set", "pnouns", "A set of pronouns.", Icon = "wc" )]
public class PronounSet : GameResource
{
	public string Name { get; set; } = "New Pronoun Set";
	public List<string> Pronouns { get; set; } = new();
}
