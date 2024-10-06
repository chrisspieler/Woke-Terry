public class SelfDestruct : Component
{
	[Property] public float Lifetime { get; set; } = 10f;

	private TimeSince _sinceEnabled;

	protected override void OnEnabled()
	{
		_sinceEnabled = 0f;
	}

	protected override void OnUpdate()
	{
		if ( _sinceEnabled > Lifetime )
		{
			DestroyGameObject();
		}
	}
}
