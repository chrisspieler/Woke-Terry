public class Mover : Component
{
	[Property] public Vector3 Direction { get; set; } = new Vector3( 1f, 0f, 0f );
	[Property] public float Speed { get; set; } = 50f;

	protected override void OnFixedUpdate()
	{
		WorldPosition += Direction * Speed * Time.Delta;
	}
}
