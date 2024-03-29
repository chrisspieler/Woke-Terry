using Sandbox;
using Sandbox.Utility;
using System;

public sealed class CameraWobble : Component
{
	[Property] public CameraComponent Camera { get; set; }
	[Property] public GameObject OrbitTarget { get; set; }
	[Property] public float OrbitDistance { get; set; } = 100f;
	[Property] public float OrbitSwaySpeed { get; set; } = 1f;
	[Property] public Angles OrbitSwayAmount { get; set; } = new Angles( 5f, 5f, 5f );
	[Property] public RangedFloat FovRange { get; set; } = new RangedFloat( 50f, 70f );
	[Property] public float ZoomSpeed { get; set; }
	[Property] public float SwaySpeed { get; set; } = 1f;
	[Property] public Angles SwayAmount { get; set; } = new Angles( 5f, 5f, 5f );

	private Angles _orbitAngle;

	protected override void OnStart()
	{
		Camera ??= Components.Get<CameraComponent>();
		_orbitAngle = OrbitTarget.Transform.Rotation;
	}

	protected override void OnPreRender()
	{
		Camera.FieldOfView = MathF.Sin( Time.Now * ZoomSpeed )
			.Remap( -1, 1, FovRange.x, FovRange.y);
		Transform.Position = GetBasePosition();
		Transform.Rotation = GetBaseRotation() * GetSway();
	}

	private Vector3 GetBasePosition()
	{
		Angles orbitAngle = OrbitTarget.Transform.Rotation;
		orbitAngle.pitch += MathF.Sin( Time.Now * OrbitSwaySpeed ) * OrbitSwayAmount.pitch;
		orbitAngle.yaw += MathF.Sin( Time.Now * OrbitSwaySpeed + 1000f ) * OrbitSwayAmount.yaw;
		var position = OrbitTarget.Transform.Position + orbitAngle.Forward * OrbitDistance;
		return position;
	}

	private Rotation GetBaseRotation()
	{
		var dir = OrbitTarget.Transform.Position - Transform.Position;
		return Rotation.LookAt( dir );
	}

	private Rotation GetSway()
	{
		var pitch = Noise.Perlin( Time.Now * SwaySpeed ) * SwayAmount.pitch;
		var yaw = Noise.Perlin( Time.Now * SwaySpeed + 1000f ) * SwayAmount.yaw;
		var roll = Noise.Perlin( Time.Now * SwaySpeed + 2000f ) * SwayAmount.roll;
		return new Angles( pitch, yaw, roll );
	}
}
